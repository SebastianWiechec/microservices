using CostsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostsApi.IServices
{
    public interface ICostsService
    {
        Task<IEnumerable<Costs>> GetCosts();
        Task<Costs> GetCostsByIdAsync(int id);
        Task<string> AddCosts(Costs costs);
        Costs UpdateCosts(Costs costs);
        Task<string> DeleteCosts(int id);

    }
}
