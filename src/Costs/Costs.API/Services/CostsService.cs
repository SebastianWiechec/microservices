using CostsApi.Data;
using CostsApi.IServices;
using CostsApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsApi.Services
{
    public class CostsService : ICostsService

    {
        ApplicationDbContext dbContext;
        public CostsService(ApplicationDbContext _db)
        {
            dbContext = _db;
        }

        /// <summary>
        /// Metoda dodająca rekord w tabeli Costs
        /// </summary>
        /// <param name="costs"></param>
        /// <returns></returns>
        public async Task<string> AddCosts(Costs costs)
        {
            dbContext.Costs.Add(costs);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult("");
        }

        /// <summary>
        /// Metoda usuwająca rekord w tabeli Costs
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> DeleteCosts(int id)
        {

            var car = dbContext.Costs.FirstOrDefault(x => x.idCosts == id);
            dbContext.Entry(car).State = EntityState.Deleted;

            dbContext.SaveChanges();
            return await Task.FromResult("");
        }

        /// <summary>
        /// Metoda pobierająca rekordy z tabeli Costs - by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Costs> GetCostsByIdAsync(int id)
        {
            var costs = await dbContext.Costs.FindAsync(id);

            return costs;
        }

        /// <summary>
        /// Metoda pobierająca rekordy z tabeli Costs
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Costs>> GetCosts()
        {
            return await dbContext.Costs.ToListAsync();
        }
        /// <summary>
        /// Metoda modyfikująca rekord w tabeli Costs
        /// </summary>
        /// <param name="costs"></param>
        /// <returns></returns>
        public Costs UpdateCosts(Costs costs)
        {
            dbContext.Entry(costs).State = EntityState.Modified;
            dbContext.SaveChanges();
            return costs;
        }
    }
}