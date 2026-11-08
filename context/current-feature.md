# Funcionalidade Atual

<!-- Ver especificação completa em context/features/F1-nucleo/F1-03-interacao.md -->

# F1-03 — Sistema de interação

## Estado

Em progresso (DOING)

## Objetivos

Uma única forma de interagir com o mundo: aproximar, ver o prompt, carregar Interact. Base para portas, objetos, esconderijos, mecanismos e diálogos.

- `IInteractable`/`InteractionKind`/`InteractionContext` (Aurora.Core).
- Nova ação `InteractHold` (mesmos bindings do `Interact`, interação Hold do Input System) — mantém o `Interact` partilhado intocado (já usado pelo `Pushable`/`RoomConnection`).
- `PlayerInteractor` (Aurora.Gameplay): deteção do alvo mais próximo por prioridade/distância, sem alocações, a 10 Hz.
- `RoomConnection` passa a implementar `IInteractable` (deixa de ser tratado à parte no `PlayerController`, como já estava anotado na F1-02).
- Prompt HUD (UI Toolkit, Aurora.UI): verbo localizado + anel de progresso no Hold, escondido em `Dialogue`/`Cutscene`/`Paused`.
- Tipos base: `Examine`, `PickUp` (stub de inventário até F1-05), `Switch`.

**Não inclui:** ferramentas (F1-05), esconderijos (F3-01), diálogo (F2-01) — mas devem encaixar nesta interface.

## Critérios de aceitação

- [ ] Com dois objetos próximos, interage com o de maior prioridade e depois o mais perto.
- [ ] Prompt aparece/desaparece suavemente e segue o objeto com a câmara em movimento.
- [ ] Interação Hold cancela se soltar antes do tempo.
- [ ] Nenhum prompt durante `Dialogue`, `Cutscene` ou `Paused`.

## Histórico

<!-- Manter atualizado. Da mais antiga para a mais recente -->

- 2026-11-08 — Depois de fechar e fundir a F1-02 em `main`, avançada a Fase 1 pela F1-03 (única `TODO` com dependências `DONE`: só precisa da F1-01). Antes de escrever código, decisão de desenho confirmada pelo utilizador: a interação *Hold* usa uma **segunda ação** `InteractHold` (Input System, interação Hold nativa, `hold:duration` ajustado em runtime por alvo via `ApplyParameterOverride`) em vez de misturar essa interação na ação `Interact` partilhada — evita arriscar o comportamento já existente do `Pushable`/`RoomConnection`. API confirmada via Context7 antes de implementar.
