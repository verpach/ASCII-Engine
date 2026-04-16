# ASCII-Engine

ASCII Wallpaper Engine для Windows 11 на **.NET 8 + WPF**.

## Что реализовано

- WinAPI-hosting окна в `WorkerW` (через `Progman + 0x052C`)
- ASCII Engine с FPS limiter и performance mode
- Renderer (ASCII frame -> `WriteableBitmap`)
- SceneManager + сцены:
  - static `.txt`
  - animated `.json`
  - video `.mp4/.avi/.mkv` -> ASCII (OpenCvSharp)
- GUI (WPF + MVVM) с вкладками:
  - Главная
  - Темы
  - Видео
  - Производительность
  - Настройки
- Config manager: `%AppData%/AsciiWallpaperEngine/config.json`
- Tray управление
- Hotkey `Ctrl+Alt+Right` для переключения темы
- Autostart через `HKCU\Software\Microsoft\Windows\CurrentVersion\Run`
- Логирование Serilog в `%AppData%/AsciiWallpaperEngine/logs/`
- DPI manifest: PerMonitorV2

## Структура

- `src/AsciiWallpaperEngine/Core` — модели, интерфейсы, DTO
- `src/AsciiWallpaperEngine/Engine` — движок, FPS, scene management, performance
- `src/AsciiWallpaperEngine/Rendering` — конвертер и рендер
- `src/AsciiWallpaperEngine/Scenes` — static/animated/video сцены
- `src/AsciiWallpaperEngine/Infrastructure` — WinAPI, config, startup, tray, logging
- `src/AsciiWallpaperEngine/ViewModels` — MVVM viewmodels
- `src/AsciiWallpaperEngine/Views` — `MainWindow`, `WallpaperWindow`

## NuGet зависимости

```powershell
dotnet add src/AsciiWallpaperEngine package CommunityToolkit.Mvvm
dotnet add src/AsciiWallpaperEngine package Serilog
dotnet add src/AsciiWallpaperEngine package Serilog.Sinks.File
dotnet add src/AsciiWallpaperEngine package Hardcodet.NotifyIcon.Wpf
dotnet add src/AsciiWallpaperEngine package NHotkey.Wpf
dotnet add src/AsciiWallpaperEngine package OpenCvSharp4
dotnet add src/AsciiWallpaperEngine package OpenCvSharp4.runtime.win
```

## Сборка (Visual Studio 2022)

1. Установить workload **.NET desktop development**.
2. Открыть `ASCII-Engine.slnx`.
3. Конфигурация: `Release | x64`.
4. Сборка.

CLI:

```powershell
dotnet build /home/runner/work/ASCII-Engine/ASCII-Engine/ASCII-Engine.slnx -c Release
```

## Запуск

```powershell
dotnet run --project /home/runner/work/ASCII-Engine/ASCII-Engine/src/AsciiWallpaperEngine/AsciiWallpaperEngine.csproj
```

## Publish single-file

```powershell
dotnet publish /home/runner/work/ASCII-Engine/ASCII-Engine/src/AsciiWallpaperEngine/AsciiWallpaperEngine.csproj `
  -c Release -r win-x64 --self-contained true `
  /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

## Ограничения

- Полноценная проверка desktop embedding, tray и fullscreen detection выполняется только на Windows 11.
- В Linux CI возможна только компиляция с `EnableWindowsTargeting=true`.
