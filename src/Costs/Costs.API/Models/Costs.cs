using System.ComponentModel.DataAnnotations;

namespace CostsApi.Models
{
    public class Costs
    {
        [Key]
        public int idCosts { get; set; }        //numer id kosztów
        public string Description { get; set; } //opis
    }
}
