# F4-01 — Menus, pausa e opções
**Estado:** TODO · **Depende de:** F1-06, F2-04 · **Assemblies:** Aurora.UI

## Objetivo
Todos os ecrãs de sistema de `uispec.md` §3 implementados em UI Toolkit, com o tema Aurora e navegação completa por gamepad.

## Contexto na história
O menu principal é a fachada do Aurora à noite, com as janelas a acender.

## Âmbito
**Inclui:** splash/aviso, menu principal, slots (novo/continuar/carregar/apagar), pausa, opções (áudio, vídeo, controlos-base, idioma), game over, créditos placeholder, `UIRouter`, tema `Aurora.tss`, `PanelSettings`.
**Não inclui:** rebinding e acessibilidade completos (F4-02).

## Especificação
1. `UIRouter` com pilha de `UIScreen`; `Back` fecha o topo; foco inicial sempre definido.
2. Tema com variáveis de `uispec.md` §1; molduras 9-slice placeholder; fontes incluídas.
3. Menu principal: Continuar (só se houver save), Novo jogo (escolhe slot), Carregar, Opções, Créditos, Sair (oculto no Web).
4. Splash: aviso de conteúdo + "Carregue em qualquer tecla" (no Web dá foco e desbloqueia áudio).
5. Opções gravadas no ficheiro de definições (F1-06), aplicadas de imediato, com "Repor predefinições".
6. Vídeo no Web: esconder resolução/V-Sync; mostrar só fullscreen e qualidade.
7. Pausa: Retomar, Livro de hóspedes, Opções, Voltar ao último checkpoint (confirmação), Menu principal (confirmação).
8. Testar 16:9, 16:10 e 21:9.

## Critérios de aceitação
- [ ] Todo o fluxo menu → jogo → pausa → opções → menu sem rato.
- [ ] Opções persistem entre sessões (PC e Web).
- [ ] Nenhuma cor literal fora do tema; nenhum texto hard-coded.
- [ ] Layout correto nas três proporções.

## Testes
- PlayMode: `UIRouter` empilha/desempilha e devolve foco ao ecrã anterior.

## Registo

## Ideias
