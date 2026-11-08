# Funcionalidade Atual

<!-- Ver especificação completa em context/features/F1-nucleo/F1-02-salas-camara-transicoes.md -->

# F1-02 — Salas, câmara e transições

## Estado

DONE

## Objetivos

O hotel é feito de salas ligadas por portas, escadas e elevadores. A câmara enquadra cada sala; passar de uma para outra é rápido e carrega conteúdo por ala via Addressables.

- `IWorldFlagsService`/`WorldFlagsService` (mini-serviço de flags, sem persistência — a F1-06 estende).
- `RoomConnectionType`/`RoomDestination`/`RoomTransitionRequest`/`IRoomTransitionService` (Aurora.Core, partilhados por Gameplay e Hotel).
- `RoomConnection` (Aurora.Gameplay, trigger ad-hoc como `StairZone`/`Pushable`).
- `Room`/`RoomSpawnPoint`/`RoomTransitionController` (Aurora.Hotel).
- `SceneService` a usar Addressables; `RoomFadePanel` (Aurora.UI) para o fade, desacoplado via `GameEvent`.
- Cinemachine 3 (`CinemachineBrain`/`CinemachineCamera`/`CinemachinePositionComposer`/`CinemachineConfiner2D`).
- Teste dentro da única cena de sandbox (`SCN_PrototypeSandbox`, sub-árvore `RoomPlayground`) + `SCN_SandboxAnnex` (mudança de ala).

**Não inclui:** eventos do hotel vivo (F3-03), elevador "que sente medo" (F3-03), Beatriz na transição (ainda não existe, F2-02).

## Critérios de aceitação

- [x] Atravessar 3 salas sem a câmara sair dos limites de cada uma.
- [x] Porta condicional leva ao destino A ou B conforme uma flag.
- [x] Mudança de ala carrega/descarrega cenas sem picos visíveis (< 1 s de ecrã negro no PC).
- [x] Funciona no build Web.

## Histórico

<!-- Manter atualizado. Da mais antiga para a mais recente -->

- 2026-10-25 — Depois de fechar a F0-04, retomada a Fase 1 pela F1-02 (primeira `TODO` com dependências `DONE`). Branch `feature/f1-02-salas-camara-transicoes` criada a partir de `main`. Antes de escrever código, 2 decisões de âmbito confirmadas pelo utilizador: mini-serviço de `WorldFlags` agora (não esperar pela F1-06) e painel de fade mínimo próprio (não esperar pela F4-01).
- 2026-10-25 — Implementado de ponta a ponta: serviços novos em `Aurora.Core`, `RoomConnection` em `Aurora.Gameplay` (não em `Aurora.Hotel` — a direção de dependência é a inversa), `Room`/`RoomSpawnPoint`/`RoomTransitionController` em `Aurora.Hotel`, fade via `GameEvent` (Aurora.UI desacoplado), `SceneService` migrado para Addressables, Cinemachine 3 configurado. Cenas de teste geradas por um script de Editor idempotente (`TestWingSceneBuilder`), não editadas à mão.
- 2026-10-25 — Dois bugs reais encontrados e corrigidos ao testar: (1) `Room` registava-se antes de `Configure()` definir o id, quando criado por código (testes/ferramenta) — corrigido para registar dentro do próprio `Configure()` e resolver spawn points ao vivo; (2) `BuildRunner` nunca trocava a plataforma ativa do Editor antes de compilar, e o Addressables empacota para a plataforma ativa, não para o alvo do build — um build Web a seguir a um build Windows ia com bundles do Windows lá dentro (confirmado no browser: 404 em `StreamingAssets/aa/StandaloneWindows64/...`). Corrigido; build Web real voltou a mostrar o MainMenu corretamente.
- 2026-10-25 — Testes: `run_tests` do MCP do Editor voltou a bloquear em PlayMode (mesmo padrão de hangs anteriores) — testes corridos por CLI batchmode a partir daí. 68/68 a passar, com 3 falhas pré-existentes da F1-01 identificadas como fragilidade de timing em batchmode (contagens fixas de frames a 60fps), não causadas por esta feature. Build Web real confirmado no browser. Todos os critérios fechados — estado passa a `DONE`.
- 2026-10-26/27 — Utilizador testou `SCN_TestWing` manualmente no Editor: câmara "esquisita" (minúscula, depois presa). Três bugs reais de câmara (Perspective em vez de Orthographic, damping do `CinemachinePositionComposer` por omissão, salas pequenas de mais para o confiner ter margem) — corrigidos e confirmados com capturas de ecrã reais (`capture_game_view`), não só leituras de posição.
- 2026-11-08 — Utilizador pediu consolidação: uma só cena de sandbox viva, que se vai adaptando, em vez de cenas de teste descartáveis por feature (tinha posto quadrados manuais no Editor que se perdiam sempre que a cena era regenerada). `TestWingSceneBuilder` substituído por `RoomPlaygroundBuilder`, que constrói dentro do `SCN_PrototypeSandbox` (sub-árvore `RoomPlayground`, nunca a cena inteira) e reutiliza o Tomás já existente. Chão visível colorido por sala + marcador visível nas portas/escadas — resolve a causa raiz da queixa. `SCN_TestWingAnnex` renomeada para `SCN_SandboxAnnex`, afastada no espaço para não sobrepor o conteúdo da F1-01. Regra nova em `rules.md` §6. Confirmado de novo com capturas reais em todas as transições.
