using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class Repositories
    {
        public IAreaRepository AreaRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IIngredientRepository IngredientRepository { get; }
        public IMealRepository MealRepository { get; }

        public Repositories(IAreaRepository areaRepository,
                            ICategoryRepository categoryRepository,
                            IIngredientRepository ingredientRepository,
                            IMealRepository mealRepository)
        {
            AreaRepository = areaRepository;
            CategoryRepository = categoryRepository;
            IngredientRepository = ingredientRepository;
            MealRepository = mealRepository;
        }
    }
}
