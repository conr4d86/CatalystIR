# Recuperación / Recovery

Mantén USB y alimentación durante una transferencia activa. Reinicia cuando
termine y el actualizador confirme el cierre, siguiendo el aviso de la app.

Conserva presets y NAM/IR originales. El editor organiza copias de ajustes
en `%LOCALAPPDATA%\CatalystCFEditor\backups` e instalaciones completas en
`%LOCALAPPDATA%\CatalystCFEditor\initial-installations\backups`.

## Si no arranca o no tiene audio

1. Conserva el mensaje y los archivos de la operación terminada.
2. Cierra los editores y abre el actualizador oficial de Line 6.
3. Con USB conectado, enciende manteniendo **CH A + CH B**.
4. Reinstala el firmware oficial del modelo correcto y espera la finalización.
5. Sigue las indicaciones oficiales y comprueba arranque y audio.

**CH B + MANUAL es un reset de fábrica que borra presets**, no el modo A+B.
La [guía publicada](RECUPERACION.md) distingue recuperación/reset, enlaza las
instrucciones oficiales y conserva la huella de la base 2.02.

El firmware oficial retira las funciones CF. Una copia de presets no sustituye
a los archivos NAM/IR. Si el equipo no aparece en recuperación, solicita soporte
del fabricante antes de repetir transferencias sin diagnóstico.

## English

Do not restart during an active transfer. Keep presets and original NAM/IRs.
After a failed operation ends, use the official Line 6 updater and power on
with **CH A + CH B** for recovery. Reinstall the correct official firmware.
**CH B + MANUAL erases presets** through factory reset. The [existing guide](RECUPERACION.md)
links official instructions and the 2.02 image hash. Contact manufacturer support
if recovery cannot detect the device. Recovery is not guaranteed for every failure.
