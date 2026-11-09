# F1-04 — Lanterna e camada do passado
**Estado:** DOING · **Depende de:** F1-03 · **Assemblies:** Aurora.Core, Aurora.Gameplay

## Objetivo
A mecânica central do jogo. A lanterna construída pelo pai ilumina e, quando focada, **revela a camada do passado**: o que o hotel foi, o que escondeu e quem os monstros eram.

## Contexto na história
"A sua luz revela vestígios do passado: uma escada antes de ter ruído, uma passagem escondida numa parede, o rosto humano por baixo de um monstro."

## Âmbito
**Inclui:** lanterna com Light2D (cone) apontável, modo foco, máscara de revelação, objetos `PastLayerObject` (visual e/ou físico), sobreaquecimento, interação com monstros (hook para F5-02).
**Não inclui:** efeitos específicos de cada boss (F5/F6), som final (F4-03).

## Especificação
1. **Apontar:** com rato, segue o cursor (ângulo limitado a ±70° da direção do jogador); com gamepad, stick direito; sem stick, aponta em frente. Ação `Lantern` liga/desliga.
2. **Luz normal:** `Light2D` *Spot* com sombras (`ShadowCaster2D` nas paredes relevantes). Ilumina sem revelar.
3. **Foco (`LanternFocus` hold):** cone estreita, intensidade sobe, tom passa a `--aurora-ghost` e ativa a **revelação**.
4. **Revelação:** shader/Sprite material (Shader Graph 2D) que usa a posição/ângulo do cone para mascarar:
   - `PastLayerObject` com `RevealMode`: `ShowWhenLit` (aparece no cone), `HideWhenLit` (o presente desaparece), `Swap`.
   - Opcional: `PhysicalWhenRevealed` — collider ativo só enquanto revelado e durante 0.5 s de graça após sair (ex.: a escada antes de ruir).
   - Uma revelação mantida durante N segundos pode tornar-se **permanente** (passagem escondida descoberta) e grava flag.
5. **Sobreaquecimento:** foco contínuo aquece a lanterna (~6 s); ao sobreaquecer pisca e apaga 3 s. Arrefece desligada ou em luz normal. Comunicado por tremeluzir + som, não por barra.
6. **Monstros:** `IRevealable` com evento `OnRevealed(float duration)` — usado pelos confrontos para mostrar o rosto humano.
7. Performance: no máximo 1 Light2D com sombras no jogador; revelação em shader, sem `Physics` por frame quando o foco está desligado.
8. Web: confirmar que o shader funciona em WebGL2 (sem compute).

## Critérios de aceitação
- [ ] Apontar funciona com rato e com stick, sem saltos ao trocar de dispositivo. _(geometria/recorte testados; falta confirmação humana da troca real de dispositivo)_
- [x] Uma escada "do passado" só é atravessável enquanto revelada (+ graça).
- [x] Uma passagem escondida fica permanente após 2 s de foco e persiste (flag).
- [x] Sobreaquecimento apaga a lanterna e recupera.
- [ ] Corre a 60 fps no PC e ≥ 30 fps no Web numa sala com 4 luzes. _(precisa de playtest humano/profiler)_

## Testes
- EditMode: cálculo "ponto dentro do cone" (`LanternConeTests`); temporizador de aquecimento (`LanternHeatTests`).
- PlayMode: revelação física liga/desliga o collider, incluindo graça e revelação permanente (`PastLayerObjectTests`).

## Registo

- 2026-11-08/09 — Implementação completa: `LanternCone`/`LanternHeat`/`ActiveCameraResolver`/`IRevealable` (Core), `LanternController` (Gameplay), `PastLayerObject`/`RevealMode` (Gameplay). Nova ação `LanternAim` (Vector2). `Aurora.Gameplay` passou a referenciar `Unity.RenderPipelines.Universal.2D.Runtime`.
- 2026-11-09 — Decisão inicial (placeholder script-driven) substituída a meio: a pedido do utilizador, o Unity AI Assistant criou um Shader Graph real (`SG_RevealPast.shadergraph`/`M_RevealPast.mat`) a partir de um prompt preparado por mim — usado desde já em vez do placeholder.
- 2026-11-09 — Dois achados importantes durante a verificação ao vivo, ambos guardados em memória para não se repetirem: (1) `Shader.SetGlobalX` não tem efeito numa propriedade que o material já tenha valor próprio (todo o Shader Graph serializa um por omissão) — cada `PastLayerObject` escreve no seu próprio material clonado em vez de depender de difusão global; (2) recompilar com o Play Mode a correr limpa o `Services` estático sem nada voltar a registá-lo, silenciosamente — sempre Parar → Recompilar → Reproduzir. Depois de corrigir ambos, confirmado visualmente por captura de ecrã: o cone segue a direção certa e um objeto de teste muda de presente para passado dentro do cone, com borda suave.
- 2026-11-09 — Detalhe completo da investigação e todos os testes (80/80 EditMode, 19/20 PlayMode — falha pré-existente da F1-01 não relacionada) em `context/current-feature.md`. Critérios 2–4 verificados por teste automatizado; 1 e 5 ficam para confirmação humana antes de fechar a feature.
- 2026-11-09 — Testado ao vivo pelo utilizador (Tomás invisível ao correr a cena, "clico com botão direito e nada acontece"). Dois bugs sem relação com o shader: (1) `CinemachineConfiner2D` da `RoomPlayground` tinha ficado preso à `Room_RoomC` de testes manuais anteriores em vez da `Room_RoomA`, afastando a câmara real de onde o jogador estava — corrigido; (2) não existia nenhum `PastLayerObject` real na cena persistente para revelar (só objetos de teste descartáveis em Play Mode) — adicionado um `PastLayerTest` permanente via `RoomPlaygroundBuilder`. Gerado também, a pedido, o visual `PFB_HandheldLantern`/`SPR_HandheldLantern.png` via Unity AI Assistant, agora filho do pivô da luz.
- 2026-11-09 — Bug reportado pelo utilizador duas vezes ("lanterna aponta para baixo", primeiro a virar à esquerda, depois à direita): a causa real era que `Light2D` trata rotação `0°` como "cima" (+Y local), não "direita" (+X) como `Mathf.Atan2(y,x)` assume — confirmado forçando `rotation.eulerAngles.z` a 0/90/180/270 e observando a direção do cone. Corrigido subtraindo 90° ao ângulo do `Atan2` antes de o aplicar. (Tentativa anterior — separar o pivô da hierarquia do Tomás para não herdar o `localScale.x` negativo do flip esquerda/direita — não era a causa, mas ficou por ser mais robusta para a posição.) Verificado ao vivo em ambas as direções.
- 2026-11-09 — Causa do segundo relato também incluía: com o rato fora da Game View, `<Mouse>/position` lê `(0,0)`, que convertido para mundo aponta para baixo-esquerda do ecrã — a lanterna clampava nesse extremo. A pedido do utilizador, adicionada uma posição de descanso: se o rato não se mexer (ou estiver fora da janela) durante `_mouseIdleTimeout` (1 s), a lanterna repousa a apontar na direção em que o Tomás está virado. Verificado por `eval` ao vivo (`coneDir=(1,0)` a virar à direita, `coneDir=(-1,0)` a virar à esquerda, rato parado).
- 2026-11-09 — Dois novos relatos do utilizador após o anterior: (1) "a lanterna está posicionada mais abaixo" a virar para a direita, prefere a posição de quando vira para a esquerda; (2) "demora imenso tempo a reagir ao rato... sempre para baixo". Causas, ambas diferentes da rotação já corrigida: (1) `Sprite.bounds` do `SPR_HandheldLantern` confirmou que o pivô da sprite gerada está perto da mão, com o corpo da lanterna pendurado por baixo e ao lado — não centrado — por isso rodar -90°/+90° (repouso direita/esquerda) não dá imagens espelhadas, dá posições verticais diferentes. Corrigido: o visual (`ApplyVisual`, novo) já não herda a rotação do `Light2D`; usa sempre a mesma fórmula do lado "direita" a partir do desvio angular (`deviationDegrees`) e espelha por `localScale.x` — confirmado por `eval` (`visualRotZ=270` igual dos dois lados, `visualScale.x` `+1`/`-1`). (2) A posição de descanso só entrava em ação ao fim de 1 s inteiro de `(0,0)` parado — passou a ser tratada como repouso imediato assim que a leitura é exatamente `(0,0)` (valor-sentinela de "sem posição real"), sem esperar o temporizador. O temporizador continua a existir só para uma posição genuína mas parada no resto do ecrã.
- 2026-11-09 — Dois relatos do utilizador outra vez após testar a correção anterior: "para a esquerda a lanterna avança para fora do tomás" e "ainda demora a responder ao rato". Nesta sessão, a simulação de input (`simulate_key`/`simulate_pointer`) revelou-se pouco fiável — `Move`/`Lantern` por vezes não respondiam a teclas simuladas apesar de `Keyboard.current` confirmar a tecla premida — por isso a verificação ao vivo via rato real não foi possível de reproduzir fielmente neste ambiente automatizado; a causa exata da distância vista no ecrã do utilizador não ficou isolada com a mesma certeza dos achados anteriores. Mesmo assim, dois reforços foram aplicados por precaução/robustez: (1) a posição do visual (`ApplyVisual`) passou a ser escrita explicitamente (`_visual.transform.position = _pivot.position`) em vez de confiar na herança normal pai/filho — antes, a posição e a rotação do visual eram calculadas em separado mas só a rotação era forçada, deixando a posição tecnicamente dependente de como a Unity resolve a hierarquia; confirmado por `eval` que a distância visual↔pivô é exatamente 0 para qualquer desvio angular (0°/±30°/±70°) testado via chamada direta a `ApplyVisual`. (2) `_mouseIdleThresholdPixels` reduzido de 2px para 0.5px — um limiar maior podia confundir um movimento lento mas real do rato (variação sub-limiar por frame, ainda que real ao longo do segundo inteiro) com inatividade. **Por confirmar pelo utilizador:** se a lentidão persistir mesmo com o rato dentro da Game View (não só fora dela), suspeita-se de a janela do Editor não estar focada — o Unity só costuma processar posição real do cursor com o painel ativo.

## Ideias
- Filtros de cor encontrados ao longo do jogo (revelar coisas diferentes) — só se a F6 precisar.
