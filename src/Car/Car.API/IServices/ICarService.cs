using CarApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarApi.IServices
{
    public interface ICarService
    {
        Task<IList<Car>> GetCars();
        Task<Car> GetCarByIdAsync(int IdCar);
        Task<string> AddCar(Car car);
        Car UpdateCar(Car car);
        Task<string> DeleteCar(int IdCar);

    }
}
