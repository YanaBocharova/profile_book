using System.Collections.Generic;
using ProfileBook.ServiceData.Enums;

namespace ProfileBook.Models
{
    public class ProfileModelComparer : IComparer<ProfileModel>
    {
        public CompareProfileSelector Selector { get; private set; }

        public ProfileModelComparer(string _selector)
        {
            switch (_selector)
            {
                case "Name":
                    Selector = CompareProfileSelector.Name;
                    break;
                case "NickName":
                    Selector = CompareProfileSelector.NickName;
                    break;
                case "Date creation":
                    Selector = CompareProfileSelector.DateCreation;
                    break;
            }
        }

        public int Compare(ProfileModel x, ProfileModel y)
        {
            switch (Selector)
            {
                case CompareProfileSelector.Name:
                    return x.Name.CompareTo(y.Name);
                case CompareProfileSelector.NickName:
                    return x.NickName.CompareTo(y.NickName);
                case CompareProfileSelector.DateCreation:
                    return x.CreationTime.CompareTo(y.CreationTime);
            }
            return 0;
        }
    }
}