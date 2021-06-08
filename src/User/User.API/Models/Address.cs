using System.ComponentModel.DataAnnotations;

namespace UserApi.Models
{
    public class Address
    {
        public int idAdresses { get; set; }
        [Key]
        public string AspNetUsersID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
    }
}
