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
Core ← Audio
Core ← Platform ← Platform.Steam
UI → (Core, Input, Narrative, Gameplay via eventos)
```
`Core` não depende de nada do projeto. `UI` nunca é referenciado por gameplay.

---

## 5. Arquitetura

### 5.1 Arranque
- `Boot.unity` contém `GameBootstrap`, que cria e regista os serviços por ordem e depois carrega `MainMenu` aditivamente.
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
| `RoomDefinition` | Sala: ligações, spawn points, se tem camada do passado, `LevelFlowDefinition` associado (§5.6). |
| `GuestBookEntry` | Entradas do livro de hóspedes (desbloqueáveis, colecionáveis). |
| `EnemyDefinition` | Inimigo genérico: id, nome, sprites/Animator Controller, nível de dificuldade, objeto de identidade (prop visual ligado à revelação da lanterna — nunca uma arma de combate), parâmetros base de comportamento (velocidade, deteção — hooks para a IA da Fase 3, ainda sem lógica). |
| `BossDefinition` | Estende `EnemyDefinition`: número de fases, dados por fase (comportamento, puzzle da lanterna, condição de transição de fase), `MemoryDefinition` recuperada ao resolver, `GuestBookEntry` associado. |
| `TrapDefinition` | Obstáculo/armadilha colocável numa sala: tipo, gatilho, intensidade (narrativa — sugestão de perigo, nunca dano real), referências VFX/SFX, cooldown. |
| `CutsceneDefinition` | Referência a um Timeline asset + condições de disparo. |
| `MusicCueDefinition` / `SfxCueDefinition` / `VfxCueDefinition` | Referências a clips/prefabs de áudio e partículas + parâmetros (volume, loop, etc.); pensados para o `IAudioService` (F4-03). |
| `NarrativeBeatDefinition` | Fino: referencia um `DialogueGraph` existente + legendas; usado como beat de um `LevelFlowDefinition`. |
| `LevelFlowDefinition` | Lista ordenada de beats (`LevelBeatType` + as *Definition concretas de cada um) — ver §5.6. |

Todos os SO têm `id` estável (string) usado no save. **Nunca** gravar referências diretas a assets.

### 5.5 Flags do mundo
`WorldFlags` = dicionário `string → int`. Convenção de chaves: `ala.objeto.estado` (ex.: `quartos.governanta.libertada`). Constantes geradas em `FlagKeys.cs` por ferramenta de Editor a partir de um SO de registo.

### 5.6 Fluxos de nível e gerador

Cada ala do hotel conta sempre a **mesma história fixa** (ver §1 e a descrição completa do projeto) — este sistema não gera conteúdo ao calhas nem varia entre jogos. É só a forma de **montar** cada ala a partir de peças de dados reutilizáveis, em vez de código/cenas escritos à mão do zero para cada uma.

- `LevelBeatType` (enum): `Cutscene`, `Exploration`, `EnemyEncounter`, `TrapGauntlet`, `Maze`, `NarrativeInteraction`, `MiniBoss`, `BonusLevel`, `Boss`.
- `LevelFlowDefinition` (SO): sequência ordenada de beats; cada beat aponta para a(s) *Definition concreta(s) a usar nesse ponto (ex.: um beat `EnemyEncounter` aponta para uma lista de `EnemyDefinition`). Vários fluxos podem existir como assets reutilizáveis entre alas.
- Cada `RoomDefinition`/ala tem um `LevelFlowDefinition` como propriedade — é aí que se escolhe **qual** sequência de beats essa ala usa.
- `LevelGeneratorWindow` (`Aurora.Editor`, menu **Aurora/Level Generator**): ferramenta de autoria assistida. Dado um `LevelFlowDefinition` + as *Definition concretas escolhidas, monta/scaffolda a cena da ala (prefabs instanciados pela ordem dos beats, triggers de transição entre eles). Corre só no Editor, nunca em runtime — não é um gerador procedimental do jogo em si.

---

## 6. Input (Input System — moderno)

- Um único asset `AuroraInput.inputactions` atribuído como **Project-wide Actions**.
- Geração de classe C# **ativa** (`AuroraInputActions`) para acesso tipado; nunca `FindAction("string")` fora do `InputService`.
- **Action Maps:**
  - `Gameplay`: Move (Vector2), Run (Button/Hold), Crouch, Interact, Lantern (toggle), LanternFocus (hold), UseTool, CycleTool (axis), CallBeatriz, Pause.
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
