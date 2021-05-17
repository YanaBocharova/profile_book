using ProfileBook.Enums;
using ProfileBook.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProfileBook.Services.Profile
{
    public interface IProfileService
    {
        Task<List<ProfileModel>> GetAllProfiles();
        void SaveProfile(ProfileModel profileModel);
        void DeleteProfile(ProfileModel profileModel);
        Task<List<ProfileModel>> SortProfiles(SortOption sortOption);
    }
}
