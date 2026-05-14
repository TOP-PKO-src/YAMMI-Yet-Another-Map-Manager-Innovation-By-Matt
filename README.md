Credits
Original Author

This project is based on the original work created by Matt (Elitrin).

Matt/Elitrin is the original creator of YAMMI and has contributed several other valuable tools widely used within the TOP/PKO community. 


# Yammi 0.6.2 Source

Yammi is an old WinForms Tales of Pirates / PKO map editor. This tree was recovered from dotPeek output and is being stabilized as a buildable source project.

This repository is intended as a base for further improvements and is shared as a way of giving back to the community.

## Layout

- `src/YetAnotherMapManager` - main WinForms editor.
- `src/Plugins` - plugin projects loaded by the editor from `bin/<Configuration>/Plugins`.
- `docs` - repo guidance and project notes.
- `progress`, `log.md`, `notes.md` - working memory for agent handoffs.

## Build

Use Visual Studio MSBuild for this old .NET Framework solution:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" "C:\TOP-PKO\Yammi_0_6_2_src\Yammi.sln" /t:Restore,Build /p:Configuration=Debug /p:Platform="Any CPU" /m:1
```

Do not use `dotnet msbuild` as the normal build path.

## Runtime

The editor reads `settings.xml` from its working directory. For the current validated Debug workflow, the useful executable is:

```text
src\YetAnotherMapManager\bin\Debug\YetAnotherMapManager.exe
```

The validated client folder is:

```text
C:\Tales of Pirates
```
