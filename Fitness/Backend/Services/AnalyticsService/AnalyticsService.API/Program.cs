using AnalyticsService.API.GrpcServices;
using AnalyticsService.Common.Entities;
using AnalyticsService.Common.Extensions;
using ReviewService.GRPC.Protos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAnalyticsCommonExtensions();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGrpcClient<ReviewProtoService.ReviewProtoServiceClient>(
    options => options.Address = new Uri(builder.Configuration["GrpcSettings:ReviewUrl"]));
builder.Services.AddScoped<ReviewGrpcService>();

builder.Services.AddAutoMapper(configuration =>
{
    configuration.CreateMap<GetReviewsResponse, ReviewType>().ReverseMap();
    configuration.CreateMap<GetReviewsResponse.Types.ReviewReply, ReviewType>().ReverseMap();
});

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
