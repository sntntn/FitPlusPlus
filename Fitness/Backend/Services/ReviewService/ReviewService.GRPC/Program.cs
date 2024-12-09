using ReviewService.Common.DTOs;
using ReviewService.Common.Extentions;
using ReviewService.GRPC.Protos;
using ReviewService.GRPC.Services;
using Steeltoe.Discovery.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddReviewCommonExtentions();
builder.Services.AddAutoMapper(configuration =>
{
    configuration.CreateMap<ReviewDTO, GetReviewsResponse.Types.ReviewReply>().ReverseMap();
});

builder.Services.AddDiscoveryClient(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ReviewServicegrpc>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
