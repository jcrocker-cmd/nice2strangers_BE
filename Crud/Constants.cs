using Org.BouncyCastle.Pqc.Crypto.Lms;
using Stripe;
using System;
using System.Threading.Tasks;

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
            public const string ResetPassword = "Reset Your Password";
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
                   $"<span><strong>Customer Name:</strong> {name}</span><br>" +
                   $"<span><strong>Customer Email</strong> {email}</span><br>" +
                   $"<span><strong>Message:</strong> {message}</span><br>";
        }


        public static string ContactUsClient(
        string name
        )
        {
            return
                    $"<span> Hello, <strong>{name}</strong></span><br><br>" +
                    $"<span> Thank you for contacting Nice2Strangers!We've received your message and appreciate you reaching out to us.</span><br><br>" +
                    $"<span> Our customer service team has your inquiry and we'll get back to you right away with a response. <br><br>You can expect to hear from us within the next 2-4 hours during business hours.</span><br>" +
                    $"<span>If you have any urgent questions in the meantime, feel free to reply to this email or check our FAQ section on our website.</span><br><br>" +
                    $"<span>We look forward to assisting you!</span><br><br>" +
                    $"<span>Best regards,</span><br><br>" +
                    $"<span>Nice2Strangers Customer Service Team</span><br>" +
                    $"<span>Email: support@nice2strangers.com</span><br>" +
                    $"<span>Hours: Monday-Friday 8AM-6PM EST</span><br>";
        }

        public static string GetResetPasswordEmailBody(string resetLink, string firstNameOrEmail)
        {
            return $@"
            <p>Hello {firstNameOrEmail},</p>
            <p>You requested a password reset. Click the button below to set a new password:</p>
            <p>
                <a href='{resetLink}' 
                   style='padding: 10px 20px; background-color: #FBD241; color: #000; text-decoration: none; border-radius: 5px;'>
                   Reset Password
                </a>
            </p>
            <p>If you didn’t request this, you can ignore this email.</p>";
        }
    }
}
