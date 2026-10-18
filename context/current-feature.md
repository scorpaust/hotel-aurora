# Funcionalidade Atual

<!-- Ver especificação completa em context/features/F1-nucleo/F1-07-assets-personagens-inimigos.md -->

# F1-07 — Assets base: personagens e inimigos (sprites, animações, Animator Controllers)

## Estado

Em progresso

## Objetivos

Transformar os spritesheets já importados em `Assets/Assets/Sprites/` (Player, Zombie, EyeGuy, Glitcher, Boss — Haunted Hotel Asset Pack, CC0) em Animation Clips e Animator Controllers utilizáveis, com um prefab de pré-visualização por personagem, para que haja algo visual e animado em jogo antes de existir arte final.

- Confirmar/ajustar import settings dos 5 spritesheets (já em Sprite Mode = Multiple, slicing feito: Player 10 frames, Zombie 7, EyeGuy 6, Glitcher 6, Boss 10).
- Mapear visualmente os frames de cada spritesheet para poses/estados (idle, andar por direção, etc.).
- Criar Animation Clips (`ANI_<Personagem>_<Estado>`) por personagem.
- Criar um Animator Controller por personagem com parâmetros mínimos (`Speed`, `IsMoving`, `Direction`) e transições.
- Criar um prefab de pré-visualização por personagem (`PFB_<Personagem>Preview` — SpriteRenderer + Animator), só para testes.
- Documentar o mapeamento de frames usado no Registo da feature.

**Não inclui:** lógica de controlo do jogador (F1-01, já desenhada para placeholder), IA de inimigos (F3+), arte final das personagens nomeadas da história (F5/F6), rigs de ossos (não há PSD com camadas aqui).

## Critérios de aceitação

- [x] Decisão tomada e registada sobre versionamento dos sprites usados (resolvido: Git LFS).
- [ ] 5 Animator Controllers criados (Player, Zombie, EyeGuy, Glitcher, Boss), cada um com pelo menos um estado idle e um estado de movimento.
- [ ] 5 prefabs de pré-visualização, cada um a reproduzir a animação correta ao mudar o parâmetro de movimento no Animator window.
- [ ] Mapeamento de frames documentado no Registo desta feature.
- [ ] Nenhum destes assets introduz dependências de gameplay (prefabs sem scripts de `Aurora.Gameplay`/`Aurora.Hotel`).

## Histórico

<!-- Manter atualizado. Da mais antiga para a mais recente -->

- 2026-10-18 — F1-07 registada como feature (gap identificado pelo Dinis, não estava no roadmap original).
- 2026-10-18 — Decisão de versionamento resolvida: asset pack passa a ser trackado via Git LFS.
- 2026-10-18 — Dinis escolhe priorizar F1-07 antes da F0-02 (não depende dela, só da F0-01 que já está DONE). F1-07 passa a funcionalidade atual, estado "Em progresso". Branch `feature/f1-07-assets-personagens-inimigos` criada a partir de `main`.
