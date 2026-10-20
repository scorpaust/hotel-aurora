# F5-03 — Ala dos Quartos: A Governanta
**Estado:** TODO · **Depende de:** F5-02 · **Assemblies:** conteúdo

## Objetivo
Primeira ala completa e o primeiro boss. Fecha o vertical slice (~40 min com o prólogo) e é a **primeira build pública no Web**.

## Contexto na história
"A Governanta, que cose hóspedes aos colchões para garantir que nunca abandonam os quartos." Antes de ser monstro, era alguém que cuidava do hotel e de quem lá dormia — e que confundiu cuidar com prender.

> **Decisão a tomar antes de implementar:** quem era a Governanta (nome, perda, o que o hotel lhe ofereceu). Proposta: uma camareira que perdeu um filho que fugiu de casa e nunca voltou; o hotel prometeu-lhe que "ninguém volta a partir". Não implementar a história de fundo sem aprovação.

## Âmbito
**Inclui:** `SCN_Rooms` (2 andares de quartos, rouparia, corredor de serviço, elevador de roupa), a Governanta (patrulha + perseguição + confronto), hóspedes cosidos aos colchões (ambiente/puzzle), fragmento 2/5 do Tomás, memória da Governanta, entradas do livro.
**Não inclui:** arte final (placeholders aceitáveis; arte é decidida após o slice).

## Especificação
1. **Investigação:** a Governanta patrulha corredores; ouve-se a tesoura. Esconderijos: armários, debaixo das camas, carrinhos de roupa. Pistas: livro de turnos com um nome riscado (lanterna revela), uma carta nunca enviada, um sapato de criança na rouparia.
2. **Mecânica da ala:** hóspedes cosidos — com o alicate, Tomás corta as linhas de um hóspede preso; ele levanta-se como fantasma e abre um caminho (ou distrai a Governanta). Cortar faz ruído.
3. **Confronto (arena: rouparia grande):**
   - Fase 1: escapar às linhas que ela lança pelo chão (agachar/esconder), chegar aos 3 cestos e revelar com a lanterna a roupa de criança escondida.
   - Fase 2: elevador de roupa — reparar com fita isoladora enquanto ela se aproxima; descer até à lavandaria.
   - Fase 3: lanterna focada no rosto dela + mostrar o sapato → ela hesita.
4. **Resolução:** Libertar (com as 3 pistas) — ela lembra-se do nome do filho, as linhas desfazem-se, hóspedes libertados; Selar — fica presa ao seu próprio colchão.
5. Recompensa: fragmento de memória 2/5 do Tomás (o pai a arranjar uma cama para ele dormir no hotel) + chave para a próxima ala.
6. Final da ala: acesso ao Salão de Banquetes (fechado — fim do slice com "Continua…" na build pública).

## Critérios de aceitação
- [ ] Ala completa em 20–25 min para jogador novo.
- [ ] Ambos os desfechos funcionam e ficam no save/livro.
- [ ] Build Web pública: carregamento ≤ 20 s em 50 Mbps, 30+ fps num portátil de gama média.
- [ ] Testado por pelo menos 3 pessoas fora do projeto; notas no `## Registo`.

## Testes
- PlayMode "smoke" do confronto com atalhos de dev.

## Registo

## Ideias
