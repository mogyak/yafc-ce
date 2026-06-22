# 직렬화 시스템

[English](Serialization.md) | [한국어](Serialization.ko.md)

프로젝트 직렬화 시스템은 다소 취약합니다.
많은 변경에는 적절히 적응하지만, 조용히 실패할 수도 있습니다.
조용한 실패 가능성을 줄이기 위해 일부 [보호 테스트](#보호-테스트)가 있으며, 관련 변경이 의도적이고 정상 동작하는지 확인합니다.

직렬화 시스템은 public instance property만 다룹니다.
크게 public property에는 여기서 'writable'과 'non-writable'이라고 부를 두 종류가 있습니다.
writable property는 public setter가 있거나, 같은 이름과 타입을 가진 생성자 매개변수가 있거나, 둘 다 있는 property입니다.
그 외 public property는 non-writable입니다.
`[Obsolete]` writable property는 getter를 생략할 수 있으며, 보통 생략하는 편이 좋습니다.

`ModelObject<>`에서 파생된 타입의 첫 번째 매개변수를 제외한 모든 생성자 매개변수는 writable property와 같은 이름과 타입을 가져야 합니다.

## Property 타입

직렬화 및 역직렬화되는 주요 데이터 구조는 `ModelObject`에서 파생된 타입과 `[Serializable]` attribute가 붙은 concrete, 즉 non-abstract class입니다.
이 타입들은 property별로 재귀적으로 직렬화됩니다.
Property는 아래 설명에 따라 처리됩니다.

|Property type|Writable|Non-writable|
|-|-|-|
|[`ModelObject` 및 파생 타입](#modelobject와-serializable-타입)|concrete이면 지원|지원|
|[`[Serializable]` class](#modelobject와-serializable-타입)|concrete이면 지원|무시|
|[`ReadOnlyCollection<>` 및 `ICollection<>` 또는 `IDictionary<,>` 구현 타입](#collection)|오류|내용이 지원되면 지원|
|[`FactorioObject`, 모든 파생 타입, `IObjectWithQuality<>`](#factorioobject와-iobjectwithquality)|abstract여도 지원|무시|
|[Native 및 native-like 타입](#native-타입)|목록에 있으면 지원|무시|
|Property에 `[SkipSerialization]`이 있는 모든 타입|무시|무시|
|그 외 타입|오류|무시|

참고:
* 생성자는 직렬화되는 collection을 non-`null` 값으로 초기화해야 합니다.
Property는 위 표의 _Property type_ 열과 일치하는 어떤 타입으로 선언해도 됩니다.
* Value type은 [지원되는 native 타입](#native-타입) 목록에 있을 때만 지원됩니다.
* `[Obsolete]` property도 같은 규칙을 따라야 합니다. 단, writable이면 getter가 없어도 됩니다.

### `ModelObject`와 `[Serializable]` 타입

각 class에는 public 생성자가 정확히 하나 있어야 합니다.
public 생성자가 여러 개라면, 직렬화 시스템은 컴파일러가 "첫 번째"라고 정하는 생성자를 사용합니다.\
**예외**: class에 `[DeserializeWithNonPublicConstructor]` attribute가 있으면 public 생성자 대신 non-public 생성자가 정확히 하나 있어야 합니다.

생성자는 다음 제한을 만족하는 한 원하는 수의 매개변수를 가질 수 있습니다.
* `ModelObject<T>`에서 직접 또는 간접적으로 파생된 타입의 첫 번째 생성자 매개변수는 일반 규칙을 따르지 않습니다.
반드시 존재해야 하고, 타입은 `T`여야 하며, 이름은 무엇이든 가능합니다.
* 그 외 각 매개변수는 class의 writable property 중 하나와 같은 타입 및 이름을 가져야 합니다.
매개변수는 직접 소유한 property나 base class에서 상속받은 property와 매칭될 수 있습니다.
* 매개변수에 기본값이 있다면 그 값은 `default`여야 합니다.
또는 이에 해당하는 값이어야 합니다. 예를 들어 reference type과 `Nullable<>`의 `null`, numeric type의 `0`, 명시적 0-parameter 생성자가 없는 value type의 `new()`가 이에 해당합니다.

지원되는 타입 중 하나가 아닌 writable property에는 `[SkipSerialization]` attribute가 있어야 합니다.

Collection property는 항상 non-writable이며, 생성자에서 non-`null` 값으로 초기화해야 합니다.
생성자는 non-writable `ModelObject` property를 초기화하지 않아도 됩니다.
초기화하지 않으면 직렬화 시스템은 프로젝트 파일에서 만난 non-`null` 값을 버립니다.

### Collection

Collection 값은 non-writable property에 저장되어야 하며, 생성자에 전달되면 안 되고, 빈 collection으로 초기화되어야 합니다.
지원되지 않는 key 또는 value type은 해당 property가 조용히 무시되게 만듭니다.
배열과 그 밖의 고정 크기 collection은 지원되는 값을 담고 있더라도 역직렬화 시 오류를 발생시킵니다. `ICollection<>`을 구현하더라도 마찬가지입니다.

Dictionary key는 `FactorioObject` 또는 그 파생 타입, `IObjectWithQuality<>`, `string`, `Guid`, `Type`이어야 합니다.

Value는 writable property에 저장할 수 있는 어떤 타입이어도 됩니다.
명시적으로 value는 collection을 포함할 수 있지만, value 자체가 collection이면 안 됩니다.
Collection 안의 `null` 값은 허용되지만 JSON에서 불러올 때 제거됩니다.

Serializer에는 `ReadOnlyCollection<>`을 수정할 수 있게 하는 특수 처리가 있습니다.
`ReadOnlyCollection<>`을 `new([])`로 초기화하면 serializer가 올바르게 채웁니다.

### `FactorioObject`와 `IObjectWithQuality<>`

역직렬화될 때 `FactorioObject`는 `Database.allObjects`에서 대응되는 object로 설정됩니다.
그 object를 찾을 수 없다면, 예를 들어 해당 object를 정의한 모드가 제거되었다면, `null`로 설정됩니다.
Property는 타입이 abstract여도 `FactorioObject` 또는 그 파생 타입을 반환할 수 있습니다.

`IObjectWithQuality<>`도 비슷하지만, object는 `ObjectWithQuality.Get`을 호출해 가져옵니다.

### Native 타입

지원되는 native 및 native-like 타입은 `int`, `float`, `bool`, `ulong`, `string`, `Type`, `Guid`, `PageReference`, 그리고 `int`를 기반으로 하는 `enum`입니다.
적용 가능한 경우 이 타입들의 `Nullable<>` 버전도 지원됩니다.

위 목록에 없는 value type은 직렬화하거나 역직렬화할 수 없습니다. 여기에는 `Tuple`, `ValueTuple`, 모든 custom `struct`가 포함됩니다.

## 보호 테스트

직렬화 시스템이 처리하는 타입을 보호하는 "unit" 테스트가 있습니다.
이 테스트들은 직렬화 시스템이 실제로 마주칠 가능성이 높은 항목을 검사하고, [Property 타입](#property-타입)의 규칙을 지키는지 확인합니다.
또한 직렬화 데이터의 변경을 검사해, 변경이 의도적인지 확인합니다.

실패 메시지는 테스트가 왜 불만인지 정확히 알려주어야 합니다.
일반적으로 테스트 실패는 다음 의미를 갖습니다.
* [`ModelObjects_AreSerializable`](../../Yafc.Model.Tests/Serialization/SerializationTypeValidation.cs)이 실패하면, `ModelObject`에서 파생된 타입에서 [규칙](#modelobject와-serializable-타입)을 위반했다고 판단한 것입니다.
* [`Serializables_AreSerializable`](../../Yafc.Model.Tests/Serialization/SerializationTypeValidation.cs)이 실패하면, `[Serializable]` 타입에서 [규칙](#modelobject와-serializable-타입)을 위반했다고 판단한 것입니다.
* [`TreeHasNotChanged`](../../Yafc.Model.Tests/Serialization/SerializationTreeChangeDetection.cs)가 실패하면, 직렬화되는 타입이나 property를 추가, 변경, 제거한 것입니다.
  * 변경이 의도적이며 직렬화를 깨뜨리지 않는지 확인하세요. 예를 들어 `List<>`와 `ReadOnlyCollection<>` 사이를 바꾸는 경우가 이에 해당합니다. 또는 `FactorioObject`에서 `List<FactorioObject>`로 바꾸는 경우처럼 필요한 변환을 처리했는지 확인하세요.
그런 다음 dictionary initializer를 변경 사항에 맞게 업데이트하세요.
큰 변경을 했다면 debugger에서 `BuildPropertyTree`를 실행해 테스트가 기대하는 initializer를 만들 수 있어야 합니다.
* 새 property를 직렬화하려고 했는데 `TreeHasNotChanged`가 실패하지 않는다면, 새 property가 ["Ignored" 범주](#property-타입) 중 하나에 들어갔을 가능성이 큽니다.
* 새 `[Serializable]` 타입을 직렬화하려고 했는데 `TreeHasNotChanged`가 실패하지 않는다면, 해당 타입의 writable property가 있는지 확인하세요.

테스트 실패는 보통 writable property와 관련되어 있습니다.
배열 타입을 반환하는 non-writable property도 테스트 실패를 일으킬 수 있습니다.

## Property 타입 변경 처리

가장 단순한 해결책은 새 property를 도입하고 기존 property에 `[Obsolete]`를 적용하는 것입니다.
JSON deserializer는 프로젝트 파일에 해당 값이 있으면 계속 읽지만, undo system과 JSON serializer는 기존 property를 무시합니다.
Obsolete writable property에서는 getter를 제거해도 되며, 제거하는 편이 좋을 수 있습니다.

Property를 obsolete 처리하는 것이 합리적이지 않은 경우도 있습니다. `FactorioObject`에서 `ObjectWithQuality<>`로 바뀐 경우가 그 예입니다.
요구 사항에 따라 `ICustomJsonDeserializer<T>`를 구현하거나, 현재 `QualityObjectSerializer<T>` 구현처럼 새 `ValueSerializer<T>`를 만들 수 있습니다. `ICustomJsonDeserializer<T>`는 [081e9c0f](https://github.com/Yafc-CE/yafc-ce/tree/081e9c0f6b47e155fbc82763590a70d90a64c83c/Yafc.Model/Data/DataClasses.cs#L819) 및 그 이전의 `ObjectWithQuality<T>`에서 사용되었습니다.

## 새 지원 타입 추가

`List<List<T>>` 대신 `List<NestedList<T>>`를 쓰는 식의 우회도 고려해 보세요.

```csharp
[Serializable]
public sealed class NestedList<T> : IEnumerable<T> /* ICollection<> 또는 IList<>를 구현하지 마세요 */ {
    public List<T> List { get; } = [];
    public IEnumerator<T> GetEnumerator() => List.GetEnumerator();
}
```

### Writable property와 collection value

타입이 이미 YAFC의 일부라면 해당 타입에 `[Serializable]`을 추가하는 것만으로 지원할 수 있을지도 모릅니다.
그것으로 충분하지 않다면 `ValueSerializer<T>`에서 파생된 새 class를 구현하세요.
직렬화되는 타입이 generic이거나 타입 계층이라면 새 타입도 generic이어야 합니다.
`IsValueSerializerSupported`와 `CreateValueSerializer`에 새로 지원하는 타입 검사를 추가하세요.

테스트는 writable property와 collection에서 새로 지원되는 타입에 맞춰 자동으로 업데이트되어야 합니다.

### Dictionary key

대응되는 `ValueSerializer<T>` 구현을 찾거나, 이전 섹션 설명처럼 새로 만드세요.
값을 string으로 변환하도록 `GetJsonProperty` override를 추가하세요.
`ReadFromJson`이 `JsonTokenType.PropertyName`에서 읽는 것을 지원하지 않는다면 그렇게 지원하도록 업데이트하거나, `ReadFromJsonProperty`를 override하세요.
어느 쪽이든 property name을 읽어 원하는 object를 반환하면 됩니다.
`IsKeySerializerSupported`에 새로 지원하는 타입 검사를 추가하세요.

테스트는 dictionary key에서 새로 지원되는 타입에 맞춰 자동으로 업데이트되어야 합니다.

### Non-writable property

non-writable property에서 새 타입을 지원하려면 다음과 같은 새 class를 구현하세요.

```csharp
internal sealed class NewReadOnlyPropertySerializer<TOwner, TPropertyType[, TOtherTypes]>(PropertyInfo property)
    where TPropertyType : NewPropertyType[<TOtherTypes>] where TOwner : class
    : PropertySerializer<TOwner, TPropertyType>(property, PropertyType.Normal, false)
```

새로 지원하는 타입이 generic이면 `TOtherTypes`를 포함하세요.
Nested object는 `ValueSerializer<TOtherType>.Default`를 사용해 직렬화하세요. 그래야 새로 도입된 `ValueSerializer<T>` 구현이 collection에서도 지원됩니다.

새로 지원하는 타입이 interface라면 `GetInterfaceSerializer`에 해당 interface type 검사를 추가하세요.

새로 지원하는 타입이 class라면 `GetInterfaceSerializer`를 호출하는 두 위치를 찾으세요.
바깥쪽 else block 안에 새로 지원하는 타입 검사를 추가하세요.

Non-writable property 지원 변경은 `AssertNoArraysOfSerializableValues` 호출 주변의 `SerializationTypeValidation` 변경이 필요할 수 있으며, `typeof(ReadOnlyCollection<>)`, `typeof(IDictionary<,>)`, `typeof(ICollection<>)` 테스트 주변의 `TreeHasNotChanged` 변경도 필요합니다.
