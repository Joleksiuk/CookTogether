
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
            string sql = @"SELECT * FROM Party WHERE Id = '" + id + "'";
            return await _db.LoadSingleResult<PartyModel, dynamic>(sql, new { });
        }
        public async Task<PartyModel> GetLatestUserParty(string userID)
        {
            string sql = @"SELECT TOP 1 * FROM Party WHERE OwnerUserId= '"+userID+"' ORDER BY CreationDate DESC";
            return await _db.LoadSingleResult<PartyModel, dynamic>(sql, new { });
        }

        public Task<List<PartyModel>> GetUserParties(UserModel user)
        {
            string sql = @"SELECT * FROM Party WHERE OwnerUserId = '"+ user.Id + "'";
            return _db.LoadData<PartyModel, dynamic>(sql, new { });
        }

        public Task InsertParty(PartyModel party)
        {
            string sql = @"insert into [dbo].[Party] (CreationDate,OwnerUserId,PartyName)
                            VALUES (@CreationDate, @OwnerUserId, @PartyName)";
            return _db.SaveData(sql, party);
        }

        public Task InsertCategoryForParty(CategoryModel category, PartyModel party)
        {
            string sql = @"insert into [dbo].[PartyCategory] (PartyId,CategoryId)
                            VALUES ( " +category.Id+ "," + party.Id +" )";
            return _db.SaveData(sql, party);
        }

        public Task InsertAreaForParty(AreaModel area, PartyModel party)
        {
            string sql = @"insert into [dbo].[PartyArea] (PartyId,CategoryId)
                            VALUES ( " + area.Id + "," + party.Id + " )";
            return _db.SaveData(sql, party);
        }

        public Task InsertPartyUserInvite(UserModel user, PartyModel party)
        {
            string sql = @"insert into [dbo].[PartyUserInvite] VALUES ( '"+user.Id+"', "+party.Id+" )";
            return _db.SaveData(sql, new { user, party });
        }

        public Task InsertPartyUser(UserModel user, PartyModel party)
        {
            string sql = @"insert into [dbo].[PartyUser] VALUES ( '"+user.Id+"', "+ party.Id+" )";
            return _db.SaveData(sql, new { user, party});
        }

        public Task InsertPartyMealChoice(MealModel meal, PartyModel party, UserModel user)
        {
            string sql = @"insert into [dbo].[PartyMealChoice] ( UserId, PartyId, MealId )
                            VALUES ( " + user.Id + "," + party.Id + ","+ meal.Id+" )";
            return _db.SaveData(sql, party);
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
            string sql = @"SELECT * FROM PartyUserInvite WHERE InvitedUserId = '" + userId + "'" + " AND PartyId = "+partyId;
            return await _db.LoadSingleResult<PartyUserInviteModel, dynamic>(sql, new { });
        }
    }
}
