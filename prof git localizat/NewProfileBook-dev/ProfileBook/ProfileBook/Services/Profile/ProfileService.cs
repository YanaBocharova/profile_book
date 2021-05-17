using ProfileBook.Enums;
using ProfileBook.Models;
using ProfileBook.Services.Repository;
using ProfileBook.Services.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileBook.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private IRepository repository;
        private ISettingsManager settingsManager;

        public ProfileService(ISettingsManager settingsManager,
                              IRepository repository)
        {
            this.repository = repository;
            this.settingsManager = settingsManager;
        }

        public async void DeleteProfile(ProfileModel profileModel)
        {
            if (profileModel != null)
            {
                await repository.DeleteAsync(profileModel);
            }
        }

        public async void SaveProfile(ProfileModel profileModel)
        {
            if (profileModel.Id == 0)
            {
                await repository.InsertAsync(profileModel);
            }
            else
            {
                await repository.UpdateAsync(profileModel);
            }
        }

        public async Task<List<ProfileModel>> GetAllProfiles()
        {
            int userId = settingsManager.UserId;
            var profiles = await repository.GetAllAsync<ProfileModel>();
            return profiles.Where(x => x.UserId == userId).ToList();
        }

        public async Task<List<ProfileModel>> SortProfiles(SortOption sortOption)
        {
            var profiles = await GetAllProfiles();
            switch (sortOption)
            {
                case SortOption.NickName:
                    return profiles.OrderBy(x => x.NickName).ToList();
                case SortOption.Name:
                    return profiles.OrderBy(x => x.Name).ToList();
                case SortOption.Date:
                default:
                    return profiles.OrderBy(x => x.Date).ToList();
            }
                    
        }
    }
}
