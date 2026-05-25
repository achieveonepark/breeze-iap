# Breeze IAP

**Breeze IAP** is a lightweight Unity wrapper around [Unity In-App Purchasing](https://docs.unity3d.com/Packages/com.unity.purchasing@5.0/manual/index.html) (v5.0.0+) that exposes a clean `async/await` API, so you can handle purchases without callbacks or boilerplate.

> **Requires** Unity 2022.3+ · Unity IAP 5.0.0+

---

## Features

- Single `await` call for initialization and purchasing
- Automatic pending-purchase queue on startup
- Built-in 10 s init timeout / 60 s purchase timeout
- One-line restore for iOS

## Quick Start

```csharp
using Achieve.BreezeIAP;

// 1. Initialize
await BreezeIAP.InitializeAsync(new[]
{
    new InitializeDto { ProductId = "gold_100",  ProductType = ProductType.Consumable },
    new InitializeDto { ProductId = "no_ads",    ProductType = ProductType.NonConsumable },
});

// 2. Handle pending purchases from a previous session
var pending = BreezeIAP.GetPendingList();
foreach (var item in pending)
{
    GiveItem(item.Product.definition.id);
    BreezeIAP.Confirm(item);
}

// 3. Purchase
var result = await BreezeIAP.PurchaseAsync("gold_100");
if (result.IsSuccess)
{
    GiveItem(result.Product.definition.id);
    BreezeIAP.Confirm(result);
}
```
