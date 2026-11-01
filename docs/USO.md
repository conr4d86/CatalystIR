# Uso de CatalystIR

## Antes de empezar

Esta edición se limita a Catalyst 60 / CX60 con firmware 2.02. El programa verifica el producto y la versión antes de escribir. Windows debe reconocer el puerto USB del ampli; si no lo hace, instala el controlador oficial desde [Line 6 Downloads](https://line6.com/software/).

Guarda tus cambios pendientes y cierra Catalyst Edit y los programas que ocupen su puerto MIDI. Es recomendable conservar además una copia propia con Catalyst Edit. La app crea su copia verificada durante cada instalación, antes de la transferencia.

Usa una conexión USB estable y evita que el ordenador entre en suspensión durante el proceso. Extrae el EXE a una carpeta donde puedas escribir; las bibliotecas y recursos integrados se preparan automáticamente al abrirlo.

## Preparar de uno a tres IR

1. Conecta el USB y enciende el ampli. La app permanece gris con **Esperando conexión de Ampli** hasta detectarlo.
2. Selecciona **IR1**, **IR2** o **IR3** y pulsa **Cargar IR**. Aparecerá un visto verde si el archivo es válido.
3. Usa WAV **mono, 48 kHz, de 1 a 2048 muestras**. Se admiten PCM de 16, 24 o 32 bits y float32; los IR cortos se rellenan con ceros. Los formatos incompatibles se rechazan antes de instalar.
4. Puedes cargar solo una ranura, dos o las tres. **Vaciar** deja esa ranura sin IR propio para la siguiente instalación; entonces usará la simulación original.

Cada apertura normal empieza con las ranuras vacías. Esto solo limpia la selección de trabajo de la app. Los IR del ampli, las copias y los perfiles se conservan. Si una instalación quedó pendiente de reinicio o activación, la app recupera sus ranuras para terminarla.

**La instalación escribe el conjunto completo:** vuelve a cargar todos los IR que quieras mantener, aunque ya estuvieran instalados. Puedes reinstalar exactamente los mismos WAV.

## Nivel y filtros por IR

- **Volumen automático:** calcula el nivel después de los filtros dejando un margen de pico de respuesta de 3 dB. Ayuda a controlar la ganancia; no garantiza igual volumen percibido entre todos los IR o guitarras.
- **Volumen manual:** desmarca el automático y ajusta los dB. El valor representa la ganancia aplicada al WAV original.
- **Cortes automáticos:** los perfiles nuevos parten de Low Cut **80 Hz** y High Cut **8000 Hz**, con pendiente de 12 dB/octava. Puedes modificar los valores o desmarcar los cortes.

Cada ranura mantiene su perfil independiente durante la preparación. La app recuerda ajustes por contenido del IR para futuras cargas. Cambiar nivel o filtros requiere volver a instalar el conjunto para escucharlo; las pestañas preparan la instalación y no cambian en vivo el sonido del ampli.

## Instalar

1. Baja Master y el nivel de monitorización de la salida que vayas a escuchar.
2. Pulsa **Instalar los IR** una vez. La app prepara el firmware, comprueba la comunicación y hace dos lecturas de cada uno de los 12 presets. Compara sus datos antes de crear la copia.
3. Espera durante la transferencia. No desconectes alimentación ni USB. El proceso puede tardar varios minutos; las pruebas anteriores han rondado los 3–4 minutos, sin que sea un plazo garantizado.
4. Cuando aparezca **Apaga y enciende tu ampli** en rojo, apágalo y enciéndelo normalmente, sin botones. Mantén la app y el USB hasta completar la activación.
5. Espera **IR guardados ✓**. Se activa la ranura seleccionada al instalar; si estaba vacía, se usa la primera ocupada. La app puede guardar el preset actual con esa selección y con sus demás ajustes actuales.
6. Prueba a volumen bajo. Después puedes cerrar la app.

## Elegir el IR para tocar

En Catalyst Edit, mantén habilitada la simulación de pantalla en la sección **CABINET / Direct Out / Phones / USB**. Las tres opciones originales de CABINET corresponden, en su orden, a **IR1**, **IR2** e **IR3**. Una opción sin IR propio conserva su simulación original.

Guarda la selección que quieras en el preset. Puedes preparar presets con distintos IR y recuperarlos desde el ampli. Los archivos quedan dentro del firmware y la selección guardada está diseñada para cargarse al encender sin la app. Los tres IR han sonado correctamente en la unidad de prueba; consulta el informe de validación para los límites de las comprobaciones de arranque y de cada salida.

La ruta del altavoz físico conserva su procesamiento. Los IR personalizados pertenecen a la simulación virtual para auriculares, Direct Out y USB.

## Copias y archivos

Las instalaciones se guardan en:

```text
%LOCALAPPDATA%\CatalystIR\v2\backups
```

Cada carpeta contiene el candidato `.ctf`, el firmware oficial de recuperación, los WAV originales cargados, `IR-slots.json`, los perfiles y la copia de presets en `presets/Catalyst-verified.catl6a`. Conserva la carpeta completa, incluidos los pares de lecturas y los archivos de verificación. El diario de la última operación está en `%LOCALAPPDATA%\CatalystIR\v2\installation.json`.

La app no lee de vuelta toda la flash para hacer una imagen completa del firmware instalado: conserva el candidato exacto enviado. Una lectura o una transferencia correcta no sustituyen la comprobación de audio.

## Si Windows o el antivirus avisan

El EXE no tiene certificado de firma de código. Windows o el antivirus pueden mostrar avisos de reputación o comportamiento. Compilarlo localmente no demuestra por sí solo que un aviso sea falso. Descarga desde la publicación indicada, compara su SHA256 y revisa el código o el resultado del análisis antes de decidir qué hacer. No es necesario desactivar globalmente el antivirus para usar estas instrucciones.

Si hay un error de instalación, conserva sus mensajes y sigue [RECUPERACION.md](RECUPERACION.md).
