# F1-08 — Cena de protótipo / sandbox de testes
**Estado:** DOING · **Depende de:** F1-07 · **Assemblies:** Nenhum (cena/conteúdo)

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
- [ ] Cena abre sem erros nem warnings novos.
- [ ] Tomás (preview) tem `Rigidbody2D` Kinematic + collider; não atravessa paredes nem mobiliário sólido.
- [ ] Escadas/elevador disparam o respetivo trigger (confirmável no Editor, sem lógica associada ainda).
- [ ] Os 4 inimigos estão na cena com física e não se sobrepõem uns aos outros por defeito (collision matrix correta).
- [ ] Escala coerente: um personagem ocupa uma fração sensata de um tile (não gigante nem minúsculo face ao chão/paredes).

## Testes
Não aplicável (cena/conteúdo, sem lógica testável). Validação é visual e por inspeção de componentes, no Editor.

## Registo

## Ideias
- Reutilizar esta cena como base para a `Tests/Scenes/SCN_InputSandbox` da F0-02 em vez de criar as duas de raiz.
- Quando a F1-02 existir, decidir se esta sandbox se torna uma "sala de testes" oficial reutilizável em QA, ou se fica só de desenvolvimento.
