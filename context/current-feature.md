# Funcionalidade Atual

<!-- Ver especificação completa em context/features/F0-fundacao/F0-02-input-system.md -->

# F0-02 — Input System e action maps

## Estado

Em revisão (REVIEW) — à espera do Dinis testar

## Objetivos

Toda a entrada do jogo passa pelo Input System moderno: um asset Project-wide, classe gerada, `IInputService` como única porta de entrada, deteção de dispositivo ativo.

- Criar `Assets/_Project/Input/AuroraInput.inputactions` e atribuí-lo como Project-wide Actions.
- Active Input Handling = Input System Package (New) apenas (já está assim desde a F0-01).
- Action maps `Gameplay`, `UI`, `Dialogue`, `Cutscene` com as ações e bindings (teclado+rato e gamepad) de `codespec.md` §6.
- Gerar a classe C# `AuroraInputActions` (namespace `Aurora.Input`).
- `IInputService` (em `Aurora.Core`) + `InputService` (em `Aurora.Input`): propriedades tipadas por mapa, `SetContext(InputContext ctx)`, `CurrentDevice` + evento `DeviceChanged`.
- Web: pedir foco do canvas no primeiro clique/tecla; documentar que `Esc` sai do fullscreen no browser.
- Cena de teste `Tests/Scenes/SCN_InputSandbox` com debug de ação disparada e dispositivo atual (só `AURORA_DEV`).

**Não inclui:** UI de rebinding (F4-02), ícones de prompts (F4-02), rumble avançado.

## Critérios de aceitação

- [ ] Nenhuma referência a `UnityEngine.Input` em todo o projeto (validador da F0-01 verde).
- [ ] Todas as ações têm binding teclado **e** gamepad.
- [ ] `SetContext(Dialogue)` impede Move de disparar; `SetContext(Gameplay)` repõe.
- [ ] Ligar um comando a meio altera `CurrentDevice` e dispara `DeviceChanged` uma única vez.
- [ ] Sandbox funciona no Editor e num build Web de teste.

## Histórico

<!-- Manter atualizado. Da mais antiga para a mais recente -->

- 2026-10-18 — F0-02 definida como funcionalidade atual, a seguir à F0-01 (DONE). Trabalho ainda não iniciado.
- 2026-10-18 — Dinis pede movimento a sério no sandbox da F1-08 em vez de atalho; escolhe fazer a F0-02 como deve ser primeiro. Branch `feature/f0-02-input-system` criada a partir de `main` (independente de F1-07/F1-08, que só dependem de F0-01).
- 2026-10-19 — F0-02 implementada por completo: `AuroraInput.inputactions` (4 action maps, 2 control schemes) atribuído como Project-wide Actions, `IInputService`/`InputService`, troca de contexto, deteção de dispositivo, cena `SCN_InputSandbox`, testes EditMode+PlayMode todos a passar. Estado passa a REVIEW — falta só testar num build Web real (fica para a F0-04, que vai trazer o pipeline de build).
