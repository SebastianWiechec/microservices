using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionApi.IServices;
using TransactionsApi.Models;

namespace TransactionsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private ITransactionService transactionService { get; }

        public TransactionsController(ITransactionService _transactionService)
        {
            transactionService = _transactionService;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<IEnumerable<Transaction>> GetTransaction()
        {
            return await transactionService.GetTransactions();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await transactionService.GetTransactionByIdAsync(id);
        }

        // PUT: api/Transactions/5
        // metoda obsługująca request PUT dla wybranego Id - api/Transactions
        [HttpPut("{id}")]
        public Transaction PutTransaction([FromBody] Transaction transaction)
        {
            return transactionService.UpdateTransaction(transaction);
        }

        // POST: api/Transactions
        // metoda dodająca transakcję
        [HttpPost]
        public async Task<String> PostTransaction([FromBody] Transaction transaction)
        {
            return await transactionService.AddTransaction(transaction);
        }

        ///<summary>
        /// metoda usuwająca transakcję
        ///</summary>
        [HttpDelete("{id}")]
        public async Task<string> DeleteTransaction(int id)
        {
            return await transactionService.DeleteTransaction(id);
        }
    }
}
