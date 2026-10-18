# F7-02 — Integração Steam
**Estado:** TODO · **Depende de:** F6-06 · **Assemblies:** Aurora.Platform.Steam (`STEAM_BUILD`)

## Objetivo
Build Windows pronto para a Steam: achievements, cloud save, rich presence, Steam Deck verificado, sem que o código Steam toque no resto do jogo.

## Âmbito
**Inclui:** Steamworks.NET dentro de `Aurora.Platform.Steam`, `SteamPlatform : IPlatformServices`, achievements, Steam Auto-Cloud, Rich Presence, overlay (pausa automática), compatibilidade Steam Deck, depots e script de upload (SteamPipe).
**Não inclui:** página da loja (marketing), Steam Workshop.

## Especificação
1. Inicialização no `GameBootstrap` só com `STEAM_BUILD`; falha de init → log + `NullPlatform` (o jogo continua a funcionar).
2. `RestartAppIfNecessary` em release; `steam_appid.txt` só em dev (no `.gitignore`).
3. **Achievements** definidos num SO (`SO_Achievements`) ligados a `GameEvent`s: terminar cada ala, libertar cada hóspede, libertar todos, não comer nada no banquete, encontrar todas as notas do pai, terminar o jogo, final alternativo (se existir).
4. **Cloud:** Steam Auto-Cloud configurado para a pasta de saves (sem API manual); conflitos resolvidos pela Steam.
5. **Rich Presence:** ala atual ("A explorar a Ala dos Quartos").
6. **Overlay:** abrir overlay → pausa.
7. **Steam Deck:** 1280×800 (16:10), texto legível, prompts de Deck, sem necessidade de teclado (nome de slots não exige texto), suspensão/retoma sem perder estado. Input via Input System (Steam Input em modo gamepad).
8. `BuildSteam` + script SteamPipe (`tools/steam/`) com app/depot ids em variáveis de ambiente, nunca no repositório.

## Critérios de aceitação
- [ ] Achievements desbloqueiam em teste com app id de desenvolvimento.
- [ ] Save sincroniza entre dois PCs via Steam Cloud.
- [ ] Jogo corre sem Steam aberta (fallback) em dev.
- [ ] Checklist Steam Deck Verified revista e anotada no registo.

## Registo

## Ideias
- Demo na Steam (Next Fest) reutilizando a flag de demo da F7-01.
