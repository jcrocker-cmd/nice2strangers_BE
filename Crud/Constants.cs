using Org.BouncyCastle.Pqc.Crypto.Lms;
using Stripe;
using System;
using System.Threading.Tasks;

namespace Crud
{
    public static class Constants
    {
        public const string AdminEmail = "narbajajc@gmail.com";

        public static class Services
        {
            public const string DroneSubject = "Drone Service Inquiry";
            public const string SMConsulting = "Social Media Consulting Inquiry";
            public const string SMCreation = "Social Media Creation Inquiry";
            public const string VideoEditing = "Video Editing Inquiry";
            public const string Software = "Software Creation Inquiry";
        }

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
            //public const string BaseUrl = "https://localhost:5001/";
            //public const string Products = $"{BaseUrl}/products";
            //public const string CreateCheckoutSession = "create-checkout-session";
            //public const string CreateCheckout = "create-checkout";
            public const string FailedOrder = "http://localhost:5173/failed-order";
            public const string SuccessOrder = "http://localhost:5173/success-order";
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


        public static string AdminServicesEmailTemplate(
        string customerName,
        string customerEmail,
        string serviceType,
        string location,
        string budget,
        string date,
        string message)
        {
            return
                $"<p>Hey Brandon,</p>" +
                $"<p><strong>{customerName}</strong> just inquired about <strong>{Constants.Services.DroneSubject}</strong> through the website.</p>" +
                $"<p>Here are the details of the request:</p>" +
                $"<ul>" +
                    $"<li><strong>Name:</strong> {customerName}</li>" +
                    $"<li><strong>Email:</strong> {customerEmail}</li>" +
                    $"<li><strong>Service Type:</strong> {serviceType}</li>" +
                    $"<li><strong>Location:</strong> {location}</li>" +
                    $"<li><strong>Budget:</strong> {budget}</li>" +
                    $"<li><strong>Date:</strong> {date}</li>" +
                    $"<li><strong>Message:</strong> {message}</li>" +
                $"</ul>" +
                $"<p>Please follow up with {customerName} at <a href=\"mailto:{customerEmail}\">{customerEmail}</a> when you can.</p>" +
                $"<p>– nice2strangers.org</p>";
        }


        public static string ClientServicesEmailTemplate(
        string name,
        string serviceType,
        string location,
        string budget,
        string date,
        string message)
            {
                return
                    $"<span>Hello, <strong>{name}</strong></span><br><br>" +
                    $"<span>Thank you for contacting Nice2Strangers! We've received your request and appreciate you reaching out.</span><br><br>" +
                    $"<span>Here’s a summary of your request:</span><br>" +
                    $"<ul>" +
                    $"<li><strong>Service Type:</strong> {serviceType}</li>" +
                    $"<li><strong>Location:</strong> {location}</li>" +
                    $"<li><strong>Budget:</strong> {budget}</li>" +
                    $"<li><strong>Date:</strong> {date}</li>" +
                    $"<li><strong>Message:</strong> {message}</li>" +
                    $"</ul><br>" +
                    $"<span>Our customer service team has received your inquiry and we'll get back to you right away with a response.</span><br>" +
                    $"<span>You can expect to hear from us within the next 2-4 hours during business hours.</span><br><br>" +
                    $"<span>If you have urgent questions in the meantime, feel free to reply to this email or check our FAQ section on our website.</span><br><br>" +
                    $"<span>We look forward to assisting you!</span><br><br>" +
                    $"<span>Best regards,</span><br>" +
                    $"<span>Nice2Strangers Customer Service Team</span><br>" +
                    $"<span>Email: support@nice2strangers.org</span><br>" +
                    $"<span>Hours: Monday-Friday 8AM-6PM EST</span><br>";
            }



        public static string AdminSMConsultingEmailTemplate(
        string customerName,
        string customerEmail,
        string platforms,
        string goals,
        string budget,
        string duration,
        string message)
        {
            return
                $"<p>Hey Brandon,</p>" +
                $"<p><strong>{customerName}</strong> just inquired about <strong>{Constants.Services.SMConsulting}</strong> through the website.</p>" +
                $"<p>Here are the details of the request:</p>" +
                $"<ul>" +
                    $"<li><strong>Name:</strong> {customerName}</li>" +
                    $"<li><strong>Email:</strong> {customerEmail}</li>" +
                    $"<li><strong>Plaforms:</strong> {platforms}</li>" +
                    $"<li><strong>Goals:</strong> {goals}</li>" +
                    $"<li><strong>Budget:</strong> {budget}</li>" +
                    $"<li><strong>Duration:</strong> {duration}</li>" +
                    $"<li><strong>Message:</strong> {message}</li>" +
                $"</ul>" +
                $"<p>Please follow up with {customerName} at <a href=\"mailto:{customerEmail}\">{customerEmail}</a> when you can.</p>" +
                $"<p>– nice2strangers.org</p>";
        }


        public static string ClientSMConsultingEmailTemplate(
        string name,
        string platforms,
        string goals,
        string budget,
        string duration,
        string message)
        {
            return
                $"<span>Hello, <strong>{name}</strong></span><br><br>" +
                $"<span>Thank you for contacting Nice2Strangers! We've received your request and appreciate you reaching out.</span><br><br>" +
                $"<span>Here’s a summary of your request:</span><br>" +
                $"<ul>" +
                $"<li><strong>Platforms:</strong> {platforms}</li>" +
                $"<li><strong>Goals:</strong> {goals}</li>" +
                $"<li><strong>Budget:</strong> {budget}</li>" +
                $"<li><strong>Duration:</strong> {duration}</li>" +
                $"<li><strong>Message:</strong> {message}</li>" +
                $"</ul><br>" +
                $"<span>Our customer service team has received your inquiry and we'll get back to you right away with a response.</span><br>" +
                $"<span>You can expect to hear from us within the next 2-4 hours during business hours.</span><br><br>" +
                $"<span>If you have urgent questions in the meantime, feel free to reply to this email or check our FAQ section on our website.</span><br><br>" +
                $"<span>We look forward to assisting you!</span><br><br>" +
                $"<span>Best regards,</span><br>" +
                $"<span>Nice2Strangers Customer Service Team</span><br>" +
                $"<span>Email: support@nice2strangers.org</span><br>" +
                $"<span>Hours: Monday-Friday 8AM-6PM EST</span><br>";
        }

        public static string AdminSMCreationEmailTemplate(
        string customerName,
        string customerEmail,
        string platforms,
        string contentType,
        string freq,
        string budget,
        string duration,
        string message)
        {
            return
                $"<p>Hey Brandon,</p>" +
                $"<p><strong>{customerName}</strong> just inquired about <strong>{Constants.Services.SMConsulting}</strong> through the website.</p>" +
                $"<p>Here are the details of the request:</p>" +
                $"<ul>" +
                    $"<li><strong>Name:</strong> {customerName}</li>" +
                    $"<li><strong>Email:</strong> {customerEmail}</li>" +
                    $"<li><strong>Plaforms:</strong> {platforms}</li>" +
                    $"<li><strong>Content Type:</strong> {contentType}</li>" +
                    $"<li><strong>Freq:</strong> {freq}</li>" +
                    $"<li><strong>Budget:</strong> {budget}</li>" +
                    $"<li><strong>Duration:</strong> {duration}</li>" +
                    $"<li><strong>Message:</strong> {message}</li>" +
                $"</ul>" +
                $"<p>Please follow up with {customerName} at <a href=\"mailto:{customerEmail}\">{customerEmail}</a> when you can.</p>" +
                $"<p>– nice2strangers.org</p>";
        }


        public static string ClientSMCreationEmailTemplate(
        string name,
        string platforms,
        string contentType,
        string freq,
        string budget,
        string duration,
        string message)
        {
            return
                $"<span>Hello, <strong>{name}</strong></span><br><br>" +
                $"<span>Thank you for contacting Nice2Strangers! We've received your request and appreciate you reaching out.</span><br><br>" +
                $"<span>Here’s a summary of your request:</span><br>" +
                $"<ul>" +
                $"<li><strong>Platforms:</strong> {platforms}</li>" +
                $"<li><strong>Content Type:</strong> {contentType}</li>" +
                $"<li><strong>Frequency:</strong> {freq}</li>" +
                $"<li><strong>Budget:</strong> {budget}</li>" +
                $"<li><strong>Duration:</strong> {duration}</li>" +
                $"<li><strong>Message:</strong> {message}</li>" +
                $"</ul><br>" +
                $"<span>Our customer service team has received your inquiry and we'll get back to you right away with a response.</span><br>" +
                $"<span>You can expect to hear from us within the next 2-4 hours during business hours.</span><br><br>" +
                $"<span>If you have urgent questions in the meantime, feel free to reply to this email or check our FAQ section on our website.</span><br><br>" +
                $"<span>We look forward to assisting you!</span><br><br>" +
                $"<span>Best regards,</span><br>" +
                $"<span>Nice2Strangers Customer Service Team</span><br>" +
                $"<span>Email: support@nice2strangers.org</span><br>" +
                $"<span>Hours: Monday-Friday 8AM-6PM EST</span><br>";
        }


        public static string AdminVideoEditingEmailTemplate(
        string customerName,
        string customerEmail,
        string platforms,
        string contentType,
        string otherCT,
        string budget,
        string duration,
        string message)
        {
            return
                $"<p>Hey Brandon,</p>" +
                $"<p><strong>{customerName}</strong> just inquired about <strong>{Constants.Services.VideoEditing}</strong> through the website.</p>" +
                $"<p>Here are the details of the request:</p>" +
                $"<ul>" +
                    $"<li><strong>Name:</strong> {customerName}</li>" +
                    $"<li><strong>Email:</strong> {customerEmail}</li>" +
                    $"<li><strong>Plaforms:</strong> {platforms}</li>" +
                    $"<li><strong>Content Type:</strong> {contentType}</li>" +
                    $"<li><strong>Other Content Type:</strong> {otherCT}</li>" +
                    $"<li><strong>Budget:</strong> {budget}</li>" +
                    $"<li><strong>Duration:</strong> {duration}</li>" +
                    $"<li><strong>Message:</strong> {message}</li>" +
                $"</ul>" +
                $"<p>Please follow up with {customerName} at <a href=\"mailto:{customerEmail}\">{customerEmail}</a> when you can.</p>" +
                $"<p>– nice2strangers.org</p>";
        }


        public static string ClientVideoEditingEmailTemplate(
        string name,
        string platforms,
        string contentType,
        string otherCT,
        string budget,
        string duration,
        string message)
        {
            return
                $"<span>Hello, <strong>{name}</strong></span><br><br>" +
                $"<span>Thank you for contacting Nice2Strangers! We've received your request and appreciate you reaching out.</span><br><br>" +
                $"<span>Here’s a summary of your request:</span><br>" +
                $"<ul>" +
                $"<li><strong>Platforms:</strong> {platforms}</li>" +
                $"<li><strong>Content Type:</strong> {contentType}</li>" +
                $"<li><strong>Other Content Type:</strong> {otherCT}</li>" +
                $"<li><strong>Budget:</strong> {budget}</li>" +
                $"<li><strong>Duration:</strong> {duration}</li>" +
                $"<li><strong>Message:</strong> {message}</li>" +
                $"</ul><br>" +
                $"<span>Our customer service team has received your inquiry and we'll get back to you right away with a response.</span><br>" +
                $"<span>You can expect to hear from us within the next 2-4 hours during business hours.</span><br><br>" +
                $"<span>If you have urgent questions in the meantime, feel free to reply to this email or check our FAQ section on our website.</span><br><br>" +
                $"<span>We look forward to assisting you!</span><br><br>" +
                $"<span>Best regards,</span><br>" +
                $"<span>Nice2Strangers Customer Service Team</span><br>" +
                $"<span>Email: support@nice2strangers.org</span><br>" +
                $"<span>Hours: Monday-Friday 8AM-6PM EST</span><br>";
        }


        public static string AdminSoftwareEmailTemplate(
        string customerName,
        string customerEmail,
        string platforms,
        string budget,
        string timeline,
        string message)
        {
            return
                $"<p>Hey Brandon,</p>" +
                $"<p><strong>{customerName}</strong> just inquired about <strong>{Constants.Services.Software}</strong> through the website.</p>" +
                $"<p>Here are the details of the request:</p>" +
                $"<ul>" +
                    $"<li><strong>Name:</strong> {customerName}</li>" +
                    $"<li><strong>Email:</strong> {customerEmail}</li>" +
                    $"<li><strong>Plaforms:</strong> {platforms}</li>" +
                    $"<li><strong>Budget:</strong> {budget}</li>" +
                    $"<li><strong>Duration:</strong> {timeline}</li>" +
                    $"<li><strong>Message:</strong> {message}</li>" +
                $"</ul>" +
                $"<p>Please follow up with {customerName} at <a href=\"mailto:{customerEmail}\">{customerEmail}</a> when you can.</p>" +
                $"<p>– nice2strangers.org</p>";
        }


        public static string ClientSoftwareEmailTemplate(
        string name,
        string platforms,
        string budget,
        string timeline,
        string message)
        {
            return
                $"<span>Hello, <strong>{name}</strong></span><br><br>" +
                $"<span>Thank you for contacting Nice2Strangers! We've received your request and appreciate you reaching out.</span><br><br>" +
                $"<span>Here’s a summary of your request:</span><br>" +
                $"<ul>" +
                $"<li><strong>Platforms:</strong> {platforms}</li>" +
                $"<li><strong>Budget:</strong> {budget}</li>" +
                $"<li><strong>Timeline:</strong> {timeline}</li>" +
                $"<li><strong>Message:</strong> {message}</li>" +
                $"</ul><br>" +
                $"<span>Our customer service team has received your inquiry and we'll get back to you right away with a response.</span><br>" +
                $"<span>You can expect to hear from us within the next 2-4 hours during business hours.</span><br><br>" +
                $"<span>If you have urgent questions in the meantime, feel free to reply to this email or check our FAQ section on our website.</span><br><br>" +
                $"<span>We look forward to assisting you!</span><br><br>" +
                $"<span>Best regards,</span><br>" +
                $"<span>Nice2Strangers Customer Service Team</span><br>" +
                $"<span>Email: support@nice2strangers.org</span><br>" +
                $"<span>Hours: Monday-Friday 8AM-6PM EST</span><br>";
        }




    }
}
