# F4-04 — Cutscenes (Timeline)
**Estado:** TODO · **Depende de:** F1-01 · **Assemblies:** Aurora.Narrative, Aurora.UI

## Objetivo
Sequências scriptadas (Timeline) que bloqueiam o jogador de forma consistente, com Skip e reposição garantida do estado de jogo — usadas pelo diálogo (efeito `StartTimeline`, F2-01), pelas revelações dos bosses, pelo prólogo e pelo final.

## Contexto na história
A chegada ao Aurora, a revelação de quem cada boss foi (lanterna + Timeline), o fundador a lembrar-se do nome da filha, a fuga final — todas scriptadas.

## Âmbito
**Inclui:** `CutscenePlayer` (Aurora.Narrative) que recebe um `CutsceneDefinition` (`codespec.md` §5.4: referência a um Timeline asset + condições de disparo), entra em `GameState.Cutscene`, toca o `PlayableDirector`, trata `SignalEmitter`/`SignalReceiver` para disparar `GameEvent`s a meio da cutscene sem o `Aurora.Narrative` ter de referenciar sistemas de gameplay diretamente, e `Skip` (hold, ação `Cutscene.Skip` já prevista em `codespec.md` §6).
**Não inclui:** autoria das cutscenes de conteúdo em si (cada feature de conteúdo cria o seu Timeline); letterboxing visual é opcional/placeholder.

## Especificação
1. `CutscenePlayer.Play(CutsceneDefinition)`: guarda o `GameState` anterior, muda para `GameState.Cutscene` (troca o `InputContext` para o mapa `Cutscene` — gameplay/UI ficam desativados exceto `Skip`), obtém/cria um `PlayableDirector`, `.Play()`.
2. No evento `stopped` do `PlayableDirector`, repõe o `GameState` anterior.
3. `Skip`: avança o director até ao fim (`director.time = director.duration` ou `Evaluate`) — tem de produzir os mesmos efeitos finais que deixar correr até ao fim (os `SignalEmitter`s relevantes disparam à mesma, não só os clips visuais).
4. Sinais: `SignalEmitter` no Timeline + `SignalReceiver` mapeado a `GameEvent`s existentes — é assim que uma cutscene afeta `WorldFlags`/outros sistemas sem acoplamento direto.
5. Fallback de segurança: `CutsceneDefinition` nula ou Timeline já disparado (evitar repetição indevida por re-trigger) regista aviso e não bloqueia o jogador indefinidamente.

## Critérios de aceitação
- [ ] Uma cutscene de teste (Timeline com 2 clips + 1 Signal) corre do início ao fim e devolve `GameState.Playing`.
- [ ] Segurar `Skip` termina a cutscene instantaneamente e os efeitos codificados por Signal acontecem na mesma.
- [ ] Durante a cutscene, `Move`/`Interact`/`Pause` não respondem; só `Skip` responde.
- [ ] Sair a meio nunca deixa um Play seguinte preso em `GameState.Cutscene`.

## Testes
- EditMode: resolução do `GameState` anterior/seguinte.
- PlayMode: Timeline de teste corre e termina; `Skip` testado com `InputTestFixture`.

## Registo

## Ideias
