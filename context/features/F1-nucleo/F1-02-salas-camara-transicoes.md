# F1-02 — Salas, câmara e transições
**Estado:** TODO · **Depende de:** F1-01 · **Assemblies:** Aurora.Gameplay, Aurora.Core

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
4. Cinemachine 3: `CinemachineCamera` com `CinemachinePositionComposer` e `CinemachineConfiner2D` (atualizado por sala); dead zone horizontal pequena; *lookahead* leve na direção do movimento; câmara *pixel perfect* se a arte final o pedir (decidir com o Dinis antes da F5).
5. `ISceneService` passa a usar Addressables (`Addressables.LoadSceneAsync`), com grupos por ala.
6. Cena de teste `SCN_TestWing` com 3 salas, 1 escada entre andares e 1 porta condicional.

## Critérios de aceitação
- [ ] Atravessar 3 salas sem a câmara sair dos limites de cada uma.
- [ ] Porta condicional leva ao destino A ou B conforme uma flag.
- [ ] Mudança de ala carrega/descarrega cenas sem picos visíveis (< 1 s de ecrã negro no PC).
- [ ] Funciona no build Web.

## Testes
- EditMode: resolução de destino de `RoomConnection` com/sem flag.
- PlayMode: transição entre salas termina com jogador no spawn certo e estado `Playing`.

## Registo

## Ideias
