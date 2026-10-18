# Regras de trabalho — Hotel Aurora

> Regras para o Claude (e para quem trabalhar no projeto). São obrigatórias; se uma regra impedir algo necessário, pára e pergunta ao Dinis.

---

## 1. Comunicação

- Conversa em **português europeu**. Código, nomes de ficheiros, classes, comentários e mensagens de commit em **inglês**.
- Texto do jogo nunca no código: vai para as String Tables (PT-PT é a língua de origem).
- Respostas curtas. No fim de cada tarefa: o que foi feito, o que falta, como testar.

## 2. Âmbito

- Trabalha **uma feature de cada vez**, a indicada ou a próxima `TODO` cujas dependências estejam `DONE`.
- Não implementes nada fora dos critérios de aceitação da feature. Ideias extra vão para a secção `## Ideias` da feature, não para o código.
- Se a feature for ambígua ou contradizer `codespec.md`/`uispec.md`, pára e pergunta antes de escrever código.
- Placeholder art é aceitável e esperado: formas simples com cor e nome. Nunca bloquear gameplay por falta de arte.

## 3. Código C#

- C# da versão suportada pelo Unity 6.3. `#nullable enable` nos assemblies de lógica pura.
- Convenções: `PascalCase` para tipos/métodos/propriedades, `_camelCase` para campos privados, `[SerializeField] private` em vez de campos públicos.
- Um tipo por ficheiro; nome do ficheiro = nome do tipo. Namespaces = `Aurora.<Assembly>`.
- Lógica testável fora de `MonoBehaviour` sempre que possível (classes C# puras + MonoBehaviour fino).
- Sem `Update()` vazios. Sem `GameObject.Find`/`FindObjectOfType` fora do bootstrap. Sem `SendMessage`.
- Assíncrono: `Awaitable` / `async` do Unity 6 com `CancellationToken` (`destroyCancellationToken`). Proibido `Task.Run`, `Thread`, `System.Threading.Timer`.
- Nada de `#if UNITY_EDITOR` espalhado em gameplay: código só de Editor vive em `Aurora.Editor`.
- Código Steam só dentro de `Aurora.Platform.Steam` e sob `#if STEAM_BUILD`.

## 4. Input

- **Só Input System.** Qualquer uso de `UnityEngine.Input`, `Input.GetKey`, `Input.GetAxis` é erro.
- Ações acedidas pela classe gerada através do `IInputService`. Gameplay não conhece teclas nem botões.
- Toda a nova ação tem bindings para `KeyboardMouse` **e** `Gamepad` no mesmo commit.
- Mudar de action map ao mudar de estado (diálogo, pausa, cutscene) — nunca desativar ações individuais para "bloquear" input.

## 5. Unity em linha de comandos (Unity CLI / batchmode)

Usa sempre o executável do Editor da versão do projeto. Variável sugerida: `UNITY` (caminho para o Unity 6.3).

| Objetivo | Comando base |
|---|---|
| Compilar / validar scripts | `"$UNITY" -batchmode -nographics -quit -projectPath . -logFile Logs/compile.log` |
| Testes EditMode | `"$UNITY" -batchmode -nographics -projectPath . -runTests -testPlatform EditMode -testResults Logs/editmode.xml -logFile Logs/tests-edit.log` |
| Testes PlayMode | idem com `-testPlatform PlayMode` |
| Build Web | `"$UNITY" -batchmode -nographics -quit -projectPath . -executeMethod Aurora.Editor.Build.BuildRunner.BuildWeb -logFile Logs/build-web.log` |
| Build Windows | `... -executeMethod Aurora.Editor.Build.BuildRunner.BuildWindows` |
| Build Steam | `... -executeMethod Aurora.Editor.Build.BuildRunner.BuildSteam` |

- Depois de cada passo de implementação: compilar e ler o log. **Erros de compilação ou warnings novos = não avançar.**
- Os builds usam **Build Profiles** (Web, Windows, Windows-Steam), definidos na feature de fundação.
- Se o Editor estiver aberto com o projeto, o batchmode falha por lock: avisa o Dinis em vez de forçar.

## 6. Assets e cenas

- Criar/alterar prefabs, cenas e ScriptableObjects através de scripts de Editor (`Aurora.Editor`) ou pedir ao Dinis para o fazer no Editor. Nunca editar YAML à mão, exceto alterações triviais e explicadas.
- `.meta` sempre versionados junto ao asset.
- Nomes: `PFB_` prefabs, `SO_` ScriptableObjects, `SCN_` cenas de ala, `SPR_`, `ANI_`, `SFX_`, `MUS_`, `UXML_`, `USS_`.
- Conteúdo novo sempre Addressable, agrupado por ala.

## 7. Git

- `.gitignore` oficial de Unity + `Logs/`, `UserSettings/`, `Builds/`.
- Git LFS para `*.psd`, `*.png` grandes, `*.wav`, `*.ogg`, `*.fbx`, `*.ttf`/`*.otf`.
- Um commit por passo lógico, em inglês, formato `feat(F2-01): lantern past-layer reveal`.
- Nunca commitar chaves, `steam_appid.txt` de produção ou credenciais.

## 8. Testes e "feito"

Uma feature só está `DONE` quando:
1. Compila sem erros nem warnings novos.
2. Testes EditMode/PlayMode da feature passam em batchmode.
3. Todos os critérios de aceitação estão verificados (marca-os `[x]` no ficheiro).
4. Funciona com teclado **e** gamepad.
5. Se tocar em rendering, carregamento ou save: build Web testado no browser.
6. `features/README.md` e o `## Registo` da feature atualizados.

## 9. Narrativa e tom

- A história é a da descrição do projeto. Não inventar acontecimentos principais, personagens novas com nome nem alterar o final.
- Tom: terror que vem da perda e da memória, não do gore. Nada de sangue explícito nem mortes gráficas; o "apanhado" é sugerido.
- Beatriz tem humor — as falas dela aliviam a tensão, nunca a anulam.
- Cada boss é uma pessoa antes de ser monstro: o puzzle de cada confronto **tem** de usar a lanterna para revelar quem foi.
