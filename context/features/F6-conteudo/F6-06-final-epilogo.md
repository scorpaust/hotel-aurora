# F6-06 — O Coração, a fuga e o final
**Estado:** TODO · **Depende de:** F6-05 · **Assemblies:** conteúdo

## Objetivo
A última oferta, a fuga do Aurora em colapso, a devolução da memória e o epílogo. O final tem de respeitar a história **sem alterações**.

## Contexto na história
Cozinha iluminada pelo sol, pai mais novo, pequeno-almoço na mesa: a manhã anterior ao desaparecimento. O Aurora oferece uma vida inteira naquele momento — basta entregar a chave e aceitar o quarto. Beatriz começa a desaparecer. Tomás percebe que tem de devolver ao hotel a primeira memória roubada (a filha do fundador, incluindo o acidente). Fuga pelo Aurora em colapso, perseguido pelos monstros que ainda preferem a mentira. O fundador lembra-se do nome da filha e abre-lhe a porta. O sol entra. Tomás sai com o pai envelhecido. Beatriz vê alguém à sua espera: "Se algum dia abrires um hotel… põe uma piscina normal." Epílogo: a chave de latão, o número do quarto mudou — agora tem o nome dele.

## Âmbito
**Inclui:** `SCN_Heart` (cozinha do sol — jogável, calma), a oferta (escolha), `SCN_Collapse` (fuga por versões em colapso das alas visitadas), quarto da filha do fundador (bolo de laranja), cena do fundador, saída, Beatriz, epílogo, créditos.

## Especificação
1. **A oferta:** a cozinha é jogável e acolhedora (música calma, sem ameaça). Interagir com objetos mostra memórias felizes. Opção explícita **"Entregar a chave"**: leva a um *final alternativo curto* (Tomás fica; créditos sombrios; voltar ao checkpoint anterior permitido). O final canónico exige recusar.
   > **Decisão a tomar antes de implementar:** se o final alternativo existe ou se a opção é só uma tentação que o jogo recusa. Não implementar sem confirmação.
2. Beatriz a desaparecer (parâmetro da F2-02) durante toda a fuga; linhas cada vez mais curtas.
3. **Fuga em colapso:** sequência de perseguições (F3-02) por trechos das alas anteriores. **Bosses selados reaparecem como perseguidores; bosses libertados aparecem como aliados** que abrem caminho — consequência visível das escolhas.
4. **Quarto da filha:** cheiro a bolo de laranja (sinal visual), brinquedos que se mexem. Lanterna revela o acidente — cena de memória com tratamento cuidadoso (sugerido, nunca gráfico).
5. **O fundador** lembra-se do nome da filha → Timeline → luz do sol entra no hotel (transição de iluminação global de todas as luzes 2D).
6. **Saída:** escadaria, Beatriz e a mãe (silhueta), a linha final dela. Desbloqueia o nome dela no livro de hóspedes (agora pode ser lido — já não tem medo).
7. **Epílogo:** casa, caixa de ferramentas, chave de latão com o nome do Tomás. Créditos sobre o livro a fechar (`uispec.md` §3.10).
8. Achievements de final (ligados em F7-02).

## Critérios de aceitação
- [ ] Final canónico fiel à história, jogável do Coração aos créditos sem soft-locks.
- [ ] Bosses libertados/selados mudam a fuga de forma visível.
- [ ] Página da Beatriz preenchida após o final.
- [ ] Guardar depois dos créditos permite "Novo jogo +" (se aprovado) ou volta ao menu.

## Testes
- PlayMode smoke da fuga com combinações de flags (todos libertados / todos selados).

## Registo

## Ideias
- Cena extra pós-créditos se todos os hóspedes forem libertados.
