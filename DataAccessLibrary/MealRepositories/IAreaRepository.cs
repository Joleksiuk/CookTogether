using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.MealRepositories
{ 
    public interface IAreaRepository
    {
        Task<List<AreaModel>> GetAreas();
        Task<AreaModel> GetAreaByName(string name);
        Task InsertAreasIfNotExists(List<AreaModel> areaModels);
    }
}
