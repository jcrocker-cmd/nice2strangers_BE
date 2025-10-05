using Crud.Contracts;
using Crud.Data;
using Crud.Models;
using Crud.Models.Entities;
using Crud.Models.Entities.Services;
using Crud.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static Crud.Service.InquiryService;

namespace Crud.Service
{

    public class ServicesService : IServicesService
    {

        private readonly IEmailService _emailService;
        private readonly ApplicationDBContext dbContext;

        public ServicesService(IEmailService emailService, ApplicationDBContext dbContext)
        {
            _emailService = emailService;
            this.dbContext = dbContext;
        }

        public async Task<string> SendEmailAsync(EmailRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ToEmail))
                throw new ArgumentException("ToEmail is required.");

            await _emailService.SendEmailAsync(request.ToEmail, request.Subject, request.Body);
            return "Email Sent";
        }

        public async Task<DroneModel> DroneServiceAsync(DroneViewModel request)
        {

            var random = new Random();
            var randomCode = random.Next(1, 10000); // 1 to 9999 inclusive
            var formattedCode = randomCode.ToString("D4");

            var name = request.Name;
            var email = request.Email;
            var servicetype = request.ServiceType;
            var location = request.Location;
            var budget = request.Budget;
            var date = request.Date;
            var message = request.Message;
            var subject = $"{Constants.Services.DroneSubject} : #{formattedCode}";

            var body = Constants.ClientServicesEmailTemplate(name, servicetype, location, budget, date, message);
            var adminBody = Constants.AdminServicesEmailTemplate(name, email, servicetype, location, budget, date, message);

            if (!string.IsNullOrEmpty(email))
            {
                try
                {
                    await _emailService.SendEmailAsync(email, subject, body);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Client email failed: {ex.Message}");
                }
            }

            await _emailService.SendEmailAsync(Constants.AdminEmail, $"Admin Notification - {subject}", adminBody);

            var drone = new DroneModel
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Subject = subject,
                ServiceType = request.ServiceType,
                Location = request.Location,
                Budget = request.Budget,
                Date = request.Date,
                Message = request.Message,
            };

            dbContext.DroneModel.Add(drone);
            await dbContext.SaveChangesAsync();

            return drone;
        }

        public async Task<List<DroneViewModel>> GetDroneServiceDataAsync()
        {
            return await dbContext.DroneModel
                .AsNoTracking()
                .Select(c => new DroneViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Subject = c.Subject,
                    ServiceType = c.ServiceType,
                    Location = c.Location,
                    Budget = c.Budget,
                    Date = c.Date,
                    Message = c.Message,
                    IsReplied = c.IsReplied,
                    CreatedDate = c.CreatedDate.ToString("MMM, dd, yyyy h:mmtt", CultureInfo.InvariantCulture)
                })
                .ToListAsync();
        }

        public async Task<string> SendReplyDroneService(SendReplyRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Subject) ||
                string.IsNullOrWhiteSpace(request.Body))
            {
                throw new ArgumentException("Email, subject, and body are required.");
            }

            try
            {
                await _emailService.SendEmailAsync(request.Email, request.Subject, request.Body);

                var contact = await dbContext.DroneModel.FirstOrDefaultAsync(c => c.Id == request.Id);
                if (contact != null)
                {
                    contact.IsReplied = true;
                    await dbContext.SaveChangesAsync();
                }

                return "Reply sent and marked as replied!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send to {request.Email}: {ex.Message}");
                throw new InvalidOperationException("Failed to send reply.", ex);
            }
        }



        public async Task<SocialMediaConsultingModel> SMConsultingServiceAsync(SocialMediaConsultingViewModel request)
        {

            var random = new Random();
            var randomCode = random.Next(1, 10000); // 1 to 9999 inclusive
            var formattedCode = randomCode.ToString("D4");

            var name = request.Name;
            var email = request.Email;
            var platformList = string.Join(", ", request.Platforms ?? new List<string>());
            var goals = request.Goals;
            var budget = request.Budget;
            var duration = request.Duration;
            var message = request.Message;
            var subject = $"{Constants.Services.SMConsulting} : #{formattedCode}";

            var body = Constants.ClientSMConsultingEmailTemplate(name, platformList, goals, budget, duration, message);
            var adminBody = Constants.AdminSMConsultingEmailTemplate(name, email, platformList, goals, budget, duration, message);

            if (!string.IsNullOrEmpty(email))
            {
                try
                {
                    await _emailService.SendEmailAsync(email, subject, body);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Client email failed: {ex.Message}");
                }
            }

            await _emailService.SendEmailAsync(Constants.AdminEmail, $"Admin Notification - {subject}", adminBody);

            var smc = new SocialMediaConsultingModel
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Subject = subject,
                Platforms = platformList,
                Goals = request.Goals,
                Budget = request.Budget,
                Duration = request.Duration,
                Message = request.Message,
            };

            dbContext.SocialMediaConsultingModel.Add(smc);
            await dbContext.SaveChangesAsync();

            return smc;
        }

        public async Task<List<SocialMediaConsultingViewModel>> GetSMConsultingData()
        {
            return await dbContext.SocialMediaConsultingModel
                .AsNoTracking()
                .Select(c => new SocialMediaConsultingViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Subject = c.Subject,
                    Platforms = new List<string> { c.Platforms }, // 👈 keep the exact string with commas
                    Goals = c.Goals,
                    Budget = c.Budget,
                    Duration = c.Duration,
                    Message = c.Message,
                    IsReplied = c.IsReplied,
                    CreatedDate = c.CreatedDate.ToString("MMM, dd, yyyy h:mmtt", CultureInfo.InvariantCulture)
                })
                .ToListAsync();
        }


        public async Task<string> SendReplySMConsulting(SendReplyRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Subject) ||
                string.IsNullOrWhiteSpace(request.Body))
            {
                throw new ArgumentException("Email, subject, and body are required.");
            }

            try
            {
                await _emailService.SendEmailAsync(request.Email, request.Subject, request.Body);

                var contact = await dbContext.SocialMediaConsultingModel.FirstOrDefaultAsync(c => c.Id == request.Id);
                if (contact != null)
                {
                    contact.IsReplied = true;
                    await dbContext.SaveChangesAsync();
                }

                return "Reply sent and marked as replied!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send to {request.Email}: {ex.Message}");
                throw new InvalidOperationException("Failed to send reply.", ex);
            }
        }


        public async Task<SocialMediaCreationModel> SMCreationServiceAsync(SocialMediaCreationViewModel request)
        {

            var random = new Random();
            var randomCode = random.Next(1, 10000); // 1 to 9999 inclusive
            var formattedCode = randomCode.ToString("D4");

            var name = request.Name;
            var email = request.Email;
            var platformList = string.Join(", ", request.Platforms ?? new List<string>());
            var contentTypeList = string.Join(", ", request.ContentType ?? new List<string>());
            var freq = request.Frequency;
            var budget = request.Budget;
            var duration = request.Duration;
            var message = request.Message;
            var subject = $"{Constants.Services.SMCreation} : #{formattedCode}";

            var body = Constants.ClientSMCreationEmailTemplate(name, platformList, contentTypeList, freq, budget, duration, message);
            var adminBody = Constants.AdminSMCreationEmailTemplate(name, email, platformList, contentTypeList, freq, budget, duration, message);

            if (!string.IsNullOrEmpty(email))
            {
                try
                {
                    await _emailService.SendEmailAsync(email, subject, body);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Client email failed: {ex.Message}");
                }
            }

            await _emailService.SendEmailAsync(Constants.AdminEmail, $"Admin Notification - {subject}", adminBody);

            var smc = new SocialMediaCreationModel
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Subject = subject,
                Platforms = platformList,
                ContentType = contentTypeList,
                Frequency = request.Frequency,
                Budget = request.Budget,
                Duration = request.Duration,
                Message = request.Message,
            };

            dbContext.SocialMediaCreationModel.Add(smc);
            await dbContext.SaveChangesAsync();

            return smc;
        }

        public async Task<List<SocialMediaCreationViewModel>> GetSMCreationData()
        {
            return await dbContext.SocialMediaCreationModel
                .AsNoTracking()
                .Select(c => new SocialMediaCreationViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Subject = c.Subject,
                    Platforms = new List<string> { c.Platforms }, 
                    ContentType = new List<string> { c.ContentType },
                    Budget = c.Budget,
                    Frequency = c.Frequency,
                    Duration = c.Duration,
                    Message = c.Message,
                    IsReplied = c.IsReplied,
                    CreatedDate = c.CreatedDate.ToString("MMM, dd, yyyy h:mmtt", CultureInfo.InvariantCulture)
                })
                .ToListAsync();
        }


        public async Task<string> SendReplySMCreation(SendReplyRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Subject) ||
                string.IsNullOrWhiteSpace(request.Body))
            {
                throw new ArgumentException("Email, subject, and body are required.");
            }

            try
            {
                await _emailService.SendEmailAsync(request.Email, request.Subject, request.Body);

                var contact = await dbContext.SocialMediaCreationModel.FirstOrDefaultAsync(c => c.Id == request.Id);
                if (contact != null)
                {
                    contact.IsReplied = true;
                    await dbContext.SaveChangesAsync();
                }

                return "Reply sent and marked as replied!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send to {request.Email}: {ex.Message}");
                throw new InvalidOperationException("Failed to send reply.", ex);
            }
        }



        public async Task<VideoEditingModel> VideoEditingServiceAsync(VideoEditingViewModel request)
        {

            var random = new Random();
            var randomCode = random.Next(1, 10000); // 1 to 9999 inclusive
            var formattedCode = randomCode.ToString("D4");

            var name = request.Name;
            var email = request.Email;
            var platformList = string.Join(", ", request.Platforms ?? new List<string>());
            var contentTypeList = string.Join(", ", request.ContentType ?? new List<string>());
            var budget = request.Budget;
            var duration = request.Duration;
            var message = request.Message;
            var otherCT = request.OtherContentType;
            var subject = $"{Constants.Services.VideoEditing} : #{formattedCode}";

            var body = Constants.ClientVideoEditingEmailTemplate(name, platformList, contentTypeList, otherCT, budget, duration, message);
            var adminBody = Constants.AdminSMCreationEmailTemplate(name, email, platformList, contentTypeList, otherCT, budget, duration, message);

            if (!string.IsNullOrEmpty(email))
            {
                try
                {
                    await _emailService.SendEmailAsync(email, subject, body);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Client email failed: {ex.Message}");
                }
            }

            await _emailService.SendEmailAsync(Constants.AdminEmail, $"Admin Notification - {subject}", adminBody);

            var data = new VideoEditingModel
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Subject = subject,
                Platforms = platformList,
                ContentType = contentTypeList,
                OtherContentType = request.OtherContentType,
                Budget = request.Budget,
                Duration = request.Duration,
                Message = request.Message,
            };

            dbContext.VideoEditingModel.Add(data);
            await dbContext.SaveChangesAsync();

            return data;
        }

        public async Task<List<VideoEditingViewModel>> GetVideoEditingData()
        {
            return await dbContext.VideoEditingModel
                .AsNoTracking()
                .Select(c => new VideoEditingViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Subject = c.Subject,
                    Platforms = new List<string> { c.Platforms },
                    ContentType = new List<string> { c.ContentType },
                    Budget = c.Budget,
                    OtherContentType = c.OtherContentType,
                    Duration = c.Duration,
                    Message = c.Message,
                    IsReplied = c.IsReplied,
                    CreatedDate = c.CreatedDate.ToString("MMM, dd, yyyy h:mmtt", CultureInfo.InvariantCulture)
                })
                .ToListAsync();
        }


        public async Task<string> SendReplyVideoEditing(SendReplyRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Subject) ||
                string.IsNullOrWhiteSpace(request.Body))
            {
                throw new ArgumentException("Email, subject, and body are required.");
            }

            try
            {
                await _emailService.SendEmailAsync(request.Email, request.Subject, request.Body);

                var contact = await dbContext.VideoEditingModel.FirstOrDefaultAsync(c => c.Id == request.Id);
                if (contact != null)
                {
                    contact.IsReplied = true;
                    await dbContext.SaveChangesAsync();
                }

                return "Reply sent and marked as replied!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send to {request.Email}: {ex.Message}");
                throw new InvalidOperationException("Failed to send reply.", ex);
            }
        }



        public async Task<WebAppModel> SoftwareServiceAsync(WebAppViewModel request)
        {

            var random = new Random();
            var randomCode = random.Next(1, 10000); // 1 to 9999 inclusive
            var formattedCode = randomCode.ToString("D4");

            var name = request.Name;
            var email = request.Email;
            var platformList = string.Join(", ", request.Platform ?? new List<string>());

            var budget = request.Budget;
            var timeline = request.Timeline;
            var message = request.Message;
            var subject = $"{Constants.Services.Software} : #{formattedCode}";

            var body = Constants.ClientSoftwareEmailTemplate(name, platformList, budget, timeline, message);
            var adminBody = Constants.AdminSoftwareEmailTemplate(name, email, platformList, budget, timeline, message);

            if (!string.IsNullOrEmpty(email))
            {
                try
                {
                    await _emailService.SendEmailAsync(email, subject, body);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Client email failed: {ex.Message}");
                }
            }

            await _emailService.SendEmailAsync(Constants.AdminEmail, $"Admin Notification - {subject}", adminBody);

            var data = new WebAppModel
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Subject = subject,
                Platform = platformList,
                Budget = request.Budget,
                ProjectType = request.ProjectType,
                Timeline = request.Timeline,
                Message = request.Message,
            };

            dbContext.WebAppModel.Add(data);
            await dbContext.SaveChangesAsync();

            return data;
        }

        public async Task<List<WebAppViewModel>> GetSoftwareData()
        {
            return await dbContext.WebAppModel
                .AsNoTracking()
                .Select(c => new WebAppViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Subject = c.Subject,
                    Platform = new List<string> { c.Platform },
                    ProjectType = c.ProjectType,
                    Budget = c.Budget,
                    Timeline = c.Timeline,
                    Message = c.Message,
                    IsReplied = c.IsReplied,
                    CreatedDate = c.CreatedDate.ToString("MMM, dd, yyyy h:mmtt", CultureInfo.InvariantCulture)
                })
                .ToListAsync();
        }


        public async Task<string> SendReplySoftware(SendReplyRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Subject) ||
                string.IsNullOrWhiteSpace(request.Body))
            {
                throw new ArgumentException("Email, subject, and body are required.");
            }

            try
            {
                await _emailService.SendEmailAsync(request.Email, request.Subject, request.Body);

                var contact = await dbContext.WebAppModel.FirstOrDefaultAsync(c => c.Id == request.Id);
                if (contact != null)
                {
                    contact.IsReplied = true;
                    await dbContext.SaveChangesAsync();
                }

                return "Reply sent and marked as replied!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send to {request.Email}: {ex.Message}");
                throw new InvalidOperationException("Failed to send reply.", ex);
            }
        }


    }
}
