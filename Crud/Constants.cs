namespace Crud
{
    public static class Constants
    {
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

    }
}
