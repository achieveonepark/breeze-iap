# Restore

Restores previously purchased non-consumable and subscription products. **Apple platforms only.**

## Signature

```csharp
public static void Restore()
```

## Behavior

- Does nothing on non-iOS platforms (safe to call unconditionally).
- Calls `IAppleExtensions.RestoreTransactions` via the Unity IAP extension provider.
- Restored products re-enter `ProcessPurchase` — handle them the same way as a normal purchase.

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
