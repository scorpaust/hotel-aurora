# Funcionalidade Atual

<!-- Ver especificação completa em context/features/F1-nucleo/F1-03-interacao.md -->

# F1-03 — Sistema de interação

## Estado

Feito (DONE)

## Objetivos

Uma única forma de interagir com o mundo: aproximar, ver o prompt, carregar Interact. Base para portas, objetos, esconderijos, mecanismos e diálogos.

- `IInteractable`/`InteractionKind`/`InteractionContext` (Aurora.Core).
- Nova ação `InteractHold` (mesmos bindings do `Interact`) — mantém o `Interact` partilhado intocado (já usado pelo `Pushable`/`RoomConnection`). Sem interação do Input System anexada: o tempo premido é medido à mão (ver Histórico).
- `PlayerInteractor` (Aurora.Gameplay): deteção do alvo mais próximo por prioridade/distância, sem alocações, a 10 Hz.
- `RoomConnection` passa a implementar `IInteractable` (deixa de ser tratado à parte no `PlayerController`, como já estava anotado na F1-02).
- Prompt HUD (UI Toolkit, Aurora.UI, `InteractionPromptPanel`): verbo localizado + anel de progresso no Hold, escondido em `Dialogue`/`Cutscene`/`Paused`.
- Tipos base: `Examine`, `PickUp` (stub de inventário até F1-05), `Switch`.

**Não inclui:** ferramentas (F1-05), esconderijos (F3-01), diálogo (F2-01) — mas devem encaixar nesta interface.

## Critérios de aceitação

- [x] Com dois objetos próximos, interage com o de maior prioridade e depois o mais perto.
- [x] Prompt aparece/desaparece suavemente e segue o objeto com a câmara em movimento.
- [x] Interação Hold cancela se soltar antes do tempo.
- [x] Nenhum prompt durante `Dialogue`, `Cutscene` ou `Paused`.

## Histórico

<!-- Manter atualizado. Da mais antiga para a mais recente -->

- 2026-11-08 — Depois de fechar e fundir a F1-02 em `main`, avançada a Fase 1 pela F1-03 (única `TODO` com dependências `DONE`: só precisa da F1-01). Antes de escrever código, decisão de desenho confirmada pelo utilizador: a interação *Hold* usa uma **segunda ação** `InteractHold` (Input System, interação Hold nativa, `hold:duration` ajustado em runtime por alvo via `ApplyParameterOverride`) em vez de misturar essa interação na ação `Interact` partilhada — evita arriscar o comportamento já existente do `Pushable`/`RoomConnection`. API confirmada via Context7 antes de implementar.
- 2026-11-08 — Implementado o núcleo: `IInteractable`/`InteractionKind`/`InteractionContext`/`InteractableSelector` (Aurora.Core, com testes EditMode), `PlayerInteractor` (Aurora.Gameplay, `Physics2D.OverlapCircle` com `ContactFilter2D` — a API `OverlapCircleNonAlloc` está obsoleta no Unity 6), `RoomConnection` migrado para `IInteractable`. Verificado ao vivo (Play Mode + `eval`): deteção do alvo, limpeza ao sair do alcance, e `Interact()` a disparar a mesma transição de sala de sempre.
- 2026-11-09 — `InteractionPromptPanel` (Aurora.UI): segue o alvo em espaço de ecrã via `Camera.WorldToViewportPoint` + `style.translate` (padrão documentado da Unity para UI Toolkit em runtime, mais robusto que `RuntimePanelUtils.ScreenToPanel`). A câmara é resolvida pela maior `depth` entre `Camera.allCameras` — há sempre várias ativas ao mesmo tempo (uma do `Boot`, a do corredor F1-01, a do `RoomPlayground` F1-02), escolher a primeira do array dava a câmara errada. Também corrigido: aceder a `component.transform` de um alvo destruído no mesmo frame lançava `MissingReferenceException` — troca de `is Component` para `as Component` + `!= null` (verificação de "fake null" da Unity).
- 2026-11-09 — Tipos base `Examine`, `PickUp`, `Switch` (Aurora.Gameplay), mais `IInventoryService` (Core, vazia até F1-05) e `GameEventString` (Core, canal genérico para o balão de pensamento do `Examine`, sem consumidor ainda).
- 2026-11-09 — **Achado importante:** `ApplyParameterOverride("hold:duration", ...)` (a API documentada, confirmada via Context7) fica registado — `GetParameterValue` devolve o valor certo — mas **não muda o tempo real de disparo** da interação *Hold* nesta versão do Input System (confirmado em isolamento, com e sem a ação/mapa reativados, e aplicado antes do primeiro `Enable()`). `PlayerInteractor` passou a medir o tempo premido à mão (`Time.deltaTime` contra `IInteractable.HoldDuration`, usando só `IsPressed()`), e a ação `InteractHold` ficou sem qualquer interação anexada. `codespec.md` §6/§5.8 atualizado para refletir isto.
- 2026-11-09 — Tentativa de verificar o Hold ao vivo (tecla simulada via MCP) deu resultados contraditórios — descoberta a causa real: o Input System só processa transições de fase de ações em Play Mode quando a janela do Editor tem foco real do SO, que uma sessão automatizada não tem. Em vez de insistir nisso, escrito `PlayerInteractorHoldTests.cs` (PlayMode, `InputTestFixture`, como o resto do projeto já fazia) — único caminho fiável para testar hold/cancelamento sem foco de janela. Os dois testes (cancela se soltar cedo; completa se segurar o suficiente) passam.
- 2026-11-09 — Suite completa corrida: 67/67 EditMode, 16/17 PlayMode. O único falhado (`PinnedAtTopOfLadder_WalkingSideways_LeavesTheStair`) é da F1-01 (escadas/ladder), não tocado nesta feature nem no diff desta branch — falha de forma consistente mesmo isolado, parece sensível à cadência real de frames do Test Runner nesta máquina/sessão. Fora do âmbito da F1-03; fica anotado para investigar separadamente.
- 2026-11-09 — Critérios de aceitação verificados: 1 (prioridade vs. distância) e 4 (sem prompt em `Paused`) confirmados ao vivo; 2 (prompt a seguir a câmara) confirmado por captura de ecrã; 3 (cancelar o Hold) confirmado pelo `PlayerInteractorHoldTests`. Feature marcada `DONE`.
