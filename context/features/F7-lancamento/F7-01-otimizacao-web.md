# F7-01 — Otimização Web
**Estado:** TODO · **Depende de:** F6-06 · **Assemblies:** Aurora.Editor, Aurora.Platform

## Objetivo
O jogo completo a carregar depressa e a correr estável no browser, com a versão Web a servir de demo/porta de entrada para a Steam.

## Âmbito
**Inclui:** auditoria de tamanho, Addressables remotos/locais por ala, compressão de texturas e áudio, stripping, memória, template final, página de hosting, decisão sobre WebGPU.

## Especificação
1. Relatório de build (Build Report) por ala; objetivo inicial ≤ 60 MB comprimido, restante carregado por ala.
2. Texturas: atlas por ala, compressão adequada a WebGL2 (ASTC/ETC2/DXT conforme suporte, com fallback); áudio Vorbis com qualidade por categoria; música em streaming.
3. Code stripping *High* com `link.xml` para tipos usados por reflexão (Newtonsoft, Localization).
4. Memória: medir pico em todas as alas; nenhum crash com 1 GB.
5. Template `AuroraMinimal` final: arte da fachada, barra de progresso, botão fullscreen, aviso para browsers móveis ("melhor no computador").
6. Hosting: headers corretos para `.br`/`.gz` e `wasm`; documentar para itch.io e servidor próprio.
7. **WebGPU:** avaliar como opção (Unity 6.6+ suporta-o oficialmente); manter WebGL2 como predefinição. Só mudar com testes em Chrome, Firefox e Safari.
8. Versão demo: build Web com flag que termina no fim do vertical slice + link para a Steam.

## Critérios de aceitação
- [ ] Carga inicial dentro do orçamento; tamanho final registado.
- [ ] 30+ fps em todas as alas num portátil de gama média (Chrome e Firefox).
- [ ] Save funciona em todos os browsers testados.
- [ ] Demo Web publicada com link para a Steam.

## Registo

## Ideias
