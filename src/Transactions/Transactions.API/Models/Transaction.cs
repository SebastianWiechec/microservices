using System;
using System.ComponentModel.DataAnnotations;

namespace TransactionsApi.Models
{
    public class Transaction
    {
        [Key]
        public int idTransactions { get; set; }
        public string User { get; set; }
        public int Car { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float Price { get; set; }
        public bool IsEnd { get; set; }
        public bool IsReturned { get; set; }
    }
}