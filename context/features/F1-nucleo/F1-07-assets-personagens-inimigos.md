# F1-07 — Assets base: personagens e inimigos (sprites, animações, Animator Controllers)
**Estado:** DOING · **Depende de:** F0-01 · **Assemblies:** Nenhum (conteúdo/arte apenas)

## Objetivo
Ter arte animável em jogo antes de existir arte final: (A) reaproveitar os 5 spritesheets já importados em `Assets/Assets/Sprites/` (Player, Zombie, EyeGuy, Glitcher, Boss — Haunted Hotel Asset Pack, CC0) para o que eles realmente são, e (B) gerar sprites novos de movimento lateral (Tomás + inimigos genéricos) com o Unity AI Assistant, no mesmo estilo visual, para o que o pack não cobre.

## Contexto na história
Nenhum diretamente — isto é preparação técnica/artística. Serve para prototipagem e testes de gameplay até existir arte final de cada personagem nomeada. Ajuda a "ver o jogo a acontecer" durante o F1 (Tomás a andar) e dá já corpo a inimigos genéricos para prototipar o F3 (perseguições, esconderijos).

## Descoberta importante (2026-10-18)
Depois de corrigir o slicing (ver Registo), inspecionei os 46 frames ampliados (nearest-neighbor) um a um. **Os 5 spritesheets não são ciclos de andar por direção** — são retratos/ícones quase-duplicados da mesma pose, por personagem, com pequenas variações de recorte. Não há "virar para a esquerda/direita/cima/baixo" em lado nenhum.

- **Player, Zombie, EyeGuy, Glitcher**: o mesmo busto/ícone repetido 6–14 vezes — encaixam como **retrato de diálogo** (`uispec.md` §3.4) ou ícone de identificação.
- **Boss**: tem a figura encapuzada de corpo inteiro (frames "cheios") e depois um sub-conjunto que é só o contorno da cara, cada vez com mais detalhe — uma possível sequência de **revelação** (contorno → sombreado), que bate certo com o mecanismo central do jogo (*"a lanterna revela o rosto humano dos monstros"*, `codespec.md` §1) e com o livro de hóspedes (`uispec.md` §3.6).

Conclusão: este pack dá arte de retrato/revelação, não de movimento no mundo. A F1-01 já está desenhada para usar placeholder no controlo do Tomás, por isso isto não bloqueia nada — mas construir "Animator Controller com Idle+Andar por direção" a partir destes frames, como esta feature previa inicialmente, não fazia sentido com o conteúdo real.

## Âmbito
**Inclui — Parte A (arte existente, pode avançar já):**
- Animator Controllers de "retrato vivo" para os 5 personagens (loop subtil entre os frames quase-duplicados — piscar/respirar — útil em diálogo).
- Protótipo da animação de revelação do Boss (contorno → sombreado) como referência para a F1-04 (lanterna/camada do passado).
- Prefabs de pré-visualização (`PFB_<Personagem>Preview` — SpriteRenderer + Animator) para os 5.

**Inclui — Parte B (arte nova, gerada pelo Dinis com o Unity AI Assistant):**
- Sprites de movimento lateral do Tomás: Idle, Walk, Run, Crouch (parado e a mover) — ver brief completo abaixo.
- Sprites de movimento genéricos para os 4 inimigos (Idle + Move/Chase), para prototipar F3.
- Depois de gerados: Animation Clips + Animator Controller + prefab de pré-visualização para cada um (esta parte faço eu, assim que os sprites existirem no projeto).

**Não inclui:**
- Lógica de controlo do jogador (isso é F1-01 — já desenhada para usar placeholder, não esta arte).
- IA/comportamento de inimigos (F3+).
- Arte final das personagens nomeadas da história (Tomás, Beatriz, Governanta, Chef, Nadador do Fundo, Noivos, Porteiro, Pai) — trabalho de conteúdo em F5/F6.
- Rigs de ossos via 2D Animation/PSD Importer (sprites simples por frame, não PSD com camadas; sem necessidade de esqueleto para já).

## Brief para o Unity AI Assistant (Parte B)

Texto pronto a colar, mais as referências de estilo a anexar.

**Referências de estilo a anexar:** `Assets/Assets/Sprites/Player.png` (paleta do Tomás) e `Assets/Assets/Sprites/Boss.png` (estética geral do pack).

> Preciso de sprites de personagem em pixel art, no mesmo estilo visual das imagens em anexo: paleta escura e pouco saturada (cinzentos e roxos/violetas, quase sem cor viva), contorno preto grosso de 1px à volta de toda a silhueta, sombreado simples com alguma textura granulada. Estética decrépita/assombrada (Haunted Hotel Asset Pack).
>
> O jogo é **vista lateral (side-view)**, não top-down — preciso de UMA direção por pose só (o jogo vira o sprite horizontalmente por código para esquerda/direita).
>
> Canvas sugerido: ~32×48 px por frame para o Tomás (corpo inteiro), à escala do resto do pack (os ícones de retrato existentes rondam 20×21 px; a figura de corpo inteiro do Boss ronda 27×37 px) — ajusta se precisares, mas mantém a mesma escala entre todas as frames de uma animação.
>
> **Tomás** (protagonista, 17 anos, mochila — ver `codespec.md`), estados:
> - Idle (parado)
> - Walk (ciclo de andar, 4–6 frames)
> - Run (ciclo de correr, mais dinâmico que o Walk)
> - Crouch idle + Crouch walk (altura reduzida, para passar por condutas)
>
> **Inimigos genéricos** (reaproveitar o visual de Zombie, EyeGuy, Glitcher e Boss já existentes), cada um só precisa de:
> - Idle
> - Move/Chase (ciclo de perseguição)
>
> Regras técnicas importantes:
> 1. Se vier tudo numa spritesheet só, deixa pelo menos 1–2 px **totalmente transparentes** a separar cada pose das vizinhas — o slicing automático do Unity funde poses que não tenham essa margem (já nos aconteceu com o pack existente).
> 2. Mantém o pé/base da personagem sempre no mesmo ponto do canvas em todas as frames de uma animação, para não "saltar" verticalmente ao animar.
> 3. Um ficheiro (ou grupo de frames) por estado, nomeado de forma óbvia (ex.: `Tomas_Walk_0..5`).

**Destino depois de gerado:** `Assets/_Project/Art/Characters/<Nome>/`, ficheiros `SPR_<Nome>_<Estado>_<N>`, clips `ANI_<Nome>_<Estado>`.

## Nota — controlo de versão (resolvido)
~~Toda a pasta `Assets/Assets/` estava excluída do git.~~ **Decidido (2026-10-18, Dinis): começar a versionar.** O bloqueio de arte/media foi removido do `.gitignore` e todo o `Assets/Assets/` (30 PNGs + README/licence do pack) está agora versionado via Git LFS (filtros já existiam em `.gitattributes` por `rules.md` §7). Já não há risco de Animation Clips/Animator Controllers/Prefabs ficarem com referências partidas para quem clonar o repositório.

## Especificação

**Parte A (arte existente):**
1. Animation Clip de "retrato vivo" por personagem (loop idle subtil), `Assets/_Project/Art/Characters/<Personagem>/`.
2. Animator Controller por personagem com esse estado (sem lógica de código associada — liga-se quando o diálogo/UI tiver o seu próprio sistema, fora desta feature).
3. Para o Boss: Animation Clip adicional da sequência de revelação (contorno → sombreado), separada do loop idle — é protótipo para a F1-04, não produto final.
4. Prefab de pré-visualização por personagem, só com SpriteRenderer + Animator (sem física, sem scripts de gameplay).
5. Testar cada Animator Controller na Animation/Animator window.

**Parte B (arte nova, após o Dinis gerar com o AI Assistant):**
6. Rever/ajustar import settings dos sprites novos (slicing com margem transparente entre poses — ver regra no brief; pivot consistente; PPU a condizer com o resto do projeto).
7. Animation Clips por estado (Idle/Walk/Run/Crouch para o Tomás; Idle/Move para cada inimigo).
8. Animator Controller por personagem com parâmetros mínimos (`Speed`, `IsRunning`, `IsCrouching` — já alinhados com os que a F1-01 espera) e transições.
9. Prefab de pré-visualização por personagem.

## Critérios de aceitação

**Parte A:**
- [x] Decisão tomada e registada sobre versionamento dos sprites usados (ver nota acima).
- [ ] 5 Animator Controllers de retrato (Player, Zombie, EyeGuy, Glitcher, Boss), cada um com um loop idle a funcionar.
- [ ] Protótipo da sequência de revelação do Boss, separado do loop idle.
- [ ] 5 prefabs de pré-visualização.
- [ ] Nenhum destes assets introduz dependências de gameplay (prefabs sem scripts de `Aurora.Gameplay`/`Aurora.Hotel`).

**Parte B (bloqueado em: Dinis gerar os sprites):**
- [ ] Sprites de movimento do Tomás e dos 4 inimigos gerados e importados.
- [ ] Animator Controllers de movimento criados a partir deles, com os parâmetros que a F1-01 espera.
- [ ] Prefabs de pré-visualização a animar corretamente ao mudar `Speed`/`IsRunning`/`IsCrouching` no Animator window.

## Testes
Não aplicável (conteúdo/arte, sem lógica testável). Validação é visual, no Editor.

## Registo
- 2026-10-18 — Dinis decidiu começar a versionar o asset pack. `Assets/Assets/` removido do `.gitignore`; 30 PNGs + README/licence commitados via Git LFS (`feat: start versioning the Haunted Hotel Asset Pack via Git LFS`).
- 2026-10-18 — Extraídos os 39 frames originais para inspeção visual (folha de contacto em artifact). Dinis confirmou que Player_4/5 (3 poses cada), Zombie_0/5 (2 poses cada) e Boss_3 (2 poses) estavam fundidos por slicing automático do Unity. Corrigido recortando pelos pontos reais (mínimo local de pixels opacos por linha, via `UnityEditor.U2D.Sprites.ISpriteEditorDataProvider` — a API clássica `TextureImporter.spritesheet` não persiste nesta versão do Unity). Total passou de 39 para 46 frames (`fix(F1-07): re-slice merged sprite frames`).
- 2026-10-18 — Inspeção visual ampliada (nearest-neighbor 10x) dos 46 frames revelou que o pack é de retrato/ícone, não de movimento — ver secção "Descoberta importante" acima. Âmbito da feature dividido em Parte A (retratos, com a arte existente) e Parte B (sprites de movimento, a gerar pelo Dinis com o Unity AI Assistant a partir do brief nesta página).

## Ideias
- Quando houver arte final de cada boss nomeado (F5/F6), repetir este processo substituindo os placeholders genéricos.
- Avaliar Animator Override Controllers para reduzir duplicação entre inimigos com esqueleto de animação parecido.
