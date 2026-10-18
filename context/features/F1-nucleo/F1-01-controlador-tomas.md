# F1-01 — Controlador do Tomás
**Estado:** TODO · **Depende de:** F0-04 · **Assemblies:** Aurora.Gameplay

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
- [ ] Correr abranda sozinho após o fôlego acabar e recupera.
- [ ] Agachado passa por conduta de 1 u de altura; não se levanta debaixo dela.
- [ ] Sobe e desce escadas de degraus e de mão sem prender.
- [ ] Empurra uma caixa para chegar a uma prateleira.
- [ ] 0 B de GC por frame no Profiler com o jogador a mover-se.

## Testes
- EditMode: `PlayerMotor` — aceleração, fôlego, transição para agachado com teto.
- PlayMode: simular input → posição avança; escada de mão termina no piso de cima.

## Registo

## Ideias
- "Coyote time" e buffer de input na entrada de escadas se parecer rígido.
