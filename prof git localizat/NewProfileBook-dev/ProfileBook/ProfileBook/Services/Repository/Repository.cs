using ProfileBook.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProfileBook.Services.Repository
{
    public class Repository : IRepository
    {
        private Lazy<SQLiteAsyncConnection> _database;

        public Repository()
        {
            _database = new Lazy<SQLiteAsyncConnection>(() =>
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "profilebook.db3");
                var database = new SQLiteAsyncConnection(path);

                database.CreateTableAsync<UserModel>().Wait();
                database.CreateTableAsync<ProfileModel>().Wait();

                return database;

            });
        }

        public Task<int> DeleteAsync<T>(T entity) where T : IEntityBase, new()
        {
            return _database.Value.DeleteAsync(entity);
        }

        public Task<List<T>> GetAllAsync<T>() where T : IEntityBase, new()
        {
            return _database.Value.Table<T>().ToListAsync();
        }

        public Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new()
        {
            return _database.Value.InsertAsync(entity);
        }

        public Task<int> UpdateAsync<T>(T entity) where T : IEntityBase, new()
        {
            return _database.Value.UpdateAsync(entity);
        }
    }
}
