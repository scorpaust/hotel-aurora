# F1-11 — HUD de jogo
**Estado:** TODO · **Depende de:** F1-03, F1-04, F1-05, F1-06 · **Assemblies:** Aurora.UI

## Objetivo
Todos os elementos do HUD de `uispec.md` §3.3 — o jogo não tem barras de vida nem HUD permanente; cada elemento aparece só quando é preciso e desaparece sozinho (`uispec.md` §2.1: "HUD quase inexistente").

## Contexto na história
A luz da lanterna, o som e a Beatriz comunicam; os números não.

## Âmbito
**Inclui:** indicador de ferramenta ativa (2s, depois esconde), indicador da lanterna a sobreaquecer, vinheta/pulsação de perigo durante perseguições (sem barra de vida), notificação de memória recuperada (desliza para fora), legenda de checkpoint ("Progresso guardado", 1,5s). O prompt de interação já existe desde a F1-03 (`InteractionPromptPanel`) — este item só o integra formalmente na mesma camada HUD, se ainda não estiver.
**Não inclui:** legendas de diálogo (F2-01), indicadores de acessibilidade específicos (F4-02).

## Especificação
1. Um único `UIDocument` de camada HUD (`PanelSettings` próprio, como a F1-03 já estabeleceu com `SO_HUDPanelSettings`) — todos os elementos acima vivem na mesma raiz.
2. Cada elemento ouve o `GameEvent`/`GameEvent<T>` relevante (ferramenta trocada, lanterna a sobreaquecer, perseguição a começar/acabar, memória recuperada, checkpoint atingido) — ao contrário do prompt de interação da F1-03 (que segue posição no mundo a cada frame, por necessidade), estes são orientados a eventos discretos, não a polling.
3. Transições USS 120–250 ms (`uispec.md` §1); nada pisca mais de 3 Hz.
4. Testar com teclado e gamepad (são só feedback, não precisam de navegação).

## Critérios de aceitação
- [ ] Trocar de ferramenta mostra o indicador 2s e esconde sozinho.
- [ ] Entrar em perseguição ativa a vinheta vermelha; sair repõe.
- [ ] Recuperar uma memória de teste mostra a notificação e desliza para fora.
- [ ] Atingir um checkpoint mostra a legenda 1,5s.

## Testes
- PlayMode: cada evento de teste ativa o elemento certo e esconde-o a tempo.

## Registo

## Ideias
