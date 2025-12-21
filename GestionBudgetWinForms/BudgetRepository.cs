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
                // simple query to validate proper connectivity
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
            // Use generic builder to avoid provider-specific enum parsing (SslMode)
            var builder = new DbConnectionStringBuilder { ConnectionString = connectionString };

            string databaseName = null;
            if (builder.ContainsKey("Database"))
                databaseName = builder["Database"] as string;
            else if (builder.ContainsKey("Initial Catalog"))
                databaseName = builder["Initial Catalog"] as string;

            // Create server connection string by removing the database key
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

            // Créer la table si elle n'existe pas
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Transactions (
                    Id INT PRIMARY KEY AUTO_INCREMENT,
                    Description VARCHAR(200) NOT NULL,
                    Montant DECIMAL(18,2) NOT NULL,
                    Type VARCHAR(50) NOT NULL,
                    Date DATETIME NOT NULL,
                    Categorie VARCHAR(100) NOT NULL
                ) ENGINE=InnoDB;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(createTableQuery, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Ajouter une transaction
        public int AjouterTransaction(Transaction transaction)
        {
            string query = @"INSERT INTO Transactions (Description, Montant, Type, Date, Categorie) 
                           VALUES (@Description, @Montant, @Type, @Date, @Categorie); 
                           SELECT LAST_INSERT_ID();";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Description", transaction.Description);
                cmd.Parameters.AddWithValue("@Montant", transaction.Montant);
                cmd.Parameters.AddWithValue("@Type", transaction.Type);
                cmd.Parameters.AddWithValue("@Date", transaction.Date);
                cmd.Parameters.AddWithValue("@Categorie", transaction.Categorie);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        // Modifier une transaction existante
        public void ModifierTransaction(int id, Transaction transaction)
        {
            string query = @"UPDATE Transactions SET Description = @Description, Montant = @Montant, Type = @Type, Date = @Date, Categorie = @Categorie WHERE Id = @Id";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Description", transaction.Description);
                cmd.Parameters.AddWithValue("@Montant", transaction.Montant);
                cmd.Parameters.AddWithValue("@Type", transaction.Type);
                cmd.Parameters.AddWithValue("@Date", transaction.Date);
                cmd.Parameters.AddWithValue("@Categorie", transaction.Categorie);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Obtenir toutes les transactions
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

        // Calculer le solde total
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
                return result != DBNull.Value ? Convert.ToDecimal(result) :0;
            }
        }

        // Supprimer une transaction
        public void SupprimerTransaction(int id)
        {
            string query = "DELETE FROM Transactions WHERE Id = @Id";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Calculer total revenus
        public decimal CalculerTotalRevenus()
        {
            string query = "SELECT IFNULL(SUM(Montant),0) FROM Transactions WHERE Type = 'Revenue'";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) :0;
            }
        }

        // Calculer total dépenses
        public decimal CalculerTotalDepenses()
        {
            string query = "SELECT IFNULL(SUM(Montant),0) FROM Transactions WHERE Type = 'Depense'";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) :0;
            }
        }
    }
}