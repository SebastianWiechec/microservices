using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarApi.Models
{
    /// <summary>
    /// Obiekt Car odpowiadający danym z bazy
    /// </summary>
    /// 
    public class Car
    {
        [Key]
        public int idCar { get; set; }              ///Numer kolejny auta - autoinkrementacja
        public string Manufacturer { get; set; }    ///Marka
        public string Model { get; set; }           ///Model
        public string Color { get; set; }           ///kolor
        public int YofProd { get; set; }            ///Rok produkcji
        public int Kilometers { get; set; }         ///Przebieg w km
        public double PriceDay { get; set; }        ///Cena za dzień wypożyczenia
        public int IsAvailable { get; set; }        ///czy dostępny - jeżeli wypożyczony - 0 
        public DateTime Insurance { get; set; }     ///Data ważności ubezpieczenia
        public int Segment { get; set; }            ///Segment aut (np. Small/Compact itp.)
        public string RegNumbers { get; set; }      ///numer rejestracyjny
        public string FilePath { get; set; }        ///ścieżka do dodatkowych danych np. zdjęcia itp.
        public DateTime TechRev { get; set; }       ///Data ważności przeglądu
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
