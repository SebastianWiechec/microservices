using CarApi.Models;
using SpendingsApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendingsApi.IServices
{
    public interface ISpendingsService
    {
        Task<IList<Spendings>> GetSpendings();
        Task<List<Spendings>> GetSpendingsByIdAsync(string id);
        Task<List<Car>> GetUserCars(string id);
        Task<string> AddSpendings(Spendings spendings);
        Spendings UpdateSpendings(Spendings spendings);
        Task<string> DeleteSpendings(int id);
        List<Log> GetLogsById(string id);
        Tuple<Task<string>, Task<string>> SetNames(int idCar, int idCost);
    }
}
