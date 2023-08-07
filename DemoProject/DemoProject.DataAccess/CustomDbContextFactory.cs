/*
 * Created By  	: Jafar Ulla
 * Created Date	: 2023-08-04
 * Updated By  	: 
 * Updated Date	: 
 * (c) Inneed Cloud.
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace DemoProject.DataAccess
{
    public class CustomDbContextFactory<T> : ICustomDbContextFactory<T> where T : DbContext
    {
        public IConfiguration _configuration;
        public CustomDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public T CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>();
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Cn"));
            return Activator.CreateInstance(typeof(T), optionsBuilder.Options) as T;
        }

        public T CreateDbContextLogDB(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>();
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("LogDBCn"));
            return Activator.CreateInstance(typeof(T), optionsBuilder.Options) as T;
        }
    }

    public interface ICustomDbContextFactory<out T> where T : DbContext
    {
        T CreateDbContext(string connectionString);
        T CreateDbContextLogDB(string connectionString);
    }


}
