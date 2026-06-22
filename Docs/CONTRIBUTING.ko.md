# 기여자 안내

[English](CONTRIBUTING.md) | [한국어](CONTRIBUTING.ko.md)

커뮤니티에서 더 즐겁게 협업하기 위해 몇 가지 사항을 안내합니다.

### 코딩을 시작하기 전에 기능 설계를 먼저 제출해 주세요

YAFC의 코드는 이미 충분히 복잡하기 때문에, 가능하면 더 단순하게 유지하려고 합니다. 즉, 새 기능이 복잡도를 높인다면 코딩을 시작하기 전에 먼저 Feature Design 이슈로 제안을 올려 주세요. 해당 기능을 YAFC에 추가할 수 있는지 함께 논의할 수 있습니다.

### 개발 환경 설정

* [set-up-git-hooks.sh](/set-up-git-hooks.sh)를 확인한 뒤 한 번 실행해 주세요. 이 스크립트는 기본 포맷이 맞는지 확인하는 검사를 `git push` 전에 실행하도록 설정합니다.

### 코딩

* 이 프로젝트에서 사용하는 규칙은 [Code Style](/Docs/CodeStyle.md)을 참고해 주세요.
* Visual Studio에서는 Code Cleanup을 실행하거나 "Ctrl+K, Ctrl+D"로 Format Document를 실행해 일부 규칙을 확인할 수 있습니다.

### Pull Request

* 변경 사항에 대한 짧은 설명을 [changelog](../changelog.txt)에 추가해 주세요.
* PR에는 이슈에 대한 간단한 설명과, PR이 그 이슈를 어떻게 해결하는지 적어 주세요.
* master 브랜치와의 충돌을 해결하기 위해 브랜치를 업데이트해야 한다면 merge commit 대신 rebase를 사용해 주세요.
* PR 리뷰 중 지적된 문제를 수정한다면, 브랜치를 동시에 rebase하지 말고 해당 문제만 고치는 별도 커밋을 push해 주세요. 이렇게 하면 리뷰어가 전체 브랜치를 다시 보는 대신 수정 커밋만 확인할 수 있습니다. 사이트에서 수정 커밋이 보이는 것을 확인한 뒤에는 그 커밋을 squash해도 됩니다. 단, GitHub가 actions 기록에서 해당 수정 커밋을 계속 확인할 수 있어야 합니다. 커밋이 사라졌거나 [명확하게 보이지 않는다면](https://github.com/Yafc-CE/yafc-ce/pull/529#issuecomment-4013056324), 해당 수정 커밋을 언급하는 댓글을 남겨 주세요.
