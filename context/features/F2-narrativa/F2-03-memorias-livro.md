# F2-03 — Memórias e livro de hóspedes
**Estado:** TODO · **Depende de:** F2-01 · **Assemblies:** Aurora.Narrative, Aurora.UI

## Objetivo
As memórias são a moeda da história. Recuperá-las desbloqueia cenas, verdade sobre os monstros e progressão. O livro de hóspedes reúne tudo.

## Contexto na história
O hotel rouba memórias "uma noite de cada vez". Cada confronto devolve uma. Tomás também recupera fragmentos da noite em que o pai o fez esquecer.

## Âmbito
**Inclui:** `MemoryDefinition`, fragmentos (colecionáveis que compõem uma memória), cena de memória (Timeline em sala "do passado" jogável curta), `GuestBookEntry`, livro de hóspedes (`uispec.md` §3.6), notas do pai, planta do hotel.
**Não inclui:** conteúdo final de cada memória (F5/F6 preenchem).

## Especificação
1. `MemoryDefinition`: id, ala, título/descrição localizados, fragmentos necessários (0–3), `Timeline` ou cena de memória, flags resultantes, fotografia (sprite).
2. Tipos: **Memória de hóspede** (de um boss — revela quem foi), **Memória do Tomás** (a noite esquecida, 5 partes ao longo do jogo), **Memória do fundador** (final).
3. Recuperar → notificação (fotografia a revelar) → se completa, toca cena; regista em `WorldFlags`.
4. **Cena de memória:** a sala muda para a versão do passado (paleta quente, sem ameaça); o jogador anda livremente 30–90 s; termina por gatilho.
5. Livro de hóspedes com 4 abas (Memórias, Hóspedes, Notas do pai, Planta). Entradas desbloqueadas por flags. **A página da Beatriz existe mas fica em branco até ao final.**
6. Planta: salas visitadas por andar, gerada a partir de `RoomDefinition` + flags de visita.

## Critérios de aceitação
- [ ] Recolher 3 fragmentos de teste completa uma memória e toca a cena.
- [ ] Livro mostra entradas certas conforme flags, navegável só com gamepad.
- [ ] Planta atualiza ao visitar uma sala nova.
- [ ] Tudo persiste no save.

## Testes
- EditMode: desbloqueio de entradas por flags; progresso de fragmentos.
- PlayMode: cena de memória entra e sai sem perder estado.

## Registo

## Ideias
