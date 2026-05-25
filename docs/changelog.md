# Changelog

## 2.0.0 — 2026-05-25

### Breaking Changes

- **Unity IAP 5.0.0+ required.** v4 and below are no longer supported.
- Removed `OnInitializeFailed(InitializationFailureReason)` overload (no-message variant removed in IAP v5).
- Removed `OnPurchaseFailed(Product, PurchaseFailureReason)` overload (replaced by `PurchaseFailureDescription` in IAP v5).

### Bug Fixes

- Fixed `NullReferenceException` caused by `initializeCompletionSource` never being initialized before `UnityPurchasing.Initialize` was called.
- Fixed inverted `_isInitialized` guard in `GetPendingList` and `PurchaseAsync` — both methods were blocking when initialized and allowing calls when not.
- Fixed missing `return` in `PurchaseAsync` after an early-exit error result.
- Removed dead code in the `List<InitializeDto>` overload (a dummy product was added after `ToArray()` was called, so it had no effect).

---

## 1.0.0 — 2024-09-17

- Initial release.
- Async/await wrapper for Unity IAP with pending-purchase queue, timeout handling, and iOS restore support.
