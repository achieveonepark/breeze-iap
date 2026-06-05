# Confirm

구매를 스토어에 확정합니다. **아이템 지급 후 반드시 호출해야 합니다.**

## 시그니처

```csharp
public static void Confirm(PurchaseResult result)
public static void Confirm(PendingOrder order)
```

## 파라미터

| 파라미터 | 타입 | 설명 |
|---|---|---|
| `result` | `PurchaseResult` | `PurchaseAsync` 반환값 또는 `GetPendingList`의 항목 |
| `order` | `PendingOrder` | Unity IAP v5의 `PendingOrder`를 직접 전달하는 저수준 오버로드 |

## 동작 방식

내부적으로 Unity IAP v5의 `StoreController.ConfirmPurchase(PendingOrder)`를 호출합니다.

`Confirm`이 호출되기 전까지 구매는 **Pending 상태**로 유지됩니다. 앱을 다시 시작하면 Unity IAP가 해당 주문을 다시 전달하고, `GetPendingList()`에 나타납니다.

`PurchaseResult` 오버로드는 내부적으로 `result.Order`를 사용합니다. `result.Order`가 `null`인 경우(실패/Deferred 결과) 경고 로그만 출력하고 반환합니다.

## 예시

### PurchaseAsync 후 확정

```csharp
var result = await BreezeIAP.PurchaseAsync("no_ads");

if (result.IsSuccess)
{
    PlayerPrefs.SetInt("no_ads", 1); // 아이템 지급 먼저
    BreezeIAP.Confirm(result);       // 그 다음 확정
}
```

### GetPendingList 항목 확정

```csharp
var pending = BreezeIAP.GetPendingList();
foreach (var item in pending)
{
    Grant(item.Product.definition.id); // 아이템 지급 먼저
    BreezeIAP.Confirm(item);           // 그 다음 확정
}
```

### 순서가 중요합니다

```csharp
// ✅ 올바른 순서
Grant(result.Product.definition.id);
BreezeIAP.Confirm(result);

// ❌ 잘못된 순서 — Confirm 후 크래시 시 아이템이 지급되지 않은 채 구매 확정
BreezeIAP.Confirm(result);
Grant(result.Product.definition.id);
```

## 관련 항목

- [`PurchaseAsync`](purchase.md)
- [`GetPendingList`](pending.md)
- [`PurchaseResult`](types.md#purchaseresult)
