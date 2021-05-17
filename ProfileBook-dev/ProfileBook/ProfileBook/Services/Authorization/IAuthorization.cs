using System.Threading.Tasks;

namespace ProfileBook.Services.Authorization
{
    public interface IAuthorization
    {
        Task<bool> IsAuthorization(string login, string password);
    }
}