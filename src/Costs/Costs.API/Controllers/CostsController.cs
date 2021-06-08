using CostsApi.Data;
using CostsApi.IServices;
using CostsApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private ICostsService costsService { get; }

        public CostsController(ICostsService _costsService)
        {
            costsService = _costsService;
        }

        /// <summary>
        /// metoda obsługująca request GET dla api/Costs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Costs>> GetCosts()
        {
            return await costsService.GetCosts();
        }

        /// <summary>
        /// metoda obsługująca request GET dla wybranego Id - api/Costs
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Costs> GetCosts(int id)
        {
            return await costsService.GetCostsByIdAsync(id);
        }

        /// <summary>
        /// metoda obsługująca request PUT dla wybranego Id - api/Costs
        /// </summary>
        /// <param name="costs"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public Costs PutCosts([FromBody] Costs costs)
        {
            return costsService.UpdateCosts(costs);
        }

        /// <summary>
        /// metoda obsługująca request POST dla api/Costs
        /// </summary>
        /// <param name="costs"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<String> PostCosts([FromBody] Costs costs)
        {
            return await costsService.AddCosts(costs);
        }

        /// <summary>
        /// metoda obsługująca request DELETE dla wybranego Id - api/Costs
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<String> DeleteCosts(int id)
        {
            return await costsService.DeleteCosts(id);
        }

        /// <summary>
        /// metoda sprawdzająca czy rekord istnieje
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool CostsExists(int id)
        {
            return _context.Costs.Any(e => e.idCosts == id);
        }
    }
}
