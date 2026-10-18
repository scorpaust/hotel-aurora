# F1-06 — Save, checkpoints e flags
**Estado:** TODO · **Depende de:** F1-02, F1-05 · **Assemblies:** Aurora.Persistence, Aurora.Core, Aurora.Gameplay

## Objetivo
Progresso guardado de forma fiável em PC e no browser, com checkpoints automáticos e flags do mundo como única fonte de estado narrativo.

## Contexto na história
O sino da receção marca cada checkpoint. "O seu quarto estará disponível até ao amanhecer."

## Âmbito
**Inclui:** `WorldFlags`, `FlagKeys` gerado, `SaveData` com versão, `ISaveService` (3 slots + ficheiro de definições), `Checkpoint` (componente), "voltar ao último checkpoint", backends PC e Web, migrações.
**Não inclui:** Steam Cloud (F7-02), UI de slots completa (F4-01 — aqui basta debug).

## Especificação
1. `WorldFlags`: `Get(string)`, `Set(string,int)`, `Increment`, evento `Changed(key)`. Registo `SO_FlagRegistry` + ferramenta `Aurora/Generate Flag Keys` → `FlagKeys.cs`.
2. `SaveData` (ver `codespec.md` §7) com `saveVersion = 1`; `SaveMigrator` com cadeia de migrações (mesmo vazia).
3. Serialização Newtonsoft; escrita **atómica** no PC (ficheiro temporário + replace) e cópia `.bak`.
4. Web: mesmo caminho lógico sobre `persistentDataPath`; garantir persistência após recarregar a página e após fechar o separador (testar Chrome e Firefox). Se for preciso forçar sincronização com IndexedDB, isolar num `WebSaveBackend` com `.jslib` mínimo.
5. `Checkpoint` (trigger por sala): grava ao entrar, uma vez por visita; toca sino + toast. Também gravação após cada confronto.
6. Carregar: restaura ala/sala/spawn, inventário, flags, posição da Beatriz (quando existir).
7. Erros de leitura → tenta `.bak` → senão slot vazio com aviso; nunca crash.
8. Menu debug (`AURORA_DEV`): guardar/carregar/apagar slot, listar flags.

## Critérios de aceitação
- [ ] Guardar, fechar, reabrir: estado idêntico (inventário, flags, sala) no PC.
- [ ] O mesmo no build Web após recarregar a página.
- [ ] Ficheiro corrompido recupera do `.bak`.
- [ ] Migração v0→v1 de teste aplicada corretamente.

## Testes
- EditMode: round-trip de `SaveData`, migrações, `WorldFlags`.
- PlayMode: checkpoint grava uma vez por visita.

## Registo

## Ideias
