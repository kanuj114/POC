using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TokenAuthentication.Database;
using TokenAuthentication.DTO;
using TokenAuthentication.Functionality;
using TokenAuthentication.Service;
using TokenAuthentication.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TokenDbContext>(c=>c.UseSqlServer(builder.Configuration.GetConnectionString("TokenConnection"))); // IOC here
builder.Services.AddScoped<IValidator<RegisterDTO>, RegisterDTOValidator>();// AddScoped is similar to asp.net/MVC
builder.Services.AddTransient<IAuthReporitory, AuthReporitory>(); // Per call : Request-Response Pattern
// use AddkeyedSingleton<> instead of using AddTransient<>. To achieve Adapter Pattern: Adaptability.
// builder.Services.AddValidatorsFromAssemblyContaining<RegisterDTOValidator>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();

