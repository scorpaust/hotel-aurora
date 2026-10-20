# F6-03 — Salão de Festas: Os Noivos
**Estado:** TODO · **Depende de:** F6-02 · **Assemblies:** conteúdo + Aurora.Hotel (loop temporal)

## Objetivo
Quarta ala. Introduz o **loop**: um casamento que se repete e que o jogador tem de alterar pequeno detalhe a pequeno detalhe.

## Contexto na história
"Os Noivos, unidos por um casamento que se repete há décadas e separados por um segredo que nenhum deles quer recordar."

> **Decisão a tomar antes de implementar:** qual é o segredo e se a resolução liberta os dois, um ou nenhum.

## Âmbito
**Inclui:** `SCN_Ballroom` (salão, capela do hotel, camarins, varanda), loop de ~3 min com estado que reinicia, dois "bosses" ligados, fragmento 5/5 do Tomás, memória dos Noivos.

## Especificação
1. **Loop:** `LoopController` reinicia salas e NPCs a cada ciclo; **mantém** o inventário do jogador e flags marcadas como `persistentInLoop`. Sinal claro de reinício (relógio do salão, valsa a recomeçar).
2. Cada ciclo o jogador pode mudar uma coisa (trocar cartões de mesa, parar o relógio, revelar com a lanterna uma carta escondida) → o casamento desenrola-se de forma diferente.
3. Os Noivos perseguem em conjunto mas separam-se quando um deles é iluminado (o outro fica cego de ciúme/medo) — uso duplo da lanterna.
4. Resolução: o segredo revelado aos dois ao mesmo tempo.
5. Recompensa: fragmento 5/5 → **memória completa do Tomás**: o pai apagou-lhe a noite para o proteger. Cena importante (Timeline), prepara o último andar.

## Critérios de aceitação
- [ ] Loop reinicia de forma fiável 20 vezes seguidas sem fugas de estado.
- [ ] O jogador percebe que está num loop sem texto explicativo (testar com 2 pessoas).
- [ ] Memória completa do Tomás desbloqueada e no livro.

## Testes
- EditMode: separação entre estado persistente e reiniciável.
- PlayMode: 3 ciclos com alterações diferentes.

## Registo

## Ideias
