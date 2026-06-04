---
layout: home

hero:
  name: Breeze IAP
  text: Unity IAP를 async/await로
  tagline: Unity IAP v5.3.0+ 기반의 경량 결제 래퍼. 콜백 없이 한 줄의 await로 초기화·구매를 처리하세요.
  actions:
    - theme: brand
      text: 시작하기
      link: /getting-started
    - theme: alt
      text: API 레퍼런스
      link: /api/initialize
    - theme: alt
      text: GitHub
      link: https://github.com/achieveonepark/breeze-iap

features:
  - icon: ⚡
    title: 단순한 async/await API
    details: InitializeAsync 한 번, PurchaseAsync 한 번. IStoreListener 구현이나 콜백 체이닝 없이 결제 흐름을 작성할 수 있습니다.
  - icon: 🔄
    title: 자동 미확정 구매 처리
    details: 앱 충돌 등으로 확정되지 않은 구매를 초기화 시점에 자동으로 수집합니다. GetPendingList()로 한 번에 처리하세요.
  - icon: ⏱️
    title: 내장 타임아웃
    details: 초기화 단계별 10초, 구매 60초 타임아웃이 내장되어 있어 스토어 무응답 상황에서도 앱이 멈추지 않습니다.
  - icon: 🍎
    title: iOS 복원 지원
    details: BreezeIAP.Restore() 한 줄로 iOS 비소모성·구독 상품 복원을 처리합니다.
---

## 빠른 시작

```csharp
using Achieve.BreezeIAP;
using UnityEngine.Purchasing;

public class ShopManager : MonoBehaviour
{
    private async void Start()
    {
        // 1. 초기화
        await BreezeIAP.InitializeAsync(new[]
        {
            new InitializeDto { ProductId = "gold_100", ProductType = ProductType.Consumable },
            new InitializeDto { ProductId = "no_ads",   ProductType = ProductType.NonConsumable },
        });

        // 2. 이전 세션의 미확정 구매 처리
        var pending = BreezeIAP.GetPendingList();
        foreach (var item in pending)
        {
            Grant(item.Product.definition.id);
            BreezeIAP.Confirm(item);
        }
    }

    public async void OnBuyGold()
    {
        // 3. 구매
        var result = await BreezeIAP.PurchaseAsync("gold_100");
        if (result.IsSuccess)
        {
            Grant(result.Product.definition.id);
            BreezeIAP.Confirm(result);
        }
    }
}
```

## 설치

**Window → Package Manager → Add package from git URL** 에 아래 URL을 입력하세요.

```
https://github.com/achieveonepark/breeze-iap.git
```

`com.unity.purchasing 5.3.0` 의존성은 Unity가 자동으로 설치합니다.
