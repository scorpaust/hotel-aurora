# F3-01 — Esconderijos e furtividade
**Estado:** TODO · **Depende de:** F1-03 · **Assemblies:** Aurora.Gameplay

## Objetivo
Sem combate, a defesa do Tomás é não ser visto nem ouvido. Esconderijos e ruído dão ao jogador controlo sobre o medo.

## Contexto na história
Armários de quartos, debaixo de camas, carrinhos de limpeza, cortinas do salão, cacifos da piscina.

## Âmbito
**Inclui:** `HidingSpot` (entrar/sair/espreitar), sistema de ruído (passos, correr, objetos derrubados), perceção base dos inimigos (visão em cone + audição), lanterna como fonte de deteção, prender a respiração.
**Não inclui:** IA específica de bosses (F5/F6), perseguições (F3-02).

## Especificação
1. `HidingSpot : IInteractable`: entrar (animação curta), ficar escondido (sprite oculto, câmara aproxima ligeiramente), `Move` horizontal para espreitar, sair. Alguns esconderijos são "conhecidos" por certos inimigos (flag).
2. **Respirar:** dentro do esconderijo, com inimigo perto, `Interact` (hold) prende a respiração até 4 s; soltar cedo demais faz ruído.
3. **Ruído:** `NoiseEmitter` com raio e intensidade — andar (baixo), correr (alto), agachado (nenhum), objetos que caem (alto). Pisos podem multiplicar (vidro partido, madeira velha).
4. **Perceção (`Perception`)**: visão em cone com raycast a paredes; audição por raio; **lanterna ligada aumenta muito a visibilidade**; estados `Idle → Suspicious → Searching → Alert`.
5. Gizmos de debug para cones e raios.

## Critérios de aceitação
- [ ] Inimigo de teste em patrulha não deteta jogador escondido nem agachado no escuro.
- [ ] Correr perto dele leva a `Searching`; lanterna à vista leva a `Alert`.
- [ ] Soltar a respiração cedo com inimigo perto revela o jogador.
- [ ] Zero alocações por frame na perceção.

## Testes
- EditMode: transições de estado da perceção com estímulos simulados.
- PlayMode: patrulha + esconderijo.

## Registo

## Ideias
