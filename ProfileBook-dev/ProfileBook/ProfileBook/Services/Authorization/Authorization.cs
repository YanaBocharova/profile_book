using ProfileBook.Models;
using ProfileBook.ServiceData.Enums;
using ProfileBook.Services.DbService;
using ProfileBook.Validators;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileBook.Services.Authorization
{
    public class Authorization : IAuthorization
    {
        private readonly IDbService dbService;

        public Authorization(IDbService _dbService) => dbService = _dbService;
        

        public async Task<bool> IsAuthorization(string login, string password)
        {
            Task<bool> t1 = Task.Run(() => UserDataValidator.IsDataValid(login.ToUpper(), CheckedItem.Login));
            Task<bool> t2 = Task.Run(() => UserDataValidator.IsDataValid(password, CheckedItem.Password));
            await Task.WhenAll(new[] { t1, t2 });

            if (!(t1.Result && t2.Result))
                return false;

            List<UserModel> users = await dbService.GetAllDataAsync<UserModel>();
            return users.Any(s => s.Login.ToUpper() == login.ToUpper() && s.Password == password);
        }
    }
}