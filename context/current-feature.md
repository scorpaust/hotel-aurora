# Funcionalidade Atual

<!-- Ver especificação completa em context/features/F0-fundacao/F0-03-bootstrap-e-servicos.md -->

# F0-03 — Bootstrap, serviços e estado de jogo

## Estado

Concluída

## Objetivos

Arranque previsível: a cena `Boot` cria os serviços, a máquina de estados global controla o fluxo, e qualquer cena pode ser testada com Play no Editor.

- `Services` (Aurora.Core): registo estático `Register<T>/Get<T>/TryGet<T>/Reset()`.
- `GameBootstrap` (Aurora.Input) em `Boot.unity`: regista `InputService`, `GameStateService`, `SceneService`; carrega `MainMenu.unity` aditivamente.
- Play-from-anywhere via `Aurora.Editor.PlayFromAnywhereBootstrapper`: cena ativa não-Boot → carrega `Boot` aditivamente, estado arranca em `Playing`.
- `GameStateService`: estados Boot/MainMenu/Playing/Paused/Dialogue/Cutscene/Chase/GameOver, tabela de transições validada, cada estado liga a um `InputContext` e a `Time.timeScale`.
- `GameEvent`/`GameEvent<T>` (SO) + `GameEventListener` para ligações no Inspector.
- `ISceneService`/`SceneService`: `LoadAdditiveAsync`/`UnloadAsync` via `Awaitable` + `SceneManager`.

**Não inclui:** save (F1-06), áudio (F4-03), Addressables por ala (F1-02).

## Critérios de aceitação

- [x] Play em `Boot` leva ao MainMenu placeholder.
- [x] Play numa cena de teste qualquer carrega `Boot` e fica jogável nessa cena.
- [x] `Paused` coloca `timeScale = 0` e ativa o mapa UI; sair repõe.
- [x] Transição inválida é rejeitada com erro claro.
- [x] Sem fugas de estado entre Plays com Domain Reload desligado.

## Histórico

<!-- Manter atualizado. Da mais antiga para a mais recente -->

- 2026-10-18 — F1-07 registada como feature (gap identificado pelo Dinis, não estava no roadmap original).
- 2026-10-18 — Decisão de versionamento resolvida: asset pack passa a ser trackado via Git LFS.
- 2026-10-18 — Dinis escolhe priorizar F1-07 antes da F0-02. Branch `feature/f1-07-assets-personagens-inimigos` criada.
- 2026-10-18 — Folha de contacto dos 39 frames originais publicada; Dinis confirma que 5 deles estavam fundidos por slicing automático. Corrigido — total passa a 46 frames.
- 2026-10-18 — Inspeção visual ampliada revela que o pack é de retrato/ícone, não de movimento lateral. Âmbito dividido em Parte A (retratos) e Parte B (sprites de movimento via Unity AI Assistant). Brief de referência preparado.
- 2026-10-18 — Dinis gera o Tomás (Idle/Walk) com o AI Assistant, depois Run/Crouch. Verificado ao nível dos dados em ambos os casos — tudo correto, uma lacuna pequena e não-bloqueante na matriz de transições (Run→Crouch_Idle em falta).
- 2026-10-18 — Dinis gera Idle/Move para os 4 inimigos genéricos. Verificado — tudo correto, ritmo de movimento distinto por criatura.
- 2026-10-18 — Apagados os 5 retratos originais a pedido do Dinis (sem referências no projeto, confirmado antes de remover). Parte A marcada como abandonada. Falta só os prefabs de pré-visualização para a Parte B ficar completa.
- 2026-10-19 — Dinis escolhe fazer a F0-03 antes da F1-01 (Controlador do Tomás), porque o ponto 9 da F1-01 ("respeita GameState") depende de um `IGameStateService` que ainda não existe. Branch `feature/f0-03-bootstrap-e-servicos` criada a partir de `main` (que já tem a F0-02 DONE).
- 2026-10-19 — F0-03 implementada por completo: `Services`, `GameStateService` (com tabela de transições e mapeamento para `InputContext`), `GameEvent`/`GameEvent<T>`/`GameEventListener`, `SceneService`, `GameBootstrap`, play-from-anywhere (em `Aurora.Editor`, não `Aurora.Input`, por causa da regra de `#if UNITY_EDITOR`). Cenas `Boot.unity`/`MainMenu.unity` criadas e testadas manualmente (arranque normal + play-from-anywhere + transição inválida). 26/26 testes (23 EditMode + 3 PlayMode) a passar. Estado passa a REVIEW.
- 2026-10-20 — Dinis testou e reportou dois avisos na consola ("No cameras rendering" e `UI/Navigate` com tipo errado). Ambos corrigidos (Camera na `Boot`; `Navigate` passa a `PassThrough` no `AuroraInput.inputactions`). Confirmado "Tudo okapa" — F0-03 marcada `DONE`.
- 2026-10-20 — F1-07 (prefabs de pré-visualização) e F1-08 (cena protótipo) fundidas em `main` antes de começar a F1-01, para esta ter acesso ao `PFB_TomasPreview` e ao `SCN_PrototypeSandbox`. Ver os registos próprios de cada uma para o histórico completo entre esta entrada e a anterior.
