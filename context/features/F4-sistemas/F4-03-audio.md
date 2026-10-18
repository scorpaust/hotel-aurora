# F4-03 — Áudio adaptativo
**Estado:** TODO · **Depende de:** F3-02 · **Assemblies:** Aurora.Audio

## Objetivo
Num jogo com pouco HUD, o som é a interface principal: tensão, direção do perigo, pistas do pai, o hotel a respirar.

## Contexto na história
Música no salão para convidados invisíveis, a campainha da receção, o telefone, a voz do pai pelas instalações elétricas.

## Âmbito
**Inclui:** `AudioMixer` (Master, Music, SFX, Ambience, UI, Voice), `IAudioService`, música por camadas ligada à tensão, ambiente por sala, SFX com pooling e variação, snapshots (normal, escondido, perseguição, memória, pausa), áudio posicional 2D, desbloqueio no Web.
**Não inclui:** música final composta (placeholders livres de direitos ou gerados temporários).

## Especificação
1. Mixer com grupos acima, volumes ligados às opções (dB, curva logarítmica).
2. **Música por camadas:** stems sincronizados (base, tensão, perigo); `HotelDirector.Tension` e estado `Chase` controlam os volumes com transições de 1–2 s.
3. **Ambiente por sala** (`RoomDefinition.ambience`), crossfade em transições.
4. **SFX:** `SfxDefinition` (clips, variação de pitch/volume, prioridade, raio); pool de `AudioSource`. Passos dependem do material do piso.
5. **Snapshots:** Escondido (abafado), Perseguição, Memória (reverb quente), Pausa (low-pass).
6. Web: só iniciar áudio depois do primeiro gesto (splash); música em *streaming* comprimido; verificar latência.
7. Legendas de efeitos importantes ligadas a F4-02 (indicadores visuais).

## Critérios de aceitação
- [ ] Volumes das opções afetam os grupos certos e persistem.
- [ ] Entrar numa perseguição sobe as camadas sem cortes audíveis.
- [ ] Escondido abafa o som; sair repõe.
- [ ] Sem erros de autoplay no Chrome/Firefox.

## Testes
- EditMode: conversão volume linear ↔ dB.

## Registo

## Ideias
