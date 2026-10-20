# F6-02 — Piscina: O Nadador do Fundo
**Estado:** TODO · **Depende de:** F6-01 · **Assemblies:** conteúdo + Aurora.Gameplay (água)

## Objetivo
Terceira ala. Introduz **água e nível variável**: quartos inundados e uma piscina sem fundo que liga partes do hotel.

## Contexto na história
"O Nadador do Fundo, que percorre uma piscina sem fundo, ligada aos quartos inundados do hotel." Beatriz despede-se no fim a pedir "uma piscina normal".

> **Decisão a tomar antes de implementar:** quem era o Nadador e o que procurava no fundo.

## Âmbito
**Inclui:** `SCN_Pool` (piscina interior, balneários, casa das máquinas, quartos inundados), sistema de nível de água, o Nadador, fragmento 4/5 do Tomás, memória do Nadador.

## Especificação
1. **Água:** volumes `WaterBody` com nível controlável por válvulas/bombas (ferramentas). Dentro de água: movimento mais lento, sem lanterna normal (só foco curto debaixo de água), tempo limite de fôlego (reutilizar o fôlego da F1-01).
2. **Nadador:** só se move dentro de água; perceção por vibração (o jogador a mover-se na água faz ruído). Fora de água o jogador está seguro — o puzzle é baixar/subir níveis para criar caminhos seguros.
3. Sprite Shape para a superfície; shader de distorção simples compatível com WebGL2.
4. Confronto: drenar a piscina em 3 fases enquanto o Nadador força as comportas; revelar com a lanterna o que está no fundo.
5. Recompensa: fragmento 4/5 (a noite: Tomás e o pai a entrar no hotel juntos).

## Critérios de aceitação
- [ ] Níveis de água persistem no save e mudam a navegação.
- [ ] Shader de água a 30+ fps no Web.
- [ ] Libertar/Selar funcionam.

## Testes
- EditMode: lógica de níveis com várias válvulas.
- PlayMode smoke do confronto.

## Registo

## Ideias
