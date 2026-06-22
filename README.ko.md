<h1 align="center">YAFC: Community Edition</h1>
<p align="center"><IMG style="width:50px; height:auto;" src="Yafc/image.ico" alt="yafc_icon.png"></p>
<p align="center"><a href="README.md">English</a> | <a href="README.ko.md">한국어</a></p>

### 왜 새 저장소인가요?

[원본 YAFC 저장소](https://github.com/ShadowTheAge/yafc)는 오랫동안 비활성 상태였습니다. 버그 수정은 쌓였지만 병합할 관리자가 없었습니다. 이 저장소는 지속적인 유지보수와 개발을 제공하기 위해 만들어졌습니다.

### 원작자와 이야기했나요?

네, 승인을 받았습니다.

<details>
<summary>스크린샷 보기</summary>
<IMG src="/Docs/Media/yafc_author_approval.png" alt="yafc_author_approval.png"/>
</details>

## YAFC란?

Yet Another Factorio Calculator, 줄여서 YAFC는 Factorio 계획 및 분석 도구입니다. 특히 모드가 많이 들어간 Factorio 게임을 다루는 데 초점을 둡니다.

<details>
<summary>YAFC가 할 수 있는 일 보기</summary>
<IMG src="/Docs/Media/Main.gif" alt="Main.gif"/>
</details>

YAFC는 단순 계산기 이상입니다. 여러 알고리즘을 사용해 모드팩의 구조를 분석하고, 후반 기지 전체 규모까지 계산할 수 있습니다. 어떤 아이템이 더 중요하고 어떤 레시피가 더 효율적인지도 판단합니다.

YAFC는 Helmod 같은 도구가 처리하기 어려웠던 [Pyanodon](https://mods.factorio.com/user/pyanodon)의 재귀 레시피를 다루기 위해 만들어졌습니다. YAFC는 Google [OrTools](https://developers.google.com/optimization)를 모델 솔버로 사용해 이런 구조를 잘 처리합니다.

YAFC에는 자체 Never Enough Items도 포함되어 있습니다. FNEI보다 더 분석적인 방식으로 레시피를 보여주며, 어떤 레시피를 쓰면 좋은지와 얼마나 필요한지도 함께 표시합니다.

## 시작하기

YAFC는 데스크톱 앱입니다. Windows 빌드가 가장 많이 테스트되었지만 macOS와 Linux도 지원합니다. 플랫폼별 의존성과 문제 해결은 [Linux 및 macOS 설치 안내](/Docs/LinuxOsxInstall.ko.md)를 참고하세요.

1. [YAFC 릴리스 페이지](https://github.com/Yafc-CE/yafc-ce/releases)로 이동합니다.
1. OS와 CPU 아키텍처에 맞는 압축 파일을 받습니다.
    - Windows: `Yafc-CE-Windows-<version>.zip`, 또는 .NET을 따로 설치하지 않으려면 self-contained Windows 압축 파일.
    - macOS Apple Silicon: `Yafc-CE-OSX-arm64-<version>.tar.gz`.
    - macOS Intel: `Yafc-CE-OSX-intel-<version>.tar.gz`.
    - Linux: 가장 간단한 설치는 `Yafc-CE-Linux-self-contained-<version>.tar.gz`, 이미 .NET 10 런타임이 있다면 `Yafc-CE-Linux-<version>.tar.gz`.
1. 필요한 런타임 의존성을 설치합니다.
    - Windows: Google OrTools에 필요한 최신 [VC Redist](https://learn.microsoft.com/en-us/cpp/windows/latest-supported-vc-redist?view=msvc-170)를 설치합니다.
    - macOS와 Linux: [Linux 및 macOS 설치 안내](/Docs/LinuxOsxInstall.ko.md)를 따릅니다.
1. 원하는 위치에 압축을 풉니다.
1. OS에 따라 `./Yafc` 또는 `./Yafc.exe`를 실행합니다.
1. YAFC가 열리면 Factorio 데이터 폴더와 모드 폴더 위치를 지정합니다. OS별 경로는 [Factorio 애플리케이션 디렉터리 위키](https://wiki.factorio.com/Application_directory#Locations)를 참고하세요.

YAFC 사용 경험을 개선하는 문서도 있습니다.

* [Gifs](/Docs/Gifs.md): 여러 사용 사례 예시입니다. 단, GIF는 트래픽을 많이 사용합니다.
* [Tips and Tricks](/Docs/TipsAndTricks.md) 및 [내장 팁](https://github.com/Yafc-CE/yafc-ce/blob/master/Yafc/Data/Tips.txt): 유용한 사용 팁입니다.
* [Shortcuts](/Docs/Shortcuts.md): 편의 단축키 모음입니다.

소스에서 YAFC를 빌드하려면 [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)를 설치하세요. 저장소 루트에서 `./build.sh`를 실행하면 모든 릴리스 타깃을 빌드할 수 있고, 필요한 플랫폼만 빌드하려면 `dotnet publish` 명령을 직접 실행할 수 있습니다.

```sh
dotnet publish Yafc/Yafc.csproj -r linux-x64 -c Release -o Build/Linux
dotnet publish Yafc/Yafc.csproj -r osx-arm64 -c Release -o Build/OSX-arm64
dotnet publish Yafc/Yafc.csproj -r osx-x64 -c Release -o Build/OSX
dotnet publish Yafc/Yafc.csproj -r win-x64 -c Release -o Build/Windows
```

자세한 내용은 [소스 빌드 안내](/Docs/HowToBuild.md)를 참고하세요.

## 주요 기능

- Factorio 2.0 이상에서 다양한 모드 조합을 지원합니다. Factorio 1.1을 지원하는 마지막 버전은 0.9.1입니다.
- 여러 페이지와 실행 취소 버튼(Ctrl+Z)을 제공합니다.
- Dependency Explorer로 어떤 개체가 무엇에 필요한지 확인할 수 있습니다.
- Never Enough Items로 어떤 아이템을 어떻게 생산할 수 있는지, YAFC가 어떤 선택지를 최적으로 보는지 확인할 수 있습니다.
- 메인 계산 시트:
    - 링크: YAFC는 링크된 물품에 대해서만 생산/소비 균형을 맞추려고 합니다. 링크되지 않은 물품은 계산하지만 균형 대상에는 넣지 않습니다. Helmod가 모든 것을 균형 맞추려다가 깊은 재귀 구조에서 깨지는 것과 다른 핵심 차이입니다.
    - 중첩 테이블: 어떤 레시피에도 중첩 테이블을 붙일 수 있습니다. 테이블을 접으면 안에 있는 모든 레시피의 요약을 볼 수 있습니다. 중첩 테이블은 자체 링크 집합을 가집니다.
    - 자동 모듈: 슬라이더 하나로 레시피에 모듈을 추가할 수 있습니다. 마일스톤을 기준으로 모듈을 넣고, 가장 이득이 큰 건물에 우선 배치합니다. <details><summary>동작 보기</summary><IMG src="/Docs/Media/AutoModules.gif" alt="AutoModules.gif"/></details>
    - 유체 온도 계산을 지원하며, 서로 다른 온도 유체를 섞지 않습니다.
    - 연료와 전력 계산을 지원합니다. 시트에 필요한 만큼의 에너지를 정확히 추가할 수도 있습니다. 단, 투입기는 포함되지 않습니다.
- 여러 분석 기능:
    - 접근성 분석은 접근할 수 없는 개체를 표시합니다. 모드와 Factorio에는 숨겨진 개체가 많습니다. 다만 모드나 맵 스크립트가 생성하는 개체를 모두 찾을 수는 없습니다.
    - 마일스톤 분석: 어떤 것이든 마일스톤으로 추가할 수 있습니다. YAFC는 해당 마일스톤 뒤에 잠긴 모든 개체에 아이콘을 표시합니다.
    - 자동화 분석은 완전히 자동화할 수 없는 개체를 찾습니다. 예를 들어 바닐라의 나무가 여기에 해당합니다.
    - 비용 분석은 각 개체에 비용을 부여합니다. 비용은 최적 레시피를 사용할 때 해당 개체를 얻기 위해 필요한 물류 작업의 합입니다.
    - 흐름 분석은 모든 비무한 연구에 충분한 과학 팩을 생산하는 기지를 계산합니다.
- [명령줄](/Docs/CLI.md)에서 프로젝트를 불러올 수 있습니다.

## 알려진 호환성 이슈

- Seablock은 먼저 [이 이슈](https://github.com/ShadowTheAge/yafc/issues/31)에 있는 활성화 목록을 확인하세요.
- 스크립트 기반 진행을 사용하는 다른 모드에서도 YAFC가 일부 아이템을 접근 불가로 판단할 수 있습니다. 완벽한 해결책은 없지만 Dependency Explorer를 열어 필요한 아이템이나 기술을 수동으로 접근 가능 표시할 수 있습니다.

모드 제작자는 YAFC용 모드별 수정 파일을 추가할 수 있습니다. 관련 [가이드](/Yafc/Data/Mod-fixes/README.md)를 참고하세요. 또한 data stage에서 `data.data_crawler` 값을 확인해 YAFC를 감지할 수 있습니다. 값은 `yafc a.b.c.d` 형태이며, 예를 들어 `yafc 2.5.6.0`처럼 표시됩니다.

YAFC는 Factorio와 완전히 동일하지 않은 환경에서 모드를 불러옵니다. 버그를 발견하면 [이슈](https://github.com/Yafc-CE/yafc-ce/issues)에 제보해 주세요.

## 기여하기

YAFC에 대해 이야기하고 싶다면 Pyanodons Discord의 [채널](https://discord.gg/b5VergGq75)에 참여하세요.  
Pull Request를 만들고 싶다면 리뷰 과정이 원활하도록 [Contributor's Guide](Docs/CONTRIBUTING.md)를 먼저 확인해 주세요.

## 라이선스

- [GNU GPL 3.0](/LICENSE)
- Copyright 2020 © ShadowTheAge
- 이 README에는 Factorio 아이콘이 들어간 GIF가 포함되어 있습니다. 모든 Factorio 아이콘의 저작권은 Wube Software에 있습니다.
- 자유 소프트웨어로 구동됩니다: .NET core, SDL2, Google Or-Tools, Lua 등. 전체 목록은 [licenses.txt](/licenses.txt)를 참고하세요.
