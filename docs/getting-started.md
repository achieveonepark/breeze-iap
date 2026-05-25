# Getting Started

## Requirements

| Requirement | Version |
|---|---|
| Unity | 2022.3 or later |
| Unity In-App Purchasing | **5.0.0 or later** |

## Installation

### Via Unity Package Manager (Git URL)

1. Open **Window → Package Manager**
2. Click **+** → **Add package from git URL…**
3. Enter:

```
https://github.com/achieveonepark/breeze-iap.git
```

Unity will automatically install the `com.unity.purchasing 5.0.0` dependency.

---

## Basic Setup

### 1. Define your products

Create `InitializeDto` entries for every product registered in App Store / Google Play.

```csharp
var products = new[]
{
    new InitializeDto { ProductId = "gold_100",  ProductType = ProductType.Consumable },
    new InitializeDto { ProductId = "gold_500",  ProductType = ProductType.Consumable },
    new InitializeDto { ProductId = "no_ads",    ProductType = ProductType.NonConsumable },
    new InitializeDto { ProductId = "vip_month", ProductType = ProductType.Subscription },
};
```

### 2. Initialize on startup

Call `InitializeAsync` once, typically in an early `MonoBehaviour.Start`:

```csharp
private async void Start()
{
    await BreezeIAP.InitializeAsync(products, isDebug: true);
}
```

`isDebug: true` enables verbose console logs. Remove it in production builds.

### 3. Process pending purchases

Always call `GetPendingList()` right after a successful init. This handles purchases that were completed in a previous session but not yet confirmed (e.g. the app crashed before `Confirm` was called).

```csharp
var pending = BreezeIAP.GetPendingList();
foreach (var item in pending)
{
    GrantItem(item);
    BreezeIAP.Confirm(item);
}
```

### 4. Purchase

```csharp
var result = await BreezeIAP.PurchaseAsync("gold_100");

if (result.IsSuccess)
{
    GrantItem(result);
    BreezeIAP.Confirm(result);
}
else
{
    Debug.LogWarning($"Purchase failed: {result.ErrorMessage}");
}
```

> **Important:** Always call `Confirm` after granting the item. Not calling `Confirm` puts the product into the pending queue on the next launch.

---

## Full Example

```csharp
using UnityEngine;
using UnityEngine.Purchasing;
using Achieve.BreezeIAP;

public class ShopManager : MonoBehaviour
{
    private async void Start()
    {
        var products = new[]
        {
            new InitializeDto { ProductId = "gold_100", ProductType = ProductType.Consumable },
            new InitializeDto { ProductId = "no_ads",   ProductType = ProductType.NonConsumable },
        };

        await BreezeIAP.InitializeAsync(products);

        // Recover any unconfirmed purchases from last session
        var pending = BreezeIAP.GetPendingList();
        foreach (var item in pending)
        {
            Grant(item.Product.definition.id);
            BreezeIAP.Confirm(item);
        }
    }

    public async void OnBuyGoldClicked()
    {
        var result = await BreezeIAP.PurchaseAsync("gold_100");

        if (result.IsSuccess)
        {
            Grant(result.Product.definition.id);
            BreezeIAP.Confirm(result);
        }
    }

    private void Grant(string productId)
    {
        // Your item grant logic here
    }
}
```
