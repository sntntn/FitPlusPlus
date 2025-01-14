using Microsoft.Extensions.DependencyInjection;
using TrainerService.Common.Data;
using TrainerService.Common.Repositories;

namespace TrainerService.Common.Extensions;

public static class TrainerCommonExtensions
{
    public static void AddTrainerCommonExtensions(this IServiceCollection services)
    {
        services.AddScoped<ITrainerContext, TrainerContext>();
        services.AddScoped<ITrainerRepository, TrainerRepository>();
    }
}