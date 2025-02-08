using PaymentService.API.Data;
using PaymentService.API.Repositories;
using PaymentService.API.Services;
using PayPalCheckoutSdk.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IPaymentContext, PaymentContext>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddSingleton<PayPalClient>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseAuthorization();

app.MapControllers();

app.Run();
