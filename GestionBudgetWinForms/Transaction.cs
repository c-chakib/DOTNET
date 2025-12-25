using System;

namespace GestionBudgetWinForms
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Montant { get; set; }
        public string Type { get; set; } 
        public DateTime Date { get; set; }
        public string Categorie { get; set; }
        public int UserId { get; set; }   

    }
}