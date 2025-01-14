using ReviewService.Common.DTOs;
using ReviewService.Common.Extensions;
using ReviewService.GRPC.Protos;
using ReviewService.GRPC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddReviewCommonExtensions();
builder.Services.AddAutoMapper(configuration =>
{
    configuration.CreateMap<ReviewDTO, GetReviewsResponse.Types.ReviewReply>().ReverseMap();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ReviewServiceGrpc>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
