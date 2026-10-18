# F6-04 — Bagageira: O Porteiro
**Estado:** TODO · **Depende de:** F6-03 · **Assemblies:** conteúdo

## Objetivo
Quinta ala e a mais agressiva. O Porteiro é quem impede as fugas — o confronto é sobre **deixar partir** quem tentou sair.

## Contexto na história
"O Porteiro, que transporta às costas todas as malas dos que tentaram fugir — com alguma coisa ainda a mexer-se lá dentro."

> **Decisão a tomar com o Dinis:** quem era o Porteiro e porque se tornou guardião das saídas.

## Âmbito
**Inclui:** `SCN_Luggage` (átrio de serviço, bagageira, garagem, elevador de carga, escadaria principal), o Porteiro, mecânica das **malas**, perseguição mais longa do jogo até aqui, acesso ao último andar.

## Especificação
1. **Malas:** espalhadas pela ala; abrir com ferramentas revela objetos de hóspedes que fugiram (entradas do livro). Algumas contêm fantasmas presos que, libertados, enfraquecem o Porteiro (menos malas às costas = mais lento).
2. O Porteiro guarda as saídas: qualquer tentativa de ir para a porta da rua aciona perseguição.
3. Confronto: elevador de carga com o Porteiro a tentar fechá-lo (reutiliza o "elevador com medo" da F3-03); lanterna revela, em cada mala, de quem era.
4. Resolução: Libertar = devolver as malas aos donos (fantasmas libertados saem pela escadaria). Selar = ele fica preso na garagem.
5. Contagem de hóspedes libertados ao longo do jogo é lida aqui (pequena variação de linhas).

## Critérios de aceitação
- [ ] A velocidade do Porteiro depende do número de malas libertadas.
- [ ] Perseguição longa com 2 checkpoints internos.
- [ ] Libertar/Selar funcionam.

## Testes
- PlayMode smoke do confronto.

## Registo

## Ideias
