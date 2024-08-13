using ReviewService.GRPC.Protos;
using TrainerService.API.Data;
using TrainerService.API.Entities;
using TrainerService.API.GrpcServices;
using TrainerService.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ITrainerContext, TrainerContext>();
builder.Services.AddScoped<ITrainerRepository, TrainerRepository>();

builder.Services.AddGrpcClient<ReviewProtoService.ReviewProtoServiceClient>(
    options => options.Address = new Uri(builder.Configuration["GrpcSettings:ReviewUrl"]));
builder.Services.AddScoped<ReviewGrpcService>();

builder.Services.AddAutoMapper(configuration =>
{
    configuration.CreateMap<GetReviewsResponse, ReviewType>().ReverseMap();
    configuration.CreateMap<GetReviewsResponse.Types.ReviewReply, ReviewType>().ReverseMap();
});

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

app.UseAuthorization();

app.MapControllers();

app.Run();
