# Funcionalidade Atual

<!-- Ver especificação completa em context/features/F1-nucleo/F1-07-assets-personagens-inimigos.md -->

# F1-07 — Assets base: personagens e inimigos (sprites, animações, Animator Controllers)

## Estado

Em progresso

## Objetivos

Ter arte animável em jogo antes de existir arte final, com sprites de movimento lateral (Tomás + 4 inimigos genéricos) gerados pelo Dinis com o Unity AI Assistant, no mesmo estilo visual do asset pack original.

**Parte A (retratos do pack original): abandonada.** Os 5 retratos originais (Player/Zombie/EyeGuy/Glitcher/Boss.png) foram apagados — não vamos construir Animator Controllers de retrato nem o protótipo de revelação do Boss a partir deles.

**Parte B (sprites de movimento gerados): quase concluída.**
- [x] Tomás: Idle, Walk, Run, Crouch_Idle, Crouch_Walk (20 frames) + Animator Controller (`IsMoving`/`IsRunning`/`IsCrouching`).
- [x] Zombie, EyeGuy, Glitcher, Boss: Idle + Move cada (8 frames cada) + Animator Controller (`IsMoving`).
- [ ] Prefabs de pré-visualização por personagem (SpriteRenderer + Animator, sem gameplay) — falta fazer.

## Critérios de aceitação

- [x] Decisão tomada sobre versionamento dos sprites usados (Git LFS).
- [x] Sprites de movimento do Tomás e dos 4 inimigos gerados e importados.
- [x] Animator Controllers de movimento criados, com os parâmetros que a F1-01 espera. Verificado ao nível dos dados (curvas, transições, parâmetros).
- [ ] Prefabs de pré-visualização a animar corretamente ao mudar os parâmetros no Animator window.
- [ ] Nenhum destes assets introduz dependências de gameplay.

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
