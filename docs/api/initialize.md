# InitializeAsync

Connects to the store and registers your product catalogue. Must be called before any other Breeze IAP method.

## Signatures

```csharp
public static async Task InitializeAsync(InitializeDto[] dtos, bool isDebug = false)
public static async Task InitializeAsync(List<InitializeDto> dtos, bool isDebug = false)
```

## Parameters

| Parameter | Type | Description |
|---|---|---|
| `dtos` | `InitializeDto[]` \| `List<InitializeDto>` | Product definitions to register with the store. |
| `isDebug` | `bool` | When `true`, prints verbose logs to the Unity console. Default `false`. |

## Behavior

- Calls are idempotent — subsequent calls while already initialized are silently ignored.
- Each step (connect / fetch products / fetch purchases) has a **10-second timeout**. If the store doesn't respond within that window, initialization is considered failed and a warning is logged.
- Internally runs the Unity IAP v5 sequence: `UnityIAPServices.StoreController()` → `Connect()` → `FetchProducts(List<ProductDefinition>)` → `FetchPurchases()`. Each `InitializeDto` is mapped to a `ProductDefinition`.

## Example

```csharp
await BreezeIAP.InitializeAsync(new[]
{
    new InitializeDto { ProductId = "gold_100",  ProductType = ProductType.Consumable },
    new InitializeDto { ProductId = "no_ads",    ProductType = ProductType.NonConsumable },
    new InitializeDto { ProductId = "vip_month", ProductType = ProductType.Subscription },
}, isDebug: true);
```

## Related

- [`InitializeDto`](types.md#initializedto)
- [`GetPendingList`](pending.md) — call immediately after a successful init
