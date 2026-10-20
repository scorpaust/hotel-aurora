# F1-10 — Fluxos de nível e gerador
**Estado:** TODO · **Depende de:** F1-09 · **Assemblies:** Aurora.Hotel, Aurora.Editor

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
3. `RoomDefinition` (já existe, `codespec.md` §5.4) ganha um campo `LevelFlowDefinition` opcional — a ala "escolhe" o fluxo que usa.
4. Dois fluxos de exemplo, como assets concretos, refletindo os dois padrões já descritos: (a) `Cutscene → Exploration → EnemyEncounter → TrapGauntlet → Maze → NarrativeInteraction → Boss`; (b) `Exploration → Cutscene → MiniBoss → EnemyEncounter → TrapGauntlet → BonusLevel → NarrativeInteraction → Maze → Boss`.
5. `LevelGeneratorWindow` (`Aurora.Editor`, `EditorWindow`, menu **Aurora/Level Generator**): escolhe um `LevelFlowDefinition` + uma `RoomDefinition` alvo; para cada beat, instancia os prefabs/triggers correspondentes na cena ativa, na ordem do fluxo, com espaçamento configurável. Não decide sozinho *quais* inimigos/cutscenes usar além do que já está no `LevelFlowDefinition` — é montagem assistida, com o Dinis a rever/ajustar cada ala gerada no Editor, não um botão "gerar jogo completo".
6. Corre inteiramente em `Aurora.Editor`; zero destas classes de ferramenta entram num build de jogador.

## Critérios de aceitação
- [ ] `LevelFlowDefinition` com os 2 fluxos de exemplo, visíveis e editáveis no Inspector.
- [ ] `LevelGeneratorWindow` abre a partir do menu, lista os `LevelFlowDefinition` existentes, e ao escolher um + aplicar, instancia placeholders na cena ativa na ordem certa dos beats (nem que sejam cubos/retângulos com o nome do beat, até existir conteúdo real de cada tipo).
- [ ] Gerar duas vezes seguidas (fluxos diferentes) não deixa lixo da tentativa anterior nem GameObjects duplicados.
- [ ] Nenhum destes scripts é referenciado por `Aurora.Gameplay`/`Aurora.Hotel` em runtime (só Editor).

## Testes
- EditMode: validação de que um `LevelFlowDefinition` com beats vazios/mal configurados é rejeitado com erro claro antes de gerar; ordem dos beats preservada depois de serializar/desserializar.

## Registo

## Ideias
- Pré-visualização do fluxo (lista horizontal de ícones por beat) na própria `LevelGeneratorWindow`, antes de gerar.
- Suporte a reordenar beats por drag-and-drop no Inspector do `LevelFlowDefinition`.
