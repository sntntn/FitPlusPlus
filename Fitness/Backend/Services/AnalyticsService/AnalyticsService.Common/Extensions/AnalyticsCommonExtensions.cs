using AnalyticsService.Common.Data;
using AnalyticsService.Common.Entities;
using AnalyticsService.Common.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AnalyticsService.Common.Extensions;

public static class AnalyticsCommonExtensions
{
    public static void AddAnalyticsCommonExtensions(this IServiceCollection services)
    {
        services.AddScoped<IAnalyticsContext, AnalyticsContext>();
        services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
    }
}