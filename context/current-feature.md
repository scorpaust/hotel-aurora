# Funcionalidade Atual

<!-- Ver especificação completa em context/features/F1-nucleo/F1-01-controlador-tomas.md -->

# F1-01 — Controlador do Tomás

## Estado

Em progresso (Etapa A concluída, Etapa B por fazer)

## Objetivos

Movimento lateral responsivo e "pesado" na medida certa: um rapaz de 17 anos com mochila, não um herói de plataformas.

**Etapa A (concluída):** andar, correr, agachar, viradas, colisão com paredes/mobiliário, gate do `GameState`, Animator ligado.
**Etapa B (por fazer):** escadas (degraus + mão), empurrar/puxar objetos, tolerância de queda, testes completos, critérios de aceitação todos fechados.

- `PlayerMovementConfig` (SO): velocidades andar/correr/agachar, aceleração/desaceleração, fôlego.
- `PlayerMotor`: lógica pura testável (sem dependência de Unity physics).
- `PlayerController`: `Rigidbody2D` Kinematic + `Physics2D.BoxCast`, lê só `IInputService.Gameplay`, respeita `GameState` (Playing/Chase).

**Não inclui:** saltar livremente, esconderijos (F3-01), dano/apanhado (F3-02).

## Critérios de aceitação

- [ ] Movimento idêntico em teclado e gamepad.
- [x] Correr abranda sozinho após o fôlego acabar e recupera.
- [ ] Agachado passa por conduta de 1 u de altura; não se levanta debaixo dela.
- [ ] Sobe e desce escadas de degraus e de mão sem prender.
- [ ] Empurra uma caixa para chegar a uma prateleira.
- [ ] 0 B de GC por frame no Profiler com o jogador a mover-se.

## Histórico

<!-- Manter atualizado. Da mais antiga para a mais recente -->

- 2026-10-20 — Depois da F0-03 (o bloqueio do GameState já está resolvido), retomada a F1-01. Branch `feature/f1-01-controlador-tomas` criada a partir da `main`.
- 2026-10-20 — Antes de começar: F1-07 (prefabs de pré-visualização) e F1-08 (cena protótipo) fundidas em `main`, para a F1-01 ter acesso ao `PFB_TomasPreview` (já com `Rigidbody2D` Kinematic certo) e ao `SCN_PrototypeSandbox`. Decisão confirmada, tal como reaproveitar o `PFB_TomasPreview` em vez de criar um placeholder novo.
- 2026-10-20 — Etapa A implementada e testada: `PlayerMovementConfig`, `PlayerMotor` (8 testes EditMode a passar — aceleração, fôlego a abrandar e recuperar, agachar, direção), `PlayerController` (2 testes PlayMode a passar — Move avança a posição, sem input não se mexe). `PlayerController` aplicado ao prefab `PFB_TomasPreview`. 36/36 testes do projeto a passar.
- 2026-10-21 — Branch `feature/f1-01-controlador-tomas` finalmente fundida em `main` (tinha ficado parada antes da reconfiguração arquitetural da F1-09/F1-10, entretanto concluída). Até aqui, `main` não tinha nenhum código de `Aurora.Gameplay` — por isso o sandbox não respondia a nenhum input ao dar Play. Resolvido o merge (conflitos só em ficheiros `.md`/`.asmdef`, nenhum em cenas/prefabs). Etapa B continua por fazer.
