# Codex Usage Tray

Indicador nativo para Windows que muestra la cuota restante de Codex en el área de notificaciones. Usa la sesión de ChatGPT administrada por Codex CLI: no necesita API key y no lee ni copia tokens.

## Qué muestra

- porcentaje disponible en el icono;
- ventana primaria (normalmente 5 horas) y secundaria (normalmente semanal), según lo que informe la cuenta;
- próximo reinicio de cada ventana;
- actualización automática cada minuto y manual desde el menú;
- colores verde, amarillo y rojo; inicio opcional con Windows.

## Requisitos

1. Windows 10/11 x64.
2. Codex CLI actualizado, disponible como `codex` en el PATH.
3. Sesión iniciada en Codex mediante ChatGPT (`codex login`).

La integración usa `codex app-server` y `account/rateLimits/read`, actualmente una interfaz experimental oficial. Si cambia, la app mostrará un estado de error sin tocar las credenciales.

## Descargar

Abrí la ejecución más reciente de **Actions**, descargá el artefacto `codex-usage-tray-win-x64` y ejecutá `CodexUsageTray.exe`. Es autocontenido: no requiere instalar .NET.

## Desarrollo

```powershell
dotnet run --project tests/CodexUsageTray.Tests
dotnet run --project src/CodexUsageTray
dotnet publish src/CodexUsageTray/CodexUsageTray.csproj -c Release -o dist
```

## Seguridad

La aplicación inicia `codex app-server` como subproceso local y habla por stdin/stdout mediante JSON-RPC. No abre puertos, no utiliza cookies del navegador, no almacena credenciales y no envía telemetría propia.

## Alcance

Mide las ventanas de uso que Codex expone para la cuenta de ChatGPT conectada. No pretende representar todos los límites independientes de conversaciones o modelos de ChatGPT.

## Licencia

MIT
