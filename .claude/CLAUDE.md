# Hotel Aurora — O Último Hóspede

Jogo 2D narrativo de terror/aventura (vista lateral) em Unity 6.3 LTS, para **Web (WebGL2)** e **PC (Windows, depois Steam)**.

Este ficheiro é só o ponto de entrada. As regras vivem em `context/`, na raiz do projeto.

## Lê sempre (carregados automaticamente)

@../context/rules.md
@../context/codespec.md
@../context/uispec.md

## Lê quando for preciso

- `context/features/README.md` — mapa de fases e estado de cada feature.
- `context/features/<fase>/<feature>.md` — **lê apenas a feature ativa** antes de começar. Não carregues as outras.
- A história completa está na descrição do projeto; o resumo jogável está em `context/codespec.md` §1.

## Como trabalhar numa feature

1. Lê a feature indicada pelo Dinis (ou a primeira com estado `TODO` no README).
2. Confirma as dependências marcadas como `DONE`. Se faltar alguma, pára e diz.
3. Propõe um plano curto (ficheiros a criar/alterar) antes de escrever código.
4. Implementa em passos pequenos; compila em batchmode depois de cada passo (ver `rules.md` §5).
5. Corre os testes da feature. Só marca `DONE` quando todos os critérios de aceitação passarem.
6. Atualiza o estado no `features/README.md` e acrescenta uma linha ao `## Registo` da própria feature.

## Nunca

- Usar `UnityEngine.Input` (input antigo). Só o Input System.
- Editar `.unity`, `.prefab` ou `.asset` à mão quando existe API de Editor para isso.
- Adicionar pacotes sem os registar em `codespec.md` §3.
- Usar threads, `Task.Run` ou `System.IO` síncrono em código que corre no Web.
