# F1-05 — Ferramentas e mochila
**Estado:** TODO · **Depende de:** F1-03 · **Assemblies:** Aurora.Gameplay, Aurora.UI

## Objetivo
Tomás resolve problemas desmontando coisas. Ferramentas equipáveis abrem/reparam/sabotam mecanismos; a mochila é o inventário diegético.

## Contexto na história
"Uma mochila cheia de ferramentas e o hábito de desmontar tudo o que não compreende." O pai também esconde ferramentas pelo hotel para o ajudar.

## Âmbito
**Inclui:** `ItemDefinition`, `Inventory` (lógica pura), ferramenta ativa, `Mechanism` interagível que exige ferramenta, mini-ação de desmontar (sequência curta de inputs), overlay da mochila (UI Toolkit), 4 ferramentas iniciais.
**Não inclui:** objetos-chave específicos de alas (F5/F6), combinação de objetos.

## Especificação
1. Ferramentas iniciais (SO): **Chave de fendas** (painéis, grelhas de condutas), **Alicate** (cabos, cadeados de arame), **Fita isoladora** (reparar fios → dar energia), **Chave de latão** (objeto-chave da história; abre portas marcadas).
2. `Inventory`: ferramentas (máx. 6) + objetos-chave (ilimitado). Eventos `ItemAdded`, `ToolEquipped`.
3. `CycleTool` troca a ferramenta ativa; HUD mostra-a 2 s (`uispec.md` §3.3).
4. `Mechanism : IInteractable`: requer `ItemDefinition` (ou nenhuma), `DisassembleSequence` (2–4 passos com direções/botões, timing relaxado, falhar só recomeça o passo). Resultado: flag + `UnityEvent`/`GameEvent`.
5. Se não tiver a ferramenta: Tomás comenta ("Preciso de um alicate.").
6. Sequências podem correr **sob pressão** (perseguição a decorrer) — o jogo não pausa.
7. Overlay da mochila (`uispec.md` §3.5): pausa, grelha navegável, descrição localizada, Equipar.

## Critérios de aceitação
- [ ] Apanhar, trocar e equipar ferramentas com teclado e gamepad.
- [ ] Um painel só abre com a chave de fendas e após a sequência.
- [ ] Sem a ferramenta, mostra o comentário certo e não abre.
- [ ] Mochila navegável só com gamepad; Back fecha.

## Testes
- EditMode: `Inventory` (limites, eventos), `DisassembleSequence` (falhar recomeça passo).
- PlayMode: mecanismo completo com ferramenta.

## Registo

## Ideias
