# Breeze IAP

## 3.0.0 2026.06.04
### Breaking Changes
- Unity IAP **v5 스토어 API**로 마이그레이션. `com.unity.purchasing` **5.3.0 이상** 필요
- 레거시 `IStoreController` / `IExtensionProvider` / `IStoreListener` 모델 제거 → `UnityIAPServices.StoreController()` 및 이벤트 기반 API(`OnPurchasePending`, `OnPurchaseConfirmed`, `OnProductsFetched`, `OnPurchasesFetched` 등) 사용
- 초기화 흐름이 `ConfigurationBuilder` + `UnityPurchasing.Initialize` → `Connect()` → `FetchProducts()` → `FetchPurchases()` 로 변경
- 구매 확정이 주문(Order) 기준으로 변경: `Confirm(Product)` 제거, `Confirm(PendingOrder)` 추가 (`Confirm(PurchaseResult)`는 유지)
- 내부 타입 `InitializeResult` 제거

### Added
- `PurchaseResult.Order`(`PendingOrder`)와 `Receipt` / `TransactionId` 접근자 추가
- `PurchaseType.Deferred` 추가 (Ask to Buy 등 외부 승인 대기)
- `StoreController.Connect()` await용 비제네릭 `Task.Timeout` 오버로드 추가

### Notes
- 상위 API(`InitializeAsync`, `PurchaseAsync`, `Confirm(PurchaseResult)`, `GetPendingList`, `Restore`)는 2.0.0과 소스 호환

## 2.0.0 2026.05.25
### Breaking Changes
- Unity In App Purchasing **5.0.0 이상**만 지원 (v4 이하 지원 종료)
- `OnInitializeFailed(InitializationFailureReason)` 오버로드 제거 (v5 API에 맞춰 message 파라미터 포함 버전만 유지)
- `OnPurchaseFailed(Product, PurchaseFailureReason)` 오버로드 제거 (v5 API인 `PurchaseFailureDescription`만 유지)

### Bug Fixes
- `initializeCompletionSource` 미초기화로 인한 NullReferenceException 수정
- `GetPendingList()`, `PurchaseAsync()` 의 `_isInitialized` 조건 반전 버그 수정
- `PurchaseAsync()` 초기화 실패 시 return 누락 수정
- `List<InitializeDto>` 오버로드의 불필요한 더미 상품 추가 코드 제거

## 🎉 1.0.0 Release! 2024.09.17
- Package release
- Adding Unity IAP basic features
- [tech-docs](https://achieveonepark.github.io/cording-library/Documents/BreezeIAP/BreezeIAP)