namespace Crud.ViewModel
{
    public class TransactionViewModel
    {
        public string Id { get; set; }
        public long Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string Refunded { get; set; }
        public string Cancel { get; set; }
        public string RefundedDate { get; set; }
        public string Created { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public string CardBrand { get; set; }
        public string Last4 { get; set; }
        public string DeclineReason { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }

    }

}
