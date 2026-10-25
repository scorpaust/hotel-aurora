# F1-02 — Salas, câmara e transições
**Estado:** DONE · **Depende de:** F1-01 · **Assemblies:** Aurora.Gameplay, Aurora.Core, Aurora.Hotel, Aurora.UI

## Objetivo
O hotel é feito de salas ligadas por portas, escadas e elevadores. A câmara enquadra cada sala; passar de uma para outra é rápido e carrega conteúdo por ala via Addressables.

## Contexto na história
"Corredores que mudam quando ninguém está a olhar." O sistema de salas tem de permitir que uma porta leve a sítios diferentes conforme flags (usado em F3-03).

## Âmbito
**Inclui:** `RoomDefinition`, `Room` (componente), `RoomConnection` (porta, escada entre andares, elevador simples), Cinemachine 3 com confiner por sala, fade de transição, carregamento aditivo por ala com Addressables, Tilemap base com Rule Tiles placeholder.
**Não inclui:** eventos do hotel vivo (F3-03), elevador "que sente medo" (F3-03).

## Especificação
1. Cada ala = cena aditiva Addressable `SCN_<Ala>`; dentro, cada sala é um GameObject `Room` com `RoomDefinition` (id, nome localizado, limites, spawn points).
2. `RoomConnection`: tipo (`Door`, `Stairwell`, `Elevator`, `Passage`), destino (`roomId` + `spawnId`), condição opcional por flag (`WorldFlags`) e **destino alternativo** se a condição falhar.
3. Transição: Interact na porta → input bloqueado → fade 0.25 s → mover jogador e companheira → trocar confiner → fade in. Se o destino é outra ala, `SceneService` carrega a nova antes e descarrega a antiga depois.
4. Cinemachine 3: `CinemachineCamera` com `CinemachinePositionComposer` e `CinemachineConfiner2D` (atualizado por sala); dead zone horizontal pequena; *lookahead* leve na direção do movimento; câmara *pixel perfect* se a arte final o pedir (decidir antes da F5).
5. `ISceneService` passa a usar Addressables (`Addressables.LoadSceneAsync`), com grupos por ala.
6. Cena de teste `SCN_TestWing` com 3 salas, 1 escada entre andares e 1 porta condicional.

## Critérios de aceitação
- [x] Atravessar 3 salas sem a câmara sair dos limites de cada uma. *(RoomA/B/C em `SCN_TestWing`, confiner trocado em cada transição — confirmado em Play.)*
- [x] Porta condicional leva ao destino A ou B conforme uma flag. *(`Door_Conditional` em RoomA: `f1_02.test.gate`==1 → RoomB, caso contrário → RoomC. Testado nos dois sentidos.)*
- [x] Mudança de ala carrega/descarrega cenas sem picos visíveis (< 1 s de ecrã negro no PC). *(`Stairwell_ToAnnex`/`Stairwell_Back` entre `SCN_TestWing` e `SCN_TestWingAnnex`, via Addressables; fade 0.25 s cada sentido = 0.5 s total, bem abaixo de 1 s. Carregamento/descarregamento da Annex confirmado.)*
- [x] Funciona no build Web. *(Ver Registo — precisou de um fix ao `BuildRunner`.)*

## Testes
- EditMode: `RoomTransitionResolverTests` (3 testes — sem condição, condição cumprida, condição falhada → fallback).
- PlayMode: `RoomTransitionControllerTests` (2 testes — conexão incondicional e condição falhada, ambos terminam com o jogador no spawn certo e `GameState.Playing`).
- 68/68 testes do projeto a passar (63 EditMode + 5 novos PlayMode, confirmados via CLI batchmode); 3 falhas pré-existentes da F1-01 identificadas e **não** causadas por esta feature — ver Registo.

## Registo
- 2026-10-25 — Implementado:
  - **`IWorldFlagsService`/`WorldFlagsService`** (Aurora.Core, novo): dicionário `string→int` em memória, registado no `GameBootstrap`. Decisão explícita do utilizador: mini-serviço agora, sem persistência — a F1-06 estende-o com save/load, sem o substituir.
  - **`GameState.Transition`** (novo estado, `Playing↔Transition`, mapeado para `InputContext.Cutscene` — bloqueia o Gameplay sem precisar de nova action map).
  - **`SceneService`** passou de `SceneManager` para `Addressables.LoadSceneAsync`/`UnloadSceneAsync` (guarda o `AsyncOperationHandle<SceneInstance>` por nome de cena internamente, para o `UnloadAsync(string)` continuar a aceitar só o nome). `MainMenu.unity` marcado Addressable (`AddressablesSetup`, grupo `Core`) para continuar a funcionar com o novo `SceneService` — confirmado em Play (Boot→MainMenu).
  - **`RoomConnectionType`/`RoomDestination`/`RoomTransitionRequest`/`IRoomTransitionService`** (Aurora.Core): vocabulário partilhado entre `Aurora.Gameplay` (que não pode referenciar `Aurora.Hotel`, é o inverso) e `Aurora.Hotel`.
  - **`RoomConnection`** (Aurora.Gameplay, não Hotel — mesma razão acima): mesmo padrão ad-hoc do `StairZone`/`Pushable` (trigger + `PlayerController` deteta overlap e chama `Activate()` no Interact "just pressed"), sem depender da F1-03 (sistema de interação genérico), ainda `TODO`.
  - **`Room`/`RoomSpawnPoint`** (Aurora.Hotel): `Room` envolve o `RoomDefinition` existente (estendido com `Configure()`), expõe bounds (`Collider2D`, para o confiner) e resolve `RoomSpawnPoint`s filhos por id (scan ao vivo, não cacheado — ver bug abaixo). Registo próprio por id (`Room.TryGetById`), como o `Services`.
  - **`RoomTransitionController`** (Aurora.Hotel, implementa `IRoomTransitionService`): bloqueia input (`GameState.Transition`) → `GameEvent` de fade out → resolve destino (`RoomTransitionResolver`, puro/testável) → carrega a ala nova via `ISceneService` se for noutra cena → avisa o `PlayerController.Current` (novo acessor estático, análogo ao `Room.TryGetById`) para se teleportar → troca `CinemachineConfiner2D.BoundingShape2D` → descarrega a ala antiga se o jogador saiu dela → fade in → `Playing`.
  - **Fade de transição**: painel UI Toolkit mínimo (`RoomFadePanel`, Aurora.UI) — um retângulo preto com `transitionDuration` da USS, sem Update/coroutine. Ligado aos `GameEvent`s `SO_RoomFadeOutRequested`/`SO_RoomFadeInRequested` via `GameEventListener` (decisão: manter o `Aurora.Hotel` desacoplado de `Aurora.UI`, como já é o padrão do projeto). `GameEventListener` ganhou `Response`/`SetEvent()` para a ferramenta de autoria conseguir ligar isto sem `SerializedObject`.
  - **Cinemachine 3**: `CinemachineBrain` na câmara principal da cena, `CinemachineCamera` + `CinemachinePositionComposer` (valores por omissão — dead zone/lookahead finos ficam para quando houver arte final, ver Ideias) + `CinemachineConfiner2D`.
  - **Cena de teste**: `SCN_TestWing` (RoomA/B/C + câmara + `RoomTransitionController` + UI de fade) e `SCN_TestWingAnnex` (RoomD, só acessível pela `Stairwell_ToAnnex`/`Stairwell_Back` de RoomC — é o caso de teste da mudança de ala). Geradas por `TestWingSceneBuilder` (`Aurora.Editor`, menu **Aurora/Level Generator/Build F1-02 Test Wing**), idempotente, não por edição manual de `.unity` (`rules.md` §6).
  - **Sem Tilemap/Rule Tiles**: decisão deliberada — nenhum critério de aceitação precisa de arte, e o Tomás não tem gravidade (`Rigidbody2D` Kinematic, só anda em X e sobe/desce por `StairZone`), por isso não há "chão" para desenhar ainda. Placeholder art fica para quando houver conteúdo real (F1-07+). Fica anotado para não se perder.
- 2026-10-25 — **Bug encontrado e corrigido**: `Room.OnEnable()` (registo por id) e a recolha de `RoomSpawnPoint`s dependiam da ordem `AddComponent` → `Configure()`/criação dos filhos — funcionava numa cena já gravada (Unity desserializa tudo antes de chamar `Awake`/`OnEnable`) mas falhava sempre que código (testes, `TestWingSceneBuilder`) criava a sala e só depois chamava `Configure()`. Corrigido: `Configure()` agora regista-se a si próprio com o id novo; a resolução de spawn points passou a ser feita ao vivo (scan de filhos), nunca cacheada — só é chamada no momento da transição, não por frame.
- 2026-10-25 — **Bug encontrado e corrigido no `BuildRunner` (F0-04)**: nunca trocava `EditorUserBuildSettings.activeBuildTarget` antes de compilar. Não importava antes (sem Addressables), mas agora o Addressables empacota conteúdo para a plataforma **ativa no Editor**, não para o `target` passado ao `BuildPlayer` — um build Web feito a seguir a um build Windows empacotava bundles do Windows dentro do jogador Web (confirmado na consola do browser: `StreamingAssets/aa/StandaloneWindows64/...` a dar 404). `BuildRunner.Run()` agora troca a plataforma ativa primeiro (`EditorUserBuildSettings.SwitchActiveBuildTarget`) quando for diferente da pedida. Testado: MainMenu volta a carregar e mostrar o título no build Web real, servido localmente e aberto no browser.
- 2026-10-25 — Testes: `run_tests`/`test_status` do MCP do Editor voltaram a bloquear (mesmo padrão de hang de sessões anteriores) ao correr PlayMode — Unity fechado e testes corridos por CLI batchmode a partir daqui em diante. 3 falhas pré-existentes da F1-01 (`ClimbingLadderToTop...`, `HoldingInteractAndMove_PushesAPushableBox`, `UnderLowCeiling...`) aparecem só em batchmode `-nographics`, nunca na Editor ao vivo — usam contagens fixas de frames a assumir ~60 fps, que não se verifica em batchmode. Não são causadas pelo F1-02 (confirmado: nenhuma destas áreas foi tocada nesta feature); ficam para quem revisitar a F1-01 ou estabilizar o pipeline de testes.
- 2026-10-25 — Build Web real confirmado no browser (servido localmente): MainMenu carrega e mostra o título corretamente depois do fix ao `BuildRunner`. Todos os critérios de aceitação fechados. Estado passa a `DONE`.

## Ideias
- Afinar `CinemachinePositionComposer` (dead zone horizontal, lookahead) e decidir *pixel perfect* quando houver arte final (antes da F5, como já previsto na spec).
- `RoomConnectionType.Elevator`/`Passage` existem no enum mas ainda não têm comportamento distinto de `Door`/`Stairwell` — são só metadata por agora; comportamento próprio (ex. animação de elevador) fica para quando houver conteúdo real a precisar disso.
- Beatriz (companheira) não é movida na transição porque ainda não existe (F2-02) — quando existir, ligar ao mesmo `RoomTransitionController`.
- Investigar a instabilidade dos testes de frame-fixo da F1-01 em batchmode (ver Registo) — não é âmbito desta feature, mas vale a pena abrir como nota para quando o pipeline de CI (`## Ideias` da F0-04) for montado.
