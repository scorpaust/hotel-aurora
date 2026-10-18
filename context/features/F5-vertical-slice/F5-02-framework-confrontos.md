# F5-02 — Framework de confrontos
**Estado:** TODO · **Depende de:** F5-01 · **Assemblies:** Aurora.Gameplay, Aurora.Narrative

## Objetivo
Estrutura reutilizável para os cinco "responsáveis" das alas. Cada confronto é um puzzle de investigação + pressão, nunca um combate.

## Contexto na história
"Cada confronto permite recuperar uma memória roubada. Descobrir quem aquelas criaturas foram revela também como vencê-las. Algumas podem ser libertadas. Outras farão tudo para impedir que Tomás lhes devolva a verdade."

## Âmbito
**Inclui:** `EncounterDefinition`, `EncounterController` com fases, pistas de identidade, resolução (Libertar / Selar), integração com perceção, perseguição, lanterna e memórias.
**Não inclui:** conteúdo de bosses (F5-03 e F6).

## Especificação
1. **Estrutura comum em 3 atos por ala:**
   - **Investigação** (exploração da ala com o boss a patrulhar/caçar): recolher **pistas de identidade** (objetos, revelações da lanterna, fragmentos da memória do hóspede).
   - **Confronto** (arena): o boss tem 2–3 fases; cada fase avança quando o jogador usa uma pista/mecanismo certo — quase sempre combinando **lanterna focada** (`IRevealable`) com um objeto do passado.
   - **Resolução:** se o jogador encontrou todas as pistas → opção **Libertar** (o boss lembra-se do nome, memória completa, final melhor para esse hóspede). Senão → **Selar** (o boss é contido; memória parcial). Ambas avançam a história.
2. `EncounterDefinition`: id, ala, pistas necessárias, fases (condições, eventos, perseguição associada), memória do hóspede, entradas do livro.
3. Fases podem trocar a arena (sub-salas), ligar perseguições (F3-02) e eventos do hotel (F3-03).
4. Falhar numa fase recomeça essa fase (checkpoint interno), não o confronto inteiro.
5. Resultado grava flags `<ala>.<boss>.libertado|selado` — usados no final (F6-06) e no livro de hóspedes.
6. Boss de teste "Manequim" numa arena de teste para validar o framework.

## Critérios de aceitação
- [ ] Boss de teste com 2 fases resolvido com lanterna + mecanismo.
- [ ] Com todas as pistas aparece a opção Libertar; sem elas, só Selar.
- [ ] Falhar na fase 2 recomeça a fase 2.
- [ ] Flags e livro atualizados conforme o resultado.

## Testes
- EditMode: avaliação de pistas → resoluções disponíveis.
- PlayMode: boss de teste do início ao fim com input simulado.

## Registo

## Ideias
