# F0-03 — Bootstrap, serviços e estado de jogo
**Estado:** TODO · **Depende de:** F0-02 · **Assemblies:** Aurora.Core, Aurora.Input

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
- [ ] Play em `SCN_Boot` leva ao MainMenu placeholder.
- [ ] Play numa cena de teste qualquer carrega `Boot` e fica jogável nessa cena.
- [ ] `Paused` coloca `timeScale = 0` e ativa o mapa UI; sair repõe.
- [ ] Transição inválida (ex.: `Boot → Chase`) é rejeitada com erro claro.
- [ ] Sem fugas de estado entre Plays com Domain Reload desligado.

## Testes
- EditMode: tabela de transições, `Services.Reset`, `GameEvent` notifica listeners pela ordem de registo.
- PlayMode: arranque a partir de uma cena de teste.

## Registo

## Ideias
