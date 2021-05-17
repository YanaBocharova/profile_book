using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ProfileBook.Services.Validators
{
    class Validators : IValidators
    {
        public bool ArePasswordsEquals(string pass, string confirmPass)
        {
            if (confirmPass.Equals(pass))
            {
                return true;
            }
            return false;
        }

        public bool IsCorrectLength(string item, int minLength)
        {
            var hasCorrectLength = new Regex(@"^.{" + $"{minLength}" + ",16}$");

            if (hasCorrectLength.IsMatch(item))
            {
                return true;
            }
            return false;
        }

        public bool IsFirstSymbolDigit(string login)
        {
            var hasNumber = new Regex(@"^[0-9]");

            if (hasNumber.IsMatch(login))
            {
                return true;
            }
            return false;
        }

        public bool IsPassAvailable(string pass)
        {
            var hasNumber = new Regex("[0-9]+");
            var hasUpperChar = new Regex("[A-Z]+");
            var hasLowerChar = new Regex("[a-z]+");

            if (hasNumber.IsMatch(pass) && hasUpperChar.IsMatch(pass) && hasLowerChar.IsMatch(pass))
            {
                return true;
            }

            return false;
        }
    }
}
