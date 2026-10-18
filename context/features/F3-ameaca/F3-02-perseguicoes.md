# F3-02 — Perseguições
**Estado:** TODO · **Depende de:** F3-01, F1-06 · **Assemblies:** Aurora.Gameplay, Aurora.Hotel

## Objetivo
Sequências de fuga tensas mas justas: o jogador sabe sempre para onde ir, falha perto do checkpoint e recomeça em segundos.

## Contexto na história
Bosses que perseguem pelos corredores; o hotel a desfazer-se atrás do Tomás; a fuga final do Aurora em colapso.

## Âmbito
**Inclui:** `ChaseSequence` (início, rota, fim), perseguidor genérico que segue o caminho com elástico, obstáculos interativos (portas a fechar, prateleiras a derrubar), "apanhado" e respawn, modo tolerante (acessibilidade).
**Não inclui:** perseguidores finais de cada boss (F5/F6 usam isto).

## Especificação
1. `ChaseSequence`: lista de salas/pontos, perseguidor, condições de vitória (chegar a X, fechar Y), checkpoint de recomeço próprio.
2. Estado global `Chase`: música, vinheta vermelha (`uispec.md` §3.3), Beatriz em modo aviso.
3. **Elástico:** o perseguidor acelera se o jogador estiver longe e abranda se estiver muito perto, para criar "quase-apanhado" sem injustiça. Parâmetros por perseguição.
4. **Obstáculos:** `ChaseObstacle` — o jogador interage para fechar/derrubar; atrasa o perseguidor N segundos. Alguns exigem ferramenta rápida (sequência de 1–2 passos).
5. **Apanhado:** fade, frase do hotel (`uispec.md` §3.9), respawn no início da perseguição ≤ 3 s. Sem animação gráfica.
6. **Modo tolerante:** perseguidor 15 % mais lento e elástico mais generoso.
7. Telemetria local (dev): número de mortes por perseguição para afinação.

## Critérios de aceitação
- [ ] Perseguição de teste com 3 salas, 1 porta para fechar e 1 obstáculo com ferramenta.
- [ ] Ser apanhado recomeça em ≤ 3 s com estado limpo.
- [ ] Modo tolerante altera a dificuldade de forma percetível.
- [ ] A direção correta é legível sem HUD (luz, Beatriz, composição).

## Testes
- EditMode: cálculo do elástico.
- PlayMode: apanhado → respawn → estado `Chase` reiniciado.

## Registo

## Ideias
