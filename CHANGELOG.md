# Breeze IAP

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