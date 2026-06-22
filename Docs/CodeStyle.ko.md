# YAFC 코드 스타일

[English](CodeStyle.md) | [한국어](CodeStyle.ko.md)

코드 스타일은 YAFC 프로젝트에서 사용하는 규칙을 설명합니다.
함께 개발하기 쉽도록 YAFC에 기여할 때는 이 코드 스타일을 따라 주세요.

### 일반 지침

핵심 목표는 코드를 유지보수하기 쉽고 읽기 쉽게 유지하는 것입니다.

* 복잡하지 않고 지저분한 우회가 없는 코드를 지향해 주세요.
* 코드를 문서화해 주세요.
* 기존 코드도 문서화할 수 있다면 더 좋습니다. 이해하기 쉬운 코드는 읽는 경험도 더 좋게 만듭니다.

### 커밋

* 동작 변경과 리팩터링은 서로 다른 커밋으로 분리해 주세요. 그래야 커밋을 더 쉽게 이해할 수 있습니다.
* 브랜치를 업데이트할 때는 merge가 아니라 rebase를 사용해 주세요. merge는 PR이 master에 병합될 때만 예상됩니다. 그 외의 경우에는 커밋 히스토리를 복잡하게 만들기 때문에 rebase를 사용해 주세요.
* 의미 있는 커밋 메시지를 작성해 주세요.
* 커밋 prefix는 나중에 커밋을 훑어보기 쉽게 해 줍니다. 커밋 제목에는 다음 prefix 사용을 권장합니다.
    * `docs`: 문서 변경.
    * `feature` 또는 `feat`: 새 기능.
    * `fix`: 버그 수정.
    * `refactor`: 리팩터링.
    * `style`: 코드 스타일 적용.
    * `style-change`: 코드 스타일 자체의 변경.
    * `chore`: 다른 prefix에 속하지 않는 변경.

### 코드

* TODO를 추가한다면 세부 내용을 커밋 메시지에 설명하거나, 가능하면 GitHub 이슈에 남겨 주세요. 그래야 TODO가 실제로 처리될 가능성이 높아집니다.

#### 빈 줄

빈 줄은 코드를 주제별 덩어리로 나누는 데 도움이 됩니다.
메서드나 키워드 같은 코드 블록 주변에는 빈 줄을 두는 것을 권장합니다.

예시:

```csharp
public int funcName() {
    if() { // 구분할 내용이 없으므로 빈 줄 없음
        for (;;) { // 구분할 내용이 많지 않으므로 빈 줄 없음
            <more calls and assignments>
        }

        <more calls and assignments> // 나머지 코드와 구분하기 위한 빈 줄
    }

    <more calls and assignments> // 코드 블록을 구분하기 위한 빈 줄
}

private void Foo() => Baz(); // 함수 사이의 빈 줄

private void One(<a long description
    that goes on for
    several lines) {

    <more calls and assignments> // 함수 정의와 본문 시작을 구분하기 위한 빈 줄
}
```

### 줄바꿈

대부분의 모니터에 맞도록 줄 길이는 190자보다 짧게 유지해 주세요.

줄을 나눌 때는 다음 예시를 선호하는 줄바꿈 방식의 기준으로 삼을 수 있습니다.

```csharp
if (recipe.subgroup == null
    // (1) 연산자에 전달되는 식 내부보다 연산자 앞에서 먼저 줄을 나눕니다.
    && imgui.BuildRedButton("Delete recipe")
        // (2) 메서드 인자 사이보다 .MethodName 앞에서 먼저 줄을 나눕니다.
        .WithTooltip(imgui,
            // (3) 메서드 인자 내부보다 메서드 인자 사이에서 먼저 줄을 나눕니다.
            "Shortcut: right-click")
    // (1)
    && imgui.CloseDropdown()) {
```

`.` `+` `&&` 같은 대부분의 연산자는 다음 줄로 보냅니다.
예외적으로 `=>`, `=> {`, `,`는 같은 줄에 둡니다.

생성자와 메서드 정의의 인자 줄바꿈은 상황에 맞게 선택해도 됩니다.
