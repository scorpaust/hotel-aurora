# F2-02 — Beatriz, a companheira
**Estado:** TODO · **Depende de:** F2-01, F1-04 · **Assemblies:** Aurora.Gameplay, Aurora.Narrative

## Objetivo
Uma companheira fantasma que segue o Tomás, comenta o mundo, aponta atalhos e abre passagens que só um fantasma atravessa — sem nunca resolver os puzzles pelo jogador.

## Contexto na história
Beatriz conhece os atalhos, faz piadas nos piores momentos, espera pela mãe e teme encontrar o próprio nome no livro de hóspedes. No fim começa a desaparecer.

## Âmbito
**Inclui:** seguimento flutuante, posicionamento inteligente (não tapa o jogador nem a lanterna), `CallBeatriz` (dica contextual), passagens fantasma, reação à lanterna, linhas ambientais por gatilho, estado "a desaparecer" (para F6-06), comportamento em perseguições.
**Não inclui:** IA de combate (não existe), escolhas que a matem.

## Especificação
1. Movimento: segue com atraso suave (spring), flutua, atravessa paredes; teleporta em transições de sala.
2. **CallBeatriz:** ela vai até ao ponto de interesse mais relevante da sala (`HintPoint` com prioridade e condição de flag) e diz uma linha. Cooldown 20 s. Depois de 3 chamadas na mesma sala, dica mais explícita.
3. **Passagens fantasma:** `GhostPassage` — Beatriz entra, aparece do outro lado e abre um trinco/alavanca. Só quando o jogador está perto e pede (Interact na passagem).
4. **Lanterna:** se o foco lhe acerta, ela fica parcialmente "humana" (cor de pele, roupa da época) e faz um comentário — a primeira vez é um momento narrativo; nas seguintes, linhas curtas e raras.
5. **Linhas ambientais:** `BeatrizTrigger` em salas (uma vez por save), piadas com prioridade baixa; nunca durante uma perseguição exceto avisos ("Esquerda! Esquerda!").
6. **Perseguições:** afasta-se, mantém-se visível, aponta a direção da fuga.
7. **Desaparecer:** parâmetro 0–1 que reduz opacidade e corta linhas (usado no final).
8. Estado gravado no save (flags + sala).

## Critérios de aceitação
- [ ] Segue o jogador por 3 salas, escadas e uma transição de ala sem ficar presa.
- [ ] CallBeatriz leva a uma pista válida e respeita cooldown.
- [ ] Passagem fantasma abre a porta do outro lado.
- [ ] Reação à lanterna acontece e a primeira vez grava flag.
- [ ] Nunca bloqueia a visão do feixe da lanterna mais de 0.5 s.

## Testes
- EditMode: escolha de `HintPoint` por prioridade/condição.
- PlayMode: seguimento após transição de sala.

## Registo

## Ideias
- Beatriz reagir ao jogador ficar parado muito tempo ("Estás a contar os azulejos?").
