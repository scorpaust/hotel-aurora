# F0-04 — Build profiles e pipeline CLI
**Estado:** REVIEW · **Depende de:** F0-03 · **Assemblies:** Aurora.Editor, Aurora.Platform

## Objetivo
Gerar builds Web e Windows por linha de comandos desde o primeiro dia, para que problemas do browser apareçam cedo e não no fim.

## Contexto na história
Nenhum.

## Âmbito
**Inclui:** Build Profiles (`Web`, `Windows`, `Windows-Steam`), `BuildRunner` com `-executeMethod`, defines por perfil, template WebGL próprio mínimo, `IPlatformServices` com `NullPlatform` e `WebPlatform` vazios, script de conveniência.
**Não inclui:** Steamworks (F7-02), otimização fina (F7-01).

## Especificação
1. **Build Profiles** (Unity 6):
   - `Web`: WebGL2, compressão Brotli (com *Decompression Fallback* ligado para servidores sem headers), Code Optimization *Disk Size with LTO* em release, *Initial memory* 256 MB, *Maximum memory* 1024 MB, exceções *Explicitly Thrown Only*.
   - `Windows`: IL2CPP, x64.
   - `Windows-Steam`: igual + define `STEAM_BUILD`.
   - Variante dev de cada um com `AURORA_DEV` e Development Build.
2. `Aurora.Editor.Build.BuildRunner` com métodos estáticos `BuildWeb`, `BuildWindows`, `BuildSteam`, `BuildWebDev`, `BuildWindowsDev`. Lêem argumentos opcionais (`-buildVersion`, `-outputPath`). Saída em `Builds/<perfil>/<versão>/`. Falha com `EditorApplication.Exit(1)` se houver erros.
3. Escrever `buildinfo.json` no build (versão, commit git se disponível, data).
4. Template WebGL `AuroraMinimal`: fundo `--aurora-ink`, barra de progresso em latão, canvas a 100 % da janela, botão de fullscreen, mensagem para browsers sem WebGL2.
5. `IPlatformServices` (Core) com `NullPlatform` (Editor/Windows sem Steam) e `WebPlatform` (por agora igual ao Null). Seleção por `PlatformBootstrap` (`Aurora.Platform`, não o `GameBootstrap`) — `Aurora.Input` não referencia `Aurora.Platform`, por isso o registo tem de viver num componente à parte no mesmo `Boot.unity` (ver Registo).
6. Script `tools/build.sh` (e `build.ps1`) que chama o Unity em batchmode com o perfil pedido; documentar uso no topo do script.
7. Servir o build Web localmente para testar: `npx http-server Builds/Web/<versão> -c-1` (ou equivalente). Nota sobre Brotli: a maior parte dos servidores estáticos não envia `Content-Encoding: br` automaticamente para `.br`/`.gz` — por isso o `decompressionFallback` fica sempre ligado no perfil Web (ver Especificação §1), que deteta a falta do header e descomprime no browser em vez de falhar. Para testar os headers a sério, usar um servidor que os envie (ex. `http-server` com `--brotli`, ou configurar o servidor de produção) antes de desligar o fallback.

## Critérios de aceitação
- [ ] `BuildWeb` e `BuildWindows` geram builds em batchmode com exit code 0. *(Código e Build Profiles prontos; execução real do build ainda por correr — ver Registo.)*
- [ ] O build Web abre no Chrome e Firefox e mostra o MainMenu placeholder; input de teclado e gamepad funciona.
- [ ] O build Windows abre e fecha corretamente.
- [ ] `BuildSteam` compila (mesmo sem Steamworks ainda) e o define está ativo. *(Define `STEAM_BUILD` confirmado no perfil `Windows-Steam`; falta correr o build para confirmar a compilação completa.)*
- [ ] Tamanho do build Web registado no `## Registo` (linha de base).

## Testes
- EditMode: `BuildArgsParser` (parsing puro de `-buildVersion`/`-outputPath`, sem precisar de correr um build). 7 testes a passar.

## Registo
- 2026-10-25 — Implementado: `IPlatformServices` (Core) + `NullPlatform`/`WebPlatform` (Aurora.Platform), registados por `PlatformBootstrap` (`Start()`, nunca `Awake()` — corre sempre depois do `Services.Reset()` do `GameBootstrap`, sem `Aurora.Input` ter de referenciar `Aurora.Platform`; ver `codespec.md` §5.1). Confirmado em Play: `Services.Get<IPlatformServices>().PlatformName == "None"` fora de Web/Steam.
  - `Aurora.Editor.Build.BuildArgsParser` (lógica pura de parsing de argumentos, testável) + `BuildRunner` (`BuildWeb`/`BuildWindows`/`BuildSteam`/`BuildWebDev`/`BuildWindowsDev`, também como itens de menu `Aurora/Build/...`) — usa `BuildPipeline.BuildPlayer(BuildPlayerOptions)` diretamente (API estável) em vez de depender do mecanismo assíncrono de "perfil ativo" do Unity 6, para ser fiável em batchmode. Perfil Web aplica `PlayerSettings.WebGL.{template,compressionFormat,decompressionFallback,exceptionSupport}` e repõe os valores antigos no `finally`; perfil Steam acrescenta `STEAM_BUILD` aos defines só durante o build (repõe depois).
  - 3 `BuildProfile` reais criados em `Assets/Settings/Build Profiles/` (`Web`, `Windows`, `Windows-Steam`) via `BuildProfileSetup` (menu `Aurora/Build/Create Build Profiles`, idempotente — salta o que já existir), usando a API nova do Unity 6 (`BuildProfile.CreateBuildProfile` + `GetInstalledPlatformModules()` para encontrar os GUIDs de plataforma instalados, confirmados ao vivo via reflexão: Windows e Web). `Windows-Steam` com `scriptingDefines = ["STEAM_BUILD"]` confirmado.
  - Template `Assets/WebGLTemplates/AuroraMinimal/index.html`: fundo `--aurora-ink`, barra de progresso em `--aurora-brass`, canvas a 100%, botão de fullscreen, aviso PT/EN para browsers sem WebGL2, pedido de foco do canvas ao primeiro clique (`codespec.md` §6).
  - `tools/build.sh`/`build.ps1`: wrappers de conveniência sobre `-executeMethod`, um por perfil (`web`/`web-dev`/`windows`/`windows-dev`/`steam`), avisam se `$UNITY`/`$env:UNITY` não estiver definido.
  - **Por fazer:** correr um build real (Web e/ou Windows) para fechar os critérios de aceitação 1–5 — decidido deixar para uma sessão em que o Editor possa ficar ocupado alguns minutos (batchmode falha com o Editor aberto, `rules.md` §5). Estado fica `REVIEW` em vez de `DONE` até isso acontecer.

## Ideias
- CI com GitHub Actions + GameCI quando o projeto estabilizar.
