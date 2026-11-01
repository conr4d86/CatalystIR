# Compilar y verificar en Windows

El repositorio incluye el código, la receta del parche vacía de IR personales, iconos, fotografía y el paquete de firmware/bibliotecas utilizado por la aplicación. No hace falta buscar DLL del fabricante para compilar esta edición.

## Requisitos

- Windows y .NET Framework 4.8 o posterior, con el compilador clásico `csc.exe`.
- PowerShell. Los scripts no descargan herramientas ni instalan controladores.
- Para usar el ampli: controlador USB del Catalyst funcionando. Las pruebas locales de código no necesitan el ampli conectado.

```powershell
.\build.ps1
.\test.ps1
```

El ejecutable queda en `build/CatalystIR.exe`. Se compila como proceso x86 para las bibliotecas integradas. El código fuente incluye recursos por hash y valida el contenedor y la plantilla antes de una transferencia.

`test.ps1` ejecuta pruebas de volumen, filtros, WAV, flujo de instalación, guardas del contenedor, perfiles, caché y selección de ranuras. Los WAV de prueba son sintéticos y se generan en `build/test-data`; no proceden del desarrollador ni se incluyen en la app. El script nunca llama a los modos de transferencia por USB.

La emulación ARM utilizada durante el desarrollo requiere herramientas adicionales y no se reproduce en este script. Su resumen figura en `VALIDACION.json`. Las comprobaciones del compilador, las pruebas de software, la emulación y la confirmación auditiva en una unidad son evidencias diferentes.

## Inventarios

`SHA256SUMS.json` registra los archivos distribuidos en el repositorio. `included/COMPONENTS.json` describe los componentes integrados y sus hashes. Para cambios deliberados de código o documentación, actualiza el inventario antes de compilar:

```powershell
.\Update-Manifest.ps1
.\build.ps1
```

Esto genera una nueva versión local de los hashes; no la convierte automáticamente en una publicación oficial. Comprueba siempre los hashes del EXE que se adjunte a cada Release. El compilador clásico puede producir binarios con distinto SHA256 aunque el código no cambie.

El workflow de Windows compila, ejecuta pruebas sin hardware y conserva el resultado como artefacto de CI. No instala firmware ni publica Releases automáticamente.

## Cambios de firmware

La plantilla del parche está fijada por hash en `OneClickBundle.cs` y `TemplateSource.cs`. Un candidato solo puede modificar los coeficientes, los bancos FFT derivados y la máscara de ranuras. No amplíes los productos o versiones admitidos únicamente porque compilen o porque compartan símbolos.

Los comandos internos de diagnóstico o instalación no forman parte del flujo de usuario. No ejecutes comandos `--install-prepared` o de activación para probar una compilación: requieren un dispositivo real y pueden escribir en él. Las pruebas automatizadas aquí no los utilizan.
