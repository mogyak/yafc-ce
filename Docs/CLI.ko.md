# 명령줄 인터페이스

[English](CLI.md) | [한국어](CLI.ko.md)

YAFC는 명령줄에서 실행할 수 있습니다.

```sh
./Yafc
./Yafc --help
./Yafc path/to/project.yafc
./Yafc path/to/Factorio/data --mods-path path/to/mods --project-file path/to/project.yafc
```

현재 내장 도움말을 보려면 다음을 실행하세요.

```sh
./Yafc --help
```

인수 없이 YAFC를 시작하면 시작 화면이 열립니다.

옵션이 아닌 인수 하나만 전달하면 Factorio 데이터 디렉터리가 아니라 프로젝트 파일로 처리됩니다.

```sh
./Yafc path/to/project.yafc
```

해당 프로젝트를 이전에 연 적이 있다면 저장된 Factorio 데이터 경로와 모드 경로를 사용합니다. 처음 여는 프로젝트라면 가장 최근에 열었던 프로젝트의 시작 설정을 사용합니다.

Factorio 데이터에서 직접 시작할 때는 데이터 디렉터리가 첫 번째 인수여야 하며, 그 뒤에 옵션이 하나 이상 있어야 합니다. 다음 옵션을 전달할 수 있습니다.

- `<data-path>`: Factorio `data` 디렉터리.
- `--mods-path <path>`: Factorio 모드 디렉터리.
- `--project-file <path>`: 불러오거나 새로 만들 YAFC 프로젝트 파일.
- `--net-production`: 선택한 아이템 또는 유체에 대해 순생산이나 순소비가 있는 레시피만 제안합니다.
- `--help`: 내장 도움말을 출력하고 종료합니다.

지정한 디렉터리는 존재해야 합니다. `--project-file`은 프로젝트 파일 자체가 새 파일일 수 있지만, 해당 파일이 들어갈 디렉터리는 존재해야 합니다.
