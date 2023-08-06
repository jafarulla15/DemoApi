/*
 * Created By  	: Jafar Ulla
 * Created Date	: 2023-08-04
 * Updated By  	: 
 * Updated Date	: 
 * (c) Inneed Cloud.
 */

using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoProject.Repository
{
    public static class RepositoryDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        public static void RepositoryRegister(this ContainerBuilder builder)
        {
            //repositories
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            //builder.RegisterType<UnitOfWork>().As<IUnitOfWork().InstancePerLifetimeScope();
           // builder.RegisterGeneric(typeof(UnitOfWork<>)).As(typeof(IUnitOfWork<>)).InstancePerLifetimeScope();
            builder.RegisterType<DataProvider>().As<IDataProvider>().InstancePerLifetimeScope();
          //  builder.RegisterGeneric(typeof(TestRepository<>)).As(typeof(IChannelRepository<>)).InstancePerLifetimeScope();
          //  builder.RegisterGeneric(typeof(UserRepository<>)).As(typeof(IUserRepository<>)).InstancePerLifetimeScope();

        }
    }

    public interface IDataProvider
    {
        string Get();
    }

    public class DataProvider : IDataProvider
    {
        public string Get()
        {
            return "Test data";
        }
    }
}
