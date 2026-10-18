# F2-04 — Localização PT-PT / EN
**Estado:** TODO · **Depende de:** F2-01 · **Assemblies:** Aurora.Core, Aurora.UI, Aurora.Narrative

## Objetivo
Todo o texto do jogo vem do pacote Localization; PT-PT é a língua de origem, EN completo antes do vertical slice público.

## Contexto na história
O nome do jogo e dos personagens não se traduz. "Aurora", "Tomás", "Beatriz" mantêm-se.

## Âmbito
**Inclui:** Locales `pt-PT` e `en`, String Tables por domínio (`UI`, `Dialogue_<Ala>`, `Items`, `Memories`, `GuestBook`, `Hints`), Asset Tables para imagens com texto, `ILocalizationService`, seleção inicial pela língua do sistema/browser, troca em runtime, fontes com fallback.
**Não inclui:** outras línguas (preparar só a estrutura).

## Especificação
1. Locales: `pt-PT` (default/fallback) e `en`.
2. Selector de arranque: língua guardada nas definições → língua do sistema → `pt-PT`.
3. Troca em runtime atualiza UI e diálogo abertos sem recarregar cena.
4. Smart Strings para variáveis (nome de ferramenta, contagens).
5. Fontes: TextCore/UI Toolkit com fallback para glifos PT; testar «», —, ç, ã, õ, ê.
6. Ferramenta `Aurora/Localization/Report`: chaves sem tradução EN, chaves órfãs, comprimento EN > 140 % do PT (risco de layout).
7. Exportar/importar tabelas em CSV para revisão externa.

## Critérios de aceitação
- [ ] Nenhuma string visível hard-coded (pesquisa por literais em UXML/C# de UI).
- [ ] Trocar língua no menu atualiza tudo imediatamente.
- [ ] Relatório de localização sem erros para o conteúdo existente.
- [ ] Web arranca na língua do browser.

## Testes
- EditMode: relatório deteta chave sem EN.

## Registo

## Ideias
- Espanhol e francês para o lançamento Steam se o orçamento permitir.
