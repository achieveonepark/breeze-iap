# Restore

이전에 구매한 비소모성 상품과 구독을 복원합니다. **Apple 플랫폼 전용입니다.**

## 시그니처

```csharp
public static void Restore()
```

## 동작 방식

- Unity IAP v5의 `StoreController.RestoreTransactions(callback)`을 호출합니다.
- v5에서 이 메서드는 Apple 확장 프로바이더(`IAppleExtensions`)에서 `StoreController`로 이동했습니다.
- **Google Play**는 `InitializeAsync` 중 `FetchPurchases`에서 소유한 상품이 자동 복원되므로 이 메서드를 호출할 필요가 없습니다.
- 복원된 구매는 `OnPurchasePending` 흐름을 통해 `GetPendingList()`에 나타납니다. 일반 구매와 동일하게 처리하고 `Confirm`을 호출하면 됩니다.
- 성공/실패 결과는 Breeze IAP 로거를 통해 콘솔에 출력됩니다.

## 예시

```csharp
// App Store 정책 — 비소모성 상품이 있는 앱은 복원 버튼을 반드시 노출해야 합니다.
public void OnRestoreClicked()
{
    BreezeIAP.Restore();
}
```

복원 후 결과 처리는 `GetPendingList()`에서 일반 미확정 구매와 동일하게 처리합니다. `PurchaseResult.Type`이 `PurchaseType.Restore`로 설정됩니다.

```csharp
// 복원 버튼 클릭
public void OnRestoreClicked()
{
    BreezeIAP.Restore();
}

// 복원 결과는 이후 씬 로드 등 다음 InitializeAsync 시점에 GetPendingList()로 처리
var pending = BreezeIAP.GetPendingList();
foreach (var item in pending)
{
    if (item.Type == PurchaseType.Restore)
    {
        ApplyRestoredPurchase(item.Product.definition.id);
    }
    BreezeIAP.Confirm(item);
}
```

## App Store 정책

App Store 리뷰 가이드라인은 비소모성 인앱 상품을 판매하는 앱에 **복원 구매 버튼**을 반드시 표시하도록 요구합니다. 이 버튼을 노출하지 않으면 심사에서 리젝될 수 있습니다.

## 관련 항목

- [`GetPendingList`](pending.md)
- [`Confirm`](confirm.md)
- [`PurchaseType`](types.md#purchasetype)
