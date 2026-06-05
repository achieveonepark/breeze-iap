# GetPendingList

초기화 중 스토어로부터 수집된 미확정 구매 목록을 반환합니다. `InitializeAsync` 직후에 호출하세요.

## 시그니처

```csharp
public static List<PurchaseResult> GetPendingList()
```

## 반환값

`List<PurchaseResult>` — 미확정 구매가 없으면 빈 리스트를 반환합니다. `InitializeAsync` 호출 전에 사용하면 `null`을 반환합니다.

## 미확정 구매란?

구매가 Pending 상태로 남는 경우:

1. 스토어에서 구매 완료 신호(`PendingOrder`)가 도착했으나
2. `Confirm`이 호출되기 전에 앱이 종료되거나 크래시된 경우

다음 앱 실행 시 Unity IAP v5의 `FetchPurchases` — `OnPurchasesFetched` 이벤트를 통해 해당 주문이 다시 전달됩니다. Breeze IAP는 이 주문들을 자동으로 수집하여 `GetPendingList()`에서 꺼낼 수 있도록 합니다.

## 예시

```csharp
await BreezeIAP.InitializeAsync(products);

var pending = BreezeIAP.GetPendingList();
if (pending != null && pending.Count > 0)
{
    foreach (var item in pending)
    {
        Grant(item.Product.definition.id);
        BreezeIAP.Confirm(item);
    }
}
```

### 구독 상품 처리

```csharp
var pending = BreezeIAP.GetPendingList();
if (pending == null) return;

foreach (var item in pending)
{
    string id = item.Product.definition.id;

    switch (item.Product.definition.type)
    {
        case ProductType.Consumable:
            GrantConsumable(id);
            break;
        case ProductType.NonConsumable:
            GrantNonConsumable(id);
            break;
        case ProductType.Subscription:
            ActivateSubscription(id);
            break;
    }

    BreezeIAP.Confirm(item);
}
```

## 관련 항목

- [`InitializeAsync`](initialize.md)
- [`Confirm`](confirm.md)
- [`PurchaseResult`](types.md#purchaseresult)
