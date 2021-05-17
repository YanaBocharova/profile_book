using ProfileBook.ServiceData.Enums;
using ProfileBook.Validators;
using System.Threading.Tasks;
using ProfileBook.Models;
using System.Collections.Generic;
using System.Linq;
using ProfileBook.Services.DbService;

namespace ProfileBook.Services.Registration
{
    public class Registration : IRegistration
    {
        private readonly IDbService dbService;

        public Registration(IDbService _dbService) => dbService = _dbService;


        public async Task<CodeUserAuthResult> IsRegistration(string login, string password, string confirmPassword)
        {
            bool result = await Task.Run(() => UserDataValidator.IsDataValid(login.ToUpper(), CheckedItem.Login));
            if (!result)
                return CodeUserAuthResult.InvalidLogin;

            result = password == confirmPassword;
            if (!result)
                return CodeUserAuthResult.PasswordMismatch;

            result = await Task.Run(() => UserDataValidator.IsDataValid(password, CheckedItem.Password));
            if (!result)
                return CodeUserAuthResult.InvalidPassword;

            List<UserModel> users = await dbService.GetAllDataAsync<UserModel>();
            result = users.Any(s => s.Login.ToUpper() == login.ToUpper());
            if (result)
                return CodeUserAuthResult.LoginTaken;

            return CodeUserAuthResult.Passed;
        }
    }
}