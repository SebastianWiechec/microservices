using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpendingsApi.Models
{
    /// <summary>
    /// Obiekt Spendings odpowiadający danym z bazy
    /// </summary>
    public class Spendings
    {
        [Key]
        public int idSpendings { get; set; }  //numer id wydatków
        public DateTime Date { get; set; }    //Data
        public int CarID { get; set; }        //numer id auta
        public int CostID { get; set; }       //numer id kosztów
        public double Price { get; set; }     //Cena
        public string idUser { get; set; }     //Id użytkownika
    }

    [Table("AspNetUsers_has_DB_Car")]
    public class UserCars
    {
        [Key]
        public int Id { get; set; }
        public string AspNetUsers_Id { get; set; }
        public int DB_Car_idCar { get; set; }
    }
}
