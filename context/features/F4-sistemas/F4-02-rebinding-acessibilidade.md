# F4-02 — Rebinding, prompts e acessibilidade
**Estado:** TODO · **Depende de:** F4-01 · **Assemblies:** Aurora.Input, Aurora.UI

## Objetivo
Controlos totalmente remapeáveis com o Input System, ícones certos para cada comando e as opções de acessibilidade de `uispec.md` §3.8.

## Contexto na história
Nenhum direto; essencial para Steam Deck e para jogadores sensíveis a sustos e flashes.

## Âmbito
**Inclui:** ecrã de rebinding por dispositivo, deteção de conflitos, gravação de overrides, sprites de prompts por família de dispositivo, "segurar vs alternar", acessibilidade (legendas, escala UI, flashes, tremor, perseguição tolerante, indicadores visuais de som).
**Não inclui:** Steam Input API nativa (o Input System cobre via Gamepad; F7-02 verifica).

## Especificação
1. Rebinding com `PerformInteractiveRebinding()`: excluir rato em movimento, `Esc` cancela; conflitos avisados com opção de trocar.
2. Overrides: `SaveBindingOverridesAsJson` / `LoadBindingOverridesFromJson` no ficheiro de definições; "Repor" por ação e geral.
3. **Prompts:** `InputPromptProvider` devolve sprite/ícone para (ação, dispositivo atual) usando `GetBindingDisplayString` + mapeamento para sprites (Keyboard, Xbox, PlayStation, Steam Deck, Genérico). Atualiza todos os prompts ao trocar dispositivo.
4. **Segurar vs alternar** para Run, Crouch e LanternFocus (o código de gameplay recebe sempre um estado booleano).
5. **Acessibilidade:** legendas (tamanho, fundo, nome de quem fala), escala UI 80–150 %, reduzir flashes (limita frequência/intensidade de luz e eventos), reduzir tremor de câmara (Cinemachine impulse multiplicado), perseguição tolerante (F3-02), indicadores visuais para sons de ameaça (seta discreta na borda do ecrã).
6. Vibração on/off e intensidade.

## Critérios de aceitação
- [ ] Remapear Interact no teclado e no gamepad e confirmar após reiniciar.
- [ ] Prompts mudam instantaneamente ao trocar de teclado para comando e vice-versa.
- [ ] Modo "alternar" para correr funciona.
- [ ] Reduzir flashes elimina flashes > 3 Hz em toda a cena de teste do hotel vivo.

## Testes
- EditMode: round-trip de overrides; resolução de ícone por dispositivo.
- PlayMode (`InputTestFixture`): rebinding simulado.

## Registo

## Ideias
