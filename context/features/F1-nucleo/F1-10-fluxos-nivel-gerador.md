# F1-10 — Fluxos de nível e gerador
**Estado:** REVIEW · **Depende de:** F1-09 · **Assemblies:** Aurora.Hotel, Aurora.Narrative, Aurora.Editor

## Objetivo
Descrever cada ala como uma sequência ordenada de "beats" (cutscene, exploração, inimigos, armadilhas, labirinto, interação narrativa, miniboss, bónus, boss) e dar uma ferramenta de Editor que monta a cena da ala a partir dessa sequência — para que montar uma ala nova seja compor peças já feitas, não escrever cada uma do zero.

## Contexto na história
Nenhum direto — infraestrutura de autoria. A ordem real dos beats de cada ala (Receção, Quartos, Salão de Banquetes, etc.) continua a ser a da história definida no projeto; este sistema só a torna mais rápida de montar e mais fácil de ajustar.

## Âmbito
**Inclui:** `LevelBeatType` (enum), `LevelFlowDefinition` (SO), associação de `LevelFlowDefinition` a `RoomDefinition`, `LevelGeneratorWindow` (ferramenta de Editor) que monta uma cena a partir de um fluxo + das *Definition concretas escolhidas.
**Não inclui:** geração procedimental em runtime (nunca existe — a história é sempre fixa); a lógica de jogo de cada beat (ex.: a IA de `EnemyEncounter` é Fase 3+, o puzzle de `Boss` é Fase 5/6); migração automática de alas já existentes — cada ala adota isto quando for construída/revista.

## Especificação
1. `LevelBeatType` (enum, `Aurora.Hotel`): `Cutscene, Exploration, EnemyEncounter, TrapGauntlet, Maze, NarrativeInteraction, MiniBoss, BonusLevel, Boss`.
2. `LevelFlowDefinition` (SO, `Aurora.Hotel`): lista ordenada de `LevelBeat` (struct/classe serializável: `LevelBeatType` + referências às *Definition concretas desse beat — ex. lista de `EnemyDefinition` para `EnemyEncounter`, uma `CutsceneDefinition` para `Cutscene`, uma `BossDefinition` para `Boss`). Vários `LevelFlowDefinition` podem existir como assets independentes e reutilizáveis.
3. `RoomDefinition` (`Aurora.Hotel` — ainda não existia em código, só na tabela de `codespec.md` §5.4; criado aqui como placeholder mínimo) com um campo `LevelFlowDefinition` opcional — a ala "escolhe" o fluxo que usa. Ligações/spawn points reais ficam para a F1-02.
4. Dois fluxos de exemplo, como assets concretos, refletindo os dois padrões já descritos: (a) `Cutscene → Exploration → EnemyEncounter → TrapGauntlet → Maze → NarrativeInteraction → Boss`; (b) `Exploration → Cutscene → MiniBoss → EnemyEncounter → TrapGauntlet → BonusLevel → NarrativeInteraction → Maze → Boss`.
5. `LevelGeneratorWindow` (`Aurora.Editor`, `EditorWindow`, menu **Aurora/Level Generator**): escolhe um `LevelFlowDefinition` + uma `RoomDefinition` alvo; para cada beat, instancia os prefabs/triggers correspondentes na cena ativa, na ordem do fluxo, com espaçamento configurável. Não decide sozinho *quais* inimigos/cutscenes usar além do que já está no `LevelFlowDefinition` — é montagem assistida, com cada ala gerada a ser revista/ajustada no Editor, não um botão "gerar jogo completo".
6. Corre inteiramente em `Aurora.Editor`; zero destas classes de ferramenta entram num build de jogador.

## Critérios de aceitação
- [x] `LevelFlowDefinition` com os 2 fluxos de exemplo, visíveis e editáveis no Inspector.
- [x] `LevelGeneratorWindow` abre a partir do menu, lista os `LevelFlowDefinition` existentes, e ao escolher um + aplicar, instancia placeholders na cena ativa na ordem certa dos beats (nem que sejam cubos/retângulos com o nome do beat, até existir conteúdo real de cada tipo).
- [x] Gerar duas vezes seguidas (fluxos diferentes) não deixa lixo da tentativa anterior nem GameObjects duplicados.
- [x] Nenhum destes scripts é referenciado por `Aurora.Gameplay`/`Aurora.Hotel` em runtime (só Editor).

## Testes
- EditMode: validação de que um `LevelFlowDefinition` com beats vazios/mal configurados é rejeitado com erro claro antes de gerar; ordem dos beats preservada depois de serializar/desserializar. 5 testes (`LevelFlowTests`) a passar.

## Registo
- 2026-10-21 — Implementado. `LevelBeatType`/`LevelBeat`/`LevelFlowDefinition` em `Aurora.Hotel`; `RoomDefinition` criado (não existia em código, só na tabela do `codespec.md`) como placeholder mínimo. `LevelFlowValidator` (lógica separada da janela, testável) + `LevelGeneratorWindow` (menu **Aurora/Level Generator**) em `Aurora.Editor`, que passou a poder referenciar `Aurora.Hotel`/`Aurora.Narrative` (`codespec.md` §4 atualizado — é tooling, nunca entra num build de jogador).
  - Placeholders: um sprite partilhado (`SPR_LevelBeatPlaceholder`, 32×32/100PPU, mesma convenção da F1-08) tingido por `SpriteRenderer.color` consoante o tipo de beat (paleta de `uispec.md` §1), em vez de 9 sprites separados.
  - Geração: limpa sempre o `LevelFlow_Generated` anterior antes de criar outro — confirmado por invocação direta (via reflexão) de `Generate()` duas vezes seguidas com fluxos diferentes: fica sempre exatamente uma raiz `LevelFlow_Generated`, com os beats e o espaçamento do último fluxo gerado, sem lixo da tentativa anterior.
  - 2 `LevelFlowDefinition` de exemplo (`SO_LevelFlow_PatternA`/`PatternB`, os dois padrões descritos no pedido original) + 1 `RoomDefinition` de exemplo a apontar para o PatternA. `LevelFlowValidator` confirmado contra os dois (passam) e contra casos propositadamente mal configurados (falham com mensagem clara). 33/33 testes EditMode a passar; `ContentDefinitionValidator.Validate()` também limpo com os novos assets.
  - Estado passa a REVIEW.

## Ideias
- Pré-visualização do fluxo (lista horizontal de ícones por beat) na própria `LevelGeneratorWindow`, antes de gerar.
- Suporte a reordenar beats por drag-and-drop no Inspector do `LevelFlowDefinition`.
