# 소스에서 YAFC 빌드하기

[English](HowToBuild.md) | [한국어](HowToBuild.ko.md)

- 소스를 다운로드합니다.
- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)를 설치합니다.
- 저장소 루트에서 `./build.sh`를 실행해 모든 릴리스 타깃을 빌드합니다.
- 빌드된 결과물은 `Build` 아래에 생성됩니다.

특정 타깃 하나만 빌드하려면 저장소 루트에서 해당 `dotnet publish` 명령을 실행하세요.

```sh
dotnet publish Yafc/Yafc.csproj -r win-x64 -c Release -o Build/Windows
dotnet publish Yafc/Yafc.csproj -r linux-x64 -c Release -o Build/Linux
dotnet publish Yafc/Yafc.csproj -r osx-arm64 -c Release -o Build/OSX-arm64
dotnet publish Yafc/Yafc.csproj -r osx-x64 -c Release -o Build/OSX
```

빌드 후 YAFC를 실행하려면 README의 설치 안내 또는 [Linux 및 macOS 설치 안내](LinuxOsxInstall.ko.md)를 참고하세요.
