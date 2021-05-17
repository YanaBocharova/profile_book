using ProfileBook.ServiceData.Enums;
using System.Threading.Tasks;

namespace ProfileBook.Services.Registration
{
    public interface IRegistration
    {
        Task<CodeUserAuthResult> IsRegistration(string login, string password, string confirmPassword);
    }
}