# YAFC의 추가 언어 지원

[English](MoreLanguagesSupport.md) | [한국어](MoreLanguagesSupport.ko.md)

시작 화면에서 YAFC가 영어가 아닌 텍스트를 표시하도록 설정할 수 있습니다.

- 시작 화면에서 "In-game objects language:" 옆의 언어 이름, 보통 "English"를 클릭합니다.
- 나타나는 드롭다운에서 원하는 언어를 선택합니다.
- 사용하는 언어가 비유럽권 글리프를 필요로 한다면 목록 아래쪽에 표시될 수 있습니다.
  - 이런 언어를 사용하려면 YAFC가 적절한 글꼴을 한 번 다운로드해야 할 수 있습니다. 글꼴 다운로드 권한을 묻는 창이 나오면 "Confirm"을 클릭하세요.
  - YAFC가 글꼴을 자동으로 다운로드하지 않게 하려면 드롭다운에서 "Select font"를 클릭하고, 해당 언어를 지원하는 글꼴 파일을 직접 선택하세요.

Factorio에서는 지원하는 언어인데 시작 화면에 표시되지 않는다면, YAFC가 해당 언어 문자열을 사용하도록 수동으로 지정할 수 있습니다.

- 텍스트 편집기로 `yafc2.config`를 엽니다. 일반적인 위치는 다음과 같습니다.
  - Windows: `%localappdata%\YAFC\yafc2.config`
  - Linux: `~/.local/share/YAFC/yafc2.config`
  - macOS: `~/Library/Application Support/yafc2.config`
- `language` 섹션을 찾아 값을 원하는 언어 코드로 바꿉니다. 언어 코드 예시는 다음과 같습니다.
	- 중국어 간체: `zh-CN`
	- 중국어 번체: `zh-TW`
	- 한국어: `ko`
	- 일본어: `ja`
	- 히브리어: `he`
	- 그 외: `Factorio/data/base/locale` 폴더에서 원하는 언어의 폴더 이름을 확인하세요.
- 번들 글꼴이 해당 언어의 글리프를 그릴 수 없다면, 시작 화면의 언어 드롭다운에서 "Select font" 버튼을 누르고 해당 언어를 지원하는 글꼴 파일을 선택하세요.
