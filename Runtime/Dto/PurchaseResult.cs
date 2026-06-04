using UnityEngine.Purchasing;

namespace Achieve.BreezeIAP
{
    public readonly struct PurchaseResult
    {
        /// <summary>
        /// 상품의 현재 상태
        /// </summary>
        public PurchaseType Type { get; init; }

        /// <summary>
        /// 시도 한 상품
        /// </summary>
        public Product Product { get; init; }

        /// <summary>
        /// Unity IAP v5의 주문 정보입니다. Confirm 시 이 주문을 확정합니다.
        /// </summary>
        public PendingOrder Order { get; init; }

        /// <summary>
        /// 에러 상세 메시지
        /// </summary>
        public string ErrorMessage { get; init; }

        /// <summary>
        /// 영수증 (JSON). 서버 검증 등에 사용합니다.
        /// </summary>
        public string Receipt => Order?.Info?.Receipt;

        /// <summary>
        /// 트랜잭션 ID
        /// </summary>
        public string TransactionId => Order?.Info?.TransactionID;

        /// <summary>
        /// Success == true => 결제 성공
        /// </summary>
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);

        public static PurchaseResult Error(string message)
        {
            return new PurchaseResult
            {
                Type = PurchaseType.Error,
                Product = null,
                Order = null,
                ErrorMessage = message
            };
        }
    }
}
