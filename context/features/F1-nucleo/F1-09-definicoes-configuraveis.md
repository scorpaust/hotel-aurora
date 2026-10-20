# F1-09 — Definições configuráveis de conteúdo
**Estado:** DONE · **Depende de:** F1-07 · **Assemblies:** Aurora.Core, Aurora.Hotel, Aurora.Narrative, Aurora.Audio

## Objetivo
Dar a inimigos, bosses, armadilhas, cutscenes, áudio e narrativa a mesma abordagem orientada a dados que `ItemDefinition`/`MemoryDefinition`/`DialogueGraph` já têm — para que cada ala da história se monte a partir de peças reutilizáveis em vez de código/cenas escritos à mão do zero. Isto não muda a história nem introduce combate: é só a camada de dados por baixo do conteúdo que já está planeado.

## Contexto na história
Nenhum direto — é infraestrutura de conteúdo. Serve para construir, mais depressa e de forma consistente, os confrontos já definidos (Governanta, Chef, Nadador do Fundo, Noivos, Porteiro, Pai) e os inimigos genéricos de cada ala.

## Âmbito
**Inclui:** `EnemyDefinition`, `BossDefinition`, `TrapDefinition`, `CutsceneDefinition`, `MusicCueDefinition`/`SfxCueDefinition`/`VfxCueDefinition`, `NarrativeBeatDefinition` (SOs, `codespec.md` §5.4); validação básica no Editor (ids únicos, referências obrigatórias preenchidas).
**Não inclui:** o sistema de fluxos/gerador que os combina (F1-10); lógica de IA dos inimigos (Fase 3+); lógica real de armadilhas (`ChaseObstacle` é da F3-02); conteúdo real de cada boss nomeado (Fase 5/6 — aqui só o *schema*, não os dados preenchidos de cada um).

## Especificação
1. `EnemyDefinition` (SO, `Aurora.Hotel`): `id` (string estável), nome localizado, `RuntimeAnimatorController`, `Sprite`/spritesheet associado, nível de dificuldade (enum `EnemyDifficulty { Low, Medium, High }`), objeto de identidade (`Sprite` — o prop visual que a lanterna revela; nunca uma arma de combate), parâmetros base de comportamento (velocidade, raio de deteção — campos simples, sem lógica associada ainda).
2. `BossDefinition` (SO, `Aurora.Hotel`) : contém (não herda de, para evitar acoplar serialização) um `EnemyDefinition` base + lista ordenada de fases (`BossPhase`: nome, comportamento/puzzle associado à lanterna, condição de transição como string livre por agora), referência a `MemoryDefinition` e a `GuestBookEntry`.
3. `TrapDefinition` (SO, `Aurora.Hotel`): id, nome, tipo (enum `TrapType` com valores placeholder — ex. `Mechanical`, `Environmental`), gatilho (string livre), intensidade narrativa (enum `Low/Medium/High`, nunca dano numérico), referências a `VfxCueDefinition`/`SfxCueDefinition`, cooldown em segundos.
4. `CutsceneDefinition` (SO, `Aurora.Narrative`): id, referência a `UnityEngine.Timeline.TimelineAsset`, condição de disparo (string livre / referência a `WorldFlags`).
5. `MusicCueDefinition`, `SfxCueDefinition`, `VfxCueDefinition` (SOs, `Aurora.Audio`): id, referência ao `AudioClip`/prefab de partículas, parâmetros (volume 0-1, loop bool, prioridade). Pensados para serem consumidos pelo `IAudioService` quando a F4-03 o implementar — por agora são só dados, sem integração de runtime.
6. `NarrativeBeatDefinition` (SO, `Aurora.Narrative`): id, referência a `DialogueGraph`, chave de legenda.
7. Todos os SOs seguem a convenção já estabelecida: `id` estável, nunca gravar referências diretas a assets fora do próprio SO, prefixo `SO_` no nome do ficheiro.
8. Validador de Editor (`Aurora.Editor`, extensão do `ProjectValidator` da F0-01 ou um validador novo): percorre os SOs de cada tipo e reporta ids duplicados ou campos obrigatórios vazios.

## Critérios de aceitação
- [x] As classes de *Definition compilam e aparecem no menu de criação de assets (`Assets/Create/Aurora/Content/...`).
- [x] Pelo menos um asset de exemplo de cada tipo, criado e preenchido, para validar o schema na prática (sem precisar de ser conteúdo final).
- [x] `BossDefinition` com 2+ fases de exemplo mostra a lista ordenada corretamente no Inspector.
- [x] Validador deteta um id duplicado proposital e reporta erro claro.
- [x] Nenhuma destas classes introduz lógica de gameplay (puro dado — sem `MonoBehaviour`, sem `Update`).

## Testes
- EditMode: validador de ids duplicados/campos obrigatórios; serialização/desserialização round-trip de `BossDefinition` com fases. 5 testes (`ContentDefinitionValidatorTests`) a passar.

## Registo
- 2026-10-20 — Implementado. Decisão de arquitetura: os tipos pura-dado partilhados entre domínios (`DialogueGraph`, `MemoryDefinition`, `GuestBookEntry` — placeholders mínimos até F2-01/F2-03 os desenvolverem a sério) ficam em `Aurora.Core`, porque `BossDefinition` (Hotel) precisa de os referenciar e `Narrative`/`Audio` são irmãos de `Hotel` no grafo de dependências (não se veem um ao outro). Em vez disso, `Aurora.Hotel` passou a poder referenciar `Aurora.Narrative` e `Aurora.Audio` diretamente (`codespec.md` §4 atualizado) — é uma direção nova e deliberada, não um atalho.
  - `IContentDefinition` (Core): contrato mínimo (`Id`) partilhado por todas as *Definition, para o validador funcionar sem depender de `Hotel`/`Narrative`/`Audio`.
  - `ContentDefinitionValidator` (Aurora.Editor): aceita uma lista de candidatos opcional (testável sem tocar no `AssetDatabase`) e por omissão varre o projeto todo. Testado com candidatos falsos (5 testes) e, à parte, confirmado contra os assets reais do projeto (duplicado proposital detetado e limpo).
  - 11 assets de exemplo criados em `Assets/_Project/Data/{Enemies,Bosses,Traps,Cutscenes,Audio,Narrative}/`, incluindo um `BossDefinition` com 2 fases e todas as referências cruzadas preenchidas (Boss→Enemy/Memory/GuestBook, Trap→Vfx/Sfx, NarrativeBeat→DialogueGraph).
- 2026-10-21 — Confirmação final: dois assets de teste criados a partir do menu `Assets/Create/Aurora/Content/...` sem `id` preenchido foram apanhados pelo validador real (`CutsceneDefinition`/`NarrativeBeatDefinition` com Id vazio), confirmando o critério "deteta id duplicado/vazio" fora dos testes automatizados. Apagados depois de confirmado; `ContentDefinitionValidator.Validate()` volta a `Passed=True`. Critérios 4/5 de `rules.md` §8 não se aplicam (sem gameplay, sem rendering/save tocados). Estado passa a `DONE`.

## Ideias
- Quando a F4-03 (áudio) existir a sério, ligar `MusicCueDefinition`/`SfxCueDefinition`/`VfxCueDefinition` ao `IAudioService` real.
- Quando a Fase 3 (IA) existir, os "parâmetros base de comportamento" do `EnemyDefinition` passam a alimentar o comportamento real.
