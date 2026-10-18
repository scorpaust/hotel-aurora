# F7-03 — Polimento, QA e release
**Estado:** TODO · **Depende de:** F7-01, F7-02 · **Assemblies:** todos

## Objetivo
Chegar ao 1.0: sem bugs bloqueantes, desempenho estável, texto revisto e builds Web e Steam publicados.

## Âmbito
**Inclui:** plano de testes completo, revisão de texto PT-PT/EN, passagem de performance, acessibilidade, crash reporting, versões e changelog, checklist de lançamento.

## Especificação
1. **Plano de testes:** percorrer o jogo completo em PC (teclado e gamepad), Steam Deck e Web (Chrome, Firefox, Edge, Safari macOS); todas as combinações de desfechos dos bosses.
2. **Soft-lock hunt:** para cada ala, lista de pontos de não-retorno e verificação de que o jogador nunca fica preso sem ferramenta/chave.
3. **Texto:** revisão PT-PT por pessoa externa; revisão EN nativa; relatório de localização limpo.
4. **Performance:** Profiler em cada ala (CPU, GC, memória); 0 picos > 33 ms em gameplay normal no PC alvo.
5. **Crash/erros:** log local rotativo + opção de enviar relatório (sem dados pessoais; RGPD — consentimento explícito e aviso de privacidade).
6. **Versões:** SemVer; `buildinfo.json`; changelog em PT e EN.
7. **Checklist de lançamento:** classificação etária (PEGI via IARC na Steam), avisos de conteúdo, créditos e licenças de terceiros (fontes, áudio, Steamworks.NET), página da loja, trailer, build de review.

## Critérios de aceitação
- [ ] Zero bugs bloqueantes e zero soft-locks conhecidos.
- [ ] Todos os achievements obtidos numa sessão de teste.
- [ ] Builds 1.0 Web e Steam publicados.

## Registo

## Ideias
