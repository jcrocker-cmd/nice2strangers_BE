namespace Crud.ViewModel
{
    public class TransactionSummaryViewModel
    {
        public int All { get; set; }
        public int Succeeded { get; set; }
        public int Refunded { get; set; }
        public int Disputed { get; set; }
        public int Failed { get; set; }
        public int Uncaptured { get; set; }
    }

}
