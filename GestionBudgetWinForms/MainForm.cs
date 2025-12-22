using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;

namespace GestionBudgetWinForms
{
    public partial class MainForm : Form
    {
        private BudgetRepository repository;
        private int editingId = 0; // 0 = no edit

        // Contrôles UI
        private DataGridView dgvTransactions;
        private TextBox txtDescription;
        private TextBox txtMontant;
        private ComboBox cmbType;
        private ComboBox cmbCategorie;
        private DateTimePicker dtpDate;
        private Button btnAjouter;
        private Button btnSupprimer;
        private Button btnRafraichir;
        private Button btnRapport;
        private Button btnModifier;
        private Button btnAnnuler;
        private Label lblSolde;
        private Label lblRevenus;
        private Label lblDepenses;
        private GroupBox grpAjouter;
        private GroupBox grpStatistiques;

        public MainForm()
        {
            InitializeComponent();

            // Lire la chaîne de connexion depuis App.config (DefaultConnection)
            var cs = ConfigurationManager.ConnectionStrings["DefaultConnection"];
            string configConnection = cs?.ConnectionString;

            // Allow override from environment variable (for Docker): DEFAULT_CONNECTION
            string envConn = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");
            string connectionString = !string.IsNullOrWhiteSpace(envConn) ? envConn : configConnection;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                MessageBox.Show("Chaîne de connexion introuvable. Vérifiez App.config (DefaultConnection).", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            repository = new BudgetRepository(connectionString);

            try
            {
                repository.InitialiserBaseDeDonnees();
                ChargerTransactions();
                MettreAJourStatistiques();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion à la base de données: " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvTransactions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Load selected into form for editing
            try
            {
                LoadSelectedTransactionToForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement de la transaction: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnModifier_Click(object sender, EventArgs e)
        {
            try
            {
                LoadSelectedTransactionToForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement de la transaction: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAnnuler_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void LoadSelectedTransactionToForm()
        {
            if (dgvTransactions.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une transaction.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvTransactions.SelectedRows[0];
            editingId = Convert.ToInt32(row.Cells["Id"].Value);
            txtDescription.Text = row.Cells["Description"].Value?.ToString();
            txtMontant.Text = row.Cells["Montant"].Value?.ToString();
            var typeVal = row.Cells["Type"].Value?.ToString();
            if (typeVal != null && cmbType.Items.Contains(typeVal)) cmbType.SelectedItem = typeVal;
            cmbCategorie.Text = row.Cells["Categorie"].Value?.ToString();
            if (row.Cells["Date"].Value != DBNull.Value) dtpDate.Value = Convert.ToDateTime(row.Cells["Date"].Value);

            btnAjouter.Text = "Enregistrer";
        }

        private void ResetForm()
        {
            editingId = 0;
            txtDescription.Clear();
            txtMontant.Clear();
            cmbType.SelectedIndex = 0;
            cmbCategorie.SelectedIndex = 0;
            dtpDate.Value = DateTime.Now;
            btnAjouter.Text = "Ajouter";
        }

        private void BtnAjouter_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDescription.Text))
                {
                    MessageBox.Show("Veuillez entrer une description.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtMontant.Text))
                {
                    MessageBox.Show("Veuillez entrer un montant.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Transaction t = new Transaction
                {
                    Description = txtDescription.Text,
                    Montant = decimal.Parse(txtMontant.Text),
                    Type = cmbType.SelectedItem.ToString(),
                    Categorie = cmbCategorie.Text,
                    Date = dtpDate.Value
                };

                if (editingId == 0)
                {
                    repository.AjouterTransaction(t);
                    MessageBox.Show("Transaction ajoutée avec succès!", "Succès",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    repository.ModifierTransaction(editingId, t);
                    MessageBox.Show("Transaction modifiée avec succès!", "Succès",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    editingId = 0;
                    btnAjouter.Text = "Ajouter";
                }

                // Réinitialiser le formulaire
                ResetForm();

                ChargerTransactions();
                MettreAJourStatistiques();
            }
            catch (FormatException)
            {
                MessageBox.Show("Le montant doit être un nombre valide.", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout: " + ex.Message, "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTransactions.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Veuillez sélectionner une transaction à supprimer.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Êtes-vous sûr de vouloir supprimer cette transaction?",
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvTransactions.SelectedRows[0].Cells["Id"].Value);
                    repository.SupprimerTransaction(id);
                    MessageBox.Show("Transaction supprimée avec succès!", "Succès",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ChargerTransactions();
                    MettreAJourStatistiques();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la suppression: " + ex.Message, "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRafraichir_Click(object sender, EventArgs e)
        {
            ChargerTransactions();
            MettreAJourStatistiques();
        }

        private void BtnRapport_Click(object sender, EventArgs e)
        {
            try
            {
                // Créer un rapport simple avec MessageBox
                decimal revenus = repository.CalculerTotalRevenus();
                decimal depenses = repository.CalculerTotalDepenses();
                decimal solde = repository.CalculerSolde();

                string rapport = $"=== RAPPORT FINANCIER ===\n\n";
                rapport += $"Total Revenus: {revenus:N2} MAD\n";
                rapport += $"Total Dépenses: {depenses:N2} MAD\n";
                rapport += $"Solde: {solde:N2} MAD\n";

                MessageBox.Show(rapport, "Rapport", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la génération du rapport: " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChargerTransactions()
        {
            try
            {
                DataTable dt = repository.ObtenirToutesLesTransactions();
                dgvTransactions.DataSource = dt;

                // Enable or disable the modifier button based on selection
                btnModifier.Enabled = dgvTransactions.SelectedRows.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des transactions: " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MettreAJourStatistiques()
        {
            try
            {
                decimal solde = repository.CalculerSolde();
                decimal revenus = repository.CalculerTotalRevenus();
                decimal depenses = repository.CalculerTotalDepenses();

                lblSolde.Text = $"Solde: {solde:N2} MAD";
                lblRevenus.Text = $"Total Revenus: {revenus:N2} MAD";
                lblDepenses.Text = $"Total Dépenses: {depenses:N2} MAD";

                lblSolde.ForeColor = solde >= 0 ? Color.Green : Color.Red;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la mise à jour des statistiques: " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}