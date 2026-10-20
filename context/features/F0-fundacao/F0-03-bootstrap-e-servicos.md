# F0-03 — Bootstrap, serviços e estado de jogo
**Estado:** DONE · **Depende de:** F0-02 · **Assemblies:** Aurora.Core, Aurora.Input

## Objetivo
Arranque previsível: a cena `Boot` cria os serviços, a máquina de estados global controla o fluxo, e qualquer cena pode ser testada com Play no Editor.

## Contexto na história
Nenhum direto.

## Âmbito
**Inclui:** `Services` (registo), `GameBootstrap`, `IGameStateService` + estados, `GameEvent`/`GameEvent<T>` em ScriptableObject, `ISceneService` mínimo (carregar/descarregar aditivo), play-from-anywhere.
**Não inclui:** save (F1-06), áudio (F4-03), Addressables por ala (F1-02 usa o mínimo).

## Especificação
1. `Aurora.Core.Services`: `Register<T>(T)`, `Get<T>()`, `TryGet<T>()`, `Reset()` (chamado em `SubsystemRegistration` por causa do Domain Reload desligado).
2. `GameBootstrap` em `SCN_Boot`: regista `InputService`, `GameStateService`, `SceneService`; depois carrega `SCN_MainMenu` (por agora uma cena vazia com texto).
3. Play-from-anywhere: se a cena ativa não for `Boot`, carregar `Boot` aditivamente primeiro e manter a cena atual como alvo (`GameStateService` arranca em `Playing`).
4. `GameStateService`: estados `Boot, MainMenu, Playing, Paused, Dialogue, Cutscene, Chase, GameOver`. Transições validadas por tabela; transição inválida faz `Debug.LogError` e é ignorada. Cada estado define o `InputContext` e o `Time.timeScale` (Paused = 0).
5. `GameEvent` (SO) com `Raise()`, `Register/Unregister`; versão genérica `GameEvent<T>`. Listener `GameEventListener` (MonoBehaviour) para ligações no Inspector.
6. `SceneService`: `LoadAdditiveAsync(string address)`, `UnloadAsync`, evento de progresso. Usa `Awaitable`. Por agora pode usar `SceneManager`; Addressables entra em F1-02.

## Critérios de aceitação
- [x] Play em `Boot` leva ao MainMenu placeholder. (Nome real da cena é `Boot.unity`, não `SCN_Boot` — ver Registo.)
- [x] Play numa cena de teste qualquer carrega `Boot` e fica jogável nessa cena.
- [x] `Paused` coloca `timeScale = 0` e ativa o mapa UI; sair repõe.
- [x] Transição inválida (ex.: `Boot → Chase`) é rejeitada com erro claro.
- [x] Sem fugas de estado entre Plays com Domain Reload desligado.

## Testes
- EditMode: tabela de transições, `Services.Reset`, `GameEvent` notifica listeners pela ordem de registo.
- PlayMode: arranque a partir de uma cena de teste.

## Registo
- 2026-10-19 — Decisão (confirmada pelo Dinis): cenas de infraestrutura seguem `codespec.md` §4 à letra — `Boot.unity`/`MainMenu.unity`, sem prefixo `SCN_` (esse fica reservado a cenas de ala, por `rules.md` §6). O texto do critério de aceitação acima ("SCN_Boot") era só uma referência informal.
- 2026-10-19 — `Services` (Aurora.Core): registo estático `Register<T>/Get<T>/TryGet<T>/Reset()`. `Reset()` corre via `[RuntimeInitializeOnLoadMethod(SubsystemRegistration)]`, mas isso só cobre recompilações — com Domain Reload *e* Scene Reload desligados (confirmado nas Editor Settings deste projeto), esse hook não garante reset em todos os Plays seguintes. Por isso o `GameBootstrap.Awake()` chama `Services.Reset()` outra vez, de forma defensiva, logo no arranque — `Awake()` corre sempre ao entrar em Play, independentemente dessas definições. Confirmado com 2 Plays seguidos sem recompilar: nenhuma fuga, nenhuma cena `Boot` duplicada.
- 2026-10-19 — Play-from-anywhere implementado em `Aurora.Editor.PlayFromAnywhereBootstrapper` (não em `Aurora.Input`), para respeitar `rules.md` §3 ("`#if UNITY_EDITOR` só em `Aurora.Editor`"). Comunica com o `GameBootstrap` via uma flag simples em `Aurora.Core.BootstrapFlags` (único acrescento ao Core para isto).
- 2026-10-19 — `GameStateService`: tabela de transições e mapeamento `GameState → InputContext` são decisões de design não detalhadas na spec original; documentadas no código. `Paused` é o único estado que usa o contexto `Pause` (Gameplay+UI juntos, como a F0-02 já suportava). `GameOver`/`Boot`/`MainMenu` usam `InputContext.UI` por omissão (nada de jogável ainda).
- 2026-10-19 — Bug de tooling encontrado e resolvido: `GameEvent.cs` e `GameEventGeneric.cs` (os dois únicos scripts `ScriptableObject` novos desta feature) ficaram "invisíveis" para o compilador do Unity (ausentes de `CompilationPipeline.GetAssemblies(...).sourceFiles`) depois de escritos via ferramenta externa — nem `AssetDatabase.Refresh()`, nem reimport forçado, nem `RequestScriptCompilation(CleanBuildCache)` resolveram. Só apagar os dois ficheiros + `.meta` e recriá-los do zero corrigiu. Causa exata não identificada (não reproduzido com um `ScriptableObject` de teste mínimo); fica registado caso aconteça outra vez.
- 2026-10-19 — Testado manualmente: Play em `Boot` → `MainMenu` placeholder visível ("Hotel Aurora"); play-from-anywhere a partir de `SCN_InputSandbox` → `Boot` carregado aditivamente, estado direto `Playing`, cena ativa mantida; transição inválida `Paused → Chase` rejeitada com erro claro na consola.
- 2026-10-20 — Dinis reportou o aviso "Display 1 No cameras rendering" ao testar. Nem `Boot` nem `MainMenu` tinham Camera (a UI Toolkit em Screen Space Overlay não precisa de câmara para desenhar, por isso o texto já aparecia, mas o aviso ficava). Adicionada uma `MainCamera` à cena `Boot` (é a cena sempre carregada), `Clear Flags = Solid Color` com `--aurora-ink` (`#1B1714`, `uispec.md` §1) em vez do preto/skybox por omissão. Confirmado: aviso desaparece, fundo correto.
- 2026-10-20 — Dinis reportou também o aviso de `UI/Navigate` (ver Registo da F0-02, onde o `AuroraInput.inputactions` vive). Corrigido lá; 26/26 testes continuam a passar.
- 2026-10-20 — Dinis confirmou "Tudo okapa" depois das duas correções. F0-03 marcada `DONE`.

## Ideias
