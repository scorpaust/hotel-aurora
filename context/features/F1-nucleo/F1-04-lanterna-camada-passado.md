# F1-04 — Lanterna e camada do passado
**Estado:** TODO · **Depende de:** F1-03 · **Assemblies:** Aurora.Gameplay

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
- [ ] Apontar funciona com rato e com stick, sem saltos ao trocar de dispositivo.
- [ ] Uma escada "do passado" só é atravessável enquanto revelada (+ graça).
- [ ] Uma passagem escondida fica permanente após 2 s de foco e persiste (flag).
- [ ] Sobreaquecimento apaga a lanterna e recupera.
- [ ] Corre a 60 fps no PC e ≥ 30 fps no Web numa sala com 4 luzes.

## Testes
- EditMode: cálculo "ponto dentro do cone"; temporizador de aquecimento.
- PlayMode: revelação física liga/desliga o collider.

## Registo

## Ideias
- Filtros de cor encontrados ao longo do jogo (revelar coisas diferentes) — só se a F6 precisar.
