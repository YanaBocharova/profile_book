namespace ProfileBook.ServiceData.Enums
{
    // what we validate, login or password
    public enum CheckedItem : byte
    {
        Login,
        Password
    }


    public enum CodeUserAuthResult : byte
    {
        Passed,
        InvalidLogin,
        LoginTaken,
        InvalidPassword,
        PasswordMismatch
    }


    public enum CompareProfileSelector : byte
    {
        Name,
        NickName,
        DateCreation
    }
}