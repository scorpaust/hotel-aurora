# Funcionalidade Atual

<!-- Ver especificação completa em context/features/F1-nucleo/F1-04-lanterna-camada-passado.md -->

# F1-04 — Lanterna e camada do passado

## Estado

Em progresso (DOING)

## Objetivos

A mecânica central do jogo. A lanterna construída pelo pai ilumina e, quando focada, revela a camada do passado: o que o hotel foi, o que escondeu e quem os monstros eram.

- Apontar: rato segue o cursor (±70° da direção do jogador), gamepad usa o stick direito, sem stick aponta em frente. Ação `Lantern` liga/desliga.
- Luz normal: `Light2D` *Spot* com sombras (`ShadowCaster2D` nas paredes relevantes), ilumina sem revelar.
- Foco (`LanternFocus`, hold): cone estreita, intensidade sobe, tom passa a `--aurora-ghost`, ativa a revelação.
- Revelação: shader/material (Shader Graph 2D) a mascarar por posição/ângulo do cone. `PastLayerObject` com `RevealMode` (`ShowWhenLit`/`HideWhenLit`/`Swap`), opcionalmente `PhysicalWhenRevealed` (collider ativo só enquanto revelado + 0.5s de graça). Revelação mantida N segundos pode tornar-se permanente e gravar flag.
- Sobreaquecimento: foco contínuo aquece (~6s); ao sobreaquecer pisca e apaga 3s; arrefece desligada ou em luz normal.
- Monstros: `IRevealable` com `OnRevealed(float duration)` — hook para os confrontos (F5-02) mostrarem o rosto humano.
- Performance: no máximo 1 `Light2D` com sombras no jogador; revelação em shader, sem `Physics` por frame com o foco desligado. Confirmar WebGL2.

**Não inclui:** efeitos específicos de cada boss (F5/F6), som final (F4-03).

## Critérios de aceitação

- [ ] Apontar funciona com rato e com stick, sem saltos ao trocar de dispositivo.
- [ ] Uma escada "do passado" só é atravessável enquanto revelada (+ graça).
- [ ] Uma passagem escondida fica permanente após 2s de foco e persiste (flag).
- [ ] Sobreaquecimento apaga a lanterna e recupera.
- [ ] Corre a 60 fps no PC e ≥30 fps no Web numa sala com 4 luzes.

## Histórico

<!-- Manter atualizado. Da mais antiga para a mais recente -->

- 2026-11-09 — Avançada a F1-04 (única `TODO` pronta: depende só da F1-03, `DONE`). Antes disso, adicionados ao roteiro 4 grupos de features em falta pedidos pelo utilizador (HUD de jogo F1-11, Cutscenes F4-04, Vídeo e gráficos F4-05, Créditos F4-06) — não bloqueiam esta feature, ficam só documentados para fases seguintes.
