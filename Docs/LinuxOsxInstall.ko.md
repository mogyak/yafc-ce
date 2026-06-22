# Linux 및 macOS 설치

[English](LinuxOsxInstall.md) | [한국어](LinuxOsxInstall.ko.md)

## 릴리스 압축 파일

최신 압축 파일은 [YAFC 릴리스 페이지](https://github.com/Yafc-CE/yafc-ce/releases)에서 받을 수 있습니다.

- Linux:
  - `Yafc-CE-Linux-self-contained-<version>.tar.gz`는 .NET 런타임을 포함하므로 가장 간단합니다.
  - `Yafc-CE-Linux-<version>.tar.gz`는 .NET 10 런타임을 별도로 설치해야 합니다.
- macOS Apple Silicon:
  - `Yafc-CE-OSX-arm64-<version>.tar.gz`를 사용하세요.
- macOS Intel:
  - `Yafc-CE-OSX-intel-<version>.tar.gz`를 사용하세요.

압축을 푼 뒤 해당 폴더에서 `Yafc` 실행 파일을 실행합니다.

```sh
chmod +x Yafc
./Yafc
```

## Linux

### Arch

YAFC-CE용 AUR 패키지가 있습니다: [`factorio-yafc-ce-git`](https://aur.archlinux.org/packages/factorio-yafc-ce-git). 설치 후 `factorio-yafc`로 실행할 수 있습니다.

현재 소스 트리는 .NET 10을 대상으로 합니다. AUR 패키지 메타데이터가 저장소보다 뒤처져 있다면 릴리스 압축 파일을 사용하거나 .NET 10 SDK로 소스에서 빌드하세요.

### Debian 계열 배포판

framework-dependent Linux 압축 파일을 사용할 경우 Microsoft에서 .NET 10 런타임을 설치하세요.

- [Debian에서 .NET 설치](https://learn.microsoft.com/en-us/dotnet/core/install/linux-debian)
- [.NET 10 다운로드](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

SDL/OpenGL 네이티브 의존성을 설치합니다.

```sh
sudo apt-get update
sudo apt-get install libsdl2-2.0-0 libsdl2-image-2.0-0 libsdl2-ttf-2.0-0 libgl1
```

런타임 기준으로 YAFC에는 다음 공유 라이브러리가 필요합니다.

- `SDL2-2.0.so.0`
- `SDL2_ttf-2.0.so.0`
- `SDL2_image-2.0.so.0`

self-contained Linux 압축 파일을 사용하면 .NET은 포함되어 있지만, SDL/OpenGL 라이브러리는 여전히 시스템 의존성입니다.

## macOS

릴리스 압축 파일을 사용할 때는 .NET 10 런타임을, 소스에서 빌드할 때는 .NET 10 SDK를 설치하세요.

- [.NET 10 다운로드](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- 또는 Homebrew 사용:

```sh
brew install dotnet
dotnet --list-runtimes
```

Homebrew는 .NET을 자체 prefix 아래에 설치합니다. 셸에서는 `dotnet`이 동작하지만 YAFC가 런타임을 찾지 못한다면 `DOTNET_ROOT`를 설정하세요.

```sh
export DOTNET_ROOT="$(brew --prefix dotnet)/libexec"
```

Apple Silicon(`osx-arm64`)의 경우 릴리스 빌드에 사용되는 SDL2, SDL2_image, SDL2_ttf, Lua 네이티브 라이브러리가 저장소에 포함되어 있습니다.

Intel macOS(`osx-x64`)에서는 사용 중인 릴리스 압축 파일에 SDL dylib가 포함되어 있지 않다면 Homebrew로 SDL을 설치하세요.

```sh
brew install sdl2 sdl2_image sdl2_ttf
```

압축을 푼 폴더에서 YAFC를 실행합니다.

```sh
chmod +x Yafc
./Yafc
```

Launch Services를 통해 실행할 수도 있습니다.

```sh
open -n ./Yafc
```

macOS가 인터넷에서 다운로드한 앱이고 공증되지 않았다는 이유로 실행을 막는다면, System Settings > Privacy & Security에서 허용하거나 압축을 푼 YAFC 폴더에서 quarantine 속성을 제거하세요.

```sh
xattr -dr com.apple.quarantine /path/to/Yafc-folder
```

## 소스에서 빌드

.NET 10 SDK를 설치한 뒤 저장소 루트에서 필요한 타깃을 publish 합니다.

```sh
dotnet restore FactorioCalc.sln
dotnet publish Yafc/Yafc.csproj -r linux-x64 -c Release -o Build/Linux
dotnet publish Yafc/Yafc.csproj -r linux-x64 --self-contained -c Release -o Build/Linux-self-contained
dotnet publish Yafc/Yafc.csproj -r osx-arm64 -c Release -o Build/OSX-arm64
dotnet publish Yafc/Yafc.csproj -r osx-x64 -c Release -o Build/OSX
```

모든 릴리스 타깃을 빌드하려면 다음을 실행합니다.

```sh
./build.sh
```

일반적으로 Lua를 직접 다시 빌드할 필요는 없습니다. 번들된 네이티브 Lua 라이브러리를 변경할 때만 Lua 빌드 스크립트를 사용하세요. 자세한 내용은 [`lua/README.md`](/lua/README.md)를 참고하세요.

## Flathub

[Flathub에 올라온 버전](https://flathub.org/apps/details/com.github.petebuffon.yafc)은 Community Edition이 아닙니다. 해당 저장소는 https://github.com/petebuffon/yafc 입니다.

## 일반 체크리스트

- framework-dependent 릴리스 압축 파일에는 .NET 10 런타임, 소스 빌드에는 .NET 10 SDK가 필요합니다.
- Linux에는 SDL2, SDL2_image, SDL2_ttf, OpenGL이 필요합니다.
- macOS Intel에서는 SDL2 라이브러리를 Homebrew로 설치해야 할 수 있습니다.
- 압축을 푼 폴더 또는 publish 결과 폴더에서 `Yafc` 실행 파일을 실행하세요.
