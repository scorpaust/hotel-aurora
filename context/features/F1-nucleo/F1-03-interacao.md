# F1-03 — Sistema de interação
**Estado:** TODO · **Depende de:** F1-01 · **Assemblies:** Aurora.Gameplay, Aurora.UI (prompt)

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
3. `Hold` usa a interação *Hold* do Input System na ação `Interact` (não temporizador manual); mostra anel de progresso no prompt.
4. Prompt (UI Toolkit, layer HUD): ícone do botão (placeholder de texto até F4-02) + verbo localizado, ancorado ao objeto em espaço de ecrã.
5. `Examine`: mostra texto curto (balão de pensamento do Tomás) sem pausar.
6. `PickUp`: adiciona ao inventário (interface vazia até F1-05) e regista flag.
7. Gizmos de Editor a mostrar raio de interação.

## Critérios de aceitação
- [ ] Com dois objetos próximos, interage com o de maior prioridade e depois o mais perto.
- [ ] Prompt aparece/desaparece suavemente e segue o objeto com a câmara em movimento.
- [ ] Interação *Hold* cancela se soltar antes do tempo.
- [ ] Nenhum prompt durante `Dialogue`, `Cutscene` ou `Paused`.

## Testes
- EditMode: seleção de candidato (prioridade/distância).
- PlayMode: Hold completo vs cancelado.

## Registo

## Ideias
