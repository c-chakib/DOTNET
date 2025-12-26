using System;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace GestionBudgetWinForms
{
    public class BudgetRepository
    {
        private string connectionString;

        public BudgetRepository(string connString)
        {
            connectionString = connString;
        }

        // Test de connexion simple
        public void TesterConnexion()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT 1", conn))
                {
                    cmd.ExecuteScalar();
                }
                conn.Close();
            }
        }

        // Créer la base (si nécessaire) et les tables si elles n'existent pas
        public void InitialiserBaseDeDonnees()
        {
            var builder = new DbConnectionStringBuilder { ConnectionString = connectionString };
            string databaseName = null;
            if (builder.ContainsKey("Database"))
                databaseName = builder["Database"] as string;
            else if (builder.ContainsKey("Initial Catalog"))
                databaseName = builder["Initial Catalog"] as string;

            var serverBuilder = new DbConnectionStringBuilder { ConnectionString = connectionString };
            if (databaseName != null)
            {
                if (serverBuilder.ContainsKey("Database")) serverBuilder.Remove("Database");
                if (serverBuilder.ContainsKey("Initial Catalog")) serverBuilder.Remove("Initial Catalog");
            }

            string serverConnectionString = serverBuilder.ConnectionString;

            using (MySqlConnection serverConn = new MySqlConnection(serverConnectionString))
            {
                serverConn.Open();
                using (var cmd = serverConn.CreateCommand())
                {
                    if (!string.IsNullOrEmpty(databaseName))
                    {
                        cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{databaseName}` CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;";
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            // Créer la table Transactions
            string createTransactionsTable = @"
                CREATE TABLE IF NOT EXISTS Transactions (
                Id INT PRIMARY KEY AUTO_INCREMENT,
                Description VARCHAR(200) NOT NULL,
                Montant DECIMAL(18,2) NOT NULL,
                Type VARCHAR(50) NOT NULL,
                Date DATETIME NOT NULL,
                Categorie VARCHAR(100) NOT NULL,
                UserId INT NOT NULL,
                CONSTRAINT FK_Transactions_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
            ) ENGINE=InnoDB;
            ";


            // Créer la table Users
            string createUsersTable = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INT PRIMARY KEY AUTO_INCREMENT,
                    Username VARCHAR(50) NOT NULL UNIQUE,
                    Password VARCHAR(255) NOT NULL
                ) ENGINE=InnoDB;";
            // 1️⃣ Users table
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(createUsersTable, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // 2️⃣ Transactions table
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(createTransactionsTable, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }

        // ----------------- Transactions -----------------

        public int AjouterTransaction(Transaction transaction)
        {
            string query = @"
        INSERT INTO Transactions 
        (Description, Montant, Type, Date, Categorie, UserId)
        VALUES 
        (@Description, @Montant, @Type, @Date, @Categorie, @UserId);
        SELECT LAST_INSERT_ID();
    ";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Description", transaction.Description);
                cmd.Parameters.AddWithValue("@Montant", transaction.Montant);
                cmd.Parameters.AddWithValue("@Type", transaction.Type);
                cmd.Parameters.AddWithValue("@Date", transaction.Date);
                cmd.Parameters.AddWithValue("@Categorie", transaction.Categorie);
                cmd.Parameters.AddWithValue("@UserId", transaction.UserId);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }


        public void ModifierTransaction(int id, Transaction transaction)
        {
            string query = @"UPDATE Transactions 
        SET Description = @Description, 
            Montant = @Montant, 
            Type = @Type, 
            Date = @Date, 
            Categorie = @Categorie
        WHERE Id = @Id AND UserId = @UserId";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Description", transaction.Description);
                cmd.Parameters.AddWithValue("@Montant", transaction.Montant);
                cmd.Parameters.AddWithValue("@Type", transaction.Type);
                cmd.Parameters.AddWithValue("@Date", transaction.Date);
                cmd.Parameters.AddWithValue("@Categorie", transaction.Categorie);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@UserId", transaction.UserId); // 🔒

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public DataTable ObtenirToutesLesTransactions()
        {
            string query = "SELECT * FROM Transactions ORDER BY Date DESC";
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                adapter.Fill(dt);
            }

            return dt;
        }

        public void SupprimerTransaction(int id, int userId)
        {
            string query = "DELETE FROM Transactions WHERE Id = @Id AND UserId = @UserId";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@UserId", userId); // 🔒

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public decimal CalculerSolde()
        {
            string query = @"SELECT (
                IFNULL((SELECT SUM(Montant) FROM Transactions WHERE Type = 'Revenue'),0) -
                IFNULL((SELECT SUM(Montant) FROM Transactions WHERE Type = 'Depense'),0)
            ) AS Solde";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        public decimal CalculerTotalRevenus()
        {
            string query = "SELECT IFNULL(SUM(Montant),0) FROM Transactions WHERE Type = 'Revenue'";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        public decimal CalculerTotalDepenses()
        {
            string query = "SELECT IFNULL(SUM(Montant),0) FROM Transactions WHERE Type = 'Depense'";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }
        public DataTable ObtenirTransactionsParUser(int userId)
        {
            string query = "SELECT * FROM Transactions WHERE UserId = @UserId ORDER BY Date DESC";
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            return dt;
        }



        // ----------------- Users -----------------

        public void AjouterUser(User user)
        {
            string hashedPassword = PasswordHelper.HashPassword(user.PasswordHash);

            string query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public bool UserExists(string username)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }


        public User GetUserByUsername(string username)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM Users WHERE Username=@Username";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Username = reader["Username"].ToString(),
                            PasswordHash = reader["Password"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        // Vérifie si l'utilisateur peut se connecter
        public bool Login(string username, string password, out User loggedInUser)
        {
            loggedInUser = GetUserByUsername(username);
            if (loggedInUser != null)
            {
                return PasswordHelper.VerifyPassword(password, loggedInUser.PasswordHash);
            }
            return false;
        }

        public void CreateDefaultUser()
        {
            User existing = GetUserByUsername("admin");
            if (existing == null)
            {
                AjouterUser(new User { Username = "admin", PasswordHash = "1234" });
            }
        }
    }
}
