# Funcionalidade Atual

<!-- Ver especificação completa em context/features/F0-fundacao/F0-04-builds-e-pipeline.md -->

# F0-04 — Build profiles e pipeline CLI

## Estado

Em revisão (REVIEW) — falta correr um build real para fechar os critérios de aceitação

## Objetivos

Gerar builds Web e Windows por linha de comandos desde o primeiro dia, para que problemas do browser apareçam cedo e não no fim.

- `IPlatformServices` (Core) + `NullPlatform`/`WebPlatform` (Aurora.Platform), registados por `PlatformBootstrap`.
- 3 `BuildProfile` reais (`Web`, `Windows`, `Windows-Steam`) em `Assets/Settings/Build Profiles/`.
- `Aurora.Editor.Build.BuildRunner` (+ `BuildArgsParser` testável) com os métodos `BuildWeb`/`BuildWindows`/`BuildSteam`/`BuildWebDev`/`BuildWindowsDev`.
- Template WebGL `AuroraMinimal`.
- `tools/build.sh`/`build.ps1`.

**Não inclui:** Steamworks (F7-02), otimização fina (F7-01).

## Critérios de aceitação

- [ ] `BuildWeb` e `BuildWindows` geram builds em batchmode com exit code 0.
- [ ] O build Web abre no Chrome e Firefox e mostra o MainMenu placeholder; input de teclado e gamepad funciona.
- [ ] O build Windows abre e fecha corretamente.
- [ ] `BuildSteam` compila (mesmo sem Steamworks ainda) e o define está ativo.
- [ ] Tamanho do build Web registado no Registo (linha de base).

## Histórico

<!-- Manter atualizado. Da mais antiga para a mais recente -->

- 2026-10-25 — Depois de fechar a F1-01 (Etapa A+B), decidido terminar a Fase 0 antes de continuar a Fase 1 — a F0-04 é a única `TODO` da Fase 0 com dependências `DONE`. Branch `feature/f0-04-builds-pipeline` criada a partir de `main`.
- 2026-10-25 — Implementado: `IPlatformServices`/`NullPlatform`/`WebPlatform`/`PlatformBootstrap`, `BuildArgsParser` (7 testes EditMode) + `BuildRunner`, 3 Build Profiles reais criados via `BuildProfileSetup` (API nova do Unity 6, confirmada por consulta ao Context7 antes de escrever código), template WebGL `AuroraMinimal`, `tools/build.sh`/`build.ps1`. 73/73 testes do projeto a passar; compilação limpa. Decidido não correr um build real nesta sessão (pode demorar vários minutos) — fica para quando o Editor puder ficar ocupado. Estado passa a REVIEW, não DONE, até isso acontecer.
- 2026-10-25 — Build Web real (Brotli, depois Gzip) hung duas vezes com o Editor ligado via MCP (Unity "Não responde" por 1h15+ e depois 10+ min) — não é específico do Brotli, causa ainda não identificada. Unity fechado por completo e build Windows real corrido por terminal (`tools/build.sh windows`), fora do Editor: **sucesso, exit code 0**, `Hotel Aurora.exe` em `Builds/Windows/0.0.1/` (130 MB), `buildinfo.json` correto. Falta: teste manual de abrir/fechar o `.exe`, resolver o build Web (hang não investigado a fundo), e o commit da alteração Brotli→Gzip em `BuildRunner.cs` (ainda por decidir se é definitiva). Estado continua REVIEW.
