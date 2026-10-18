# F0-01 — Projeto, pacotes e assemblies
**Estado:** TODO · **Depende de:** — · **Assemblies:** todos (criação)

## Objetivo
Deixar o projeto Unity 6.3 LTS configurado com a stack de `codespec.md` §3, a estrutura de pastas de §4 e os assembly definitions com dependências corretas.

## Contexto na história
Nenhum. É a fundação.

## Âmbito
**Inclui:** projeto URP 2D, pacotes, pastas, `.asmdef`, `.gitignore`, `.gitattributes` (LFS), Project Settings base.
**Não inclui:** código de gameplay, input (F0-02), builds (F0-04).

## Especificação
1. Projeto criado a partir do template **Universal 2D** do Unity 6.3 LTS.
2. Pacotes em `Packages/manifest.json` (versões verificadas compatíveis com 6000.3): URP, Input System, Cinemachine 3, Localization, Addressables, 2D Animation, PSD Importer, 2D Tilemap Extras, 2D SpriteShape, Timeline, Test Framework, Newtonsoft JSON.
3. Estrutura `Assets/_Project/...` e `Assets/Tests/...` exatamente como em `codespec.md` §4 (pastas vazias com `.gitkeep` se necessário).
4. Um `.asmdef` por pasta de `Scripts/`, referências só no sentido descrito em §4. `Aurora.Editor` com plataforma só Editor. `Aurora.Platform.Steam` com *Define Constraint* `STEAM_BUILD`.
5. Project Settings:
   - Color Space: Linear.
   - Enter Play Mode Options: desativar Domain Reload e Scene Reload (código tem de respeitar isto: limpar estáticos com `[RuntimeInitializeOnLoadMethod(SubsystemRegistration)]`).
   - Script compilation: Scripting Define Symbols por plataforma vazios por agora.
   - Company `Dinis`, Product `Hotel Aurora`, versão `0.0.1`.
6. Script de Editor `Aurora.Editor.ProjectValidator` com menu `Aurora/Validate Project` que verifica: pacotes obrigatórios presentes, Active Input Handling = Input System, ausência de `UnityEngine.Input` no código (grep simples em `Assets/_Project/Scripts`).

## Critérios de aceitação
- [ ] Projeto abre sem erros nem warnings na consola.
- [ ] Compila em batchmode (`rules.md` §5) com log limpo.
- [ ] Todos os `.asmdef` existem e uma referência proibida (ex.: Gameplay → UI) dá erro de compilação.
- [ ] `Aurora/Validate Project` corre e reporta tudo verde.
- [ ] `.gitignore` e `.gitattributes` com LFS configurados.

## Testes
- EditMode: `ProjectValidatorTests` — o validador deteta um ficheiro de teste com `Input.GetKey`.

## Registo

## Ideias
