using Crud.Contracts;
using Crud.Data;
using Crud.Service;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.SqlServer;
using Crud.Data;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.UseUrls("http://0.0.0.0:80");

// Allow CORS
// Allow CORS
builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowViteDev", policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Register your DbContext
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Register your custom service
builder.Services.AddScoped<ISP_EmployeeService, SP_EmployeeService>();
builder.Services.AddScoped<IEmployeeJobService, EmployeeJobService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<INewsletterService, NewsletterService>();
builder.Services.AddScoped<IInquiryService, InquiryService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IJobService, JobService>();



// Add Hangfire services
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.FromSeconds(15),
        UseRecommendedIsolationLevel = true,
        UsePageLocksOnDequeue = true,
        DisableGlobalLocks = true
    });
});


builder.Services.AddHangfireServer();

builder.Services.AddSignalR();

var app = builder.Build();

//No Need Update Database
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
//    dbContext.Database.Migrate();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// Enable serving static files like images, CSS, JS, etc.
app.UseStaticFiles();
app.UseCors("AllowViteDev");
app.UseAuthorization();
app.MapControllers();
app.MapHub<Crud.Hubs.EmployeeHub>("/employeeHub");

// Add Hangfire dashboard
app.UseHangfireDashboard();

// Schedule the recurring job
RecurringJob.AddOrUpdate<EmployeeJobService>("add-random-employee",job => job.AddRandomEmployeeAsync(),"0 * * * *");

RecurringJob.AddOrUpdate<JobService>("backup-db", job => job.BackupDatabaseAsync(), "* * * * *");

app.Run();
