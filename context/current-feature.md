# Funcionalidade Atual

<!-- Ver especificação completa em context/features/F0-fundacao/F0-01-projeto-e-pacotes.md -->

# F0-01 — Projeto, pacotes e assemblies

## Estado

Não iniciada

## Objetivos

Deixar o projeto Unity 6.3 LTS configurado com a stack de `codespec.md` §3, a estrutura de pastas de §4 e os assembly definitions com dependências corretas.

- Projeto criado a partir do template Universal 2D do Unity 6.3 LTS.
- Pacotes em `Packages/manifest.json` (versões compatíveis com 6000.3): URP, Input System, Cinemachine 3, Localization, Addressables, 2D Animation, PSD Importer, 2D Tilemap Extras, 2D SpriteShape, Timeline, Test Framework, Newtonsoft JSON.
- Estrutura `Assets/_Project/...` e `Assets/Tests/...` exatamente como em `codespec.md` §4.
- Um `.asmdef` por pasta de `Scripts/`, com referências só no sentido descrito em §4. `Aurora.Editor` só para Editor. `Aurora.Platform.Steam` com Define Constraint `STEAM_BUILD`.
- Project Settings: Color Space Linear; Enter Play Mode Options sem Domain Reload/Scene Reload; Company `Dinis`, Product `Hotel Aurora`, versão `0.0.1`.
- Script de Editor `Aurora.Editor.ProjectValidator` (menu `Aurora/Validate Project`) que valida pacotes, Active Input Handling = Input System e ausência de `UnityEngine.Input` em `Assets/_Project/Scripts`.
- `.gitignore` e `.gitattributes` (LFS) configurados.

**Não inclui:** código de gameplay, input (F0-02), builds (F0-04).

## Critérios de aceitação

- [ ] Projeto abre sem erros nem warnings na consola.
- [ ] Compila em batchmode (`rules.md` §5) com log limpo.
- [ ] Todos os `.asmdef` existem e uma referência proibida (ex.: Gameplay → UI) dá erro de compilação.
- [ ] `Aurora/Validate Project` corre e reporta tudo verde.
- [ ] `.gitignore` e `.gitattributes` com LFS configurados.

## Histórico

<!-- Manter atualizado. Da mais antiga para a mais recente -->

- 2026-10-18 — F0-01 definida como funcionalidade atual (primeira feature do projeto, Fase 0 — Fundação). Trabalho ainda não iniciado.