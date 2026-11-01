# Autoría y componentes incluidos

CatalystIR es un proyecto independiente de **conr4d**. Line 6, Catalyst y sus demás marcas pertenecen a sus respectivos titulares. No es un producto oficial ni se presenta como aprobado por Line 6.

La licencia MIT del repositorio cubre el código original de CatalystIR. No concede una licencia sobre el firmware, las bibliotecas del fabricante, la fotografía del producto, componentes de Microsoft ni IR de terceros.

El contenido integrado incluye:

- Firmware oficial Catalyst 2.02 usado como base. SHA256: `f9fe2482e561bef8a04128a38af89cadee4f8d06071b06a9053e36c206471ae8`.
- Motores y recursos de Catalyst Edit y Line 6 Central para comunicar y transferir por USB. El inventario por archivo está en `included/COMPONENTS.json`.
- Bibliotecas de ejecución presentes en ese inventario, que mantienen sus respectivos titulares.
- Fotografía frontal del Catalyst CX60 procedente de la [página del fabricante](https://line6.com/catalyst-guitar-amps/). Su referencia original figura en `assets/catalyst-cx60-front.source.json`.

El icono propio de CatalystIR se genera mediante `CreateAppIcon.cs`.

Los paquetes de publicación no contienen los WAV, presets ni copias privadas usados durante el desarrollo. Los recursos integrados del fabricante se conservan en el ejecutable y en el paquete de compilación; no se sustituye esta distribución por una edición que exija conseguirlos por separado.
