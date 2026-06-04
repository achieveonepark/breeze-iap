using System.Collections.Generic;
using UnityEngine.Purchasing;

namespace Achieve.BreezeIAP
{
    /// <summary>
    /// Unity IAP v5의 <see cref="StoreController"/> 이벤트를 구독하여
    /// BreezeIAP의 상태와 TaskCompletionSource로 연결합니다.
    /// </summary>
    internal class BreezeIAPReceiver
    {
        private StoreController _controller;

        public void Subscribe(StoreController controller)
        {
            _controller = controller;

            controller.OnStoreConnected += OnStoreConnected;
            controller.OnStoreDisconnected += OnStoreDisconnected;

            controller.OnProductsFetched += OnProductsFetched;
            controller.OnProductsFetchFailed += OnProductsFetchFailed;

            controller.OnPurchasePending += OnPurchasePending;
            controller.OnPurchaseConfirmed += OnPurchaseConfirmed;
            controller.OnPurchaseFailed += OnPurchaseFailed;
            controller.OnPurchaseDeferred += OnPurchaseDeferred;

            controller.OnPurchasesFetched += OnPurchasesFetched;
            controller.OnPurchasesFetchFailed += OnPurchasesFetchFailed;
        }

        public void Unsubscribe()
        {
            if (_controller == null) return;

            _controller.OnStoreConnected -= OnStoreConnected;
            _controller.OnStoreDisconnected -= OnStoreDisconnected;

            _controller.OnProductsFetched -= OnProductsFetched;
            _controller.OnProductsFetchFailed -= OnProductsFetchFailed;

            _controller.OnPurchasePending -= OnPurchasePending;
            _controller.OnPurchaseConfirmed -= OnPurchaseConfirmed;
            _controller.OnPurchaseFailed -= OnPurchaseFailed;
            _controller.OnPurchaseDeferred -= OnPurchaseDeferred;

            _controller.OnPurchasesFetched -= OnPurchasesFetched;
            _controller.OnPurchasesFetchFailed -= OnPurchasesFetchFailed;

            _controller = null;
        }

        private void OnStoreConnected()
        {
            BreezeIAPLog.Debug("Store connected.");
        }

        private void OnStoreDisconnected(StoreConnectionFailureDescription description)
        {
            BreezeIAPLog.Warning($"Store disconnected: {description.message}");
        }

        private void OnProductsFetched(List<Product> products)
        {
            BreezeIAPLog.Debug($"{products.Count} product(s) fetched.");
            BreezeIAP.productsFetchSource?.TrySetResult(true);
        }

        private void OnProductsFetchFailed(ProductFetchFailed failed)
        {
            BreezeIAPLog.Warning($"Products fetch failed: {failed.FailureReason}");
            BreezeIAP.productsFetchSource?.TrySetResult(false);
        }

        /// <summary>
        /// 결제가 완료되어 확정(Confirm) 대기 상태가 된 주문입니다.
        /// 진행 중인 구매 요청이 있으면 해당 요청에, 없으면 보류 목록에 추가합니다.
        /// </summary>
        private void OnPurchasePending(PendingOrder order)
        {
            Product product = BreezeIAP.GetProduct(order);

            if (BreezeIAP.purchaseCompletionSource != null)
            {
                BreezeIAPLog.Info($"Purchase pending for [{BreezeIAP.GetProductId(order)}].");

                var result = new PurchaseResult
                {
                    Type = PurchaseType.Purchase,
                    Product = product,
                    Order = order,
                    ErrorMessage = string.Empty
                };

                BreezeIAP.purchaseCompletionSource.TrySetResult(result);
                return;
            }

            BreezeIAPLog.Debug($"Pending order received for [{BreezeIAP.GetProductId(order)}].");

            BreezeIAP.AddPendingList(new PurchaseResult
            {
                Type = PurchaseType.Pending,
                Product = product,
                Order = order,
                ErrorMessage = string.Empty
            });
        }

        private void OnPurchaseConfirmed(Order order)
        {
            BreezeIAPLog.Info($"Purchase confirmed for [{BreezeIAP.GetProductId(order)}].");
        }

        private void OnPurchaseFailed(FailedOrder order)
        {
            string message = $"{order.FailureReason}: {order.Details}";
            BreezeIAPLog.Warning($"Purchase failed for [{BreezeIAP.GetProductId(order)}] - {message}");

            BreezeIAP.purchaseCompletionSource?.TrySetResult(PurchaseResult.Error(message));
        }

        private void OnPurchaseDeferred(DeferredOrder order)
        {
            BreezeIAPLog.Info($"Purchase deferred for [{BreezeIAP.GetProductId(order)}].");

            BreezeIAP.purchaseCompletionSource?.TrySetResult(new PurchaseResult
            {
                Type = PurchaseType.Deferred,
                Product = BreezeIAP.GetProduct(order),
                Order = null,
                ErrorMessage = string.Empty
            });
        }

        /// <summary>
        /// FetchPurchases 결과입니다. 미확정(Pending) 주문을 보류 목록에 채웁니다.
        /// </summary>
        private void OnPurchasesFetched(Orders orders)
        {
            BreezeIAPLog.Debug($"Purchases fetched. pending={orders.PendingOrders.Count}, deferred={orders.DeferredOrders.Count}, confirmed={orders.ConfirmedOrders.Count}");

            foreach (var order in orders.PendingOrders)
            {
                BreezeIAP.AddPendingList(new PurchaseResult
                {
                    Type = PurchaseType.Pending,
                    Product = BreezeIAP.GetProduct(order),
                    Order = order,
                    ErrorMessage = string.Empty
                });
            }

            BreezeIAP.purchasesFetchSource?.TrySetResult(true);
        }

        private void OnPurchasesFetchFailed(PurchasesFetchFailureDescription description)
        {
            BreezeIAPLog.Warning($"Purchases fetch failed: {description.FailureReason} - {description.Message}");
            BreezeIAP.purchasesFetchSource?.TrySetResult(false);
        }
    }
}
