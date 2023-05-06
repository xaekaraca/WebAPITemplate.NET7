using Data.Context.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Context;

public static class Bootstrapper
{
    public static void AddSqlDb<TContext>(this IServiceCollection services, IConfiguration configuration)
        where TContext : SqlContext
    {
        services.AddDbContext<TContext>(options => options.UseSqlServer(configuration.GetConnectionString($"{typeof(TContext).Name}")));
    }
}