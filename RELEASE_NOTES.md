# CatalystIR 2.0 · Primera edición experimental

**Autor:** conr4d · **Etiqueta propuesta:** `v2.0.0-alpha.1`

Aplicación Windows de un solo EXE que permite preparar e instalar de uno a tres IR propios dentro del Catalyst 60/CX60 con firmware base 2.02.

- Ranuras **IR1**, **IR2** e **IR3**, con ajustes independientes.
- Normalización automática y nivel manual por IR.
- Low Cut 80 Hz y High Cut 8 kHz por defecto, ajustables y con bypass.
- Una instalación para el conjunto completo; las ranuras vacías usan sus simulaciones originales.
- Copia verificada de los 12 presets antes de cada transferencia.
- Aviso rojo para reiniciar normalmente después de la transferencia.
- Inicio vacío de la app y posibilidad de reinstalar los mismos WAV.
- Icono de Windows y contenido integrado, sin ejecutables auxiliares para la transferencia.

Los tres IR han sido instalados y han sonado correctamente según la prueba del desarrollador en su ampli. Los resultados de una unidad no acreditan todos los modelos, equipos Windows o salidas. Consulta `VALIDACION.json` para las pruebas de software y los límites de la verificación física.

**Descarga recomendada:** `CatalystIR.exe`, o el ZIP de Windows con las instrucciones y el firmware oficial de recuperación. El ZIP de código fuente incluye también el contenido integrado necesario para compilar.

Formato IR: WAV mono a 48 kHz, 1–2048 muestras, PCM 16/24/32 o float32. El EXE no lleva IR comerciales ni presets del desarrollador.

Lee [USO](docs/USO.md) y [RECUPERACION](docs/RECUPERACION.md) antes de instalar. La modificación es experimental, bajo tu responsabilidad, y puede requerir restaurar el firmware oficial si falla. El ejecutable no dispone de firma de código; se publican sus hashes SHA256.
