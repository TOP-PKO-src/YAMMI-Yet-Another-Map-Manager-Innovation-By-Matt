---
name: yammi-map-editor
description: Use when fixing, building, or modernizing the Yammi 0.6.2 Tales of Pirates map editor in C:\TOP-PKO\Yammi_0_6_2_src, including .NET Framework WinForms build repair, PKO map/area/texture plugin behavior, and source-backed map-format validation.
---

# Yammi Map Editor Skill

## Startup

1. Open `C:\TOP-PKO\Yammi_0_6_2_src\AGENTS.md`.
2. Open `C:\TOP-PKO\Yammi_0_6_2_src\docs\agents\yammi-map-editor-agent.md`.
3. Open only the exact source files needed for the current issue.


## Build Command

Use Visual Studio MSBuild:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" "C:\TOP-PKO\Yammi_0_6_2_src\Yammi.sln" /t:Restore,Build /p:Configuration=Debug /p:Platform="Any CPU" /m:1
```

Avoid `dotnet msbuild` unless specifically investigating SDK/MSBuild behavior. It can fail on old .NET Framework resource generation for this solution.

## Project Shape

- Main app: `src\YetAnotherMapManager`
- Full solution: `Yammi.sln`
- Plugins: `src\Plugins\AreaManager`, `src\Plugins\AutoTextureBlending`, `src\Plugins\HeightMapGeneratorPlugin`, `src\Plugins\IsleGenerator`, `src\Plugins\TextureAutoAssign`, `src\Plugins\TextureTweaker`, `src\Plugins\TileInfoView`
- Plugin binaries copy into `src\YetAnotherMapManager\bin\Debug\Plugins`

## Common Repair Patterns

- Decompiled pointer arithmetic may need plain integer offset math before casting to pointers.
- Generated resource wrappers may reference namespace paths that collide with class names.
- Decompiled plugin projects may miss standard references such as `System`.
- Keep encoding changes minimal. Many files are old generated/decompiled files.

## Source Truth

Use `C:\References` only for targeted format questions. For map behavior, look for exact map, terrain, area, tile, texture, and scene-loading source files instead of reading the full reference tree.

## Validation

For code changes, run the Visual Studio MSBuild command above and report the result. For UI/runtime work, build first, then launch the built executable only when the change requires runtime validation.

## Ledger

After meaningful batches update:

- `C:\TOP-PKO\Yammi_0_6_2_src\log.md`
- `C:\TOP-PKO\Yammi_0_6_2_src\notes.md`
- `C:\TOP-PKO\Yammi_0_6_2_src\progress\progress-YYYY-MM-DD-HH-MM.md`
