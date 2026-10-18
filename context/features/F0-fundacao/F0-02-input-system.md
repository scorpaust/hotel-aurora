# F0-02 — Input System e action maps
**Estado:** TODO · **Depende de:** F0-01 · **Assemblies:** Aurora.Input, Aurora.Core

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
- [ ] Nenhuma referência a `UnityEngine.Input` em todo o projeto (validador da F0-01 verde).
- [ ] Todas as ações têm binding teclado **e** gamepad.
- [ ] `SetContext(Dialogue)` impede Move de disparar; `SetContext(Gameplay)` repõe.
- [ ] Ligar um comando a meio altera `CurrentDevice` e dispara `DeviceChanged` uma única vez.
- [ ] Sandbox funciona no Editor e num build Web de teste.

## Testes
- EditMode: mapeamento de `InputContext` → mapas ativos.
- PlayMode (`InputTestFixture`): simular teclado e gamepad, verificar `Move` e `DeviceChanged`.

## Registo

## Ideias
- Gyro do comando PlayStation para apontar a lanterna (pós-lançamento).
