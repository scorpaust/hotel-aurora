# F2-01 — Sistema de diálogo
**Estado:** TODO · **Depende de:** F1-06 · **Assemblies:** Aurora.Narrative, Aurora.UI

## Objetivo
Diálogos com escolhas, condições e efeitos sobre flags, autoráveis em ScriptableObjects e sempre localizados.

## Contexto na história
Conversas com Beatriz, com o hotel (vozes no telefone, a campainha), com hóspedes fantasma e com os bosses antes/depois de serem libertados.

## Âmbito
**Inclui:** `DialogueGraph` (SO com lista de nós), `DialogueRunner`, condições e efeitos, UI de diálogo (`uispec.md` §3.4), balões ambientais não bloqueantes, editor simples no Inspector.
**Não inclui:** editor visual de grafos, vozes gravadas.

## Especificação
1. Nó: `id`, `speakerId`, `lineKey` (Localization), `portraitVariant`, `next` ou `choices[]`, `conditions[]`, `effects[]`.
2. Condições: `FlagEquals/AtLeast`, `HasItem`, `HasMemory`. Efeitos: `SetFlag`, `GiveItem`, `RaiseEvent`, `StartTimeline`.
3. `DialogueRunner.Start(graph)` → `GameState.Dialogue` → mapa `Dialogue`; no fim volta ao estado anterior.
4. Texto com escrita progressiva; `Advance` completa a linha ou avança; `Skip` (hold) salta para a próxima escolha.
5. **Balões ambientais** (`AmbientLine`): texto sobre a cabeça de quem fala, não pausa, fila com prioridade, usados pela Beatriz em F2-02.
6. Validador de Editor: nós sem saída, chaves de localização em falta, ids duplicados.
7. Registo de escolhas feitas (flags) para o final e para o livro de hóspedes.

## Critérios de aceitação
- [ ] Diálogo de teste com 2 escolhas altera uma flag e muda uma linha posterior.
- [ ] Navegação completa só com gamepad.
- [ ] Balão ambiental aparece durante a exploração sem bloquear input.
- [ ] Validador apanha uma chave de localização em falta.

## Testes
- EditMode: avaliação de condições/efeitos; validador.
- PlayMode: fluxo completo de um grafo de teste.

## Registo

## Ideias
- Importar diálogos de uma folha (CSV/Sheets) se o volume de texto crescer.
