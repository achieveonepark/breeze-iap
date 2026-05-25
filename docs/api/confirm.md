# Confirm

Finalizes a purchase with the store. Call this **after** you have safely granted the item to the player.

## Signatures

```csharp
public static void Confirm(PurchaseResult product)
public static void Confirm(Product product)
```

## Parameters

| Parameter | Type | Description |
|---|---|---|
| `product` | `PurchaseResult` | The result returned by `PurchaseAsync` or an item from `GetPendingList`. |
| `product` | `Product` | A raw Unity IAP `Product` object, if you are managing purchases at a lower level. |

## Behavior

Calls `IStoreController.ConfirmPendingPurchase` on the underlying Unity IAP controller. Until this is called, the purchase stays pending and will re-appear on the next app launch via `GetPendingList`.

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
