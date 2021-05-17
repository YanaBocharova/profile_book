using ProfileBook.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProfileBook.Services.Repository
{
    public interface IRepository
    {
        Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new();
        Task<int> UpdateAsync<T>(T entity) where T : IEntityBase, new();
        Task<int> DeleteAsync<T>(T entity) where T : IEntityBase, new();
        Task<List<T>> GetAllAsync<T>() where T : IEntityBase, new();
        Task<List<ProfileModel>> GetProfilesAsync(string owner);
    }
}