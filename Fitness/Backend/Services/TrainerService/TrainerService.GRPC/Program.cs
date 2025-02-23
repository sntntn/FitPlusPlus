using TrainerService.Common.Entities;
using TrainerService.Common.Extensions;
using TrainerService.GRPC.Protos;
using TrainerService.GRPC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddTrainerCommonExtensions();
builder.Services.AddAutoMapper(configuration =>
{
    configuration.CreateMap<GetTrainerResponse, Trainer>().ReverseMap();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<TrainerServiceGrpc>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();