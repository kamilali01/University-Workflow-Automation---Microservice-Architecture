using Enrollment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EnrollmentDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// Http clients (simple)
builder.Services.AddHttpClient("Student", c =>
    c.BaseAddress = new Uri(builder.Configuration["Services:Student"]!));

builder.Services.AddHttpClient("Course", c =>
    c.BaseAddress = new Uri(builder.Configuration["Services:Course"]!));

builder.Services.AddHttpClient("Notification", c =>
    c.BaseAddress = new Uri(builder.Configuration["Services:Notification"]!));

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
