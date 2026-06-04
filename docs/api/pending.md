# GetPendingList

Returns the list of purchases that were completed by the store but not yet confirmed by your game. Call this once after `InitializeAsync`.

## Signature

```csharp
public static List<PurchaseResult> GetPendingList()
```

## Returns

`List<PurchaseResult>` — may be empty if there are no unconfirmed purchases. Returns `null` if called before `InitializeAsync`.

## When does a purchase become pending?

A purchase enters the pending list when:

1. The store reported a successful purchase (it arrived as a `PendingOrder`).
2. The app crashed or was closed before `Confirm` was called.

On the next launch, Unity IAP v5 re-delivers the order through `FetchPurchases` (`OnPurchasesFetched`) during initialization. Breeze IAP captures any `PendingOrder` in the pending list so you can process it safely.

## Example

```csharp
await BreezeIAP.InitializeAsync(products);

// Always drain the pending list right after init
var pending = BreezeIAP.GetPendingList();
if (pending != null)
{
    foreach (var item in pending)
    {
        Grant(item.Product.definition.id);
        BreezeIAP.Confirm(item);
    }
}
```

## Related

- [`InitializeAsync`](initialize.md)
- [`Confirm`](confirm.md)
- [`PurchaseResult`](types.md#purchaseresult)
