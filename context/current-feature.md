# Funcionalidade Atual

<!-- Ver especificação completa em context/features/F1-nucleo/F1-08-cena-prototipo-sandbox.md -->

# F1-08 — Cena de protótipo / sandbox de testes

## Estado

Concluída (REVIEW — à espera de confirmação do Dinis)

## Objetivos

Uma cena jogável, não ligada a nenhuma ala da história, para testar personagens, inimigos, armadilhas e elementos de transição (escadas, elevador) com física e colisões a sério.

- `Tests/Scenes/SCN_PrototypeSandbox.unity`, ~20×14 tiles.
- Re-slice de `Tiles.png` (grelha 8×8) + ajuste de PPU à escala dos personagens.
- Tilemap de chão + paredes com `TilemapCollider2D`.
- Mobiliário de `Environmental/` com colisão sólida; escadas/elevador como triggers.
- Os 5 prefabs da F1-07 instanciados com física (Tomás `Rigidbody2D` Kinematic, per F1-01).
- Layers de física + collision matrix; câmara simples a enquadrar a sala.

**Não inclui:** lógica de controlo (F1-01), interação (F1-03), transições reais (F1-02), IA (F3+), Cinemachine (F1-02).

## Critérios de aceitação

- [x] Cena abre sem erros nem warnings novos.
- [x] Tomás (preview) com `Rigidbody2D` Kinematic + collider; paredes/mobiliário sólidos bloqueiam fisicamente.
- [x] Escadas/elevador disparam trigger (sem lógica associada ainda).
- [x] 4 inimigos na cena com física.
- [x] Escala coerente entre personagens e tiles (confirmado visualmente).

## Histórico

<!-- Manter atualizado. Da mais antiga para a mais recente -->

- 2026-10-18 — F1-08 registada e definida como funcionalidade atual, a seguir à F1-07 (REVIEW — prefabs concluídos, ainda sem merge). Branch `feature/f1-08-prototype-scene` criada a partir de `feature/f1-07-assets-personagens-inimigos` (precisa dos prefabs dessa branch). Plano confirmado pelo Dinis.
- 2026-10-18 — Cena construída: `Tiles.png` afinal não serve para chão/parede (são ícones decorativos) — gerados tiles placeholder próprios. Grid+Tilemap, mobiliário, triggers de escadas/elevador e os 5 personagens instanciados com física. Corrigido um problema de sorting (personagens escondidos atrás do chão). Confirmado por captura do Game View, 0 erros. Estado passa a REVIEW.
