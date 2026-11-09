# F4-05 — Vídeo e gráficos
**Estado:** TODO · **Depende de:** F4-01 · **Assemblies:** Aurora.UI, Aurora.Core

## Objetivo
O separador "Vídeo" de `uispec.md` §3.8 completo: resolução, ecrã inteiro/janela, V-Sync, limite de fps, calibração de brilho, grão de filme.

## Contexto na história
Nenhum direto — conforto e acessibilidade visual (ligado ao "reduzir flashes" da F4-02).

## Âmbito
**Inclui:** ecrã de Vídeo dentro do `UIRouter` (F4-01); resolução + modo janela/ecrã inteiro (oculto no Web, `uispec.md` §3.8); V-Sync; limite de fps; imagem de referência para calibração de brilho; grão de filme on/off; tudo gravado nas Definições (ficheiro separado do progresso, `codespec.md` §7).
**Não inclui:** qualidade gráfica por tier (Low/Med/High de sombras/pós-processamento) — fica em Ideias até haver necessidade concreta em hardware fraco.

## Especificação
1. Resolução: lista de `Screen.resolutions` + toggle janela/ecrã inteiro — escondido no Web.
2. V-Sync: `QualitySettings.vSyncCount` 0/1.
3. Limite de fps: `Application.targetFrameRate`; desativado (ou ignorado) se V-Sync estiver ligado.
4. Brilho: imagem de referência clássica de calibração + slider que ajusta a exposição do Volume global URP.
5. Grão de filme: toggle que ativa/desativa o override de Film Grain no Volume global (`codespec.md` §3: "Post-processing (vinheta, grão, aberração)").
6. Tudo persistido no ficheiro de definições, aplicado de imediato (mesmo padrão da F4-01 item 5).

## Critérios de aceitação
- [ ] Mudar resolução/V-Sync/fps aplica-se de imediato e persiste entre sessões.
- [ ] No Web, resolução/janela ficam escondidos; o resto funciona.
- [ ] Brilho e grão de filme têm efeito visível imediato.

## Testes
- EditMode: serialização das definições de vídeo.

## Registo

## Ideias
- Tiers de qualidade gráfica (Low/Med/High) se o F7-01 (otimização Web) ou o feedback de hardware fraco vier a pedir.
