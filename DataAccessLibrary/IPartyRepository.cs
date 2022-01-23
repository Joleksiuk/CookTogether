
using DataAccessLibrary.Models;
using DataAccessLibrary.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IPartyRepository
    {
        Task<List<PartyModel>> GetAllParties();
        Task<List<PartyModel>> GetUserParties(UserModel userId);
        Task InsertParty(PartyModel user);
        Task<PartyModel> GetPartyById(string id);
        Task<PartyUserInviteModel> GetInviteByIds(string userId, int partyId);
        Task<PartyModel> GetLatestUserParty(string userId);
        Task<List<UserModel>> GetAllPartyMembers(PartyModel party, UserModel user);
        Task<List<UserModel>> GetUsersInvitedToParty(PartyModel party, UserModel user);
        Task<List<UserModel>> GetNotInvitedNotMembersUsersToParty(PartyModel party, UserModel user);
        Task RemoveUserFromParty(UserModel userId, PartyModel partyId);
        Task RemovePartyUserInvite(UserModel user, PartyModel party);
        Task<List<PartyUserInviteModel>> GetUserPendingInvites(UserModel user);
        Task<List<DisplayPartyInviteModel>> GetUserPendingDisplayInvites(UserModel user);

        Task InsertCategoryForParty(CategoryModel category, PartyModel party);
        Task InsertAreaForParty(AreaModel area, PartyModel party);
        Task InsertPartyUserInvite(UserModel user, PartyModel party);
        Task InsertPartyUser(UserModel user, PartyModel party);
        Task InsertPartyMealChoice(MealModel meal, PartyModel party, UserModel user);

    }
}
