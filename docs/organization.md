# Yammi Organization

## Canonical Structure

`Yammi.sln` at the repository root is the canonical solution.

```text
src/
  YetAnotherMapManager/        main WinForms editor
  Plugins/
    AreaManager/
    AutoTextureBlending/
    HeightMapGeneratorPlugin/
    IsleGenerator/
    TextureAutoAssign/
    TextureTweaker/
    TileInfoView/
docs/                          project guidance
```

## Plugin Output

Plugin projects intentionally output to:

```text
src\YetAnotherMapManager\bin\<Configuration>\Plugins
```

This matches `ApplicationControler.GetPlugins()`, which searches for plugin DLLs under the app working directory's `Plugins` folder.

## DotPeek Leftovers

This source was exported from dotPeek. Per-project `.sln` files were removed from the upload-ready layout. `Yammi.sln` is the supported build entry.

## Artifact Rules

Keep these out of source review:

- `bin/`
- `obj/`
- `settings.xml`
- `cache/`
- old `validation-output/`

Runtime settings and validation outputs are useful locally, but they should not define the source layout.
