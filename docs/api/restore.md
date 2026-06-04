# Restore

Restores previously purchased non-consumable and subscription products. **Apple platforms only.**

## Signature

```csharp
public static void Restore()
```

## Behavior

- Calls `StoreController.RestoreTransactions(callback)` directly. In Unity IAP v5 this method moved off the Apple extension provider onto the controller itself.
- Primarily relevant on Apple platforms. On Google Play, owned products are restored automatically via `FetchPurchases` during `InitializeAsync`.
- Restored purchases re-enter the `OnPurchasePending` flow and surface through [`GetPendingList`](pending.md) — handle them the same way as a normal purchase, then `Confirm`.
- The success/failure result is logged via the Breeze IAP logger.

## Example

```csharp
// Typically hooked up to a "Restore Purchases" button (required by App Store guidelines)
public void OnRestoreClicked()
{
    BreezeIAP.Restore();
}
```

> App Store Review Guidelines require a visible **Restore Purchases** button in any app that sells non-consumable products.

## Related

- [`GetPendingList`](pending.md)
- [`Confirm`](confirm.md)
