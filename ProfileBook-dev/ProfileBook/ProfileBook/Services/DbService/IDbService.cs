using ProfileBook.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProfileBook.Services.DbService
{
    public interface IDbService
    {
        Task<int> InsertDataAsync<T>(T entity) where T : IEntityBase, new();
        Task<int> UpdateDataAsync<T>(T entity) where T : IEntityBase, new();
        Task<int> DeleteDataAsync<T>(T entity) where T : IEntityBase, new();
        Task<List<T>> GetAllDataAsync<T>() where T : IEntityBase, new();
        Task<List<ProfileModel>> GetOwnersProfilesAsync(string owner);
    }
}