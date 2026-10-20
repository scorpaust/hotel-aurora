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

- 2026-10-19 — Dinis escolhe fazer a F0-03 antes da F1-01 (Controlador do Tomás), porque o ponto 9 da F1-01 ("respeita GameState") depende de um `IGameStateService` que ainda não existe. Branch `feature/f0-03-bootstrap-e-servicos` criada a partir de `main` (que já tem a F0-02 DONE).
- 2026-10-19 — F0-03 implementada por completo: `Services`, `GameStateService` (com tabela de transições e mapeamento para `InputContext`), `GameEvent`/`GameEvent<T>`/`GameEventListener`, `SceneService`, `GameBootstrap`, play-from-anywhere (em `Aurora.Editor`, não `Aurora.Input`, por causa da regra de `#if UNITY_EDITOR`). Cenas `Boot.unity`/`MainMenu.unity` criadas e testadas manualmente (arranque normal + play-from-anywhere + transição inválida). 26/26 testes (23 EditMode + 3 PlayMode) a passar. Estado passa a REVIEW.
- 2026-10-20 — Dinis testou e reportou dois avisos na consola ("No cameras rendering" e `UI/Navigate` com tipo errado). Ambos corrigidos (Camera na `Boot`; `Navigate` passa a `PassThrough` no `AuroraInput.inputactions`). Confirmado "Tudo okapa" — F0-03 marcada `DONE`.
