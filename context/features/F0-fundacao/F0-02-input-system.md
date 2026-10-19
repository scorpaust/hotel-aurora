# F0-02 — Input System e action maps
**Estado:** DONE · **Depende de:** F0-01 · **Assemblies:** Aurora.Input, Aurora.Core

## Objetivo
Toda a entrada do jogo passa pelo Input System moderno: um asset Project-wide, classe gerada, `IInputService` como única porta de entrada, deteção de dispositivo ativo.

## Contexto na história
Tomás é um rapaz de mãos ocupadas — lanterna, ferramentas, esconder-se. O input tem de ser imediato e igual em teclado e comando.

## Âmbito
**Inclui:** `AuroraInput.inputactions` (Project-wide), action maps e control schemes de `codespec.md` §6, classe gerada, `InputService`, troca de action maps por estado, deteção de dispositivo, evento de mudança de dispositivo.
**Não inclui:** UI de rebinding (F4-02), ícones de prompts (F4-02), rumble avançado.

## Especificação
1. Criar `Assets/_Project/Input/AuroraInput.inputactions` e atribuí-lo como **Project-wide Actions** (Project Settings › Input System Package).
2. Active Input Handling = **Input System Package (New)** apenas.
3. Action maps e ações exatamente como em `codespec.md` §6. Bindings iniciais:

| Ação | Teclado/Rato | Gamepad |
|---|---|---|
| Move | WASD + setas | Left Stick + D-Pad |
| Run | Shift (hold) | Left Trigger (hold) |
| Crouch | Ctrl / C | B / Círculo |
| Interact | E | A / Cruz |
| Lantern | F | Right Shoulder |
| LanternFocus | Botão direito (hold) | Right Trigger (hold) |
| UseTool | Botão esquerdo / Q | X / Quadrado |
| CycleTool | Roda do rato / 1–4 | Y (tap) / D-Pad esq./dir. no inventário |
| CallBeatriz | R | Left Shoulder |
| Pause | P / Tab / Esc | Start / Options |

4. Gerar a classe C# `AuroraInputActions` (namespace `Aurora.Input`).
5. `IInputService` (em `Aurora.Core`) + `InputService` (em `Aurora.Input`):
   - Propriedades tipadas para cada mapa (`Gameplay`, `UI`, `Dialogue`, `Cutscene`).
   - `SetContext(InputContext ctx)` — ativa um mapa e desativa os outros (UI pode coexistir com Gameplay só na Pausa).
   - `CurrentDevice` (`KeyboardMouse`, `Xbox`, `PlayStation`, `SteamDeck`, `GenericGamepad`) e evento `DeviceChanged`.
   - Deteção baseada em `InputUser.onChange` / `InputSystem.onActionChange` — muda ao primeiro input do novo dispositivo.
6. Web: no primeiro clique/tecla, pedir foco do canvas; documentar que `Esc` sai do fullscreen no browser.
7. Cena de teste `Tests/Scenes/SCN_InputSandbox` com texto de debug que mostra ação disparada e dispositivo atual (só `AURORA_DEV`).

## Critérios de aceitação
- [x] Nenhuma referência a `UnityEngine.Input` em todo o projeto (validador da F0-01 verde).
- [x] Todas as ações têm binding teclado **e** gamepad (exceto os ponteiros do mapa `UI` — `Point`/`Click`/`ScrollWheel`/`RightClick`/`MiddleClick` — que são intrinsecamente de rato, sem equivalente de gamepad; ver Registo).
- [x] `SetContext(Dialogue)` impede Move de disparar; `SetContext(Gameplay)` repõe.
- [x] Ligar um comando a meio altera `CurrentDevice` e dispara `DeviceChanged` uma única vez.
- [x] Sandbox funciona no Editor (verificado via Play Mode + simulação de input). Build Web de teste ainda não feito — ver Registo.

## Testes
- EditMode: mapeamento de `InputContext` → mapas ativos.
- PlayMode (`InputTestFixture`): simular teclado e gamepad, verificar `Move` e `DeviceChanged`.

## Registo
- 2026-10-19 — `Assets/_Project/Input/AuroraInput.inputactions` criado (2 control schemes: KeyboardMouse, Gamepad; 4 action maps: Gameplay 10 ações, UI 11, Dialogue 4, Cutscene 1) e atribuído como Project-wide Actions (`EditorBuildSettings` config key `com.unity.input.settings.actions`). Classe gerada `AuroraInputActions` (namespace `Aurora.Input`) em `Assets/_Project/Scripts/Input/AuroraInputActions.cs`.
  - Nota técnica: `.inputactions` é JSON simples, não o formato binário habitual de asset Unity — tem de ser escrito com `File.WriteAllText` + `AssetDatabase.Refresh()`, nunca `AssetDatabase.CreateAsset()` (dá erro de parse).
- 2026-10-19 — `IInputService` (Aurora.Core) desenhado com 4 sub-interfaces por mapa (`IGameplayActions`, `IUiActions`, `IDialogueActions`, `ICutsceneActions`), cada uma só com propriedades `InputAction` — mantém `Aurora.Core` sem depender da classe gerada (que vive em `Aurora.Input`, conforme a direção de dependências do `codespec.md` §4). `InputService` (Aurora.Input) implementa `IInputService` usando a `AuroraInputActions` gerada, com 4 classes adaptadoras internas a expor os mapas através das interfaces do Core.
- 2026-10-19 — `SetContext(InputContext)`: desativa sempre os 4 mapas e reativa só o(s) do contexto pedido. `Pause` é o único contexto que ativa `Gameplay` e `UI` em simultâneo (conforme especificado). Deteção de dispositivo via `InputSystem.onActionChange` (evento `ActionPerformed`), classificando por tipo de device (`Keyboard`/`Mouse` → `KeyboardMouse`) ou por `displayName` do gamepad (Xbox/PlayStation/Steam Deck/genérico).
- 2026-10-19 — Testes: 5 EditMode (`InputServiceContextTests` — mapeamento de contexto → mapas ativos) e 2 PlayMode (`InputServiceDeviceTests`, usando `InputTestFixture` — Move responde a WASD; `DeviceChanged` dispara uma vez ao mudar de teclado para gamepad). Todos a passar.
  - Nota: a `Dispose()` gerada chama `UnityEngine.Object.Destroy()`, que é ilegal em Edit Mode — os testes EditMode não chamam `Dispose()` (só desativam mapas), deixando o `InputActionAsset` ser limpo no próximo domain reload. Os testes PlayMode chamam `Dispose()` normalmente em `TearDown` (Play Mode permite `Destroy`).
- 2026-10-19 — Cena `Tests/Scenes/SCN_InputSandbox` criada (registada no Build Settings) com `InputSandboxHud` (`Aurora.Input`, gated por `#if AURORA_DEV`) — mostra dispositivo atual, última ação disparada e valor de `Move`, via UI Toolkit (`UIDocument` + `SO_InputSandboxPanelSettings`). Verificado em Play Mode no Editor: tecla D dispara `Move` e atualiza o HUD corretamente.
  - Símbolo `AURORA_DEV` adicionado aos Scripting Define Symbols de Standalone e WebGL (antes não existia nenhum). Será removido dos builds de release pela F0-04.
- 2026-10-19 — Pendente: testar a sandbox num build Web real (critério de aceitação 5). O módulo WebGL está instalado, mas ainda não há pipeline de build (F0-04) nem template WebGL customizado — mudar de build target agora é uma operação longa e disruptiva (reimport completo). Decisão (confirmada pelo Dinis): fazer esse teste em conjunto com a F0-04, quando o pipeline de build existir. Por omissão, o pedido de foco do canvas no Web (item 6 da Especificação) também fica para a F0-04, já que depende do template `index.html` do WebGL que essa feature vai criar.
- 2026-10-19 — Dinis testou a `SCN_InputSandbox` no Editor (Play Mode): `Move` responde a WASD, `Last action` mostra o nome da última ação disparada, `Device` muda corretamente. Confirmado "os inputs estão ok" — feature marcada `DONE`.

## Ideias
- Gyro do comando PlayStation para apontar a lanterna (pós-lançamento).
