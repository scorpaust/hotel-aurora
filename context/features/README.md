# Features — mapa de fases

Estados: `TODO` · `DOING` · `REVIEW` (feito, à espera de teste/confirmação) · `DONE` · `BLOCKED`

Cada ficheiro segue o mesmo modelo: objetivo, contexto na história, âmbito, especificação, critérios de aceitação, testes, registo.
Trabalha por ordem. Uma feature só começa quando as dependências estão `DONE`.

---

## Fase 0 — Fundação
Objetivo: projeto limpo, input moderno e builds Web/PC a funcionar **antes** de haver jogo.

| ID | Feature | Depende de | Estado |
|---|---|---|---|
| F0-01 | [Projeto, pacotes e assemblies](F0-fundacao/F0-01-projeto-e-pacotes.md) | — | DONE |
| F0-02 | [Input System e action maps](F0-fundacao/F0-02-input-system.md) | F0-01 | DONE |
| F0-03 | [Bootstrap, serviços e estado de jogo](F0-fundacao/F0-03-bootstrap-e-servicos.md) | F0-02 | DONE |
| F0-04 | [Build profiles e pipeline CLI](F0-fundacao/F0-04-builds-e-pipeline.md) | F0-03 | TODO |

## Fase 1 — Núcleo jogável
Objetivo: Tomás a andar numa sala de teste, com lanterna, ferramentas e checkpoints.

| ID | Feature | Depende de | Estado |
|---|---|---|---|
| F1-01 | [Controlador do Tomás](F1-nucleo/F1-01-controlador-tomas.md) | F0-04 | TODO |
| F1-02 | [Salas, câmara e transições](F1-nucleo/F1-02-salas-camara-transicoes.md) | F1-01 | TODO |
| F1-03 | [Sistema de interação](F1-nucleo/F1-03-interacao.md) | F1-01 | TODO |
| F1-04 | [Lanterna e camada do passado](F1-nucleo/F1-04-lanterna-camada-passado.md) | F1-03 | TODO |
| F1-05 | [Ferramentas e mochila](F1-nucleo/F1-05-ferramentas-mochila.md) | F1-03 | TODO |
| F1-06 | [Save, checkpoints e flags](F1-nucleo/F1-06-save-checkpoints.md) | F1-02, F1-05 | TODO |
| F1-07 | [Assets base: personagens e inimigos](F1-nucleo/F1-07-assets-personagens-inimigos.md) | F0-01 | REVIEW |
| F1-08 | [Cena de protótipo / sandbox de testes](F1-nucleo/F1-08-cena-prototipo-sandbox.md) | F1-07 | REVIEW |
| F1-09 | [Definições configuráveis de conteúdo](F1-nucleo/F1-09-definicoes-configuraveis.md) | F1-07 | DONE |
| F1-10 | [Fluxos de nível e gerador](F1-nucleo/F1-10-fluxos-nivel-gerador.md) | F1-09 | REVIEW |

## Fase 2 — Narrativa
| ID | Feature | Depende de | Estado |
|---|---|---|---|
| F2-01 | [Sistema de diálogo](F2-narrativa/F2-01-dialogo.md) | F1-06 | TODO |
| F2-02 | [Beatriz, a companheira](F2-narrativa/F2-02-beatriz.md) | F2-01, F1-04 | TODO |
| F2-03 | [Memórias e livro de hóspedes](F2-narrativa/F2-03-memorias-livro.md) | F2-01 | TODO |
| F2-04 | [Localização PT-PT / EN](F2-narrativa/F2-04-localizacao.md) | F2-01 | TODO |

## Fase 3 — O hotel caça
| ID | Feature | Depende de | Estado |
|---|---|---|---|
| F3-01 | [Esconderijos e furtividade](F3-ameaca/F3-01-esconderijos.md) | F1-03 | TODO |
| F3-02 | [Perseguições](F3-ameaca/F3-02-perseguicoes.md) | F3-01, F1-06 | TODO |
| F3-03 | [Eventos do hotel vivo](F3-ameaca/F3-03-hotel-vivo.md) | F3-02 | TODO |

## Fase 4 — Interface, áudio e acessibilidade
| ID | Feature | Depende de | Estado |
|---|---|---|---|
| F4-01 | [Menus, pausa e opções](F4-sistemas/F4-01-menus-opcoes.md) | F1-06, F2-04 | TODO |
| F4-02 | [Rebinding, prompts e acessibilidade](F4-sistemas/F4-02-rebinding-acessibilidade.md) | F4-01 | TODO |
| F4-03 | [Áudio adaptativo](F4-sistemas/F4-03-audio.md) | F3-02 | TODO |

## Fase 5 — Vertical slice
Objetivo: os primeiros ~40 min completos e polidos. **Primeira build pública no Web (itch.io ou página própria).**

| ID | Feature | Depende de | Estado |
|---|---|---|---|
| F5-01 | [Prólogo e Receção](F5-vertical-slice/F5-01-prologo-rececao.md) | Fases 1–4 | TODO |
| F5-02 | [Framework de confrontos](F5-vertical-slice/F5-02-framework-confrontos.md) | F5-01 | TODO |
| F5-03 | [Ala dos Quartos — A Governanta](F5-vertical-slice/F5-03-governanta.md) | F5-02, F1-10 | TODO |

## Fase 6 — Conteúdo completo
| ID | Feature | Depende de | Estado |
|---|---|---|---|
| F6-01 | [Salão de Banquetes — O Chef](F6-conteudo/F6-01-chef.md) | F5-03 | TODO |
| F6-02 | [Piscina — O Nadador do Fundo](F6-conteudo/F6-02-nadador.md) | F6-01 | TODO |
| F6-03 | [Salão de Festas — Os Noivos](F6-conteudo/F6-03-noivos.md) | F6-02 | TODO |
| F6-04 | [Bagageira — O Porteiro](F6-conteudo/F6-04-porteiro.md) | F6-03 | TODO |
| F6-05 | [Último andar — O Pai](F6-conteudo/F6-05-ultimo-andar.md) | F6-04 | TODO |
| F6-06 | [O Coração, a fuga e o final](F6-conteudo/F6-06-final-epilogo.md) | F6-05 | TODO |

## Fase 7 — Lançamento
| ID | Feature | Depende de | Estado |
|---|---|---|---|
| F7-01 | [Otimização Web](F7-lancamento/F7-01-otimizacao-web.md) | F6-06 | TODO |
| F7-02 | [Integração Steam](F7-lancamento/F7-02-steam.md) | F6-06 | TODO |
| F7-03 | [Polimento, QA e release](F7-lancamento/F7-03-qa-release.md) | F7-01, F7-02 | TODO |

---

## Modelo para novas features

```md
# FX-YY — Título
**Estado:** TODO · **Depende de:** … · **Assemblies:** …

## Objetivo
## Contexto na história
## Âmbito
**Inclui:** …
**Não inclui:** …
## Especificação
## Critérios de aceitação
- [ ] …
## Testes
## Registo
## Ideias
```
