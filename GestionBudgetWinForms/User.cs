using System;

namespace GestionBudgetWinForms
{
    public class User
    {
        public int Id { get; set; }           // maps to "id" column in MySQL
        public string Username { get; set; }  // maps to "username" column
        public string PasswordHash { get; set; } // store hashed password
        
    }
}
