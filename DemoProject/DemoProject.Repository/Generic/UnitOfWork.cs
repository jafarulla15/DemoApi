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
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DemoProject.Repository
{
    public class UnitOfWork: IUnitOfWork 
    {
        public readonly DPDbContext _dbContext;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(DPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public TContext DbContext => _dbContext;

        public  IGenericRepository<T> Repository<T>() where T : class, new()
        {
            if (_repositories == null)
                _repositories = new Dictionary<Type, object>();

            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new GenericRepository<T>(_dbContext);
            }

            return (IGenericRepository<T>)_repositories[type];
        }

        //public void Rollback()
        //{
        //    foreach (var entry in _dbContext.ChangeTracker.Entries())
        //    {
        //        switch (entry.State)
        //        {
        //            case EntityState.Added:
        //                entry.State = EntityState.Detached;
        //                break;
        //        }
        //    }
        //}

        public  int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return _dbContext.SaveChanges(acceptAllChangesOnSuccess);
        }

        public  async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess)
        {
            return await _dbContext.SaveChangesAsync(acceptAllChangesOnSuccess);
        }

        public  int SaveChanges()
        {
            int value = _dbContext.SaveChanges();
            foreach (var entry in _dbContext.ChangeTracker.Entries().ToArray())
            {
                entry.State = EntityState.Detached;
            }
            return value;
        }

        public  async Task<int> SaveChangesAsync()
        {
            int value = await _dbContext.SaveChangesAsync();
            foreach (var entry in _dbContext.ChangeTracker.Entries().ToArray())
            {
                entry.State = EntityState.Detached;
            }
            return value;
        }

        public List<T> RawSqlQuery<T>(string query) where T : class, new()
        {
            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                _dbContext.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {
                    var lst = new List<T>();
                    var lstColumns = new T().GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
                    while (reader.Read())
                    {
                        var newObject = new T();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var name = reader.GetName(i);
                            PropertyInfo prop = lstColumns.FirstOrDefault(a => a.Name.ToLower().Equals(name.ToLower()));
                            if (prop == null)
                            {
                                continue;
                            }
                            var val = reader.IsDBNull(i) ? null : reader[i];
                            prop.SetValue(newObject, val, null);
                        }
                        lst.Add(newObject);
                    }
                    return lst;
                }

            }
        }

        public async Task<List<T>> RawSqlQueryAsync<T>(string query) where T : class, new()
        {
            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                await _dbContext.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var lst = new List<T>();
                    var lstColumns = new T().GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
                    while (await reader.ReadAsync())
                    {
                        var newObject = new T();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var name = reader.GetName(i);
                            PropertyInfo prop = lstColumns.FirstOrDefault(a => a.Name.ToLower().Equals(name.ToLower()));
                            if (prop == null)
                            {
                                continue;
                            }
                            var val = reader.IsDBNull(i) ? null : reader[i];
                            prop.SetValue(newObject, val, null);
                        }
                        lst.Add(newObject);
                    }
                    return lst;
                }

            }
        }

        private bool _disposed = false;
        /// <summary>
        /// Dispose the dbContext after finish task
        /// </summary>
        /// <param name="disposing">Flag for indicating desposing or not</param>
        public  void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                {
                    _dbContext.Dispose();
                    if (_repositories != null)
                    {
                        _repositories.Clear();
                    }
                }

            _disposed = true;
        }

        /// <summary>
        /// Dispose the dbContext after finish task
        /// </summary>
        public  void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
