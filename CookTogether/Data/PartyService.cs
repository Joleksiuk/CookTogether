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

        private static readonly int MIN_MEALS_PER_PARTY_NUMBER = 5;
        private static readonly int MAX_MEALS_PER_PARTY_NUMBER = 20;

        public PartyService(            
                IPartyRepository partyRepository,
                IUserRepository userRepository,
                MealRepositories mealRepositories
            )
        {
            this.partyRepository = partyRepository;
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

        public async Task<PartyModel> CreateParty(List<CategoryModel> categories, List<AreaModel> areas, string name, string ownerId, DateTime currentDate)
        {
            PartyModel partyModel = new PartyModel
            {
                CreationDate = currentDate,
                OwnerUserId = ownerId,
                PartyName = name
            };

            await partyRepository.InsertParty(partyModel);
            PartyModel newPartyModel = await partyRepository.GetLatestUserParty(ownerId);

            await partyRepository.InsertPartyUser(new UserModel { Id = ownerId }, newPartyModel);

            foreach (CategoryModel category in categories)
            {
                await partyRepository.InsertCategoryForParty(category, newPartyModel);
            }

            foreach (AreaModel area in areas)
            {
                await partyRepository.InsertAreaForParty(area, newPartyModel);
            }

            List<MealModel> partyMeals = await FindMealsForParty(categories, areas);
            List<PartyMealModel> partyMealModels = new();
            foreach(var mealModel in partyMeals)
            {
                partyMealModels.Add(new PartyMealModel
                {
                    MealId = mealModel.Id,
                    PartyId = newPartyModel.Id
                });
            }

            await partyRepository.InsertPartyMeals(partyMealModels);

            return newPartyModel;
        }

        private async Task<List<MealModel>> FindMealsForParty(List<CategoryModel> categories, List<AreaModel> areas)
        {
            List<int> categoryIds = new();
            List<int> areaIds = new();
            categories.ForEach(category => categoryIds.Add(category.Id));
            areas.ForEach(area => areaIds.Add(area.Id));

            Random rand = new Random(DateTime.Now.Millisecond);

            List<MealModel> meals = await mealRepositories.MealRepository.GetMealsByCategoriesAndAreas(categoryIds, areaIds);
            return meals.OrderBy(x => rand.Next()).Take(MAX_MEALS_PER_PARTY_NUMBER).ToList<MealModel>();
        }

        public async Task<List<MealModel>> GetPartyMealsForUser(int partyId, string userId)
        {
            var mealsTask = partyRepository.GetPartyMealsById(partyId);
            var choicesTask = partyRepository.GetUserPartyChoices(partyId, userId);

            await Task.WhenAll(mealsTask, choicesTask);

            List<MealModel> meals = mealsTask.Result;
            List<PartyMealChoiceModel> choices = choicesTask.Result;

            meals.RemoveAll(meal => choices.Any(choice => choice.MealId == meal.Id));
            return meals;
        }

        public async Task SaveChoice(int partyId, string userId, int mealId, bool choice)
        {
            PartyMealChoiceModel choiceModel = new PartyMealChoiceModel
            {
                PartyId = partyId,
                UserId = userId,
                MealId = mealId,
                Picked = choice
            };
            await partyRepository.InsertPartyMealChoice(choiceModel);
        }

        public async Task<bool> CategoriesAndAreasHaveMinNumOfMeals(List<CategoryModel> categories, List<AreaModel> areas)
        {
            List<int> categoryIds = new();
            List<int> areaIds = new();
            categories.ForEach(category => categoryIds.Add(category.Id));
            areas.ForEach(area => areaIds.Add(area.Id));

            List<MealModel> meals = await mealRepositories.MealRepository.GetMealsByCategoriesAndAreas(categoryIds, areaIds);
            return meals.Count >= MIN_MEALS_PER_PARTY_NUMBER;
        }
    }
}
