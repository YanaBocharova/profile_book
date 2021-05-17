using System;
using System.Collections.Generic;
using System.Text;

namespace ProfileBook.Services.Validators
{
    interface IValidators
    {
        bool IsPassAvailable(string pass);
        bool IsFirstSymbolDigit(string login);
        bool ArePasswordsEquals(string pass, string confirmPass);
        bool IsCorrectLength(string item, int minLength);
    }
}
