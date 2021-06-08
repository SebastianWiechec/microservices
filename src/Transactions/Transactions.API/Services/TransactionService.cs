using Microsoft.EntityFrameworkCore;
using SpendingsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionApi.IServices;
using TransactionsApi.Data;
using TransactionsApi.Models;

namespace TransactionApi.Services
{
    public class TransactionService : ITransactionService
    {
        ApplicationDbContext dbContext;

        public TransactionService(ApplicationDbContext _db)
        {
            dbContext = _db;
        }

        ///<summary>
        /// metoda dodająca nową transakcję
        ///</summary>
        public async Task<string> AddTransaction(Transaction Transaction)
        {
            try
            {
                dbContext.Transaction.Add(Transaction);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return await Task.FromResult(e.ToString());
            }

            var newCar = new UserCars();
            newCar.AspNetUsers_Id = Transaction.User;
            newCar.DB_Car_idCar = Transaction.Car;

            var car = await dbContext.Car.FindAsync(Transaction.Car);
            car.IsAvailable = 0;
            dbContext.Car.Update(car);

            dbContext.UserCars.Add(newCar);

            await dbContext.SaveChangesAsync();

            return await Task.FromResult("OK");
        }

        public async Task<string> DeleteTransaction(int id)
        {
            var Transaction = dbContext.Transaction.FirstOrDefault(x => x.idTransactions == id);
            dbContext.Entry(Transaction).State = EntityState.Deleted;
            dbContext.SaveChanges();
            return await Task.FromResult("OK");
        }

        public async Task<Transaction> GetTransactionByIdAsync(int idTransactions)
        {
            var Transaction = await dbContext.Transaction.FindAsync(idTransactions);

            return Transaction;
        }

        public async Task<IList<Transaction>> GetTransactions()
        {
            return await dbContext.Transaction.ToListAsync();
        }

        public Transaction UpdateTransaction(Transaction transaction)
        {
            dbContext.Entry(transaction).State = EntityState.Modified;
            dbContext.SaveChanges();
            return transaction;
        }

    }
}
