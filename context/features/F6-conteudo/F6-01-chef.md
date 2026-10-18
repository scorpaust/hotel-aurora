# F6-01 — Salão de Banquetes: O Chef
**Estado:** TODO · **Depende de:** F5-03 · **Assemblies:** conteúdo (+ mecânicas novas em Aurora.Gameplay se necessário)

## Objetivo
Segunda ala. Introduz **tentação**: o hotel oferece coisas ao jogador e aceitá-las tem custo.

## Contexto na história
"O Chef do Banquete, uma criatura que prepara as refeições preferidas das vítimas e transforma a saudade numa fome impossível de saciar." O jantar servido para convidados invisíveis na receção vinha daqui.

> **Decisão a tomar com o Dinis:** quem era o Chef e qual a refeição que ele próprio perdeu.

## Âmbito
**Inclui:** `SCN_Banquet` (salão, cozinhas, câmara frigorífica, monta-cargas), o Chef, mecânica de **pratos-tentação**, fragmento 3/5 do Tomás, memória do Chef.

## Especificação
1. **Pratos-tentação:** mesas com a comida preferida de alguém. Comer (Interact) dá um benefício imediato (lanterna arrefece de vez, abre caminho) mas **apaga uma entrada do livro de hóspedes** ou um fragmento menor (flag `tentacao.aceite.*`). O jogo nunca obriga a comer.
2. Um dos pratos é o bolo de laranja (ligação ao quarto da filha do fundador) — não comível; revelado pela lanterna como pista do final.
3. Investigação: cozinhas como labirinto de vapor (visibilidade reduzida), o Chef guiado pelo **cheiro/ruído** (perceção auditiva aumentada).
4. Confronto: câmara frigorífica e cozinha principal; usar a lanterna para mostrar ao Chef o que ele realmente cozinhava (o prato da sua memória) e o monta-cargas para fugir/voltar.
5. Recompensa: fragmento 3/5 (o pai a fazer o pequeno-almoço na cozinha do hotel — prefigura o final).

## Critérios de aceitação
- [ ] Ala completa sem comer nada, e também comendo tudo — ambos jogáveis.
- [ ] Custos da tentação refletidos no livro e no save.
- [ ] Libertar/Selar funcionam pelo framework F5-02.

## Testes
- PlayMode smoke do confronto.

## Registo

## Ideias
