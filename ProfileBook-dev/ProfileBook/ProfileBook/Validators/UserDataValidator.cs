using ProfileBook.ServiceData.Enums;
using ProfileBook.ServiceData.Constants;

namespace ProfileBook.Validators
{
    public static class UserDataValidator
    {
        public static bool IsDataValid(string data, CheckedItem checkedItem)
        {
            if (checkedItem == CheckedItem.Login)
                return IsLoginValid(data);
            return IsPasswordValid(data);
        }


        private static bool IsLoginValid(string login)
        {
            bool lengthRequirements = login.Length >= Constants.MIN_LENGTH_LOGIN && login.Length <= Constants.MAX_LENGTH;
            bool firstCharacterIsNotDigit = char.IsDigit(login[0]);
            return lengthRequirements && !firstCharacterIsNotDigit;
        }


        private static bool IsPasswordValid(string password)
        {
            bool lengthRequirements = password.Length >= Constants.MIN_LENGTH_PASSWORD && password.Length <= Constants.MAX_LENGTH;
            bool hasUpperCaseLetter = false;
            bool hasLowerCaseLetter = false;
            bool hasDigit           = false;

            if (lengthRequirements)
            {
                foreach(char c in password)
                {
                    if (char.IsUpper(c)) hasUpperCaseLetter = true;
                    else if (char.IsLower(c)) hasLowerCaseLetter = true;
                    else if (char.IsDigit(c)) hasDigit = true;
                }
            }

            return lengthRequirements && hasUpperCaseLetter && hasLowerCaseLetter && hasDigit;
        }
    }
}