using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Extensions
{
    public static class HostExtension
    {
        public  static IHost MigrateAsync<TContext>(this IHost host,
                                                 Action<TContext,IServiceProvider> seeder,
                                                 int retry = 0) where TContext : OrderContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<TContext>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);
                    context.Database.Migrate();
                    seeder(context, services);
                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (Exception ex)
                {

                    logger.LogError(ex, $"An error has occured while trying to migrate the database used on Context ", typeof(TContext).Name);
                    if(retry < 50)
                    {
                        retry++;
                        Thread.Sleep(2000);
                        MigrateAsync<TContext>(host, seeder, retry);
                    }
                }
            }
            return host;
        }
    }
}
