# F0-01 — Projeto, pacotes e assemblies
**Estado:** REVIEW · **Depende de:** — · **Assemblies:** todos (criação)

## Objetivo
Deixar o projeto Unity 6000.6.x configurado com a stack de `codespec.md` §3, a estrutura de pastas de §4 e os assembly definitions com dependências corretas.

## Contexto na história
Nenhum. É a fundação.

## Âmbito
**Inclui:** projeto URP 2D, pacotes, pastas, `.asmdef`, `.gitignore`, `.gitattributes` (LFS), Project Settings base.
**Não inclui:** código de gameplay, input (F0-02), builds (F0-04).

## Especificação
1. Projeto criado a partir do template **Universal 2D** do Unity 6000.6.x.
2. Pacotes em `Packages/manifest.json` (versões verificadas compatíveis com 6000.6): URP, Input System, Cinemachine 3, Localization, Addressables, 2D Animation, PSD Importer, 2D Tilemap Extras, 2D SpriteShape, Timeline, Test Framework, Newtonsoft JSON.
3. Estrutura `Assets/_Project/...` e `Assets/Tests/...` exatamente como em `codespec.md` §4 (pastas vazias com `.gitkeep` se necessário).
4. Um `.asmdef` por pasta de `Scripts/`, referências só no sentido descrito em §4. `Aurora.Editor` com plataforma só Editor. `Aurora.Platform.Steam` com *Define Constraint* `STEAM_BUILD`.
5. Project Settings:
   - Color Space: Linear.
   - Enter Play Mode Options: desativar Domain Reload e Scene Reload (código tem de respeitar isto: limpar estáticos com `[RuntimeInitializeOnLoadMethod(SubsystemRegistration)]`).
   - Script compilation: Scripting Define Symbols por plataforma vazios por agora.
   - Company `Dinis`, Product `Hotel Aurora`, versão `0.0.1`.
6. Script de Editor `Aurora.Editor.ProjectValidator` com menu `Aurora/Validate Project` que verifica: pacotes obrigatórios presentes, Active Input Handling = Input System, ausência de `UnityEngine.Input` no código (grep simples em `Assets/_Project/Scripts`).

## Critérios de aceitação
- [x] Projeto abre sem erros nem warnings na consola. *(os únicos warnings são "will not be compiled, because it has no scripts" nos assemblies ainda vazios — Core, Input, Gameplay, Hotel, Narrative, Audio, Persistence, Platform, Platform.Steam, UI, Tests.PlayMode — esperado, pois o código de gameplay está fora do âmbito desta feature.)*
- [ ] Compila em batchmode (`rules.md` §5) com log limpo. *(Editor estava aberto durante a implementação; por `rules.md` §5 não se força batchmode com lock. Validado em alternativa via Editor ao vivo (MCP): `recompile_status` limpo após cada passo. Falta confirmação em batchmode real — corre quando o Editor estiver fechado.)*
- [x] Todos os `.asmdef` existem com o grafo de `codespec.md` §4. *(A deteção de referência circular não foi possível confirmar em runtime: tentei adicionar `Aurora.UI` às referências de `Aurora.Gameplay` para provocar o ciclo Gameplay↔UI, mas como nenhum dos dois assemblies tem ainda scripts, o Unity nem chega a compilá-los — não há erro a ver. Fica confirmado só quando houver código real nos dois lados.)*
- [x] `Aurora/Validate Project` corre e reporta tudo verde. *(Verificado chamando `ProjectValidator.Validate()` diretamente via Editor — o item de menu em si não foi invocado por abrir um diálogo modal bloqueante incompatível com automação; a lógica é a mesma.)*
- [x] `.gitignore` e `.gitattributes` com LFS configurados. *(LFS para `*.psd .png .wav .ogg .fbx .ttf .otf` por `rules.md` §7 — ficam configurados mas inertes, porque o `.gitignore` continua a excluir toda a arte/media nesta fase, por decisão anterior do Dinis.)*

## Testes
- EditMode: `ProjectValidatorTests` — 2 testes, ambos a passar (`FindLegacyInputUsages_DetectsLegacyInputGetKey`, `FindLegacyInputUsages_IgnoresInputSystemCode`).

## Registo
- 2026-10-18 — Implementação em `feature/f0-01-projeto-e-pacotes`: pacotes (Cinemachine 3, Localization, Addressables, Newtonsoft JSON) adicionados via Package Manager; estrutura `Assets/_Project/...` e `Assets/Tests/...` criada com `.gitkeep` nas pastas vazias; 13 `.asmdef` criados com o grafo de dependências de `codespec.md` §4; `Aurora.Editor.ProjectValidator` + `ProjectValidatorTests` escritos e a passar; Project Settings (Color Space Linear já estava certo; Enter Play Mode Options a desativar Domain Reload e Scene Reload; Company/Product/Version); `.gitattributes` com filtros LFS restaurados. Estado passa a REVIEW — falta confirmação de batchmode e do Dinis.

## Ideias
