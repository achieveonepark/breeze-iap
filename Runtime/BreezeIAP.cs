using System.Collections.Generic;
using UnityEngine.Purchasing;
using System.Threading.Tasks;
using System;
using UnityEngine;

namespace Achieve.BreezeIAP
{
    public class BreezeIAP
    {
        internal static IStoreController controller;
        internal static IExtensionProvider extensionProvider;

        internal static List<PurchaseResult> pendingList;
        internal static TaskCompletionSource<PurchaseResult> purchaseCompletionSource;
        internal static TaskCompletionSource<InitializeResult> initializeCompletionSource;

        internal static bool isCheckingPendingList;

        private static BreezeIAPReceiver _receiver;
        private static bool _isInitialized;

        /// <summary>
        /// 스토어에 등록 된 ProductID들을 기반으로 IAP를 Initialize합니다.
        /// </summary>
        /// <param name="dtos">IAP Initialize에 필요한 데이터</param>
        /// <param name="isDebug">Debug.Log를 찍을 것인지?</param>
        public static async Task InitializeAsync(InitializeDto[] dtos, bool isDebug = false)
        {
            if (_isInitialized) return;
            BreezeIAPLog.CurrentLogLevel = isDebug ? BreezeIAPLog.LogLevel.Debug : BreezeIAPLog.LogLevel.Info;

            _receiver = new BreezeIAPReceiver();
            pendingList = new List<PurchaseResult>();
            initializeCompletionSource = new TaskCompletionSource<InitializeResult>();

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            for (int i = 0; i < dtos.Length; i++)
            {
                var dto = dtos[i];
                builder.AddProduct(dto.ProductId, dto.ProductType);
            }

            isCheckingPendingList = true;
            UnityPurchasing.Initialize(_receiver, builder);

            var result = await initializeCompletionSource.Task.Timeout(TimeSpan.FromSeconds(10));
            _isInitialized = result.IsInitialized;

            if (!_isInitialized)
            {
                BreezeIAPLog.Warning("Initialize failed: No response after Initialize");
                return;
            }

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
            controller.InitiatePurchase(productId);

            var product = await purchaseCompletionSource.Task.Timeout(TimeSpan.FromSeconds(60));
            BreezeIAPLog.Info($"The payment for item [{productId}] was successful!");

            purchaseCompletionSource = null;

            return product;
        }

        /// <summary>
        /// 구매를 확정합니다. 아이템 지급 후 반드시 호출해주세요.
        /// </summary>
        public static void Confirm(PurchaseResult product)
        {
            controller.ConfirmPendingPurchase(product.Product);
            BreezeIAPLog.Info($"I confirmed product [{product.Product.definition.id}].");
        }

        /// <summary>
        /// 구매를 확정합니다. 아이템 지급 후 반드시 호출해주세요.
        /// </summary>
        public static void Confirm(Product product)
        {
            controller.ConfirmPendingPurchase(product);
            BreezeIAPLog.Info($"I confirmed product [{product.definition.id}].");
        }

        /// <summary>
        /// 구매한 상품을 복원합니다. (Apple 전용, 소모성 상품 제외)
        /// </summary>
        public static void Restore()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                var apple = extensionProvider.GetExtension<IAppleExtensions>();

                apple.RestoreTransactions((result, reason) =>
                {
                    BreezeIAPLog.Info(result ? "Restore successful!" : $"Restore failed...{reason}");
                });
            }
        }

        internal static void AddPendingList(PurchaseResult product)
        {
            if (isCheckingPendingList)
            {
                pendingList.Add(product);
            }
        }
    }
}
