/*
 * Created By  	: Jafar Ulla
 * Created Date	: 2023-08-04
 * Updated By  	: 
 * Updated Date	: 
 * (c) Inneed Cloud.
 */


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DemoProject.Utilities;
using Microsoft.Extensions.Configuration;
using EntityFrameworkCore.UseRowNumberForPaging;

namespace DemoProject.DataAccess
{
    /// <summary>
    /// Represents extensions of DbContextOptionsBuilder
    /// </summary>
    public static class DbContextOptionsBuilder
    {
        /// <summary>
        /// SQL Server specific extension method for Microsoft.EntityFrameworkCore.DbContextOptionsBuilder
        /// </summary>
        /// <param name="optionsBuilder">Database context options builder</param>
        /// <param name="services">Collection of service descriptors</param>
        public static void UseSqlServerWithLazyLoading(this Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder, 
            IServiceCollection services, IConfiguration configuration)
        {
            var appConfig = services.BuildServiceProvider().GetRequiredService<AppConfig>();
            var dbContextOptionsBuilder = optionsBuilder.UseLazyLoadingProxies();

            if (appConfig.UseRowNumberForPaging)
            {
                dbContextOptionsBuilder.UseSqlServer(configuration.GetConnectionString("Cn"), option =>
                    option.UseRowNumberForPaging());
            }
            else
            {
                dbContextOptionsBuilder.UseSqlServer(configuration.GetConnectionString("Cn"));
            } 
        }

        /// <summary>
        /// SQL Server specific extension method for Microsoft.EntityFrameworkCore.DbContextOptionsBuilder
        /// </summary>
        /// <param name="optionsBuilder">Database context options builder</param>
        /// <param name="services">Collection of service descriptors</param>
        public static void UseSqlServerWithLazyLoadingForArchiveDB(this Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder,
            IServiceCollection services, IConfiguration configuration)
        {
            var appConfig = services.BuildServiceProvider().GetRequiredService<AppConfig>();
            var dbContextOptionsBuilder = optionsBuilder.UseLazyLoadingProxies();

            if (appConfig.UseRowNumberForPaging)
            {
                dbContextOptionsBuilder.UseSqlServer(configuration.GetConnectionString("Cn"), option =>
                    option.UseRowNumberForPaging());
            }
            else
            {
                dbContextOptionsBuilder.UseSqlServer(configuration.GetConnectionString("Cn"));
            }
        }
    }
}
