# Yammi Workspace Map

## Mission

- Keep `C:\TOP-PKO\Yammi_0_6_2_src` buildable as an old .NET Framework WinForms map editor.
- Repair decompiler/build breakage with the smallest maintainable changes.
- Use Tales of Pirates source references only for targeted map-format or runtime behavior questions.
- Preserve the old app shape until build and basic map workflows are understood.

## Required Skills

Use these for non-trivial work:

- CREATE YOUR OWN

## First Files

Open only what the current task needs. Start with:

1. `C:\TOP-PKO\Yammi_0_6_2_src\AGENTS.md`
2. `C:\TOP-PKO\Yammi_0_6_2_src\.codex\skills\yammi-map-editor\SKILL.md`
3. `C:\TOP-PKO\Yammi_0_6_2_src\docs\agents\yammi-map-editor-agent.md`
4. The specific `.cs`, `.resx`, `.csproj`, or map-format files being changed

## Active Code Roots

- Main app: `C:\TOP-PKO\Yammi_0_6_2_src\src\YetAnotherMapManager`
- Main solution: `C:\TOP-PKO\Yammi_0_6_2_src\Yammi.sln`
- Plugins: `src\Plugins\AreaManager`, `src\Plugins\AutoTextureBlending`, `src\Plugins\HeightMapGeneratorPlugin`, `src\Plugins\IsleGenerator`, `src\Plugins\TextureAutoAssign`, `src\Plugins\TextureTweaker`, `src\Plugins\TileInfoView`
- Plugin output: `C:\TOP-PKO\Yammi_0_6_2_src\src\YetAnotherMapManager\bin\Debug\Plugins`
- Consolidated source references, when needed: `C:\References`

## Build

Use Visual Studio MSBuild for this old .NET Framework solution:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" "C:\TOP-PKO\Yammi_0_6_2_src\Yammi.sln" /t:Restore,Build /p:Configuration=Debug /p:Platform="Any CPU" /m:1
```

Do not use `dotnet msbuild` as the default build path. It can fail on `GenerateResource` task hosting for this solution even when Visual Studio MSBuild succeeds.

## Operating Rules

- Keep this lane separate from PKOStudio. Do not copy source between roots unless the task explicitly asks for a reviewed integration.
- Search with `rg` or `rg --files` first, byte-cap unknown output, and inspect focused file sections.
- Preserve decompiled source carefully. Prefer narrow fixes over modernizing broad project structure.
- Use `apply_patch` for manual edits. Keep comments sparse and useful.
- Treat the worktree as dirty. Never revert unrelated changes.
- Update `log.md`, `notes.md`, and `progress\progress-YYYY-MM-DD-HH-MM.md` after meaningful batches.

## Current Build Notes

- The solution builds with Visual Studio 2022 MSBuild after the 2026-05-13 repair batch.
- Fixed build issues so far:
  - `src\YetAnotherMapManager\Rendering\FastBitmap.cs` pointer arithmetic and explicit dispose implementation.
  - `src\Plugins\AreaManager\Properties\Resources.cs` generated-resource self-reference collision.
  - `src\Plugins\TextureTweaker\TextureTweaker.csproj` missing `System` reference.
