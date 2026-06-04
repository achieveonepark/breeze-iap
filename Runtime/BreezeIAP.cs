using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Achieve.BreezeIAP
{
    /// <summary>
    /// Unity IAP v5.3.0 기반의 결제 래퍼입니다.
    /// </summary>
    public class BreezeIAP
    {
        internal static StoreController controller;

        internal static List<PurchaseResult> pendingList;
        internal static TaskCompletionSource<PurchaseResult> purchaseCompletionSource;
        internal static TaskCompletionSource<bool> productsFetchSource;
        internal static TaskCompletionSource<bool> purchasesFetchSource;

        internal static bool isCheckingPendingList;

        private static BreezeIAPReceiver _receiver;
        private static bool _isInitialized;

        private const int ConnectTimeoutSeconds = 10;
        private const int FetchTimeoutSeconds = 10;
        private const int PurchaseTimeoutSeconds = 60;

        /// <summary>
        /// 스토어에 등록 된 ProductID들을 기반으로 IAP를 Initialize합니다.
        /// </summary>
        /// <param name="dtos">IAP Initialize에 필요한 데이터</param>
        /// <param name="isDebug">Debug.Log를 찍을 것인지?</param>
        public static async Task InitializeAsync(InitializeDto[] dtos, bool isDebug = false)
        {
            if (_isInitialized) return;
            BreezeIAPLog.CurrentLogLevel = isDebug ? BreezeIAPLog.LogLevel.Debug : BreezeIAPLog.LogLevel.Info;

            pendingList = new List<PurchaseResult>();
            isCheckingPendingList = true;

            controller = UnityIAPServices.StoreController();
            _receiver = new BreezeIAPReceiver();
            _receiver.Subscribe(controller);

            // 미확정 주문은 FetchPurchases 결과(OnPurchasesFetched)에서 직접 수집합니다.
            controller.ProcessPendingOrdersOnPurchasesFetched(false);

            // 1) 스토어 연결
            try
            {
                await controller.Connect().Timeout(TimeSpan.FromSeconds(ConnectTimeoutSeconds));
            }
            catch (Exception e)
            {
                BreezeIAPLog.Warning($"Initialize failed: Could not connect to store. {e.Message}");
                return;
            }

            // 2) 상품 정보 조회
            productsFetchSource = new TaskCompletionSource<bool>();

            var definitions = new List<ProductDefinition>(dtos.Length);
            for (int i = 0; i < dtos.Length; i++)
            {
                var dto = dtos[i];
                definitions.Add(new ProductDefinition(dto.ProductId, dto.ProductType));
            }

            controller.FetchProducts(definitions);

            bool productsFetched;
            try
            {
                productsFetched = await productsFetchSource.Task.Timeout(TimeSpan.FromSeconds(FetchTimeoutSeconds));
            }
            catch (TimeoutException)
            {
                BreezeIAPLog.Warning("Initialize failed: No response after FetchProducts");
                return;
            }
            finally
            {
                productsFetchSource = null;
            }

            if (!productsFetched)
            {
                BreezeIAPLog.Warning("Initialize failed: FetchProducts failed");
                return;
            }

            // 3) 기존 구매(미확정 주문) 조회
            purchasesFetchSource = new TaskCompletionSource<bool>();
            controller.FetchPurchases();

            try
            {
                await purchasesFetchSource.Task.Timeout(TimeSpan.FromSeconds(FetchTimeoutSeconds));
            }
            catch (TimeoutException)
            {
                BreezeIAPLog.Warning("FetchPurchases timed out. Continuing without pending orders.");
            }
            finally
            {
                purchasesFetchSource = null;
            }

            _isInitialized = true;
            BreezeIAPLog.Info("Initialize Successful!");
        }

        /// <summary>
        /// 스토어에 등록 된 ProductID들을 기반으로 IAP를 Initialize합니다.
        /// </summary>
        /// <param name="dtos">IAP Initialize에 필요한 데이터</param>
        /// <param name="isDebug">Debug.Log를 찍을 것인지?</param>
        public static async Task InitializeAsync(List<InitializeDto> dtos, bool isDebug = false)
        {
            await InitializeAsync(dtos.ToArray(), isDebug);
        }

        /// <summary>
        /// Initialize 후에 해당 메소드를 꼭 호출하여 Pending 상품에 대한 처리를 진행합니다.
        /// </summary>
        public static List<PurchaseResult> GetPendingList()
        {
            if (!_isInitialized)
            {
                BreezeIAPLog.Warning($"It is not initialized. Call method : {nameof(GetPendingList)}");
                return null;
            }

            BreezeIAPLog.Debug("Get PendingList...");
            isCheckingPendingList = false;
            return pendingList;
        }

        /// <summary>
        /// 스토어에 등록 된 ProductId를 입력하여 상품 구매를 시도합니다.
        /// </summary>
        /// <param name="productId"></param>
        public static async Task<PurchaseResult> PurchaseAsync(string productId)
        {
            if (!_isInitialized)
            {
                BreezeIAPLog.Warning($"It is not initialized. Call method : {nameof(PurchaseAsync)}");
                return PurchaseResult.Error("초기화 실패");
            }

            purchaseCompletionSource = new TaskCompletionSource<PurchaseResult>();

            BreezeIAPLog.Info($"Attempt to pay for product [{productId}]...");
            controller.PurchaseProduct(productId);

            try
            {
                var result = await purchaseCompletionSource.Task.Timeout(TimeSpan.FromSeconds(PurchaseTimeoutSeconds));

                if (result.IsSuccess)
                {
                    BreezeIAPLog.Info($"The payment for item [{productId}] was successful!");
                }

                return result;
            }
            catch (TimeoutException)
            {
                BreezeIAPLog.Warning($"The payment for item [{productId}] timed out.");
                return PurchaseResult.Error("결제 시간 초과");
            }
            finally
            {
                purchaseCompletionSource = null;
            }
        }

        /// <summary>
        /// 구매를 확정합니다. 아이템 지급 후 반드시 호출해주세요.
        /// </summary>
        public static void Confirm(PurchaseResult result)
        {
            if (result.Order == null)
            {
                BreezeIAPLog.Warning($"There is no order to confirm. Call method : {nameof(Confirm)}");
                return;
            }

            controller.ConfirmPurchase(result.Order);
            BreezeIAPLog.Info($"I confirmed product [{GetProductId(result.Order)}].");
        }

        /// <summary>
        /// 미확정 주문(PendingOrder)을 확정합니다. 아이템 지급 후 반드시 호출해주세요.
        /// </summary>
        public static void Confirm(PendingOrder order)
        {
            if (order == null)
            {
                BreezeIAPLog.Warning($"There is no order to confirm. Call method : {nameof(Confirm)}");
                return;
            }

            controller.ConfirmPurchase(order);
            BreezeIAPLog.Info($"I confirmed product [{GetProductId(order)}].");
        }

        /// <summary>
        /// 구매한 상품을 복원합니다. (Apple 전용, 소모성 상품 제외)
        /// </summary>
        public static void Restore()
        {
            if (!_isInitialized)
            {
                BreezeIAPLog.Warning($"It is not initialized. Call method : {nameof(Restore)}");
                return;
            }

            controller.RestoreTransactions((success, error) =>
            {
                BreezeIAPLog.Info(success ? "Restore successful!" : $"Restore failed...{error}");
            });
        }

        internal static void AddPendingList(PurchaseResult product)
        {
            if (isCheckingPendingList)
            {
                pendingList.Add(product);
            }
        }

        /// <summary>
        /// 주문(Order)의 장바구니에서 첫 번째 상품을 가져옵니다.
        /// </summary>
        internal static Product GetProduct(Order order)
        {
            if (order?.CartOrdered == null) return null;

            foreach (var item in order.CartOrdered.Items())
            {
                return item.Product;
            }

            return null;
        }

        internal static string GetProductId(Order order)
        {
            return GetProduct(order)?.definition?.id ?? "unknown";
        }
    }
}
