# Funcionalidade Atual

<!-- Ver especificação completa em context/features/F1-nucleo/F1-09-definicoes-configuraveis.md -->

# F1-09 — Definições configuráveis de conteúdo

## Estado

Em revisão (REVIEW) — à espera de teste/confirmação

## Objetivos

Dar a inimigos, bosses, armadilhas, cutscenes, áudio e narrativa a mesma abordagem orientada a dados que `ItemDefinition`/`MemoryDefinition`/`DialogueGraph` já têm — para que cada ala da história se monte a partir de peças reutilizáveis.

- `EnemyDefinition`, `BossDefinition`, `TrapDefinition`, `CutsceneDefinition`, `MusicCueDefinition`/`SfxCueDefinition`/`VfxCueDefinition`, `NarrativeBeatDefinition` (SOs em `Aurora.Hotel`/`Aurora.Narrative`/`Aurora.Audio`).
- Validador de Editor para ids duplicados/campos obrigatórios em falta.

**Não inclui:** o sistema de fluxos/gerador que os combina (F1-10, feature seguinte); lógica de IA (Fase 3+); conteúdo real de cada boss nomeado (Fase 5/6).

## Critérios de aceitação

- [ ] As 9 classes de *Definition compilam e aparecem no menu de criação de assets.
- [ ] Pelo menos um asset de exemplo de cada tipo, criado e preenchido.
- [ ] `BossDefinition` com 2+ fases de exemplo mostra a lista ordenada corretamente no Inspector.
- [ ] Validador deteta um id duplicado proposital e reporta erro claro.
- [ ] Nenhuma destas classes introduz lógica de gameplay.

## Histórico

<!-- Manter atualizado. Da mais antiga para a mais recente -->

- 2026-10-20 — Pedida uma reconfiguração arquitetural: sistemas de conteúdo orientados a dados (inimigos/bosses/armadilhas/cutscenes/áudio/narrativa configuráveis, com propriedades herdadas) e um sistema de fluxos de nível + gerador no menu do Unity, antes de continuar a Etapa B da F1-01 (empurrar/puxar).
- 2026-10-20 — Esclarecida a dúvida central antes de avançar: este sistema serve para construir a **mesma história fixa** mais depressa (não torna o jogo procedimental/variável), e as "armas" dos inimigos/bosses são objetos de identidade narrativos ligados à lanterna, não combate — mantém `codespec.md` §1 ("sem combate direto") intacto.
- 2026-10-20 — Registadas duas novas features na Fase 1: F1-09 (definições configuráveis) e F1-10 (fluxos de nível + gerador), com F5-03/Fase 6 a passarem a depender de F1-10. `codespec.md` §5.4/§5.6 atualizado com o novo modelo de dados. Branch `feature/f1-09-definicoes-configuraveis` criada a partir de `main` (independente da F1-01, cujo Etapa B fica em pausa).
- 2026-10-20 — F1-09 implementada: `EnemyDefinition`/`BossDefinition`/`TrapDefinition` (Hotel), `CutsceneDefinition`/`NarrativeBeatDefinition` (Narrative), `MusicCueDefinition`/`SfxCueDefinition`/`VfxCueDefinition` (Audio), mais `IContentDefinition`/`DialogueGraph`/`MemoryDefinition`/`GuestBookEntry` (Core, partilhados). `Aurora.Hotel` passou a poder referenciar `Aurora.Narrative`/`Aurora.Audio` (decisão de arquitetura registada no Registo da F1-09). Validador de ids duplicados/campos vazios, 11 assets de exemplo (incluindo um Boss com 2 fases), 28/28 testes a passar. Estado passa a REVIEW.
