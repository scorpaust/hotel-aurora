# F1-07 — Assets base: personagens e inimigos (sprites, animações, Animator Controllers)
**Estado:** TODO · **Depende de:** F0-01 · **Assemblies:** Nenhum (conteúdo/arte apenas)

## Objetivo
Transformar os spritesheets já importados em `Assets/Assets/Sprites/` (Player, Zombie, EyeGuy, Glitcher, Boss — Haunted Hotel Asset Pack, CC0) em Animation Clips e Animator Controllers utilizáveis, com um prefab de pré-visualização por personagem, para que haja algo visual e animado em jogo antes de existir arte final.

## Contexto na história
Nenhum diretamente — isto é preparação técnica/artística. Os cinco sprites existentes (Player, Zombie, EyeGuy, Glitcher, Boss) são genéricos do asset pack e não correspondem 1:1 às personagens nomeadas da história (Tomás, Beatriz, a Governanta, o Chef, etc.); servem para prototipagem e testes de gameplay até existir arte final de cada um. Ajuda a "ver o jogo a acontecer" durante o F1 (Tomás a andar) e dá já corpo a inimigos genéricos para prototipar o F3 (perseguições, esconderijos).

## Âmbito
**Inclui:**
- Confirmar/ajustar import settings dos 5 spritesheets existentes (já estão em Sprite Mode = Multiple, com sprites individuais nomeados e slicing feito: Player 10 frames, Zombie 7, EyeGuy 6, Glitcher 6, Boss 10).
- Mapear visualmente os frames de cada spritesheet para poses/estados (idle, andar por direção, etc.) — a confirmar com o Dinis e, quando útil, com o Unity AI Assistant (tem acesso às imagens dentro do Editor e pode sugerir agrupamentos/animações).
- Criar Animation Clips (`ANI_<Personagem>_<Estado>`) por personagem a partir desse mapeamento.
- Criar um Animator Controller por personagem com os parâmetros mínimos para o movimento 2D (ex.: `Speed`, `IsMoving`, `Direction`) e as transições entre estados.
- Criar um prefab simples de pré-visualização por personagem (`PFB_<Personagem>Preview` — SpriteRenderer + Animator, sorting layer correto) só para testes, não para gameplay final.
- Decidir e documentar, no `## Registo` desta feature, o destino definitivo dos assets dentro de `Assets/_Project/Art/Characters/<Personagem>/` (ver nota abaixo sobre controlo de versão).

**Não inclui:**
- Lógica de controlo do jogador (isso é F1-01 — que já está desenhado para usar placeholder, não esta arte).
- IA/comportamento de inimigos (F3+).
- Arte final das personagens nomeadas da história (Tomás, Beatriz, Governanta, Chef, Nadador do Fundo, Noivos, Porteiro, Pai) — essa é trabalho de conteúdo em F5/F6, feito quando houver arte própria.
- Rigs de ossos via 2D Animation/PSD Importer (os spritesheets são sprites simples por frame, não PSD com camadas; não há necessidade de esqueleto para já).

## Nota importante — controlo de versão
Toda a pasta `Assets/Assets/` (o asset pack, incluindo estes 5 spritesheets) está atualmente **excluída do git** (`.gitignore`), por decisão explícita do Dinis numa sessão anterior ("só código"). Os filtros Git LFS já estão configurados em `.gitattributes` (`rules.md` §7) mas inertes por causa disso.

Isto cria um problema real para esta feature: Animation Clips, Animator Controllers e Prefabs criados aqui vão referenciar os sprites por GUID. Se os `.png`/`.meta` de origem continuarem fora do git, esses assets ficam **partidos** para quem clonar o repositório (incluindo eventuais builds CI/CD) — só funcionam na máquina do Dinis.
Antes de avançar com a criação dos clips/controllers, é preciso decidir:
1. Começar a versionar (via LFS, já configurado) pelo menos os sprites efetivamente usados em animações, ou
2. Manter tudo fora do git por agora e aceitar que estes assets só funcionam localmente até essa decisão mudar.

A opção 1 é a recomendada, dado que o LFS já está pronto para este cenário exato.

## Especificação
1. Rever import settings dos 5 `.png` (compressão, pivot, PPU — já estão a 100 PPU, pivot 0.5/0.5 por sprite individual; confirmar se serve para o tamanho de jogo pretendido).
2. Para cada personagem, listar o mapeamento frame → pose no `## Registo` (ex.: `Player_0..3` = idle por direção, `Player_4..9` = andar, etc. — a confirmar visualmente, não assumir).
3. Criar os Animation Clips por personagem/estado, com `Assets/_Project/Art/Characters/<Personagem>/`.
4. Criar o Animator Controller por personagem com states + transições (sem lógica de código associada ainda — isso liga-se ao personagem/inimigo quando esse tiver o seu próprio MonoBehaviour, fora desta feature).
5. Criar o prefab de pré-visualização por personagem, só com SpriteRenderer + Animator (sem física, sem scripts de gameplay).
6. Testar cada Animator Controller na Animation/Animator window e, se fizer sentido, com uma cena de teste simples (reutilizar/ligar com `Tests/Scenes/SCN_InputSandbox` da F0-02, se já existir, para ver o Player preview a reagir ao Move).

## Critérios de aceitação
- [ ] Decisão tomada e registada sobre versionamento dos sprites usados (ver nota acima).
- [ ] 5 Animator Controllers criados (Player, Zombie, EyeGuy, Glitcher, Boss), cada um com pelo menos um estado idle e um estado de movimento.
- [ ] 5 prefabs de pré-visualização, cada um a reproduzir a animação correta ao mudar o parâmetro de movimento no Animator window.
- [ ] Mapeamento de frames documentado no Registo desta feature (serve de referência para arte final mais tarde).
- [ ] Nenhum destes assets introduz dependências de gameplay (prefabs não têm scripts de `Aurora.Gameplay`/`Aurora.Hotel`).

## Testes
Não aplicável (conteúdo/arte, sem lógica testável). Validação é visual, no Editor.

## Registo

## Ideias
- Quando houver arte final de cada boss nomeado (F5/F6), repetir este processo substituindo os placeholders genéricos.
- Avaliar Animator Override Controllers para reduzir duplicação entre inimigos com esqueleto de animação parecido.
