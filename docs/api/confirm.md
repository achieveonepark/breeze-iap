# Confirm

Finalizes a purchase with the store. Call this **after** you have safely granted the item to the player.

## Signatures

```csharp
public static void Confirm(PurchaseResult result)
public static void Confirm(PendingOrder order)
```

## Parameters

| Parameter | Type | Description |
|---|---|---|
| `result` | `PurchaseResult` | The result returned by `PurchaseAsync` or an item from `GetPendingList`. |
| `order` | `PendingOrder` | A raw Unity IAP v5 `PendingOrder`, if you are managing orders at a lower level. |

## Behavior

Calls `StoreController.ConfirmPurchase(PendingOrder)` on the underlying Unity IAP v5 controller. Until this is called, the order stays pending and will re-appear on the next app launch via `GetPendingList`.

> **Migration note (v5):** Unity IAP v5 confirms an **order**, not a `Product`. The old `Confirm(Product)` overload has been replaced by `Confirm(PendingOrder)`. When you pass a `PurchaseResult`, Breeze IAP uses its `Order` field internally.

## Example

### Confirming after PurchaseAsync

```csharp
var result = await BreezeIAP.PurchaseAsync("no_ads");

if (result.IsSuccess)
{
    PlayerPrefs.SetInt("no_ads", 1); // grant first
    BreezeIAP.Confirm(result);       // then confirm
}
```

### Confirming from the pending list

```csharp
var pending = BreezeIAP.GetPendingList();
foreach (var item in pending)
{
    Grant(item.Product.definition.id);
    BreezeIAP.Confirm(item);
}
```

## Related

- [`PurchaseAsync`](purchase.md)
- [`GetPendingList`](pending.md)
