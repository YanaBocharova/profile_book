using ProfileBook.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProfileBook.Services.Authorization
{
    interface IAuthorizationService
    {
        Task<bool> SignIn(string login, string password);
        void SignUp(UserModel userModel);
        Task<bool> IsLoginBusy(string login);
    }
}
