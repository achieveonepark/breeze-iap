# InitializeAsync

스토어에 연결하고 상품 카탈로그를 등록합니다. 다른 모든 Breeze IAP 메서드보다 먼저 호출해야 합니다.

## 시그니처

```csharp
public static async Task InitializeAsync(InitializeDto[] dtos, bool isDebug = false)
public static async Task InitializeAsync(List<InitializeDto> dtos, bool isDebug = false)
```

## 파라미터

| 파라미터 | 타입 | 설명 |
|---|---|---|
| `dtos` | `InitializeDto[]` \| `List<InitializeDto>` | 스토어에 등록할 상품 정의 목록 |
| `isDebug` | `bool` | `true`이면 Unity 콘솔에 상세 로그를 출력합니다. 기본값 `false` |

## 동작 방식

내부적으로 Unity IAP v5 시퀀스를 순서대로 실행합니다:

1. `UnityIAPServices.StoreController()` — 컨트롤러 인스턴스 생성
2. `Connect()` — 스토어 연결 (10초 타임아웃)
3. `FetchProducts(List<ProductDefinition>)` — 상품 정보 조회 (10초 타임아웃)
4. `FetchPurchases()` — 미확정 구매 조회 (10초 타임아웃)

각 단계에 **10초 타임아웃**이 적용됩니다. 타임아웃 초과 시 경고 로그를 남기고 초기화를 중단합니다.

- 이미 초기화된 상태에서 재호출하면 조용히 무시됩니다 (멱등성).
- `InitializeDto` 각각은 내부적으로 `ProductDefinition`으로 변환됩니다.

## 예시

```csharp
await BreezeIAP.InitializeAsync(new[]
{
    new InitializeDto { ProductId = "gold_100",  ProductType = ProductType.Consumable },
    new InitializeDto { ProductId = "no_ads",    ProductType = ProductType.NonConsumable },
    new InitializeDto { ProductId = "vip_month", ProductType = ProductType.Subscription },
}, isDebug: true);
```

### List\<T\> 오버로드

```csharp
var list = new List<InitializeDto>
{
    new InitializeDto { ProductId = "gold_100", ProductType = ProductType.Consumable },
};

await BreezeIAP.InitializeAsync(list);
```

## 초기화 실패 시

타임아웃이나 스토어 오류로 초기화가 실패하면 `InitializeAsync`는 반환되지만 내부 상태(`_isInitialized`)가 `false`로 남습니다. 이후 `PurchaseAsync`, `GetPendingList` 호출 시 즉시 오류 결과 또는 `null`을 반환합니다.

재시도가 필요한 경우 `InitializeAsync`를 다시 호출하세요.

## 관련 항목

- [`InitializeDto`](types.md#initializedto)
- [`GetPendingList`](pending.md) — 초기화 직후 호출하여 미확정 구매를 처리
