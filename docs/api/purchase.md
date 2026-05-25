# PurchaseAsync

Initiates a purchase for the given product ID and returns when the store responds.

## Signature

```csharp
public static async Task<PurchaseResult> PurchaseAsync(string productId)
```

## Parameters

| Parameter | Type | Description |
|---|---|---|
| `productId` | `string` | The product ID exactly as registered in App Store Connect / Google Play Console and passed to `InitializeAsync`. |

## Returns

[`PurchaseResult`](types.md#purchaseresult) — check `IsSuccess` to determine the outcome.

## Behavior

- Returns an error `PurchaseResult` immediately if `InitializeAsync` has not been called yet.
- Has a **60-second timeout**. If the store doesn't respond within that window, a timeout error result is returned.
- On success, the purchase is left in a **Pending** state until you call [`Confirm`](confirm.md).

> **Always** call `Confirm` after granting the item. If you skip it, the purchase reappears in [`GetPendingList`](pending.md) on the next launch.

## Example

```csharp
var result = await BreezeIAP.PurchaseAsync("gold_100");

if (result.IsSuccess)
{
    GiveGold(100);
    BreezeIAP.Confirm(result);
}
else
{
    ShowError(result.ErrorMessage);
}
```

## Related

- [`PurchaseResult`](types.md#purchaseresult)
- [`Confirm`](confirm.md)
