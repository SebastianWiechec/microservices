using Microsoft.EntityFrameworkCore;
using SpendingsApi.Data;
using SpendingsApi.IServices;
using SpendingsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SpendingsApi.Services
{
    public class SpendingsService : ISpendingsService

    {
        ApplicationDbContext dbContext;

        public SpendingsService(ApplicationDbContext _db)
        {
            dbContext = _db;
        }

        /// <summary>
        /// Metoda dodająca rekord w tabeli Spendings
        /// </summary>
        /// <param name="spendings"></param>
        /// <returns></returns>
        public async Task<string> AddSpendings(Spendings spendings)
        {
            dbContext.Spendings.Add(spendings);
            await dbContext.SaveChangesAsync();
            Serilog.Log.Information($@"Dodano nowe wydatki!, User: {spendings.idUser}, Date: {spendings.Date}, Car: {spendings.CarID}");
            return await Task.FromResult("OK");

        }

        /// <summary>
        /// Metoda usuwająca rekord w tabeli Spendings
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> DeleteSpendings(int id)
        {
            var spendings = dbContext.Spendings.FirstOrDefault(x => x.idSpendings == id);
            dbContext.Entry(spendings).State = EntityState.Deleted;

            dbContext.SaveChanges();
            return await Task.FromResult("");
        }

        /// <summary>
        /// Metoda pobierająca rekordy z tabeli Spendings - by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<Spendings>> GetSpendingsByIdAsync(string id)
        {
            var spendings = await dbContext.Spendings.Where(s => s.idUser == id).ToListAsync();

            return spendings;
        }

        public async Task<List<CarApi.Models.Car>> GetUserCars(string id)
        {
            var userCars = dbContext.UserCars.Where(uc => uc.AspNetUsers_Id == id).ToList();

            var cars = await dbContext.Car.ToListAsync();
            cars = cars.Where(c => userCars.Any(us => c.idCar == us.DB_Car_idCar)).ToList();

            return cars;
        }

        /// <summary>
        /// Metoda pobierająca rekordy z tabeli Spendings
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Spendings>> GetSpendings()
        {
            return await dbContext.Spendings.ToListAsync();
        }

        /// <summary>
        /// Metoda modyfikująca rekord w tabeli Spendings
        /// </summary>
        /// <param name="spendings"></param>
        /// <returns></returns>
        public Spendings UpdateSpendings(Spendings spendings)
        {
            dbContext.Entry(spendings).State = EntityState.Modified;
            dbContext.SaveChanges();
            return spendings;
        }
        public List<Log> GetLogsById(string id)
        {
            var logs = dbContext.Logs.Where(s => s.Message.Contains(id)).ToList();

            return logs;
        }

        public Tuple<Task<string>, Task<string>> SetNames(int idCar, int idCost)
        {
            var carName = Task.FromResult(dbContext.Car.Where(c => c.idCar == idCar).FirstOrDefault().Model);
            var costName = Task.FromResult(dbContext.Costs.Where(c => c.idCosts == idCar).FirstOrDefault().Description);

            return new Tuple<Task<string>, Task<string>>(carName, costName);
        }

    }
}
