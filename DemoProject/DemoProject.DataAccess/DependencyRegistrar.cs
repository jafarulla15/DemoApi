/*
 * Created By  	: Jafar Ulla
 * Created Date	: 2023-08-04
 * Updated By  	: 
 * Updated Date	: 
 * (c) Inneed Cloud.
 */

using Autofac;
using DemoProject.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DemoProject.DataAccess
{
    public static class DependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        public static void DataAccessRegister(this ContainerBuilder builder, AppConfig config)
        {
            //data layer
            builder.Register(context => new DPDbContext(context.Resolve<DbContextOptions<DPDbContext>>())).InstancePerLifetimeScope();

        }
    }
}
