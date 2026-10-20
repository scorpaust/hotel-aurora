# Funcionalidade Atual

<!-- Ver especificação completa em context/features/F1-nucleo/F1-10-fluxos-nivel-gerador.md -->

# F1-10 — Fluxos de nível e gerador

## Estado

DONE

## Objetivos

Descrever cada ala como uma sequência ordenada de "beats" (cutscene, exploração, inimigos, armadilhas, labirinto, interação narrativa, miniboss, bónus, boss) e dar uma ferramenta de Editor que monta a cena da ala a partir dessa sequência.

- `LevelBeatType`, `LevelBeat`, `LevelFlowDefinition` (SOs em `Aurora.Hotel`).
- `RoomDefinition` (criado agora — só existia na tabela do `codespec.md`, não em código).
- `LevelGeneratorWindow` (menu **Aurora/Level Generator**) + `LevelFlowValidator`, ambos em `Aurora.Editor`.

**Não inclui:** geração procedimental em runtime (nunca existe); a lógica de jogo de cada beat; migração automática de alas já existentes.

## Critérios de aceitação

- [x] `LevelFlowDefinition` com os 2 fluxos de exemplo, visíveis e editáveis no Inspector.
- [x] `LevelGeneratorWindow` abre a partir do menu, lista os `LevelFlowDefinition` existentes, e ao escolher um + aplicar, instancia placeholders na cena ativa na ordem certa dos beats.
- [x] Gerar duas vezes seguidas (fluxos diferentes) não deixa lixo da tentativa anterior nem GameObjects duplicados.
- [x] Nenhum destes scripts é referenciado por `Aurora.Gameplay`/`Aurora.Hotel` em runtime (só Editor).

## Histórico

<!-- Manter atualizado. Da mais antiga para a mais recente -->

- 2026-10-21 — Implementado na branch `feature/f1-10-fluxos-nivel-gerador` (a partir de `main`, já com F1-09 integrada). `LevelBeatType`/`LevelBeat`/`LevelFlowDefinition`/`RoomDefinition` em `Aurora.Hotel`; `LevelFlowValidator`/`LevelGeneratorWindow` em `Aurora.Editor` (que passou a referenciar `Aurora.Hotel`/`Aurora.Narrative` — só tooling, nunca no build do jogador). Placeholders de geração usam um sprite partilhado tingido por tipo de beat, em vez de 9 sprites separados. Geração testada duas vezes seguidas (via reflexão) sem deixar lixo. 2 fluxos de exemplo + 1 `RoomDefinition` de exemplo criados. 33/33 testes EditMode a passar (5 novos `LevelFlowTests`). Estado passa a REVIEW.
- 2026-10-21 — Confirmado. Estado passa a `DONE`, merge para `main`.
