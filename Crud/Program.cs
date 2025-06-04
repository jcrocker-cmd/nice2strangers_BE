using Crud.Contracts;
using Crud.Data;
using Crud.Service; // ✅ Add this to import your service namespace
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Register your DbContext
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Register your custom service
builder.Services.AddScoped<ISP_EmployeeService, SP_EmployeeService>();
//builder.Services.AddScoped<SP_EmployeeService>();
// OR if using interface-based DI
// builder.Services.AddScoped<ISP_EmployeeService, SP_EmployeeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
