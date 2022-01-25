
using DataAccessLibrary.Models;
using DataAccessLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class PartyRepository : IPartyRepository
    {
        private readonly ISqlDataAccess _db;

        public PartyRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<List<PartyModel>> GetAllParties()
        {
            string sql = @"SELECT * FROM Party";
            return _db.LoadData<PartyModel, dynamic>(sql, new { });
        }

        public async Task<PartyModel> GetPartyById(string id)
        {
            string sql = @"SELECT * FROM Party WHERE Id = @PartyId ";
            return await _db.LoadSingleResult<PartyModel, dynamic>(sql, new { PartyId =id});
        }
        public async Task<PartyModel> GetLatestUserParty(string userID)
        {
            string sql = @"SELECT TOP 1 * FROM Party WHERE OwnerUserId = @UserId ORDER BY CreationDate DESC";
            return await _db.LoadSingleResult<PartyModel, dynamic>(sql, new { UserId = userID });
        }

        public Task<List<PartyModel>> GetUserParties(UserModel user)
        {
            string sql = @"SELECT * FROM Party JOIN PartyUser ON PartyUser.PartyId = Party.Id AND UserId = @UserId";
            return _db.LoadData<PartyModel, dynamic>(sql, new { UserId = user.Id});
        }

        public Task InsertParty(PartyModel party)
        {
            string sql = @"insert into [dbo].[Party] (CreationDate,OwnerUserId,PartyName) VALUES (@CreationDate, @OwnerUserId, @PartyName)";
            return _db.SaveData(sql, party);
        }

        public Task InsertCategoryForParty(CategoryModel category, PartyModel party)
        {
            string sql = @"insert into [dbo].[PartyCategory] (PartyId,CategoryId) VALUES ( @PartyId, @CategoryId )";
            return _db.SaveData(sql, new { CategoryId = category.Id, PartyId = party.Id});
        }

        public Task InsertAreaForParty(AreaModel area, PartyModel party)
        {
            string sql = @"insert into [dbo].[PartyArea] (PartyId,AreaId) VALUES (  @PartyId, @AreaId )";
            return _db.SaveData(sql, new { AreaId = area.Id, PartyId = party.Id });
        }

        public Task InsertPartyUserInvite(UserModel user, PartyModel party)
        {
            string sql = @"insert into [dbo].[PartyUserInvite] (InvitedUserId, PartyId) VALUES (@UserId , @PartyId )";
            return _db.SaveData(sql, new { UserId = user.Id, PartyId = party.Id });
        }

        public Task InsertPartyUser(UserModel user, PartyModel party)
        {
            string sql = @"insert into [dbo].[PartyUser] (UserId, PartyId) VALUES ( @UserId, @PartyId )";
            return _db.SaveData(sql, new { UserId = user.Id, PartyId = party.Id});
        }

        public Task InsertPartyMealChoice(MealModel meal, PartyModel party, UserModel user)
        {
            string sql = @"insert into [dbo].[PartyMealChoice] ( UserId, PartyId, MealId )
                            VALUES ( @UserId, @PartyId, @MealId )";
            return _db.SaveData(sql, new { UserId = user.Id , PartyId = party.Id, MealId = meal.Id});
        }

        public Task<List<UserModel>> GetAllPartyMembers(PartyModel party, UserModel user)
        {
            string sql = @"SELECT * FROM AspNetUsers as users
                            WHERE users.Id IN(
	                            SELECT p.UserId FROM PartyUser as p
	                            WHERE p.PartyId = @partyId )";
            return _db.LoadData<UserModel, dynamic>(sql, new { partyId = party.Id  });
        }

        public Task<List<UserModel>> GetUsersInvitedToParty(PartyModel party, UserModel user)
        {
            string sql = @"SELECT * FROM AspNetUsers as users
                            WHERE users.Id IN(
	                            SELECT pv.InvitedUserId FROM PartyUserInvite as pv
	                            WHERE pv.PartyId = @partyId )";
            return _db.LoadData<UserModel, dynamic>(sql, new { partyId = party.Id });
        }

        public Task<List<UserModel>> GetNotInvitedNotMembersUsersToParty(PartyModel party, UserModel user)
        {
            string sql = @"SELECT * FROM AspNetUsers as users
                            WHERE users.Id NOT IN(
	                            SELECT pv.InvitedUserId FROM PartyUserInvite as pv
	                            WHERE pv.PartyId = @partyId
	                            UNION
	                            SELECT p.UserId FROM PartyUser as p
	                            WHERE p.PartyId =  @partyId )";
            return _db.LoadData<UserModel, dynamic>(sql, new { partyId = party.Id });
        }

        public Task RemoveUserFromParty(UserModel user, PartyModel party)
        {
            string sql = @"DELETE FROM [dbo].[PartyUser] WHERE UserId = @UserId AND PartyId = @PartyId";
            return _db.SaveData(sql, new { UserId = user.Id, PartyId = party.Id });
        }

        public Task RemovePartyUserInvite(UserModel user, PartyModel party)
        {
            string sql = @"DELETE FROM [dbo].[PartyUserInvite] WHERE InvitedUserId = @UserId AND PartyId = @PartyId";
            return _db.SaveData(sql, new {UserId = user.Id, PartyId = party.Id});
        }

        public Task<List<DisplayPartyInviteModel>> GetUserPendingDisplayInvites(UserModel user)
        {
            string sql = @"SELECT inv.InvitedUserId, inv.PartyId, p.PartyName FROM Party as p
                            JOIN (
	                            SELECT * FROM PartyUserInvite as pv WHERE InvitedUserId = @UserId )as inv
	                            ON inv.PartyId = p.Id";
            return _db.LoadData<DisplayPartyInviteModel, dynamic>(sql, new { UserId = user.Id });
        }

        public Task<List<PartyUserInviteModel>> GetUserPendingInvites(UserModel user)
        {
            string sql = @"SELECT * FROM PartyUserInvite as pv WHERE InvitedUserId = @UserId";
            return _db.LoadData<PartyUserInviteModel, dynamic>(sql, new { UserId = user.Id });
        }

        public async Task<PartyUserInviteModel> GetInviteByIds(string userId, int partyId)
        {
            string sql = @"SELECT * FROM PartyUserInvite WHERE InvitedUserId = @UserId AND PartyId = @PartyId";
            return await _db.LoadSingleResult<PartyUserInviteModel, dynamic>(sql, new { UserId = userId, PartyId = partyId});
        }

        public Task<List<UserModel>> GetFriendedAndNotMemeberListOfUsers(string userId, int partyId)
        {
            string sql = @"SELECT * FROM[dbo].[AspNetUsers] as users
                            WHERE users.Id <> @UserId AND users.Id IN(
                                 SELECT[FirstUserID] FROM [dbo].[AspFriendships]
                                 WHERE [SecondUserID]= @UserId 
                                 UNION
                                 SELECT [SecondUserID] FROM[dbo].[AspFriendships]
                                 WHERE [FirstUserID]= @UserId 
                            )
                            AND Id NOT IN(
                                SELECT InvitedUserId FROM PartyUserInvite

                                WHERE PartyId = @PartyId)
                            AND Id NOT IN(
                                SELECT UserId FROM PartyUser

                                WHERE PartyId = @PartyId )";
            return _db.LoadData<UserModel, dynamic>(sql, new { UserId = userId, PartyId = partyId });
        }



        public Task RemoveAllCategoriesFromParty(int partyId)
        {
            string sql = @"DELETE FROM PartyCategory WHERE PartyId = @PartyId";
            return _db.SaveData(sql, new { PartyId = partyId});
        }

        public Task RemoveAllMembersFromParty(int partyId)
        {
            string sql = @"DELETE FROM PartyUser WHERE PartyId = @PartyId";
            return _db.SaveData(sql, new { PartyId = partyId });
        }

        public Task RemoveAllAreasFromParty(int partyId)
        {
            string sql = @"DELETE FROM PartyArea WHERE PartyId = @PartyId";
            return _db.SaveData(sql, new { PartyId = partyId });
        }

        public Task RemoveAllInvitesToParty(int partyId)
        {
            string sql = @"DELETE FROM PartyUserInvite WHERE PartyId = @PartyId";
            return _db.SaveData(sql, new { PartyId = partyId });
        }

        public Task RemoveParty(int partyId)
        {
            string sql = @"DELETE FROM Party WHERE Id = @PartyId";
            return _db.SaveData(sql, new { PartyId = partyId });
        }

        public Task RemoveAllMealChoicesOfParty(int partyId)
        {
            string sql = @"DELETE FROM PartyMealChoice WHERE PartyId = @PartyId";
            return _db.SaveData(sql, new { PartyId = partyId });
        }

        public Task InsertPartyMeals(List<PartyMealModel> partyMeals)
        {
            string sql = @"INSERT INTO [dbo].[PartyMeal] (PartyId, MealId)
                           VALUES (@PartyId, @MealId)";
            return _db.SaveData(sql, partyMeals);
        }

        public Task<List<MealModel>> GetPartyMealsById(int partyId)
        {
            string sql = @"SELECT * FROM [dbo].[Meal]
                          INNER JOIN [PartyMeal] 
                          ON [PartyMeal].[PartyId] = @Id
                          AND [PartyMeal].[MealId] = [Meal].[Id]";
            return _db.LoadData<MealModel, dynamic>(sql, new { Id = partyId });
        }

        public Task<List<PartyMealChoiceModel>> GetUserPartyChoices(int partyId, string userId)
        {
            string sql = @"SELECT * FROM [dbo].[PartyMealChoice]
                           WHERE [PartyId] = @PartyId AND [UserId] = @UserId";
            return _db.LoadData<PartyMealChoiceModel, dynamic>(sql, new { PartyId = partyId, UserId = userId });
        }

        public Task RemoveAllMealsOfParty(int partyId)
        {
            string sql = @"DELETE FROM PartyMeal WHERE PartyId = @PartyId";
            return _db.SaveData(sql, new { PartyId = partyId });
        }

        public Task<int> GetNumberOfPartyMembersWhoVoted(int partyId)
        {
            throw new NotImplementedException();
        }
    }
}
