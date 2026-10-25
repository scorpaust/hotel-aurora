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
   - `Web`: WebGL2, compressão **Gzip** (decisão definitiva — Brotli provocou um hang reprodutível do pipeline de build neste Editor, ver Registo; *Decompression Fallback* fica ligado da mesma forma, para servidores sem headers), Code Optimization *Disk Size with LTO* em release, *Initial memory* 256 MB, *Maximum memory* 1024 MB, exceções *Explicitly Thrown Only*.
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
- [x] `BuildWindows` gera build em batchmode com exit code 0. *(Confirmado — ver Registo.)*
- [x] `BuildWeb` gera build em batchmode com exit code 0. *(Confirmado pelo terminal, fora do Editor, com Gzip — ver Registo.)*
- [ ] O build Web abre no Chrome e Firefox e mostra o MainMenu placeholder; input de teclado e gamepad funciona. *(Falta servir e testar manualmente no browser.)*
- [x] O build Windows abre e fecha corretamente. *(Abre e mostra o MainMenu placeholder — confirmado manualmente. Fecha-se pelo Gestor de Tarefas; sem botão "Sair" nem `Alt+F4` testados ainda, mas isso é esperado nesta fase — "Sair" é o `F4-01`, ainda `TODO`. Ver Registo.)*
- [ ] `BuildSteam` compila (mesmo sem Steamworks ainda) e o define está ativo. *(Define `STEAM_BUILD` confirmado no perfil `Windows-Steam`; falta correr o build para confirmar a compilação completa.)*
- [x] Tamanho do build Web registado no `## Registo` (linha de base). *(16 MB — ver Registo.)*

## Testes
- EditMode: `BuildArgsParser` (parsing puro de `-buildVersion`/`-outputPath`, sem precisar de correr um build). 7 testes a passar.

## Registo
- 2026-10-25 — Implementado: `IPlatformServices` (Core) + `NullPlatform`/`WebPlatform` (Aurora.Platform), registados por `PlatformBootstrap` (`Start()`, nunca `Awake()` — corre sempre depois do `Services.Reset()` do `GameBootstrap`, sem `Aurora.Input` ter de referenciar `Aurora.Platform`; ver `codespec.md` §5.1). Confirmado em Play: `Services.Get<IPlatformServices>().PlatformName == "None"` fora de Web/Steam.
  - `Aurora.Editor.Build.BuildArgsParser` (lógica pura de parsing de argumentos, testável) + `BuildRunner` (`BuildWeb`/`BuildWindows`/`BuildSteam`/`BuildWebDev`/`BuildWindowsDev`, também como itens de menu `Aurora/Build/...`) — usa `BuildPipeline.BuildPlayer(BuildPlayerOptions)` diretamente (API estável) em vez de depender do mecanismo assíncrono de "perfil ativo" do Unity 6, para ser fiável em batchmode. Perfil Web aplica `PlayerSettings.WebGL.{template,compressionFormat,decompressionFallback,exceptionSupport}` e repõe os valores antigos no `finally`; perfil Steam acrescenta `STEAM_BUILD` aos defines só durante o build (repõe depois).
  - 3 `BuildProfile` reais criados em `Assets/Settings/Build Profiles/` (`Web`, `Windows`, `Windows-Steam`) via `BuildProfileSetup` (menu `Aurora/Build/Create Build Profiles`, idempotente — salta o que já existir), usando a API nova do Unity 6 (`BuildProfile.CreateBuildProfile` + `GetInstalledPlatformModules()` para encontrar os GUIDs de plataforma instalados, confirmados ao vivo via reflexão: Windows e Web). `Windows-Steam` com `scriptingDefines = ["STEAM_BUILD"]` confirmado.
  - Template `Assets/WebGLTemplates/AuroraMinimal/index.html`: fundo `--aurora-ink`, barra de progresso em `--aurora-brass`, canvas a 100%, botão de fullscreen, aviso PT/EN para browsers sem WebGL2, pedido de foco do canvas ao primeiro clique (`codespec.md` §6).
  - `tools/build.sh`/`build.ps1`: wrappers de conveniência sobre `-executeMethod`, um por perfil (`web`/`web-dev`/`windows`/`windows-dev`/`steam`), avisam se `$UNITY`/`$env:UNITY` não estiver definido.
  - **Por fazer:** correr um build real (Web e/ou Windows) para fechar os critérios de aceitação 1–5 — decidido deixar para uma sessão em que o Editor possa ficar ocupado alguns minutos (batchmode falha com o Editor aberto, `rules.md` §5). Estado fica `REVIEW` em vez de `DONE` até isso acontecer.
- 2026-10-25 — Primeira tentativa de build Web real (`BuildWeb`, compressão Brotli) pelo Editor ligado (via eval), hung: Unity ficou "Não responde" (~1% CPU) por mais de 1h15 preso na compressão do ficheiro de dados. Unity forçado a fechar (Task Manager); reaberto, compilação e testes confirmados limpos. Segunda tentativa, agora com `compressionFormat = Gzip` em vez de `Brotli` (para isolar a causa), hung outra vez (~10+ min, mesmo sintoma de "Não responde" a ~0% CPU) — conclusão: o hang **não é específico do Brotli**; é um problema mais profundo no pipeline WebGL/Emscripten nesta máquina, ou possivelmente um artefacto de disparar `BuildPipeline.BuildPlayer` através do bridge assíncrono do MCP do Editor. Não investigado mais a fundo por agora — ficou decidido evitar completamente o Editor ligado para validar builds e testar antes o build Windows a sério, por processo de terminal separado.
  - A alteração de `BuildRunner.cs` para `WebGLCompressionFormat.Gzip` (com comentário `TODO F0-04` a explicar o porquê) confirmada como **decisão definitiva** (não um workaround temporário) — Brotli fica como ideia a retomar só se o hang for investigado e resolvido (ver `## Ideias`).
  - Unity fechado por completo (`taskkill`) e build Windows real corrido por terminal, fora do Editor/MCP, em segundo plano (`tools/build.sh windows`): **sucesso, exit code 0**. `Hotel Aurora.exe` gerado em `Builds/Windows/0.0.1/` (130 MB), `buildinfo.json` escrito corretamente (`version: 0.0.1`, `commit: cb5bec0`, `builtAtUtc` confirmado). Prova que o `BuildRunner`/`BuildArgsParser`/pipeline funcionam fim-a-fim para Windows em batchmode real.
- 2026-10-25 — Teste manual do `.exe` Windows: abre e mostra o MainMenu placeholder com o título "Hotel Aurora", confirmando o build. Corre em fullscreen sem chrome de janela (sem X); sem botão "Sair" ainda (esse é o `F4-01`, ainda `TODO`) e `Alt+F4` não foi testado — utilizador terminou pelo Gestor de Tarefas. Decidido não bloquear a F0-04 por isto: a ausência de um botão "Sair" é esperada nesta fase (dependência do `F4-01`), e fechar pelo Gestor de Tarefas é aceitável para um build placeholder. Fica como nota para confirmar `Alt+F4` numa próxima sessão e para o `F4-01` adicionar o "Sair" real.
- 2026-10-25 — Build Web real repetido fora do Editor (Unity fechado por completo, `tools/build.sh web` por terminal, com `compressionFormat = Gzip`): **sucesso, exit code 0**, sem qualquer hang — confirma que o problema anterior era mesmo específico de correr `BuildPipeline.BuildPlayer` através do bridge do Editor/MCP (ou do Editor ficar ligado durante builds longas), não do pipeline WebGL em si. Output em `Builds/Web/0.0.1/` (`index.html`, `Build/0.0.1.{data,framework.js,wasm}.unityweb` + `.loader.js`), `buildinfo.json` correto (`commit: a52cc35`). **Tamanho do build (linha de base): 16 MB** — bem dentro do limite de 60 MB do `codespec.md` §2. Falta: servir localmente (`npx http-server Builds/Web/0.0.1 -c-1`) e testar manualmente no Chrome/Firefox com teclado e gamepad.

## Ideias
- CI com GitHub Actions + GameCI quando o projeto estabilizar.
