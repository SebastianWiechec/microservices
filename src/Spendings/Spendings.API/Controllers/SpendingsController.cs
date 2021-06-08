using CarApi.Models;
using Microsoft.AspNetCore.Mvc;
using SpendingsApi.IServices;
using SpendingsApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendingsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpendingsController : ControllerBase
    {
        private ISpendingsService spendingsService { get; }
        private IEmailService emailService { get; }
        public SpendingsController(ISpendingsService _spendingsService, IEmailService email)
        {
            spendingsService = _spendingsService;
            emailService = email;
        }

        /// <summary>
        /// metoda obsługująca request GET dla api/Spendings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IList<Spendings>> GetSpendings()
        {
            return await spendingsService.GetSpendings();
        }

        // GET api/<SpendingsController>/spending/5
        [HttpGet]
        [Route("[action]/{id}")]
        public  List<Log> GetLogs(string id)
        {
            return  spendingsService.GetLogsById(id);
        }

        /// <summary>
        /// metoda obsługująca request GET dla wybranego Id - api/Spendings
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<SpendingsController>/spending/5
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<List<Spendings>> GetSpendings(string id)
        {
            return await spendingsService.GetSpendingsByIdAsync(id);
        }

        // GET api/<SpendingsController>/usercars/5
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<List<Car>> GetUserCars(string id)
        {
            return await spendingsService.GetUserCars(id);
        }

        /// <summary>
        /// metoda obsługująca request PUT dla wybranego Id - api/Spendings
        /// </summary>
        /// <param name="spendings"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public Spendings PutSpendings([FromBody] Spendings spendings)
        {
            return spendingsService.UpdateSpendings(spendings);
        }

        /// <summary>
        /// metoda obsługująca request POST dla api/Spendings
        /// </summary>
        /// <param name="spendings"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task EmailAsync([FromBody] Email email)
        {
            var spendings = await spendingsService.GetSpendingsByIdAsync(email.IdUser);

            var emailSenderDecorator = new EmailServiceDecorator(emailService, spendingsService);
            email.Html += emailSenderDecorator.CreateHTMLTableAsync(spendings);

            emailService.Send(email);
        }

        /// <summary>
        /// metoda obsługująca request POST dla api/Spendings
        /// </summary>
        /// <param name="spendings"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<String> AddSpending([FromBody] Spendings spendings)
        {
            return await spendingsService.AddSpendings(spendings);
        }

        /// <summary>
        ///  metoda obsługująca request DELETE dla wybranego Id - api/Spendings
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<String> DeleteSpendings(int id)
        {
            return await spendingsService.DeleteSpendings(id);
        }
    }
}
