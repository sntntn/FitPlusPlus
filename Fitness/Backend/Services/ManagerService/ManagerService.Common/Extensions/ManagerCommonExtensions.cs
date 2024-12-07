using ManagerService.Common.Data;
using ManagerService.Common.Repositories;
using Microsoft.Extensions.DependencyInjection;
using ReviewService.Common.DTOs;
using ReviewService.Common.Entities;
using ReviewService.Common.Repositories;

namespace ManagerService.Common.Extensions;

public static class ManagerCommonExtensions
{
    public static void AddManagerCommonExtentions(this IServiceCollection services)
    {
        services.AddScoped<IManagerContext, ManagerContext>();
        services.AddScoped<IManagerRepository, ManagerRepository>();
    }
}