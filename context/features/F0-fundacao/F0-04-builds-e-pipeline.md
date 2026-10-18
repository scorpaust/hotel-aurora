# F0-04 — Build profiles e pipeline CLI
**Estado:** TODO · **Depende de:** F0-03 · **Assemblies:** Aurora.Editor, Aurora.Platform

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
5. `IPlatformServices` (Core) com `NullPlatform` (Editor/Windows sem Steam) e `WebPlatform` (por agora igual ao Null). Seleção no `GameBootstrap` por defines.
6. Script `tools/build.sh` (e `build.ps1`) que chama o Unity em batchmode com o perfil pedido; documentar uso no topo do script.
7. Servir o build Web localmente para testar: documentar `npx http-server Builds/Web/<versão> -c-1` ou similar, com nota sobre headers Brotli.

## Critérios de aceitação
- [ ] `BuildWeb` e `BuildWindows` geram builds em batchmode com exit code 0.
- [ ] O build Web abre no Chrome e Firefox e mostra o MainMenu placeholder; input de teclado e gamepad funciona.
- [ ] O build Windows abre e fecha corretamente.
- [ ] `BuildSteam` compila (mesmo sem Steamworks ainda) e o define está ativo.
- [ ] Tamanho do build Web registado no `## Registo` (linha de base).

## Testes
- EditMode: `BuildRunner` resolve caminhos e versões a partir de argumentos.

## Registo

## Ideias
- CI com GitHub Actions + GameCI quando o projeto estabilizar.
