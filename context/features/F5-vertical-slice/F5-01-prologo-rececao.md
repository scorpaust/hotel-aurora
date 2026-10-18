# F5-01 — Prólogo e Receção
**Estado:** TODO · **Depende de:** Fases 1–4 · **Assemblies:** conteúdo (cenas, SO, Timeline)

## Objetivo
Os primeiros 15–20 minutos completos: do telefonema em casa até à primeira chegada às alas. Ensina tudo sem tutoriais em texto.

## Contexto na história
Casa do Tomás → telefone ("Se receberes um convite, não venhas…") → batem à porta → envelope, chave de latão, fotografia ("O seu quarto estará disponível até ao amanhecer.") → fachada do Aurora com as janelas acesas → receção impecável, música, jantar para convidados invisíveis → a porta desaparece → campainha: "Bem-vindo de volta, Tomás." → encontra Beatriz → recupera a lanterna do pai.

## Âmbito
**Inclui:** `SCN_Home` (curta, ensina mover/interagir), `SCN_Exterior`, `SCN_Reception` (receção, salão, escritório da manutenção), primeira cena de memória do Tomás (fragmento 1/5), encontro com Beatriz, obtenção da lanterna, primeiro evento do hotel (corredor que muda), primeira mini-perseguição curta, porta de acesso à Ala dos Quartos.
**Não inclui:** a Governanta (F5-03).

## Especificação (batidas)
1. **Casa:** noite; telefone toca → diálogo sem escolha; batidas → envelope (PickUp: chave de latão, fotografia). Ensina Move, Interact, Examine.
2. **Exterior:** caminhada até ao portão; janelas acendem uma a uma (Timeline). Portão abre-se sozinho.
3. **Receção:** tudo limpo e iluminado. Malas, música, mesa posta. Ao avançar, a porta de entrada desaparece (cutscene curta). Campainha toca sozinha → linha do hotel.
4. **Primeiro sinal do pai:** lâmpada a piscar (ElectricalHint) leva ao escritório da manutenção.
5. **Beatriz:** aparece a fazer uma piada; diálogo de apresentação (2 escolhas de tom, ambas válidas). Ensina CallBeatriz e passagem fantasma para entrar no escritório.
6. **Lanterna:** dentro do escritório, num painel (chave de fendas encontrada antes) → lanterna do pai. Primeira revelação: as marcas do pai na parede, e uma fotografia do Tomás pequeno no hotel → **fragmento de memória 1/5** (Tomás criança, de mão dada com o pai na receção).
7. **O hotel acorda:** luzes mudam de quente para frio; corredor que muda; primeiro esconderijo ensinado com um "hóspede" errante (sem perigo real); mini-perseguição de 1 sala até à porta da Ala dos Quartos (fechada com a chave de latão).
8. Checkpoints: entrada da receção, depois da lanterna, antes da perseguição.

## Critérios de aceitação
- [ ] Jogável do início ao fim sem texto de tutorial; cada ação nova é ensinada por situação.
- [ ] Duração 15–20 min para um jogador novo (medir com 2 testers).
- [ ] Todas as linhas em PT-PT e EN.
- [ ] Funciona no build Web com carga inicial dentro do orçamento (`codespec.md` §2).

## Testes
- PlayMode "smoke": percorrer a sequência com input simulado até ao checkpoint final (pode usar atalhos de dev).

## Registo

## Ideias
