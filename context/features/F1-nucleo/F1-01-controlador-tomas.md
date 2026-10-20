# F1-01 — Controlador do Tomás
**Estado:** DOING (Etapa A concluída, Etapa B por fazer) · **Depende de:** F0-04 · **Assemblies:** Aurora.Gameplay

## Objetivo
Movimento lateral responsivo e "pesado" na medida certa: um rapaz de 17 anos com mochila, não um herói de plataformas.

## Contexto na história
Tomás não luta — foge, esconde-se, sobe, rasteja por condutas. O movimento tem de transmitir vulnerabilidade sem ser frustrante.

## Âmbito
**Inclui:** andar, correr, agachar/rastejar, subir/descer escadas (de degraus e de mão), cair de pequenas alturas, empurrar/puxar objetos leves, animator com estados, placeholder art.
**Não inclui:** saltar livremente (não há salto — só transposição contextual de obstáculos baixos via interação), esconderijos (F3-01), dano/apanhado (F3-02).

## Especificação
1. `PlayerController` (MonoBehaviour fino) + `PlayerMotor` (lógica pura testável) com `Rigidbody2D` *Kinematic* e `Physics2D` casts (controlo preciso em escadas e rampas).
2. Velocidades em `PlayerMovementConfig` (SO): andar ~2.2 u/s, correr ~4.5 u/s, agachado ~1.2 u/s; aceleração/desaceleração curtas (≤ 0.12 s).
3. **Correr** consome *fôlego* oculto: após ~4 s abranda e ouve-se a respiração (sem barra no HUD). Recupera parado ou a andar.
4. **Agachar**: reduz o collider; obrigatório para condutas e por baixo de mesas. Não se levanta se houver teto.
5. **Escadas**: zonas `StairZone` com entrada por cima/baixo; Move vertical entra na escada. Escadas de mão: movimento só vertical, sair em cima/baixo.
6. Quedas: até 2 u sem consequência; mais que isso é impedido por design (sem dano por queda).
7. Empurrar/puxar: objetos `Pushable` com massa, Interact segurado + Move.
8. Animator (placeholder: retângulo com olhos e mochila) com parâmetros `Speed`, `IsRunning`, `IsCrouching`, `OnStairs`, `IsPushing`. Sprite vira com a direção.
9. Lê input só via `IInputService.Gameplay`. Respeita `GameState` (não se move fora de `Playing`/`Chase`).

## Critérios de aceitação
- [ ] Movimento idêntico em teclado e gamepad (stick analógico permite andar devagar).
- [x] Correr abranda sozinho após o fôlego acabar e recupera.
- [ ] Agachado passa por conduta de 1 u de altura; não se levanta debaixo dela. (Agachar por `Crouch` já funciona; a parte "obrigatório sob teto" fica para a Etapa B, quando houver condutas na cena.)
- [ ] Sobe e desce escadas de degraus e de mão sem prender.
- [ ] Empurra uma caixa para chegar a uma prateleira.
- [ ] 0 B de GC por frame no Profiler com o jogador a mover-se. (Revisão visual do código não mostra alocações no `Update`; confirmação com o Profiler fica para o fecho da feature.)

## Testes
- EditMode: `PlayerMotor` — aceleração, fôlego, transição para agachado com teto. 8 testes (`PlayerMotorTests`) a passar.
- PlayMode: simular input → posição avança; escada de mão termina no piso de cima. `PlayerControllerTests` (2) a passar: `Move` mantido avança a posição, sem input não se mexe. Falta o teste da escada de mão (Etapa B).

## Registo
- 2026-10-20 — **Etapa A concluída.** `PlayerMovementConfig` (SO), `PlayerMotor` (lógica pura: aceleração/desaceleração, fôlego, agachar, direção), `PlayerController` (MonoBehaviour: `Rigidbody2D` Kinematic + `Physics2D.BoxCast` para colisão, lê só `IInputService.Gameplay`, respeita `GameState` Playing/Chase, liga ao Animator).
  - Reaproveitado o `PFB_TomasPreview` da F1-07 (já tinha `Rigidbody2D` Kinematic + `BoxCollider2D` certos) e o `Tomas_Controller` existente (`IsMoving`/`IsRunning`/`IsCrouching` já corretos); só acrescentei os parâmetros `Speed`, `OnStairs`, `IsPushing`.
  - `PlayerController` aplicado ao prefab (não só à instância na cena), para ficar disponível em qualquer cena/teste futuro.
  - F1-07 e F1-08 fundidas em `main` antes de começar, para ter acesso a estes assets (ver `current-feature.md`).
  - Bug de teste encontrado e corrigido: `InputTestFixture.Setup()` isola/reinicia o Input System do teste, mas o `InputService` que o `GameBootstrap` já tinha registado foi criado *antes* disso — as bindings ficam presas aos dispositivos antigos, e `PlayerController` (que lê via `Services.Get<IInputService>()`) não via o Move. Corrigido registando um `InputService` novo no `Setup()` do teste, depois de adicionar o teclado (mesmo padrão já usado em `InputServiceDeviceTests`). Não é um bug de produção — é um artefacto da forma como o `InputTestFixture` isola o Input System para testes.
  - Nomes das layers de física (`Player`=8, `Enemy`=9, `Environment`=10, `Trigger`=11, definidas pela F1-08) não aparecem no Editor nesta sessão (ficaram "sem nome" em memória, apesar de corretas no `TagManager.asset` em disco) — parece exigir reiniciar o Editor para recarregar. Usei o índice 10 diretamente no `_collisionMask` do `PlayerController`; confirmar o nome "Environment" aparece depois de reiniciar o Unity.

## Ideias
- "Coyote time" e buffer de input na entrada de escadas se parecer rígido.
- "Coyote time" e buffer de input na entrada de escadas se parecer rígido.
