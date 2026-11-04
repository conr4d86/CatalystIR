# Catalyst CF Editor 1.0 — release draft / borrador

Publication follows final user review. This is the first **Catalyst CF Editor**
release, a successor to the separately versioned CatalystIR 2.0 loader.
It does not rename or remove the older CatalystIR releases.

## Español

- Editor de sonido y gestión de NAM/IR en una aplicación Windows.
- Cinco pestañas principales: Sonido, Presets, Sistema, Avanzado y Firmware CF.
- Dos posiciones NAM A2 Lite y tres posiciones IR independientes.
- Carga de recursos por USB sin reinstalar el firmware completo en cada cambio,
  después de la instalación inicial del CF compatible.
- Boost de fábrica y aproximaciones EP Boost, 808 y SD-1, con controles
  Drive/Level/Tone/ON y conmutadores adicionales del EP.
- Conversión de IR decodificables al formato interno; arrastrar o cargar.
- Cortes 80 Hz / 8 kHz ajustables y nivel automático/manual por IR.
- Nombres NAM visibles, efectos agrupados por tipo y PRE/POST independientes.
- Nuevo icono, textura tolex, paneles translúcidos y escalado proporcional.
- Corrección de la separación entre diales, selector y botones del preamp.
- Consulta del destino de guardado directamente al SDK, sin depender del
  índice almacenado en la caché de pantalla.

El motor r95 fue instalado y arrancó en la unidad 60/CX60 del desarrollador.
El usuario comunicó una buena valoración sonora; las mediciones cortas de los
tres preamps mantuvieron NAM 2 sin nuevos retrasos registrados. La revisión
manual de la app corregida sigue pendiente. Consulta [validación](CF_EDITOR_VALIDATION.md).

La actualización de distribución/guardado de la app no requiere reinstalar
el motor r95 ya instalado en esa unidad. Los nuevos usuarios necesitarán el
CF compatible; su habilitación y paquete público se cerrarán antes del lanzamiento.

## English

One Windows application combines a sound editor and five NAM/IR positions.
Compatible CF enables individual USB resource updates after initial firmware
installation. The editor adds circuit-informed EP/808/SD-1 approximations,
IR conversion and level/filter controls, grouped effects, tolex styling and
proportional scaling. The app update fixes pedal spacing and reads the live
save destination instead of relying on the asynchronous display cache.

The r95 engine booted and produced audio on one Catalyst 60/CX60. Short NAM 2
observations recorded no additional late periods with the three pedals.
Manual save/layout retesting remains pending. No physical 100/200 support claim
or exact POD Go equivalence is made.

## Release checklist

- Record final user save/layout result and any fixes.
- Finalize the source/build package and installation enablement.
- Capture the approved interface and update screenshots.
- Verify final EXE/recovery hashes and archive previous versions.
- Obtain final publication OK; upload CatalystCFEditor.exe and verify download.
- Replace draft wording and add the verified direct download links.
