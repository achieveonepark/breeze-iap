# Types

## InitializeDto

Describes a single product to register with the store.

```csharp
public readonly struct InitializeDto
{
    public string      ProductId   { get; init; }
    public ProductType ProductType { get; init; }
}
```

| Field | Type | Description |
|---|---|---|
| `ProductId` | `string` | Must match the product ID in App Store Connect / Google Play Console exactly. |
| `ProductType` | `ProductType` | `Consumable`, `NonConsumable`, or `Subscription` (from `UnityEngine.Purchasing`). |

---

## PurchaseResult

The result of a [`PurchaseAsync`](purchase.md) call or an item from [`GetPendingList`](pending.md).

```csharp
public readonly struct PurchaseResult
{
    public PurchaseType Type          { get; init; }
    public Product      Product       { get; init; }
    public PendingOrder Order         { get; init; }
    public string       ErrorMessage  { get; init; }
    public string       Receipt       => Order?.Info?.Receipt;
    public string       TransactionId => Order?.Info?.TransactionID;
    public bool         IsSuccess     => string.IsNullOrEmpty(ErrorMessage);
}
```

| Field | Type | Description |
|---|---|---|
| `Type` | `PurchaseType` | Indicates how the purchase arrived. |
| `Product` | `Product` | The Unity IAP `Product` object. Use `Product.definition.id` to get the product ID. |
| `Order` | `PendingOrder` | The Unity IAP v5 order. Pass it to [`Confirm`](confirm.md) to finalize the purchase. `null` for failed / deferred results. |
| `ErrorMessage` | `string` | Non-empty when `IsSuccess` is `false`. |
| `Receipt` | `string` | Receipt JSON, taken from `Order.Info.Receipt`. Useful for server-side validation. |
| `TransactionId` | `string` | The store transaction ID, from `Order.Info.TransactionID`. |
| `IsSuccess` | `bool` | `true` when the purchase completed without error. |

---

## PurchaseType

```csharp
public enum PurchaseType
{
    Purchase, // Normal purchase flow
    Pending,  // Re-delivered from a previous session
    Deferred, // Awaiting external approval (e.g. Ask to Buy)
    Restore,  // Restored on iOS
    Error     // Purchase failed
}
```
