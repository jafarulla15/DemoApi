/*
 * Created By  	: Jafar Ulla
 * Created Date	: 2023-08-04
 * Updated By  	: 
 * Updated Date	: 
 * (c) Inneed Cloud.
 */

using DemoProject.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoProject.Repository
{
    public interface IUnitOfWork : IDisposable 
    {
        //DPDbContext DbContext { get; }
        //void Rollback();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync();
        IGenericRepository<T> Repository<T>() where T : class, new();
        List<T> RawSqlQuery<T>(string query) where T : class, new();
        Task<List<T>> RawSqlQueryAsync<T>(string query) where T : class, new();
    }
}
