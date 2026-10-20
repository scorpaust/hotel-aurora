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
- 2026-10-20 — **Bug real encontrado e corrigido:** Dinis reportou que o Tomás não se mexia mesmo com input. Causa: `Physics2D.queriesStartInColliders` estava a `true` (omissão do Unity) — qualquer `BoxCast` que comece sobreposto a um colisor conta isso como acerto a distância 0. O Tomás, em repouso no `SCN_PrototypeSandbox`, já estava ligeiramente sobreposto ao `Tilemap_Walls` e ao `PFB_BigDoor` (ambos na layer `Environment`), por isso **todas** as direções do `BoxCast` do `PlayerController` davam `distance=0` e o movimento ficava sempre bloqueado. Corrigido em `ProjectSettings/Physics2DSettings.asset` (`m_QueriesStartInColliders: 0`) — é a prática recomendada para character controllers cinemáticos baseados em casts. Confirmado por inspeção direta (`Physics2D.BoxCast` passa a devolver distâncias reais em vez de 0 sempre) e pelos 36 testes automatizados, que continuam todos a passar.
  - Também ativado `PlayerSettings.runInBackground = true`: descobri, ao tentar confirmar visualmente, que o loop do Editor em Play Mode praticamente para (poucos frames por segundo) quando a janela não tem foco do sistema operativo — o que também explica porque é que a minha simulação de teclas por automação parecia não funcionar. Isto não afeta o Dinis a testar normalmente (só precisa de clicar na Game View antes de carregar nas teclas, como é costume no Unity), mas `runInBackground` é uma rede de segurança sensata de qualquer forma.
- 2026-10-20 — Dinis confirmou que anda/corre/agacha bem, mas reportou que o Tomás passa por dentro dos inimigos (Zombie/EyeGuy/Glitcher/Boss). Causa: `_collisionMask` do `PlayerController` só tinha a layer `Environment` (10) — faltava a `Enemy` (9). Corrigido no prefab (`_collisionMask = 1536`, ambas as layers). Os inimigos ainda não têm comportamento (F3+); isto só os torna sólidos para o Tomás embater, como uma parede.
  - Dinis reportou que continuava a "passar por detrás" — confirmado por `OverlapBoxAll` que a colisão já funcionava (o Zombie aparece corretamente na lista de sobreposições), o que sobrava era só visual: todos os personagens (Tomás e os 4 inimigos) tinham o mesmo `sortingOrder` (3), por isso a ordem de desenho entre eles era arbitrária quando se tocavam. Corrigido: `sortingOrder` do Tomás sobe para 5, para ficar sempre visualmente à frente dos inimigos (convenção comum em side-scrollers).
  - **Bug real #2:** Dinis reportou "não colide com paredes nem com blocos" e "sai do ecrã". Dois problemas distintos, ambos confirmados e corrigidos:
    1. *Tunelamento por paredes finas:* com `Physics2D.queriesStartInColliders = false` (fix anterior), um `BoxCast` que comece exatamente encostado a um colisor fino (como os do `TilemapCollider2D`, que são só contornos, sem espessura) ignora esse contacto e não encontra nada — o Tomás "escorregava" para o outro lado assim que ficava flush contra uma parede. Corrigido com uma margem de segurança (`SkinWidth = 0.02`): o `PlayerController` faz sempre o cast um pouco mais longe do que precisa e nunca deixa o Tomás ficar mesmo encostado. Confirmado a reverter a correção (`SkinWidth = 0`) e a ver o novo teste `HoldingMoveTowardThinWall_NeverTunnelsThrough` falhar (atravessava mesmo), depois a repor e a ver passar.
    2. *Sala sem fecho:* o colisor das paredes do `SCN_PrototypeSandbox` só cobre x ∈ [-0.32, 6.72] — não há nada a fechar o lado direito da sala depois da área mobilada, por isso o Tomás entrava em espaço vazio infinito (daí "sai do ecrã": a câmara, fixa, ficava muito para trás). Não é bug do `PlayerController` — é a cena de teste incompleta. Adicionado `Wall_RightBoundary` (BoxCollider2D, layer `Environment`) a selar o lado direito, só para a sandbox ser utilizável; os limites "a sério" das salas são trabalho da F1-02.
  - Novo teste de regressão `PlayerControllerTests.HoldingMoveTowardThinWall_NeverTunnelsThrough` (PlayMode): cria uma parede fina de teste, mantém o Move premido tempo suficiente para atravessar várias vezes a distância até ela, confirma que a posição final nunca ultrapassa a parede.
- 2026-10-20 — Dinis reportou "o Tomás deixou de andar" depois do fix da parede direita. Não consegui reproduzir num teste limpo (moveu-se bem). O próprio Dinis percebeu a causa real: o ponto de partida do Tomás estava espremido entre a `PFB_BigDoor` e o `Prop_FourPoster` com quase nenhuma folga (confirmado: gap de 0.374 contra uma largura do Tomás de 0.32 — com a margem de segurança de 0.02 de cada lado, sobrava quase nada). Isto nunca se notou antes porque a colisão não funcionava.
  - Inspeção mais larga revelou que **toda** a sala tinha objetos quase encostados uns aos outros (gaps de 0.05–0.13 entre vários pares, todos menores que o Tomás) — a F1-08 montou isto antes de haver colisão a sério, só para teste visual/animação.
  - Decisão (confirmada pelo Dinis): em vez de espremer tudo num corredor apertado, **estendi a sala** — pintei mais 15 colunas de `Tilemap_Floor`/`Tilemap_Walls` (de 20 para 35 colunas, ~6.4u → ~11.2u de largura), movi o selo da parede direita para o novo limite, removi o `Wall_RightBoundary` manual (já não é preciso, a tilemap estendida sela sozinha) e reespacei os 13 objetos com ~0.5u de folga entre cada um. Confirmado: casts em ambas as direções a partir do spawn do Tomás dão distâncias positivas; 37/37 testes continuam a passar; captura do Game View confirma os gaps visíveis.
- 2026-10-20 — **Etapa A confirmada pelo Dinis** ("tudo ok" — moveu a câmara manualmente em Play Mode só para ver melhor, não precisa de guardar, o Unity descarta isso ao sair do Play). Andar/correr/agachar/direção, colisão com paredes/mobiliário/inimigos e ordem de desenho todos a funcionar como esperado na sandbox. Etapa B (escadas, empurrar caixas, tolerância de queda, testes que faltam) fica para a próxima sessão de trabalho.
- 2026-10-20 — Animação de "Push" preparada com antecedência para a Etapa B: Dinis gerou `Tomas_Push.png` (4 frames, 32×48px, pivot base igual aos restantes) via Unity AI Assistant a partir de um brief com os parâmetros técnicos exatos do `Tomas.png` existente. Já veio corretamente cortado e com `Tomas_Push.anim` já criado (5 keyframes a 5fps, loop fechado 0→0.8s). Só faltava ligar ao Animator: acrescentado o estado `Push` ao `Tomas_Controller`, com transições `AnyState → Push` (`IsPushing` true) e `Push → Idle` (`IsPushing` false). Verificado em Play Mode (`Animator.SetBool`) que entra e sai do estado corretamente, sprite `Tomas_Push_0` a renderizar. A lógica de `Pushable`/empurrar caixas em si continua por fazer (Etapa B).

## Ideias
- "Coyote time" e buffer de input na entrada de escadas se parecer rígido.
