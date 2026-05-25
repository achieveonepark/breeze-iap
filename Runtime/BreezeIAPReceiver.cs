using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Achieve.BreezeIAP
{
    internal class BreezeIAPReceiver : IDetailedStoreListener
    {
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            BreezeIAP.controller = controller;
            BreezeIAP.extensionProvider = extensions;
            BreezeIAP.initializeCompletionSource.TrySetResult(InitializeResult.Success());
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            BreezeIAP.initializeCompletionSource.TrySetResult(InitializeResult.Error($"{error}: {message}"));
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            PurchaseResult purchaseResult = new PurchaseResult
            {
                Type = PurchaseType.Error,
                Product = product,
                ErrorMessage = $"{failureDescription.reason}: {failureDescription.message}"
            };

            BreezeIAP.purchaseCompletionSource?.TrySetResult(purchaseResult);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            PurchaseType type = BreezeIAP.isCheckingPendingList ? PurchaseType.Pending : PurchaseType.Purchase;

            PurchaseResult purchaseResult = new PurchaseResult
            {
                Type = type,
                Product = purchaseEvent.purchasedProduct,
                ErrorMessage = string.Empty
            };

            BreezeIAP.AddPendingList(purchaseResult);
            BreezeIAP.purchaseCompletionSource?.TrySetResult(purchaseResult);

            return PurchaseProcessingResult.Pending;
        }
    }
}
