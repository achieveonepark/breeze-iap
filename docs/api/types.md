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

## InitializeResult

Returned internally during initialization. You don't typically use this directly.

```csharp
public readonly struct InitializeResult
{
    public bool   IsInitialized { get; init; }
    public string ErrorMessage  { get; init; }
    public bool   IsSuccess     => string.IsNullOrEmpty(ErrorMessage);
}
```

---

## PurchaseResult

The result of a [`PurchaseAsync`](purchase.md) call or an item from [`GetPendingList`](pending.md).

```csharp
public readonly struct PurchaseResult
{
    public PurchaseType Type         { get; init; }
    public Product      Product      { get; init; }
    public string       ErrorMessage { get; init; }
    public bool         IsSuccess    => string.IsNullOrEmpty(ErrorMessage);
}
```

| Field | Type | Description |
|---|---|---|
| `Type` | `PurchaseType` | Indicates how the purchase arrived. |
| `Product` | `Product` | The Unity IAP `Product` object. Use `Product.definition.id` to get the product ID. |
| `ErrorMessage` | `string` | Non-empty when `IsSuccess` is `false`. |
| `IsSuccess` | `bool` | `true` when the purchase completed without error. |

---

## PurchaseType

```csharp
public enum PurchaseType
{
    Purchase, // Normal purchase flow
    Pending,  // Re-delivered from a previous session
    Restore,  // Restored on iOS
    Error     // Purchase failed
}
```
