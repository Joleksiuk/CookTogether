
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

        Task<List<UserModel>> GetFriendedAndNotMemeberListOfUsers(string user, int partyId);

        Task InsertCategoryForParty(CategoryModel category, PartyModel party);
        Task InsertAreaForParty(AreaModel area, PartyModel party);
        Task InsertPartyUserInvite(UserModel user, PartyModel party);
        Task InsertPartyUser(UserModel user, PartyModel party);
        Task InsertPartyMealChoice(PartyMealChoiceModel choiceModel);

        Task RemoveAllCategoriesFromParty(int partyId);
        Task RemoveAllMembersFromParty(int partyId);
        Task RemoveAllAreasFromParty(int partyId);
        Task RemoveAllInvitesToParty(int partyId);
        Task RemoveParty(int partyId);
        Task RemoveAllMealChoicesOfParty(int partyId);
        Task RemoveAllMealsOfParty(int partyId);

        Task InsertPartyMeals(List<PartyMealModel> partyMeals);
        Task<List<MealModel>> GetPartyMealsById(int partyId);
        Task<List<PartyMealChoiceModel>> GetUserPartyChoices(int partyId, string userId);
        Task<int> GetNumberOfPartyMembersWhoVoted(int partyId);
    }
}
