using CarApi.Data;
using CarApi.IServices;
using CarApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarApi.Services
{
    public class CarService : ICarService
    {
        ApplicationDbContext dbContext;

        public CarService(ApplicationDbContext _db)
        {
            dbContext = _db;
        }

        /// <summary>
        /// Metoda dodająca rekord w tabeli Car
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public async Task<string> AddCar(Car car)
        {
            dbContext.Car.Add(car);
            await dbContext.SaveChangesAsync();
            return await Task.FromResult("");
        }

        /// <summary>
        /// Metoda usuwająca rekord w tabeli Car
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> DeleteCar(int id)
        {
            var car = dbContext.Car.FirstOrDefault(x => x.idCar == id);
            dbContext.Entry(car).State = EntityState.Deleted;
            dbContext.SaveChanges();
            return await Task.FromResult("");
        }

        /// <summary>
        /// Metoda pobierająca rekordy z tabeli Car - by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Car> GetCarByIdAsync(int id)
        {
            var car = await dbContext.Car.FindAsync(id);

            return car;
        }

        /// <summary>
        /// Metoda pobierająca rekordy z tabeli Car
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Car>> GetCars()
        {
            return await dbContext.Car.ToListAsync();
        }

        /// <summary>
        /// Metoda modyfikująca rekord w tabeli Car
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public Car UpdateCar(Car car)
        {
            dbContext.Entry(car).State = EntityState.Modified;
            dbContext.SaveChanges();

            var userCar = dbContext.UserCars.FirstOrDefault(x => x.DB_Car_idCar == car.idCar);

            if (userCar != null)
                dbContext.Entry(userCar).State = EntityState.Deleted;

            dbContext.SaveChanges();
            return car;
        }

    }
}
