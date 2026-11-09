# F4-06 — Créditos
**Estado:** TODO · **Depende de:** F4-01 · **Assemblies:** Aurora.UI

## Objetivo
Um ecrã de créditos partilhado: acessível do Menu Principal (`uispec.md` §3.2) e reutilizado no final do jogo, a rolar sobre o livro de hóspedes a fechar (`uispec.md` §3.10, F6-06).

## Contexto na história
No final, os créditos rolam sobre o livro de hóspedes a fechar; terminam com a chave de latão e o nome do Tomás.

## Âmbito
**Inclui:** `CreditsScreen` (UI Toolkit) com texto a rolar a velocidade configurável, `Back` cancela (chamado do Menu) ou salta para o fim, conteúdo de créditos num SO/JSON simples (não hard-coded — mas sem necessidade de `Localization`, por serem nomes próprios).
**Não inclui:** a cena do livro de hóspedes a fechar em si (arte/animação, F6-06) — este item só fornece o ecrã/rolo de texto que a F6-06 invoca por cima.

## Especificação
1. `CreditsScreen`: lista de secções (estúdio, cargos, agradecimentos) definida num asset simples; rola a velocidade constante configurável.
2. Invocado do Menu Principal sobre um fundo próprio; `Back` sai a qualquer momento e volta ao Menu.
3. Invocado pela F6-06 sobre o livro de hóspedes a fechar — aceita um parâmetro de fundo transparente para esse caso (a F6-06 decide se aí também se pode saltar).

## Critérios de aceitação
- [ ] Créditos do Menu Principal rolam do início ao fim; `Back` volta ao Menu a qualquer momento.
- [ ] O mesmo ecrã funciona com fundo transparente (verificado visualmente sobre um placeholder colorido).

## Testes
- PlayMode: `Back` fecha o ecrã e devolve o foco ao Menu Principal.

## Registo

## Ideias
