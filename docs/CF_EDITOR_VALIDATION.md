# Validation status / Estado de validación

One-unit physical observations and local checks are distinguished below.
This report contains no device identifiers, personal presets or capture data.

| Check | Evidence | Status |
| --- | --- | --- |
| r95 engine installation | Confirmed transfer/closure; user confirmed normal boot and sound | One Catalyst 60/CX60 |
| Resources preserved | Five pre-update slot receipts matched the prepared image | Same payloads and metadata |
| NAM uploads over USB | Earlier transfer/readback and user listening result | Observed on the development unit |
| EP with NAM 2 | 32 reads over 31.92 s; ~1499.97 blocks/s | No added late blocks or recorded NAM/preamp faults |
| 808 with NAM 2 | 32 reads over 31.86 s; ~1499.95 blocks/s | No added late blocks or recorded NAM/preamp faults |
| SD-1 with NAM 2 | 32 reads over 31.83 s; ~1499.99 blocks/s | No added late blocks or recorded NAM/preamp faults |
| r95 listening | User reported that it sounds very good | Positive report; no POD Go fidelity comparison |
| New preamp layout | 40 cases, two NAM paths, four types, five scales | Local containment/non-overlap checks passed |
| 125% and 160% panel sizes | Component tested separately when the whole window exceeded the test desktop | Panel checks only at those scales |
| Save destination update | Fresh SDK read; 12 target cases and invalid/racing-state rejection | Local checks passed; manual save pending |
| Full manual app review, new pedal persistence and all settings/outputs | No complete matrix yet | Pending |
| Catalyst 100/200 hardware | Family handling exists in code | Not physically tested |

Measured dispatch cycles exclude work outside those dispatches. They do not
prove worst-case capacity. Numeric model checks, timing, transfer success and
audible behavior are different evidence. This is not a zero-failure guarantee.

## Español

R95 arrancó y produjo audio en una unidad 60/CX60. Los tres preamps mantuvieron
NAM 2 activo durante las ventanas medidas y el usuario comunicó un buen resultado
sonoro. Falta la revisión manual de la app corregida, guardado, persistencia
de nuevos tipos y otros ajustes/salidas/modelos. El EXE espera el OK final.
