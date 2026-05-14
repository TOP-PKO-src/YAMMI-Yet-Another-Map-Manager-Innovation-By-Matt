# Yammi Map Editor Agent

## Purpose

Use this agent map for the Yammi 0.6.2 Tales of Pirates map editor side lane. The immediate goal is to keep the old WinForms editor buildable and understandable, then improve map-format behavior with targeted Tales of Pirates source references.

## Boundaries

- Primary root: `C:\TOP-PKO\Yammi_0_6_2_src`
- Keep old UI/runtime behavior stable until the build and map workflows are validated.

## Startup

1. `C:\TOP-PKO\Yammi_0_6_2_src\AGENTS.md`
2. `C:\TOP-PKO\Yammi_0_6_2_src\.codex\skills\yammi-map-editor\SKILL.md`
3. The exact files involved in the current build, plugin, or map-format issue

## Current Status

- `Yammi.sln` builds with Visual Studio 2022 MSBuild in Debug Any CPU.
- `dotnet msbuild` is not the preferred build tool for this solution because it hit .NET Framework resource-generation task hosting issues.
- The first repair batch fixed `FastBitmap`, `AreaManager` resource references, and `TextureTweaker` references.
- Source is organized under `src`:
  - main app: `src\YetAnotherMapManager`
  - plugins: `src\Plugins\*`
  - plugin output: `src\YetAnotherMapManager\bin\Debug\Plugins`
- Runtime smoke tooling and generated validation outputs are intentionally not part of the upload-ready source layout.

## Useful Build Command

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" "C:\TOP-PKO\Yammi_0_6_2_src\Yammi.sln" /t:Restore,Build /p:Configuration=Debug /p:Platform="Any CPU" /m:1
```

## Next Good Tasks

- Launch the built editor and verify basic startup/plugin loading.
- Identify the native map files and client folders Yammi expects.
- Compare Yammi map loading/saving behavior against targeted files in `C:\References`.
- Add focused smoke notes for opening, editing, and saving one known-good map.
