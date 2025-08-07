namespace Crud
{
    public static class Constants
    {
        public const string AdminEmail = "narbajajc@gmail.com";

        public static class Common
        {
            public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            public const string DateFormat = "yyyy-MM-dd";
            public const string LogFilePath = @"C:\LogPath";
        }

        public static class ExportEmployee
        {
            public const string Header = "Id,Name,Email,Phone,Salary,CreatedDate,DepartmentId";
            // Constant format string to be used with string.Format
            public const string RowFormat = "{0},{1},{2},{3},{4},{5},{6}";
        }

        public static class URL
        {
            public const string BaseUrl = "https://localhost:5001/";
            public const string Products = $"{BaseUrl}/products";
            public const string CreateCheckoutSession = "create-checkout-session";
            public const string CreateCheckout = "create-checkout";
        }

        public static class Subject
        {   
            public const string Refund = "Refund Processed";
            public const string Inquiry = "Inquiry";
        }

        public static string GetRefundEmailBody(
            string chargeId,
            string receiptUrl,
            string CustomerEmail,
            string CustomerName,
            string CardBrand,
            string Last4,
            string description,
            decimal amount,
            string currency)
        {
            return
                   $"<span>This email is to confirm that your refund has been issued by Nice2Strangers. It can take approximately 10 days to appear on your statement.If it takes longer please contact your bank for assistance.</span><br><br>" +
                   $"<span><strong>Charge ID:</strong> {chargeId}</span><br>" +
                   $"<span>Customer Name: {CustomerName}</span><br>" +
                   $"<span>Customer Email: {CustomerEmail}</span><br>" +
                   $"<span>Card Brand: {CardBrand}</span><br>" +
                   $"<span>Card Last 4 Digits: **** **** **** {Last4}</span><br>" +
                   $"<span><strong>Product:</strong> {description}</span><br>" +
                   $"<span><strong>Amount Refunded:</strong> {amount} {currency}</span><br><br>" +
                   $"<span><strong>View Receipt URL:</strong> {receiptUrl}</span><br>";
        }

        public static string ContactUs(
        string name,
        string message,
        string email
        )
        {
            return
                   $"<span>This email is to confirm that your refund has been issued by Nice2Strangers. It can take approximately 10 days to appear on your statement.If it takes longer please contact your bank for assistance.</span><br><br>" +
                   $"<span>Customer Name: {name}</span><br>" +
                   $"<span>Customer Email: {email}</span><br>" +
                   $"<span><strong>Product:</strong> {message}</span><br>" +
        }
    }
}
