# Funcionalidade Atual

<!-- Ver especificação completa em context/features/F1-nucleo/F1-08-cena-prototipo-sandbox.md -->

# F1-08 — Cena de protótipo / sandbox de testes

## Estado

Em progresso

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

- [ ] Cena abre sem erros nem warnings novos.
- [ ] Tomás (preview) com `Rigidbody2D` Kinematic + collider; não atravessa paredes/mobiliário sólido.
- [ ] Escadas/elevador disparam trigger (sem lógica associada ainda).
- [ ] 4 inimigos na cena com física, collision matrix correta.
- [ ] Escala coerente entre personagens e tiles.

## Histórico

<!-- Manter atualizado. Da mais antiga para a mais recente -->

- 2026-10-18 — F1-08 registada e definida como funcionalidade atual, a seguir à F1-07 (REVIEW — prefabs concluídos, ainda sem merge). Branch `feature/f1-08-prototype-scene` criada a partir de `feature/f1-07-assets-personagens-inimigos` (precisa dos prefabs dessa branch). Plano confirmado pelo Dinis, trabalho ainda não iniciado.
