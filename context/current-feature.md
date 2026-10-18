# Funcionalidade Atual

<!-- Ver especificação completa em context/features/F0-fundacao/F0-01-projeto-e-pacotes.md -->

# F0-01 — Projeto, pacotes e assemblies

## Estado

Concluída

## Objetivos

Deixar o projeto Unity 6000.6.x configurado com a stack de `codespec.md` §3, a estrutura de pastas de §4 e os assembly definitions com dependências corretas.

- Projeto criado a partir do template Universal 2D do Unity 6000.6.x.
- Pacotes em `Packages/manifest.json` (versões compatíveis com 6000.6): URP, Input System, Cinemachine 3, Localization, Addressables, 2D Animation, PSD Importer, 2D Tilemap Extras, 2D SpriteShape, Timeline, Test Framework, Newtonsoft JSON.
- Estrutura `Assets/_Project/...` e `Assets/Tests/...` exatamente como em `codespec.md` §4.
- Um `.asmdef` por pasta de `Scripts/`, com referências só no sentido descrito em §4. `Aurora.Editor` só para Editor. `Aurora.Platform.Steam` com Define Constraint `STEAM_BUILD`.
- Project Settings: Color Space Linear; Enter Play Mode Options sem Domain Reload/Scene Reload; Company `Dinis`, Product `Hotel Aurora`, versão `0.0.1`.
- Script de Editor `Aurora.Editor.ProjectValidator` (menu `Aurora/Validate Project`) que valida pacotes, Active Input Handling = Input System e ausência de `UnityEngine.Input` em `Assets/_Project/Scripts`.
- `.gitignore` e `.gitattributes` (LFS) configurados.

**Não inclui:** código de gameplay, input (F0-02), builds (F0-04).

## Critérios de aceitação

- [x] Projeto abre sem erros nem warnings na consola.
- [ ] Compila em batchmode (`rules.md` §5) com log limpo. *(não corrido — Editor estava aberto; validado por equivalente via Editor ao vivo.)*
- [x] Todos os `.asmdef` existem com o grafo de `codespec.md` §4. *(referência proibida ainda não demonstrável em runtime — Gameplay/UI não têm scripts.)*
- [x] `Aurora/Validate Project` corre e reporta tudo verde.
- [x] `.gitignore` e `.gitattributes` com LFS configurados.

## Histórico

<!-- Manter atualizado. Da mais antiga para a mais recente -->

- 2026-10-18 — F0-01 definida como funcionalidade atual (primeira feature do projeto, Fase 0 — Fundação). Trabalho ainda não iniciado.
- 2026-10-18 — Decisão: projeto fica em Unity 6000.6.6f1 (Editor já aberto nesta versão) em vez de 6.3 LTS; `codespec.md`, `CLAUDE.md` e esta feature atualizados para refletir isso.
- 2026-10-18 — Estado passa a "Em progresso". Branch `feature/f0-01-projeto-e-pacotes` criada a partir de `main`.
- 2026-10-18 — Implementação concluída na branch (pacotes, estrutura de pastas, 13 `.asmdef`, `ProjectValidator` + testes, Project Settings, `.gitattributes` LFS). Estado da feature em `features/F0-fundacao/F0-01-projeto-e-pacotes.md` passa a REVIEW — falta confirmação em batchmode (Editor estava aberto) e revisão do Dinis antes de fazer merge para `main`.
- 2026-10-18 — Dinis aprovou. Merge (fast-forward) de `feature/f0-01-projeto-e-pacotes` para `main`; branch apagada. Estado passa a "Concluída".
