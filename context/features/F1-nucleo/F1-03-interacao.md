# F1-03 — Sistema de interação
**Estado:** DONE · **Depende de:** F1-01 · **Assemblies:** Aurora.Core, Aurora.Gameplay, Aurora.UI (prompt)

## Objetivo
Uma única forma de interagir com o mundo: aproximar, ver o prompt, carregar Interact. Base para portas, objetos, esconderijos, mecanismos e diálogos.

## Contexto na história
"Qualquer avaria tem uma causa." Tomás examina tudo; o jogo deve recompensar a curiosidade.

## Âmbito
**Inclui:** interface `IInteractable`, deteção do alvo mais próximo, prioridades, prompt contextual (UI Toolkit), interações instantâneas e de *hold*, tipos base: `Examine`, `PickUp`, `Door` (usa F1-02), `Switch`.
**Não inclui:** ferramentas (F1-05), esconderijos (F3-01), diálogo (F2-01) — mas devem encaixar nesta interface.

## Especificação
1. `IInteractable`: `string VerbKey` (chave de localização), `bool CanInteract(InteractionContext)`, `InteractionKind Kind` (`Press`, `Hold`), `float HoldDuration`, `void Interact(InteractionContext)`, `int Priority`.
2. `PlayerInteractor`: `OverlapCircle` à frente do jogador (sem alocações — buffer fixo), escolhe o candidato por prioridade e depois distância. Atualiza alvo no máximo a 10 Hz.
3. `Hold` usa uma ação própria `InteractHold` (mesmos bindings do `Interact`, para não arriscar o `Pushable`/`RoomConnection` que já dependem do `Interact`). O tempo é medido à mão (`IsPressed()` + `Time.deltaTime` contra `IInteractable.HoldDuration`) — a interação *Hold* nativa do Input System com `ApplyParameterOverride("hold:duration", ...)` era a API documentada para isto, mas confirmou-se ao vivo que o override não muda o tempo real de disparo nesta versão do pacote; mostra anel de progresso no prompt.
4. Prompt (UI Toolkit, layer HUD): ícone do botão (placeholder de texto até F4-02) + verbo localizado, ancorado ao objeto em espaço de ecrã.
5. `Examine`: mostra texto curto (balão de pensamento do Tomás) sem pausar.
6. `PickUp`: adiciona ao inventário (interface vazia até F1-05) e regista flag.
7. Gizmos de Editor a mostrar raio de interação.

## Critérios de aceitação
- [x] Com dois objetos próximos, interage com o de maior prioridade e depois o mais perto.
- [x] Prompt aparece/desaparece suavemente e segue o objeto com a câmara em movimento.
- [x] Interação *Hold* cancela se soltar antes do tempo.
- [x] Nenhum prompt durante `Dialogue`, `Cutscene` ou `Paused`.

## Testes
- EditMode: seleção de candidato (prioridade/distância) — `InteractableSelectorTests` (4 testes).
- PlayMode: Hold completo vs cancelado — `PlayerInteractorHoldTests` (2 testes).

## Registo

- 2026-11-08/09 — Implementação completa: `IInteractable`/`InteractionKind`/`InteractionContext`/`InteractableSelector` (Core), `PlayerInteractor` (Gameplay, `Physics2D.OverlapCircle` com `ContactFilter2D` — `OverlapCircleNonAlloc` está obsoleta no Unity 6), `RoomConnection` migrado para `IInteractable`, tipos base `Examine`/`PickUp`/`Switch`, `IInventoryService` (Core, vazia até F1-05), `GameEventString` (Core), `InteractionPromptPanel` (UI, segue o alvo via `Camera.WorldToViewportPoint`+`style.translate`, resolvendo a câmara ativa pela maior `depth` entre `Camera.allCameras`).
- 2026-11-09 — Descoberto que `ApplyParameterOverride("hold:duration", ...)` não altera o tempo real de disparo da interação *Hold* nesta versão do Input System (API documentada, confirmada via Context7, mas sem efeito prático — testado em isolamento de várias formas). Pivô para medição manual do tempo premido em `PlayerInteractor`; `InteractHold` ficou sem interação nenhuma anexada. Ver detalhe em `current-feature.md` e `codespec.md` §5.8/§6.
- 2026-11-09 — Todos os critérios de aceitação verificados (ao vivo via Play Mode + `eval`/capturas de ecrã para 1/2/4; `PlayerInteractorHoldTests` para 3 — testar *hold* ao vivo falhou por o Input System exigir foco real da janela do SO em Play Mode, que uma sessão automatizada não tem). Suite completa: 67/67 EditMode, 16/17 PlayMode (o falhado é da F1-01, não relacionado).

## Ideias
