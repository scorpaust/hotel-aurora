# Code Spec — Hotel Aurora

> Fonte de verdade técnica. Se o código contradiz este ficheiro, ou o código está errado ou este ficheiro tem de ser atualizado **na mesma alteração**.

---

## 1. O jogo em resumo (o que o código tem de servir)

| Elemento | Decisão |
|---|---|
| Género | Aventura narrativa 2D de terror leve, exploração + puzzles + perseguições. Sem combate direto. |
| Vista | Lateral (side-view), câmara a seguir o jogador, andares do hotel ligados por escadas, elevadores e passagens. |
| Protagonista | **Tomás** (17). Anda, corre, agacha-se, esconde-se, sobe escadas, interage, usa ferramentas. |
| Aliada | **Beatriz** (fantasma). Companheira que segue o jogador, comenta, indica atalhos, abre passagens "fantasma". |
| Mecânica central | **Lanterna do pai** — luz 2D que revela a *camada do passado*: objetos, passagens e o rosto humano dos monstros. |
| Ferramentas | Mochila de ferramentas (chave de fendas, alicate, fita isoladora…) para **desmontar/reparar** mecanismos. |
| Progressão | Cada ala tem um "responsável" (boss-puzzle). Vencer = descobrir quem foi e recuperar uma **Memória**. Alguns podem ser libertados. |
| Ameaça constante | **O Hotel vivo**: escadas que se dobram, elevadores que esmagam, espelhos dessincronizados, quartos que se desfazem. |
| Alas / bosses | Receção (tutorial) → Quartos (Governanta) → Salão de Banquetes (Chef) → Piscina (Nadador do Fundo) → Salão de Festas (Noivos) → Átrio/Bagageira (Porteiro) → Último Andar (Pai) → Coração (Cozinha do sol) → Fuga do Aurora em colapso. |
| Final | Devolver a memória da filha do fundador. Epílogo com a chave de latão. |
| Duração-alvo | 4–6 h. |
| Idiomas | PT-PT (original) e EN. Preparado para mais. |

---

## 2. Plataformas e limites

| Alvo | Notas |
|---|---|
| **Web** | WebGL2 (predefinido). WebGPU só como opção futura, nunca obrigatório. Build ≤ 60 MB comprimido no carregamento inicial (conteúdo extra via Addressables). Memória ≤ 1 GB. Sem threads. Teclado+rato e gamepad (Gamepad API). |
| **PC Windows** | Build IL2CPP x64. Teclado+rato, Xbox/PlayStation/Steam Deck. 60 fps em hardware integrado modesto. |
| **Steam** | Integração por trás de interface (`IPlatformServices`), ativa só com `STEAM_BUILD`. Achievements, Cloud Save, Rich Presence, Steam Input via Input System. |

Defines de compilação:
- `UNITY_WEBGL` (automático) — caminhos específicos do browser.
- `STEAM_BUILD` — ativa Steamworks. Nunca presente em builds Web.
- `AURORA_DEV` — cheats, consola de debug, saltar níveis. Nunca em release.

---

## 3. Stack e pacotes

**Editor:** Unity **6000.6.x (6000.6.6f1)**. Não subir de versão sem decisão explícita.

| Pacote | Uso |
|---|---|
| Universal RP (2D Renderer) | Render 2D, **Light2D** (lanterna, ambientes), **Shadow Caster 2D**, Post-processing (vinheta, grão, aberração). |
| Input System (1.2x) | **Único** sistema de input. Project-wide Actions. `Active Input Handling = Input System Package (New)`. |
| Cinemachine 3 | Câmara: seguir jogador, confiner por sala, shake, mudanças de enquadramento nos bosses. |
| UI Toolkit | Toda a UI de jogo e menus (UXML + USS). uGUI só se for inevitável e justificado. |
| Localization | Strings e assets localizados (PT-PT, EN). Nenhum texto visível hard-coded. |
| Addressables | Carregamento por ala/cena; reduz download inicial no Web. |
| 2D Animation + PSD Importer | Personagens com rig de ossos (Tomás, Beatriz, bosses). |
| 2D Tilemap + Tilemap Extras | Construção das salas. Rule Tiles para paredes/chão. |
| Sprite Shape | Elementos orgânicos (piscina, tecidos, escadas tortas). |
| Timeline | Cutscenes e sequências scriptadas. |
| Test Framework | EditMode + PlayMode. |
| Newtonsoft JSON (com.unity.nuget.newtonsoft-json) | Serialização do save. |
| Steamworks.NET | Só em `STEAM_BUILD`, dentro de `Aurora.Platform.Steam`. |

**Proibido:** UnityEngine.Input, Resources/ para conteúdo novo (usar Addressables), `Find*` em runtime fora do bootstrap, `SendMessage`, singletons estáticos com estado de jogo (ver §5.3).

---

## 4. Estrutura de pastas

```
Assets/
  _Project/
    Art/            (Sprites, Tilesets, Animations, Materials, VFX)
    Audio/          (Music, SFX, Ambience, Mixers)
    Data/           (ScriptableObjects: Items, Memories, Dialogues, Rooms, Events)
    Input/          AuroraInput.inputactions  ← Project-wide
    Localization/   (String Tables, Asset Tables)
    Prefabs/
    Scenes/
      Boot.unity          (sempre carregada: serviços)
      MainMenu.unity
      Wings/              (uma cena aditiva por ala; salas como sub-cenas ou prefabs)
    Scripts/
      Core/           Aurora.Core.asmdef
      Input/          Aurora.Input.asmdef
      Gameplay/       Aurora.Gameplay.asmdef
      Hotel/          Aurora.Hotel.asmdef        (eventos do hotel vivo)
      Narrative/      Aurora.Narrative.asmdef    (diálogo, memórias, livro de hóspedes)
      UI/             Aurora.UI.asmdef
      Audio/          Aurora.Audio.asmdef
      Persistence/    Aurora.Persistence.asmdef
      Platform/       Aurora.Platform.asmdef  (+ Steam/ Aurora.Platform.Steam.asmdef, define STEAM_BUILD)
      Editor/         Aurora.Editor.asmdef    (só Editor: ferramentas, validadores, build scripts)
    UI/             (UXML, USS, Themes, Fonts)
  Tests/
    EditMode/       Aurora.Tests.EditMode.asmdef
    PlayMode/       Aurora.Tests.PlayMode.asmdef
```

Dependências entre assemblies (só neste sentido):

```
Core ← Input ← Gameplay ← Hotel
Core ← Persistence
Core ← Narrative ← Gameplay
Core ← Narrative ← Hotel
Core ← Audio ← Hotel
Core ← Platform ← Platform.Steam
UI → (Core, Input, Narrative, Gameplay via eventos)
```
`Core` não depende de nada do projeto. `UI` nunca é referenciado por gameplay. `Hotel` pode referenciar `Narrative` e `Audio` (bosses/armadilhas apontam para memórias, diálogos e cues de áudio/vfx — ver §5.4/§5.6), mas `Narrative` e `Audio` nunca referenciam `Hotel`. Como `Hotel` depende de `Gameplay` e não o inverso, qualquer coisa que o `Gameplay` precise de acionar (ex.: uma porta/escada entre salas) tem de viver no próprio `Gameplay` e falar com o `Hotel` só através de tipos partilhados em `Core` (ver §5.7) — nunca por referência direta. `Editor` (ferramentas/validadores/build scripts, nunca entra num build de jogador) pode referenciar qualquer assembly de domínio de que precise — atualmente `Core`, `Hotel`, `Gameplay`, `UI` e `Narrative`, para o `ContentDefinitionValidator`/`LevelFlowValidator`/`LevelGeneratorWindow` (§5.6) e o `RoomPlaygroundBuilder`/`AddressablesSetup` (§5.7).

---

## 5. Arquitetura

### 5.1 Arranque
- `Boot.unity` contém `GameBootstrap`, que cria e regista os serviços por ordem e depois carrega `MainMenu` aditivamente.
- O mesmo `GameObject` tem também `PlatformBootstrap` (`Aurora.Platform`), que regista `IPlatformServices` em `Start()` — nunca em `Awake()`, para correr sempre depois do `Services.Reset()` do `GameBootstrap`, já que `Aurora.Input` não referencia `Aurora.Platform` (ver §4) e por isso não pode registar este serviço diretamente.
- Em Editor, ao dar Play em qualquer cena, um `[RuntimeInitializeOnLoadMethod]` garante que `Boot` é carregada primeiro (play-from-anywhere).

### 5.2 Serviços
Registo simples em `Aurora.Core.Services` (service locator explícito, sem framework de DI):

| Serviço | Responsabilidade |
|---|---|
| `IInputService` | Expõe ações do Input System, troca de action maps, deteção de dispositivo, rebinding. |
| `IGameStateService` | Máquina de estados global: Boot, MainMenu, Playing, Paused, Dialogue, Cutscene, Chase, GameOver. |
| `ISaveService` | Guardar/carregar slots; abstrai disco vs IndexedDB vs Steam Cloud. |
| `ISceneService` | Carregamento aditivo via Addressables, ecrãs de transição. |
| `IAudioService` | Música por camadas, SFX, ambiente, snapshots do mixer. |
| `ILocalizationService` | Wrapper fino sobre o pacote Localization. |
| `IPlatformServices` | Achievements, cloud, presence. Implementações: `NullPlatform`, `WebPlatform`, `SteamPlatform`. |
| `IWorldFlagsService` | Flags do mundo (§5.5). Implementação atual (`WorldFlagsService`) é só em memória; a F1-06 adiciona persistência sem trocar a interface. |
| `IRoomTransitionService` | Orquestra uma transição entre salas/alas (§5.7). Implementado por `RoomTransitionController` (Hotel). |

### 5.3 Comunicação
- **Eventos via ScriptableObject** (`GameEvent`, `GameEvent<T>`) para comunicação entre sistemas desacoplados (ex.: `OnMemoryRecovered`, `OnHotelEventTriggered`).
- Referências diretas por `[SerializeField]` dentro do mesmo prefab/sistema.
- Estado do jogo vive em objetos serializáveis (`GameState`, `PlayerState`, `WorldFlags`), não em campos estáticos.

### 5.4 Dados (ScriptableObjects)
| SO | Conteúdo |
|---|---|
| `ItemDefinition` | Ferramentas e objetos-chave (id, nome localizado, ícone, se é ferramenta). |
| `MemoryDefinition` | Memória recuperável (id, ala, fragmentos, cutscene associada, efeito na história). |
| `DialogueGraph` | Nós de diálogo, condições sobre flags, efeitos. |
| `HotelEventDefinition` | Evento do hotel vivo *ambiente* (tipo, gatilho, intensidade, cooldown) — não confundir com `TrapDefinition` abaixo, que é um obstáculo colocado numa sala. |
| `RoomDefinition` | Sala: id, nome, se tem camada do passado, `LevelFlowDefinition` associado (§5.6). Ligações e spawn points **não** vivem aqui — são do componente `Room`/`RoomConnection` em cena (§5.7); este SO é só metadata partilhável. |
| `GuestBookEntry` | Entradas do livro de hóspedes (desbloqueáveis, colecionáveis). |
| `EnemyDefinition` | Inimigo genérico: id, nome, sprites/Animator Controller, nível de dificuldade, objeto de identidade (prop visual ligado à revelação da lanterna — nunca uma arma de combate), parâmetros base de comportamento (velocidade, deteção — hooks para a IA da Fase 3, ainda sem lógica). |
| `BossDefinition` | Estende `EnemyDefinition`: número de fases, dados por fase (comportamento, puzzle da lanterna, condição de transição de fase), `MemoryDefinition` recuperada ao resolver, `GuestBookEntry` associado. |
| `TrapDefinition` | Obstáculo/armadilha colocável numa sala: tipo, gatilho, intensidade (narrativa — sugestão de perigo, nunca dano real), referências VFX/SFX, cooldown. |
| `CutsceneDefinition` | Referência a um Timeline asset + condições de disparo. |
| `MusicCueDefinition` / `SfxCueDefinition` / `VfxCueDefinition` | Referências a clips/prefabs de áudio e partículas + parâmetros (volume, loop, etc.); pensados para o `IAudioService` (F4-03). |
| `NarrativeBeatDefinition` | Fino: referencia um `DialogueGraph` existente + legendas; usado como beat de um `LevelFlowDefinition`. |
| `LevelFlowDefinition` | Lista ordenada de `LevelBeat` (`LevelBeatType` + as *Definition concretas de cada um) — ver §5.6. |

Todos os SO têm `id` estável (string) usado no save. **Nunca** gravar referências diretas a assets.

### 5.5 Flags do mundo
`WorldFlags` = dicionário `string → int`, acedido via `IWorldFlagsService` (`Services.Get<IWorldFlagsService>()`). Convenção de chaves: `ala.objeto.estado` (ex.: `quartos.governanta.libertada`). A implementação atual (`WorldFlagsService`, F1-02) é só em memória, sem persistência — a F1-06 envolve-a num serviço com save/load sem trocar a interface. Constantes geradas em `FlagKeys.cs` por ferramenta de Editor a partir de um SO de registo (ainda `TODO`).

### 5.6 Fluxos de nível e gerador

Cada ala do hotel conta sempre a **mesma história fixa** (ver §1 e a descrição completa do projeto) — este sistema não gera conteúdo ao calhas nem varia entre jogos. É só a forma de **montar** cada ala a partir de peças de dados reutilizáveis, em vez de código/cenas escritos à mão do zero para cada uma.

- `LevelBeatType` (enum): `Cutscene`, `Exploration`, `EnemyEncounter`, `TrapGauntlet`, `Maze`, `NarrativeInteraction`, `MiniBoss`, `BonusLevel`, `Boss`.
- `LevelFlowDefinition` (SO): sequência ordenada de beats; cada beat aponta para a(s) *Definition concreta(s) a usar nesse ponto (ex.: um beat `EnemyEncounter` aponta para uma lista de `EnemyDefinition`). Vários fluxos podem existir como assets reutilizáveis entre alas.
- Cada `RoomDefinition`/ala tem um `LevelFlowDefinition` como propriedade — é aí que se escolhe **qual** sequência de beats essa ala usa.
- `LevelGeneratorWindow` (`Aurora.Editor`, menu **Aurora/Level Generator**): ferramenta de autoria assistida. Escolhido um `LevelFlowDefinition` existente (validado primeiro por `LevelFlowValidator`), instancia um placeholder por beat na cena ativa, na ordem certa, espaçados e coloridos por tipo — até existir conteúdo real (prefabs de triggers/encontros) para cada tipo de beat. Repetir a geração substitui a tentativa anterior (`LevelFlow_Generated`), nunca acumula lixo. Corre só no Editor, nunca em runtime — não é um gerador procedimental do jogo em si.

### 5.7 Salas e transições (F1-02)

- `Room` (`Aurora.Hotel`, componente em cena): envolve um `RoomDefinition`, expõe o `Collider2D` usado como `CinemachineConfiner2D.BoundingShape2D` e resolve os `RoomSpawnPoint` filhos por id (scan ao vivo, não cacheado — só corre no momento da transição). Regista-se por id num registo estático próprio (`Room.TryGetById`), como o `Services`.
- `RoomConnection` (**`Aurora.Gameplay`**, não `Hotel` — ver §4): porta/escada/elevador/passagem. Implementa `IInteractable` (§5.8) desde a F1-03 — é o `PlayerInteractor` genérico que a deteta e aciona, não mais um trigger ad-hoc tratado à parte no `PlayerController`. Carrega um `RoomConnectionType`, um `RoomDestination` (roomId + spawnId + cena opcional para mudar de ala) e, opcionalmente, uma condição por `WorldFlags` com um `RoomDestination` alternativo.
- `RoomTransitionRequest`/`RoomDestination`/`RoomConnectionType`/`IRoomTransitionService` vivem em `Core` — é o vocabulário partilhado que permite ao `RoomConnection` (Gameplay) pedir uma transição sem o `Gameplay` ter de referenciar o `Hotel`.
- `RoomTransitionController` (`Aurora.Hotel`, implementa `IRoomTransitionService`): bloqueia input (`GameState.Transition`) → dispara um `GameEvent` de fade out → resolve o destino (`RoomTransitionResolver`, lógica pura testável) → carrega a ala nova via `ISceneService` se o destino for noutra cena → teleporta o jogador (via `PlayerController.Current`, acessor estático análogo ao `Room.TryGetById`) → troca o `CinemachineConfiner2D` → descarrega a ala antiga se o jogador saiu dela → dispara o `GameEvent` de fade in → `GameState.Playing`.
- Fade: painel UI Toolkit mínimo (`RoomFadePanel`, `Aurora.UI`), ouvido por `GameEventListener`s ligados aos `GameEvent`s `SO_RoomFadeOutRequested`/`SO_RoomFadeInRequested` — mantém o `Hotel` desacoplado do `UI`, como o resto do projeto (§5.3).
- Testado dentro da **única cena de sandbox** (`SCN_PrototypeSandbox`, `rules.md` §6): uma sub-árvore `RoomPlayground` (3 salas + câmara + transições, com chão colorido visível por sala — nunca só colliders invisíveis) construída por `RoomPlaygroundBuilder` (`Aurora.Editor`, menu **Aurora/Level Generator/Build Room Playground (F1-02)**). Este script só apaga/reconstrói a sub-árvore `RoomPlayground`, nunca a cena inteira — o conteúdo da F1-01/F1-08 ao lado fica intocado. `SCN_SandboxAnnex` (cena separada, carregada aditivamente) existe só para testar a mudança de ala (criar/destruir essa cena é o próprio teste); a sua sala fica bem afastada no espaço (x=200) para não sobrepor nada.
- **Importante para builds:** desde que Addressables entrou em jogo, qualquer `BuildRunner.Run()` troca `EditorUserBuildSettings.activeBuildTarget` para o alvo pedido antes de compilar — o Addressables empacota conteúdo para a plataforma ativa no Editor, não para o alvo do `BuildPlayer`. Sem isto, um build para uma plataforma X corrido a seguir a um build para Y fica com os bundles de Y lá dentro.

### 5.8 Interação (F1-03)

- `IInteractable` (`Aurora.Core`): `VerbKey` (chave de localização), `Kind` (`Press`/`Hold`), `HoldDuration`, `Priority`, `CanInteract(InteractionContext)`, `Interact(InteractionContext)`. `InteractionContext` é um `readonly struct` só com o `Actor` (o `GameObject` do jogador).
- `InteractableSelector` (`Aurora.Core`, lógica pura testável): dado um conjunto de candidatos `(IInteractable, distância)`, escolhe o de maior `Priority` e, em empate, o mais perto.
- `PlayerInteractor` (`Aurora.Gameplay`, no mesmo `GameObject` que o `PlayerController`): `Physics2D.OverlapCircle` (buffer fixo, sem alocações) à frente do jogador, no máximo a 10 Hz. Ações: `Interact` (partilhada com `Pushable`) para alvos `Press`; `InteractHold` (ação própria, sem interação do Input System anexada — ver §6) para alvos `Hold`, cujo tempo premido é medido à mão por `Time.deltaTime` contra `IInteractable.HoldDuration` do alvo atual — soltar antes do tempo reinicia o progresso sem chamar `Interact()`.
- `RoomConnection` (§5.7) implementa `IInteractable` diretamente; é o único tipo base que já existia antes da F1-03.
- Tipos base novos, todos em `Aurora.Gameplay`: `Examine` (interação `Press`; não pausa o jogo — só levanta um `GameEventString` opcional com uma chave de texto, para uma futura UI/narrativa mostrar o balão de pensamento do Tomás, `uispec.md` §3.4); `PickUp` (interação `Press`; chama `IInventoryService.AddItem` — interface vazia em `Core` até à F1-05 — e regista uma `WorldFlag`, depois desativa o próprio `GameObject`); `Switch` (interação `Press` ou `Hold` à escolha, liga/desliga ou alterna uma `WorldFlag` e levanta um `GameEvent` opcional — o "mecanismo" genérico atrás de alavancas, botões e armadilhas).
- Prompt de ecrã: `InteractionPromptPanel` (`Aurora.UI`, `UIDocument` próprio, `SO_HUDPanelSettings`): segue `PlayerInteractor.CurrentTarget` em espaço de ecrã via `Camera.WorldToViewportPoint` + `style.translate` (nunca `style.left/top`, para não forçar reflow a cada frame). Resolve a câmara ativa via `ActiveCameraResolver` (`Aurora.Core`, §5.9) — podem existir várias câmaras simultaneamente (a do Boot, a do corredor F1-01, a do `RoomPlayground` F1-02), só a de maior `depth` é a que se vê. Escondido fora de `Playing`/`Chase` (`IGameStateService`).

### 5.9 Lanterna e camada do passado (F1-04)

- `ActiveCameraResolver` (`Aurora.Core`): utilitário partilhado — resolve "a" câmara ativa pela maior `depth` entre `Camera.allCameras`. Extraído da F1-03 (`InteractionPromptPanel`) quando a F1-04 (mira do rato) precisou exatamente da mesma lógica.
- `LanternCone` (`Aurora.Core`, `readonly struct` puro e testável): origem, direção, meio-ângulo e alcance; `Contains(point)` decide se um ponto está dentro — usado tanto para a revelação visual como para a física.
- `LanternHeat` (`Aurora.Core`, classe pura testável): temporizador de sobreaquecimento — aquece com o foco ligado, arrefece sem foco, bloqueia (`IsOverheated`) durante `lockoutSeconds` ao atingir `Heat01 = 1`.
- `LanternController` (`Aurora.Gameplay`, no `PlayerController`): mira (rato segue o cursor convertido para mundo via `ActiveCameraResolver`, stick direito do gamepad, ou a direção a que o jogador está virado sem nenhum dos dois), sempre recortada a ±70° da direção do jogador. Liga/desliga com `Lantern`; foco (`LanternFocus`, hold) estreita o cone, sobe a intensidade e ativa a revelação, com uma transição suave (`Mathf.MoveTowards`, não instantânea). Expõe `Cone`/`IsRevealing`/`EdgeSoftness` para qualquer `PastLayerObject` próximo. Luz: `Light2D` tipo `Point` (o "Spot" da UI do URP 2D) com sombras. Visual: `PFB_HandheldLantern` (`SPR_HandheldLantern.png`, gerado via Unity AI Assistant) como filho do mesmo pivô da luz — roda com a mira, visível sempre que `_isOn` (mesmo sobreaquecida: Tomás continua a segurá-la, só não tira luz dela). **Importante:** a rotação `0°` do `Light2D` aponta para +Y local (cima), não para +X como `Mathf.Atan2(y,x)` assume — confirmado ao vivo forçando valores conhecidos de `rotation.eulerAngles.z` e observando a direção real do cone (`0°→cima, 90°→esquerda, 180°→baixo, 270°→direita`); por isso o ângulo calculado por `Atan2` tem sempre de subtrair 90° antes de ser aplicado como `Quaternion.Euler`, senão o feixe aponta para baixo independentemente do lado para que o jogador está virado. **Posição de descanso do rato:** com o dispositivo `KeyboardMouse`, se a posição do rato no ecrã (`LanternAim`) não se mover mais do que `_mouseIdleThresholdPixels` durante `_mouseIdleTimeout` segundos — o que inclui o cursor estar fora da Game View, que lê como posição fixa em `(0,0)` — a lanterna descansa a apontar na direção em que o Tomás está virado, em vez de seguir a conversão de ecrã-para-mundo de uma posição parada/inválida (que antes produzia um ângulo extremo, visto pelo jogador como "o feixe aponta sempre para baixo").
- `PastLayerObject` (`Aurora.Gameplay`): objeto com uma camada escondida do passado. `RevealMode` (`ShowWhenLit`/`HideWhenLit`/`Swap`) é só convenção de autoria — o shader mistura por conteúdo das texturas, não por este valor (uma textura transparente de um dos lados dá os efeitos Show/Hide). Revelação física opcional (`PhysicalWhenRevealed`): liga o `Collider2D` enquanto `LanternCone.Contains` for verdadeiro, com `graceSeconds` de tolerância a seguir. Revelação permanente opcional: acumula tempo revelado e, ao atingir `secondsToBecomePermanent`, fixa-se e grava uma `WorldFlag` — nessa altura ignora a câmara real e alimenta o shader com um cone "infinito", para nunca mais depender de onde a lanterna aponta.
- **Shader da revelação:** `SG_RevealPast.shadergraph` + `M_RevealPast.mat` (`Assets/_Project/Art/Shaders|Materials`) — Shader Graph URP Sprite Unlit real, autorado no Editor (não gerado por script — ver Registo da feature para o porquê), com propriedades `_ConeOrigin`/`_ConeDirection`/`_ConeHalfAngle`/`_ConeRange`/`_EdgeSoftness`/`_Focus`/`_PresentTex`/`_PastTex`/`_PastTint`. **Importante:** estas propriedades têm de ser escritas por `Material.Set*` em cada instância — `Shader.SetGlobalX` não chega a um material que já tem valor próprio para essa propriedade (todo o Shader Graph serializa um valor por omissão), por isso cada `PastLayerObject` lê `LanternController.Cone`/`IsRevealing`/`EdgeSoftness` e escreve-os no seu próprio clone do material a cada frame, em vez de a `LanternController` os difundir globalmente.
- `IRevealable` (`Aurora.Core`): `OnRevealed(float duration)` — hook para os monstros mostrarem o rosto humano (F5-02). Só a interface existe nesta feature, tal como o `IInventoryService` da F1-03 — a deteção/disparo fica para quem a consumir primeiro.

---

## 6. Input (Input System — moderno)

- Um único asset `AuroraInput.inputactions` atribuído como **Project-wide Actions**.
- Geração de classe C# **ativa** (`AuroraInputActions`) para acesso tipado; nunca `FindAction("string")` fora do `InputService`.
- **Action Maps:**
  - `Gameplay`: Move (Vector2), Run (Button/Hold), Crouch, Interact, **InteractHold** (mesmos bindings do Interact, sem interação do Input System anexada — F1-03; `PlayerInteractor` mede o tempo premido à mão contra `IInteractable.HoldDuration` por alvo. A API documentada para isto seria a interação *Hold* nativa com `ApplyParameterOverride("hold:duration", ...)`, mas confirmou-se ao vivo, nesta versão do Input System, que o override fica registado mas não altera o tempo real de disparo — daí a medição manual. Mantém-se uma ação separada do `Interact` partilhado com `Pushable`/`RoomConnection`, só para isolamento de responsabilidade, não por causa da interação), Lantern (toggle), LanternFocus (hold), **LanternAim** (Vector2 — F1-04; posição do rato ou stick direito do gamepad, interpretado de forma diferente conforme `IInputService.CurrentDevice`, nunca os dois ao mesmo tempo), UseTool, CycleTool (axis), CallBeatriz, Pause.
  - `UI`: mantém os nomes e tipos do mapa predefinido (exigido pelo UI Toolkit). Acrescentar só `Back` alternativo e `TabLeft/TabRight`.
  - `Dialogue`: Advance, Skip, ChooseUp/Down.
  - `Cutscene`: Skip (hold).
- **Control Schemes:** `KeyboardMouse`, `Gamepad`. Steam Deck/Steam Input entram como Gamepad.
- Interações: usar `Hold`, `Tap`, `MultiTap` do Input System em vez de temporizadores manuais.
- **Rebinding** em runtime com `PerformInteractiveRebinding`; overrides gravados como JSON (`SaveBindingOverridesAsJson`) nas definições.
- **Ícones de prompt** dependentes do dispositivo ativo (teclado, Xbox, PlayStation, Deck) via `InputUser`/`onActionChange`.
- Rumble (`Gamepad.SetMotorSpeeds`) com opção para desligar; ignorado no Web se não suportado.
- Web: pedir foco do canvas no primeiro clique; não depender de `Escape` (o browser usa-o para sair do fullscreen) — `Pause` também em `P`/`Tab`.

---

## 7. Persistência

- Formato: JSON (Newtonsoft) com `saveVersion` e migrações.
- Conteúdo: slot, timestamp, ala/sala atual, ponto de checkpoint, inventário (ids), memórias (ids), `WorldFlags`, estado de Beatriz, tempo jogado.
- **Checkpoints automáticos** ao entrar em salas marcadas e após cada confronto. Sem save manual em qualquer sítio (mantém tensão); 3 slots.
- PC: `Application.persistentDataPath`. Steam: mesmo ficheiro + Steam Auto-Cloud.
- Web: `persistentDataPath` (IndexedDB). Escrever de forma assíncrona, confirmar que o ficheiro persiste após recarregar a página (teste manual obrigatório na feature de save).
- Definições (áudio, idioma, bindings, acessibilidade) num ficheiro separado do progresso.

---

## 8. Performance

- Alvo: 60 fps; Web aceita 30 fps estáveis em portáteis fracos.
- Zero alocações por frame em código de gameplay (verificar com Profiler; `GC.Alloc` em `Update` = bug).
- Pooling para projéteis/partículas/objetos que aparecem e desaparecem.
- Light2D: máximo de ~8 luzes ativas visíveis; Shadow Casters só onde contam.
- Sprite Atlas por ala; compressão ASTC/DXT conforme plataforma; áudio comprimido Vorbis, música em streaming.
- Usar `Awaitable` (Unity 6) para fluxos assíncronos. Nada de `Task.Run`.

---

## 9. Testes

- **EditMode:** lógica pura (inventário, flags, save/migrações, condições de diálogo, máquina de estados).
- **PlayMode:** movimento do jogador, interação, lanterna a revelar camada do passado, transição de salas, um fluxo de checkpoint.
- Simular input em testes com `InputTestFixture`.
- Cada feature indica os testes mínimos nos critérios de aceitação.
