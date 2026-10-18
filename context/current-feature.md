# Funcionalidade Atual

<!-- Ver especificação completa em context/features/F1-nucleo/F1-07-assets-personagens-inimigos.md -->

# F1-07 — Assets base: personagens e inimigos (sprites, animações, Animator Controllers)

## Estado

Em progresso

## Objetivos

Ter arte animável em jogo antes de existir arte final: (A) reaproveitar os 5 spritesheets já importados em `Assets/Assets/Sprites/` (Player, Zombie, EyeGuy, Glitcher, Boss) para o que eles realmente são, e (B) gerar sprites novos de movimento lateral (Tomás + inimigos genéricos) com o Unity AI Assistant, no mesmo estilo visual.

**Descoberta:** os 5 spritesheets não são ciclos de andar — são retratos/ícones quase-duplicados da mesma pose. Servem para retrato de diálogo (Player/Zombie/EyeGuy/Glitcher) e, no caso do Boss, têm uma possível sequência de revelação (contorno→sombreado) que bate certo com o mecanismo da lanterna. Não há arte de movimento lateral no pack — essa tem de ser gerada (Parte B).

**Parte A (arte existente, em curso):**
- Animator Controller de "retrato vivo" (loop idle subtil) por personagem.
- Protótipo da sequência de revelação do Boss.
- Prefab de pré-visualização por personagem.

**Parte B (bloqueado em: Dinis gerar os sprites com o Unity AI Assistant):**
- Sprites de movimento lateral: Tomás (Idle/Walk/Run/Crouch) + inimigos genéricos (Idle/Move).
- Animation Clips + Animator Controller + prefab de pré-visualização para cada um, depois de gerados.

**Não inclui:** lógica de controlo do jogador (F1-01, já desenhada para placeholder), IA de inimigos (F3+), arte final das personagens nomeadas da história (F5/F6).

## Critérios de aceitação

**Parte A:**
- [x] Decisão tomada sobre versionamento dos sprites usados (Git LFS).
- [ ] 5 Animator Controllers de retrato, cada um com loop idle a funcionar.
- [ ] Protótipo da sequência de revelação do Boss.
- [ ] 5 prefabs de pré-visualização.
- [ ] Nenhum destes assets introduz dependências de gameplay.

**Parte B:**
- [ ] Sprites de movimento gerados e importados.
- [ ] Animator Controllers de movimento com os parâmetros que a F1-01 espera.
- [ ] Prefabs de pré-visualização a animar corretamente.

## Histórico

<!-- Manter atualizado. Da mais antiga para a mais recente -->

- 2026-10-18 — F1-07 registada como feature (gap identificado pelo Dinis, não estava no roadmap original).
- 2026-10-18 — Decisão de versionamento resolvida: asset pack passa a ser trackado via Git LFS.
- 2026-10-18 — Dinis escolhe priorizar F1-07 antes da F0-02. Branch `feature/f1-07-assets-personagens-inimigos` criada.
- 2026-10-18 — Folha de contacto dos 39 frames originais publicada; Dinis confirma que 5 deles estavam fundidos por slicing automático (3+3+2+2+2 poses). Corrigido via API moderna do Sprite Editor Data Provider — total passa a 46 frames.
- 2026-10-18 — Inspeção visual ampliada revela que o pack é de retrato/ícone, não de movimento lateral. Âmbito dividido em Parte A (retratos, arte existente) e Parte B (sprites de movimento, a gerar pelo Dinis com o Unity AI Assistant). Preparado brief de referência completo (estilo, tamanhos, estados, regras técnicas) na própria página da feature.
