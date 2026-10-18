# UI Spec — Hotel Aurora

> Toda a UI é feita em **UI Toolkit** (UXML + USS). Nenhum texto visível hard-coded: tudo via Localization.

---

## 1. Direção visual

**Tom:** hotel de luxo dos anos 30–50 que apodreceu por dentro. Elegância art déco por cima, ferrugem e humidade por baixo. A UI deve parecer *papelaria do hotel* — cartões de receção, etiquetas de bagagem, o livro de hóspedes — e degradar-se quando o hotel está "acordado".

| Token | Valor | Uso |
|---|---|---|
| `--aurora-ink` | `#1B1714` | Fundo dos painéis, texto sobre papel |
| `--aurora-paper` | `#E9DFC8` | Papel/cartões |
| `--aurora-brass` | `#B88A3E` | Destaques, foco, seleção (latão da chave) |
| `--aurora-brass-dim` | `#7A5C2A` | Estados inativos |
| `--aurora-blood` | `#7E1F1F` | Perigo, perseguição, dano |
| `--aurora-ghost` | `#9FC4C7` | Beatriz, camada do passado, memórias |
| `--aurora-light` | `#FFE3A3` | Luz da lanterna, prompts de interação |

- **Tipografia:** uma serif art déco para títulos (ex.: *Limelight*, *Poiret One*) e uma serif legível para texto corrido (ex.: *Cormorant Garamond* / *EB Garamond*). Fontes incluídas no projeto (o Web não tem fontes do sistema). Fallback com glifos PT completos (ã, õ, ç, «»).
- **Tamanho mínimo de texto:** 24 px a 1080p. Escala da UI ajustável (80–150 %).
- **Molduras:** cantos art déco em 9-slice. Nada de cantos arredondados genéricos.
- **Animação:** transições USS de 120–250 ms. Nada pisca mais de 3 vezes por segundo.

---

## 2. Princípios

1. **HUD quase inexistente.** O jogo comunica pelo mundo: a luz da lanterna, o som, a Beatriz. Só aparecem elementos quando são precisos e desaparecem sozinhos.
2. **Diegético quando possível.** Inventário = abrir a mochila. Memórias = fotografias no livro de hóspedes. Mapa = planta do hotel na receção, anotada pelo pai.
3. **Navegável só com gamepad e só com teclado.** Todo o ecrã tem foco inicial definido e ordem de navegação explícita.
4. **Prompts mudam com o dispositivo** (teclado, Xbox, PlayStation, Steam Deck) no instante em que o jogador troca.
5. **O hotel mente.** Em momentos scriptados a UI pode distorcer-se (texto que muda, opções que trocam de lugar). Nunca nos menus de sistema (Pausa, Opções, Sair) — esses são sempre fiáveis.

---

## 3. Ecrãs

### 3.1 Arranque / splash
Logótipo, aviso de conteúdo (terror leve, sons súbitos), "Carregue em qualquer tecla". No Web: este é também o clique que dá foco ao canvas e desbloqueia áudio.

### 3.2 Menu principal
Fundo: fachada do Aurora à noite, janelas a acender uma a uma. Opções: **Continuar**, **Novo jogo**, **Carregar**, **Opções**, **Créditos**, **Sair** (oculto no Web).

### 3.3 HUD (in-game)
| Elemento | Quando aparece |
|---|---|
| Prompt de interação | Junto ao objeto, ao entrar no raio. Ícone do botão + verbo ("Desmontar", "Esconder"). |
| Ferramenta ativa | Canto inferior; 2 s após trocar, depois esconde. |
| Indicador da lanterna | Só se a lanterna estiver a falhar (sobreaquecimento): o feixe tremeluz + ícone discreto. |
| Indicador de perigo | Vinheta vermelha + pulsação no ecrã durante perseguições. Sem barras de vida. |
| Notificação de memória | Fotografia que "revela" no canto e desliza para fora. |
| Legenda de checkpoint | Sino da receção + "Progresso guardado" durante 1,5 s. |

### 3.4 Diálogo
- Caixa em baixo, estilo cartão de receção. Retrato à esquerda (Tomás, Beatriz, NPC). Nome em latão.
- Texto com efeito de escrita (velocidade configurável, instantâneo nas opções).
- Escolhas: lista vertical, máximo 4. A escolha "honesta" nunca é indicada visualmente.
- Linhas da Beatriz que surgem durante a exploração aparecem como **balão flutuante** sem pausar o jogo.

### 3.5 Mochila (inventário)
Overlay que pausa o jogo. Mochila aberta vista de cima; ferramentas em grelha; objetos-chave numa bolsa à parte. Selecionar = descrição + "Equipar". Navegação em grelha com gamepad.

### 3.6 Livro de hóspedes (diário / colecionáveis)
Abas: **Memórias** (fotografias recuperadas), **Hóspedes** (quem eram os monstros, desbloqueado ao descobrir), **Notas do pai** (avisos encontrados), **Planta** (mapa por andar, salas visitadas). Páginas viram com `TabLeft/TabRight`.

### 3.7 Pausa
Retomar, Livro de hóspedes, Opções, Voltar ao último checkpoint, Menu principal. Sempre legível, sem distorção.

### 3.8 Opções
- **Áudio:** geral, música, efeitos, ambiente, voz (se houver).
- **Vídeo:** resolução, ecrã inteiro/janela, V-Sync, limite de fps, brilho (calibração com imagem de referência), grão de filme on/off.
- **Controlos:** rebinding por dispositivo, sensibilidade, vibração on/off, inverter eixos, "segurar vs alternar" para correr/agachar/focar lanterna.
- **Acessibilidade:** legendas (tamanho, fundo, nome de quem fala), escala da UI, reduzir flashes, reduzir tremor de câmara, modo de perseguição "mais tolerante", indicadores visuais para sons importantes.
- **Idioma:** PT-PT / EN; texto e legendas independentes.

### 3.9 Game over / apanhado
Ecrã curto (≤ 2 s) com uma frase do hotel ("O seu quarto ainda está disponível."), depois volta ao checkpoint. Sem menu intermédio.

### 3.10 Créditos
Rolam sobre o livro de hóspedes a fechar. No final, a chave de latão com o nome de Tomás.

---

## 4. Implementação

- Um `UIDocument` por camada: `HUD`, `Overlay` (mochila/livro/pausa), `Dialogue`, `System` (loading, toasts).
- Ecrãs como classes `UIScreen` com `Show()`, `Hide()`, `FocusFirst()`; um `UIRouter` gere a pilha (Back fecha o topo).
- Estilos em `UI/Themes/Aurora.tss` com as variáveis da §1. Nenhuma cor literal em USS fora do tema.
- Bindings ao estado via eventos (`GameEvent`) ou runtime data binding do UI Toolkit; a UI nunca altera gameplay diretamente — envia pedidos aos serviços.
- `PanelSettings` com Scale Mode *Scale With Screen Size*, referência 1920×1080, match 0.5.
- Testar em 16:9, 16:10 (Steam Deck) e 21:9; safe area respeitada.
