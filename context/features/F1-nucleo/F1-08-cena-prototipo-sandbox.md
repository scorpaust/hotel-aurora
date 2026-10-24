# F1-08 — Cena de protótipo / sandbox de testes
**Estado:** REVIEW · **Depende de:** F1-07 · **Assemblies:** Nenhum (cena/conteúdo)

## Objetivo
Uma cena jogável, não ligada a nenhuma ala da história, para testar personagens, inimigos, armadilhas e elementos de transição (escadas, elevador) com física e colisões a sério — algo que imite bem a escala e a sensação do jogo final, sem ser uma sala de produção.

## Contexto na história
Nenhum — é uma cena de teste/sandbox, não faz parte do mapa do hotel.

## Âmbito
**Inclui:**
- Cena `Tests/Scenes/SCN_PrototypeSandbox.unity`, tamanho moderado (~20×14 tiles).
- Re-slicing de `Assets/Assets/Tiles/Tiles.png` (grelha 8×8, confirmada a partir do `.tres` do Godot) e ajuste de PPU para ficar à escala dos personagens (32×48px/100PPU).
- Tilemap de chão + Tilemap de paredes com `TilemapCollider2D` (Composite) a fechar a sala.
- Mobiliário de `Assets/Assets/Environmental/` (Bath, BigDoor, Fireplace, FourPoster, GrandfatherClock, StorageBox, WasherDryer) com `BoxCollider2D` sólido do tamanho do sprite.
- `StairsUp`/`StairsDown`/`ElevatorDoors`/`ElevatorEmpty` como zonas de trigger (`BoxCollider2D isTrigger`, tag própria) — só placeholders visuais/físicos, sem lógica de transição.
- Instâncias dos 5 prefabs da F1-07 (`PFB_TomasPreview` + 4 inimigos): Tomás com `Rigidbody2D` *Kinematic* (consistente com a F1-01 §1), inimigos com `Rigidbody2D` + collider, espalhados pela sala.
- Layers de física (`Player`, `Enemy`, `Environment`, `Trigger`) com matriz de colisão sensata.
- Câmara simples (Camera2D ortográfica) a enquadrar a sala — sem Cinemachine (isso é F1-02).

**Não inclui:**
- Lógica de controlo do jogador (F1-01), interação (F1-03), transições reais de sala/elevador (F1-02), IA de inimigos (F3+).
- Cinemachine / seguimento de câmara (F1-02).
- Conteúdo narrativo ou de produção — isto nunca faz parte do mapa final do hotel.

## Especificação
1. Re-slice `Tiles.png` (TextureImporter → `ISpriteEditorDataProvider`, grelha 8×8 = ~100 tiles) e criar os assets `Tile` (2D Tilemap) a partir dos sprites relevantes (chão, parede, variações).
2. Criar a cena, `Grid` com 2 `Tilemap` (Chão, Paredes) + `TilemapRenderer` + `TilemapCollider2D` + `CompositeCollider2D` nas paredes (`Rigidbody2D Static` no GameObject das paredes, exigido pelo Composite Collider).
3. Pintar um layout de sala simples mas não minúsculo (~20×14), com uma divisória interior opcional para testar line-of-sight/esconderijos mais tarde.
4. Colocar o mobiliário com colisão sólida; colocar escadas/elevador como triggers com tag (`Stairs`, `Elevator`).
5. Configurar `Physics2D` Layers e a Layer Collision Matrix em Project Settings.
6. Instanciar os 5 prefabs de personagens/inimigos com os componentes de física corretos, distribuídos pela sala.
7. Câmara a enquadrar a sala inteira (ou a maior parte), sorting layers corretos para chão/paredes/mobiliário/personagens.
8. Guardar a cena e confirmar sem erros na consola.

## Critérios de aceitação
- [x] Cena abre sem erros nem warnings novos.
- [x] Tomás (preview) tem `Rigidbody2D` Kinematic + `BoxCollider2D`; `TilemapCollider2D`+`CompositeCollider2D` nas paredes bloqueiam fisicamente (layer `Environment`). Mobiliário decorativo passou a `Trigger` (ver Registo da F1-01-B) — só os objetos manipuláveis (conduta, escada, caixa empurrável) é que continuam sólidos.
- [x] Escadas/elevador são `BoxCollider2D isTrigger` com tag própria (`Stairs`/`Elevator`), layer `Trigger` — disparam, sem lógica associada ainda.
- [x] Os 4 inimigos estão na cena com `Rigidbody2D` + collider, layer `Enemy`; matriz de colisão por omissão do Unity já cobre tudo o que era pedido (nada ficou propositadamente desligado — ver Registo).
- [x] Escala coerente: tile a 0,32u, personagem com ~1 tile de largura e ~1,5 de altura — confirmado visualmente por captura do Game View.

## Testes
Não aplicável (cena/conteúdo, sem lógica testável). Validação é visual e por inspeção de componentes, no Editor.

## Registo
- 2026-10-18 — **Correção importante:** a primeira versão da cena foi montada como vista de cima (mobiliário colado às 4 paredes de uma planta, como um dungeon-crawler top-down). O jogo é vista lateral (`codespec.md` §1) — não há planta vista de cima. Reconstruída como uma faixa de chão única (y=0) onde tudo assenta, altura da sala reduzida de 14 para 8 tiles (altura de teto, não um campo vazio), porta de ligação entre salas ao nível do chão em vez de a meio do ar. Corrigido também: o Tomás nascia em cima da `BigDoor` ("o player está entalado"); a câmara mostrava a sala toda de uma vez e fazia tudo parecer minúsculo — passou a um zoom de jogo (`orthographicSize 1.8`) a enquadrar uma sala de cada vez, mais parecido com o que uma câmara lateral real mostraria.
- 2026-10-18 — Identificado que `BigDoor.png` eram 4 frames reais de abertura a aparecer todos de uma vez (nunca tinha sido fatiada). Corrigida (mesma técnica do re-slicing da F1-07) e promovida, com o elevador e as escadas, a prefabs com Animator próprio: `PFB_BigDoor` (Closed↔Open, sequência real de 4 frames), `PFB_Elevator` (junta `ElevatorDoors`/`ElevatorEmpty` num só prefab), `PFB_StairsUp`/`PFB_StairsDown` (sem animação, são ícones estáticos). Sem arte nova — só organização do que já existia. Armadilhas/obstáculos interativos ficam para a F3-02 (pack não tem arte para isso).
- 2026-10-18 — Inspecionado `Tiles.png` ampliado (8x nearest-neighbor): é um conjunto de ícones decorativos e algarismos de número de quarto, não um tileset de chão/parede repetível. Gerados `SPR_FloorPlaceholder`/`SPR_WallPlaceholder` (32×32px/100PPU, paleta igual à dos personagens) em vez disso — placeholder aceitável numa cena sandbox (`rules.md` §2).
- 2026-10-18 — Cena construída via Editor MCP (`eval`, API do Unity diretamente — não via Unity AI Assistant, por ser trabalho estrutural de física/colliders mais adequado a verificação direta): Grid + Tilemap chão/paredes (`CompositeCollider2D`), 7 peças de mobiliário com colisão sólida, 4 triggers (escadas×2, elevador×2), os 5 personagens da F1-07 instanciados com `Rigidbody2D` Kinematic. Layers `Player`/`Enemy`/`Environment`/`Trigger` e tags `Stairs`/`Elevator` criadas.
- 2026-10-18 — Personagens inicialmente invisíveis (mesmo `sortingOrder` que o Tilemap do chão escondia-os) — corrigido subindo o `sortingOrder` dos `SpriteRenderer` dos personagens para 3. Confirmado por captura do Game View: sala, mobiliário e os 5 personagens todos visíveis e à escala certa. 0 erros na consola. Estado passa a REVIEW.
- 2026-10-24 — **Regra de colisão revista**, depois de a F1-01 tornar a colisão real: com o mobiliário todo sólido, andar pela sandbox ficava penoso (sem salto, qualquer peça de decoração vira obstáculo total). Decisão: só bloqueiam fisicamente os objetos manipuláveis/interativos (conduta, `StairZone`, caixa empurrável — e portas/elevador/escadas da F1-08, que vão ganhar interação real na F1-03). O resto da mobília decorativa (`Prop_FourPoster`, `Prop_Fireplace`, `Prop_GrandfatherClock`, `Prop_Bath`, `Prop_StorageBox`, `Prop_WasherDryer`) passou da layer `Environment` para `Trigger` — deixam de bloquear o Tomás, mas mantêm o `BoxCollider2D` (agora `isTrigger`) para a F3-03 (eventos do hotel vivo) poder disparar sustos/sugestões ao jogador quando ele passa perto, sem precisar de reconstruir a cena. Confirmado por `BoxCast` direto contra cada peça: nenhuma bloqueia; só objetos reais (paredes, inimigos) continuam a aparecer no cast. 66/66 testes do projeto continuam a passar (mudança só de cena/layers, sem código).

## Ideias
- Reutilizar esta cena como base para a `Tests/Scenes/SCN_InputSandbox` da F0-02 em vez de criar as duas de raiz.
- Quando a F1-02 existir, decidir se esta sandbox se torna uma "sala de testes" oficial reutilizável em QA, ou se fica só de desenvolvimento.
- A F3-03 pode aproveitar os triggers da mobília decorativa (ver Registo de 2026-10-24) como gatilhos prontos para eventos ambientais/sustos, em vez de criar zonas novas de raiz.
