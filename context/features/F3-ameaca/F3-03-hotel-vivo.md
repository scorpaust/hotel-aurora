# F3-03 — Eventos do hotel vivo
**Estado:** TODO · **Depende de:** F3-02 · **Assemblies:** Aurora.Hotel

## Objetivo
O próprio edifício é um antagonista. Um diretor de eventos dispara armadilhas e anomalias com ritmo, para que o hotel pareça vivo sem se tornar previsível nem cansativo.

## Contexto na história
"Escadas dobram-se sob os seus pés, elevadores tentam esmagá-lo, espelhos devolvem movimentos diferentes dos seus e quartos inteiros desfazem-se quando ele encontra a saída." O pai, em segredo, sabota portas e envia avisos pelas instalações elétricas.

## Âmbito
**Inclui:** `HotelDirector` (ritmo/tensão), `HotelEventDefinition`, 6 eventos-base, avisos do pai.
**Não inclui:** eventos únicos de cada ala (criados nas features de conteúdo com estas peças).

## Especificação
1. **HotelDirector:** valor de tensão 0–1 que sobe com tempo em sala/ruído e desce após eventos; dispara eventos elegíveis da sala com cooldown global e por tipo; nunca durante diálogo/memória; eventos scriptados ignoram o diretor.
2. **Eventos-base** (cada um `HotelEventDefinition` + prefab):
   - **Escada que se dobra** — degraus inclinam até rampa; o jogador escorrega para baixo se não sair em 1.5 s.
   - **Elevador com medo** — elevador que treme, recusa andares, pode fechar portas para esmagar (fugir a tempo; falhar = apanhado).
   - **Espelho dessincronizado** — o reflexo atrasa-se e depois faz outra coisa; alguns espelhos mostram a solução de um puzzle.
   - **Corredor que muda** — ao sair do campo de visão (porta fechada atrás), a `RoomConnection` muda de destino (usa F1-02).
   - **Quarto que se desfaz** — ao encontrar a saída, o quarto colapsa (tiles a cair, sprites a desintegrar) e começa uma mini-perseguição.
   - **Campainha / vozes** — sons de receção e chamadas pelo nome em salas vazias (sem perigo, só tensão).
3. **Avisos do pai:** `ElectricalHint` — lâmpadas que piscam em código (padrão) indicando caminho seguro; tomadas que faíscam junto a ferramentas escondidas. Desbloqueiam "Notas do pai" (F2-03) quando reconhecidos.
4. Todos os eventos respeitam "reduzir flashes" e "reduzir tremor de câmara" (F4-02).

## Critérios de aceitação
- [ ] Os 6 eventos funcionam isolados numa sala de teste.
- [ ] Com o diretor ativo 10 min numa ala de teste: nenhum evento repetido seguido, nenhum durante diálogo.
- [ ] Corredor que muda altera o destino sem o jogador ver a troca.
- [ ] Aviso elétrico leva a uma ferramenta escondida.

## Testes
- EditMode: elegibilidade e cooldowns do diretor.
- PlayMode: elevador esmaga se o jogador ficar; não esmaga se sair.

## Registo

## Ideias
- Eventos que respondem à forma de jogar (quem usa muito a lanterna vê mais espelhos).
