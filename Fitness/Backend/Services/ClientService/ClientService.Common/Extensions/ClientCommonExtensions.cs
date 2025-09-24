using ClientService.Common.Data;
using ClientService.Common.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ClientService.Common.Extensions;

public static class ClientCommonExtensions
{
    public static void AddClientCommonExtensions(this IServiceCollection services)
    {
        services.AddScoped<IContext, Context>();
        services.AddScoped<IRepository, Repository>();
    }
}