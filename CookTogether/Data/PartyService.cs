using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DataAccessLibrary;
using DataAccessLibrary.Models;
using CookTogether.Models;
using DataAccessLibrary.MealRepositories;

namespace CookTogether.Data
{
    public class PartyService
    {
        private readonly MealRepositories mealRepositories;
        private readonly IPartyRepository partyRepository;
        private readonly IUserRepository userRepository;

        public PartyService(            
                IPartyRepository partyRepository,
                IUserRepository userRepository,
                MealRepositories mealRepositories
            )
        {
            this.partyRepository = partyRepository;
            this.userRepository = userRepository;
            this.mealRepositories = mealRepositories;
        }

        public void AddUserToParty(UserModel user, PartyModel party)
        {
            partyRepository.InsertPartyUser(user, party);
        }

        public void RemoveUserFromParty(UserModel user, PartyModel party)
        {
            partyRepository.RemoveUserFromParty(user, party);
        }

        public void InviteUserToParty(UserModel user, PartyModel party)
        {
            partyRepository.InsertPartyUserInvite(user, party);
        }

        public void CancelUserInvitationToParty(UserModel user, PartyModel party)
        {
            partyRepository.RemovePartyUserInvite(user, party);
        }

        public void RemoveParty(int partyId)
        {
            partyRepository.RemoveAllAreasFromParty(partyId);
            partyRepository.RemoveAllCategoriesFromParty(partyId);
            partyRepository.RemoveAllInvitesToParty(partyId);
            partyRepository.RemoveAllMembersFromParty(partyId);
            partyRepository.RemoveAllMealChoicesOfParty(partyId);
            partyRepository.RemoveParty(partyId);

        }
    }
}
