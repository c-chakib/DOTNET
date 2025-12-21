namespace GestionBudgetWinForms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Text = "MainForm";
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000,700);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            // Note: control fields are declared in MainForm.cs partial class
            // Initialize controls properties and add them to the form

            // GroupBox pour ajouter une transaction
            grpAjouter = new System.Windows.Forms.GroupBox();
            grpAjouter.Text = "Ajouter une transaction";
            grpAjouter.Location = new System.Drawing.Point(10,10);
            grpAjouter.Size = new System.Drawing.Size(960,120);

            // Description
            var lblDescriptionLabel = new System.Windows.Forms.Label();
            lblDescriptionLabel.Text = "Description:";
            lblDescriptionLabel.Location = new System.Drawing.Point(10,25);
            lblDescriptionLabel.Size = new System.Drawing.Size(80,20);
            grpAjouter.Controls.Add(lblDescriptionLabel);

            txtDescription = new System.Windows.Forms.TextBox();
            txtDescription.Location = new System.Drawing.Point(100,25);
            txtDescription.Size = new System.Drawing.Size(200,20);
            grpAjouter.Controls.Add(txtDescription);

            // Montant
            var lblMontantLabel = new System.Windows.Forms.Label();
            lblMontantLabel.Text = "Montant:";
            lblMontantLabel.Location = new System.Drawing.Point(320,25);
            lblMontantLabel.Size = new System.Drawing.Size(60,20);
            grpAjouter.Controls.Add(lblMontantLabel);

            txtMontant = new System.Windows.Forms.TextBox();
            txtMontant.Location = new System.Drawing.Point(390,25);
            txtMontant.Size = new System.Drawing.Size(100,20);
            grpAjouter.Controls.Add(txtMontant);

            // Type
            var lblTypeLabel = new System.Windows.Forms.Label();
            lblTypeLabel.Text = "Type:";
            lblTypeLabel.Location = new System.Drawing.Point(510,25);
            lblTypeLabel.Size = new System.Drawing.Size(40,20);
            grpAjouter.Controls.Add(lblTypeLabel);

            cmbType = new System.Windows.Forms.ComboBox();
            cmbType.Location = new System.Drawing.Point(560,25);
            cmbType.Size = new System.Drawing.Size(120,20);
            cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbType.Items.AddRange(new string[] { "Revenue", "Depense" });
            cmbType.SelectedIndex =0;
            grpAjouter.Controls.Add(cmbType);

            // Catégorie
            var lblCategorieLabel = new System.Windows.Forms.Label();
            lblCategorieLabel.Text = "Catégorie:";
            lblCategorieLabel.Location = new System.Drawing.Point(10,60);
            lblCategorieLabel.Size = new System.Drawing.Size(80,20);
            grpAjouter.Controls.Add(lblCategorieLabel);

            cmbCategorie = new System.Windows.Forms.ComboBox();
            cmbCategorie.Location = new System.Drawing.Point(100,60);
            cmbCategorie.Size = new System.Drawing.Size(200,20);
            cmbCategorie.Items.AddRange(new string[] {
            "Salaire", "Freelance", "Investissement",
            "Alimentation", "Transport", "Logement",
            "Loisirs", "Santé", "Éducation", "Autre"
            });
            cmbCategorie.SelectedIndex =0;
            grpAjouter.Controls.Add(cmbCategorie);

            // Date
            var lblDateLabel = new System.Windows.Forms.Label();
            lblDateLabel.Text = "Date:";
            lblDateLabel.Location = new System.Drawing.Point(320,60);
            lblDateLabel.Size = new System.Drawing.Size(60,20);
            grpAjouter.Controls.Add(lblDateLabel);

            dtpDate = new System.Windows.Forms.DateTimePicker();
            dtpDate.Location = new System.Drawing.Point(390,60);
            dtpDate.Size = new System.Drawing.Size(200,20);
            dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            grpAjouter.Controls.Add(dtpDate);

            // Bouton Ajouter
            btnAjouter = new System.Windows.Forms.Button();
            btnAjouter.Text = "Ajouter";
            btnAjouter.Location = new System.Drawing.Point(700,25);
            btnAjouter.Size = new System.Drawing.Size(100,30);
            btnAjouter.BackColor = System.Drawing.Color.FromArgb(0,120,215);
            btnAjouter.ForeColor = System.Drawing.Color.White;
            btnAjouter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAjouter.Click += BtnAjouter_Click;
            grpAjouter.Controls.Add(btnAjouter);

            this.Controls.Add(grpAjouter);

            // GroupBox Statistiques
            grpStatistiques = new System.Windows.Forms.GroupBox();
            grpStatistiques.Text = "Statistiques";
            grpStatistiques.Location = new System.Drawing.Point(10,140);
            grpStatistiques.Size = new System.Drawing.Size(960,80);

            lblRevenus = new System.Windows.Forms.Label();
            lblRevenus.Text = "Total Revenus:0.00 MAD";
            lblRevenus.Location = new System.Drawing.Point(20,25);
            lblRevenus.Size = new System.Drawing.Size(250,25);
            lblRevenus.Font = new System.Drawing.Font("Arial",10, System.Drawing.FontStyle.Bold);
            lblRevenus.ForeColor = System.Drawing.Color.Green;
            grpStatistiques.Controls.Add(lblRevenus);

            lblDepenses = new System.Windows.Forms.Label();
            lblDepenses.Text = "Total Dépenses:0.00 MAD";
            lblDepenses.Location = new System.Drawing.Point(20,50);
            lblDepenses.Size = new System.Drawing.Size(250,25);
            lblDepenses.Font = new System.Drawing.Font("Arial",10, System.Drawing.FontStyle.Bold);
            lblDepenses.ForeColor = System.Drawing.Color.Red;
            grpStatistiques.Controls.Add(lblDepenses);

            lblSolde = new System.Windows.Forms.Label();
            lblSolde.Text = "Solde:0.00 MAD";
            lblSolde.Location = new System.Drawing.Point(300,30);
            lblSolde.Size = new System.Drawing.Size(300,30);
            lblSolde.Font = new System.Drawing.Font("Arial",14, System.Drawing.FontStyle.Bold);
            lblSolde.ForeColor = System.Drawing.Color.Blue;
            grpStatistiques.Controls.Add(lblSolde);

            btnRapport = new System.Windows.Forms.Button();
            btnRapport.Text = "Rapport par Catégorie";
            btnRapport.Location = new System.Drawing.Point(650,30);
            btnRapport.Size = new System.Drawing.Size(150,35);
            btnRapport.Click += BtnRapport_Click;
            grpStatistiques.Controls.Add(btnRapport);

            this.Controls.Add(grpStatistiques);

            // DataGridView pour afficher les transactions
            dgvTransactions = new System.Windows.Forms.DataGridView();
            dgvTransactions.Location = new System.Drawing.Point(10,230);
            dgvTransactions.Size = new System.Drawing.Size(960,350);
            dgvTransactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvTransactions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.MultiSelect = false;
            dgvTransactions.ReadOnly = true;
            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.RowHeadersVisible = false;
            dgvTransactions.CellDoubleClick += DgvTransactions_CellDoubleClick;
            this.Controls.Add(dgvTransactions);

            // Boutons d'action
            btnSupprimer = new System.Windows.Forms.Button();
            btnSupprimer.Text = "Supprimer";
            btnSupprimer.Location = new System.Drawing.Point(10,590);
            btnSupprimer.Size = new System.Drawing.Size(100,35);
            btnSupprimer.BackColor = System.Drawing.Color.FromArgb(192,0,0);
            btnSupprimer.ForeColor = System.Drawing.Color.White;
            btnSupprimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSupprimer.Click += BtnSupprimer_Click;
            this.Controls.Add(btnSupprimer);

            btnRafraichir = new System.Windows.Forms.Button();
            btnRafraichir.Text = "Rafraîchir";
            btnRafraichir.Location = new System.Drawing.Point(120,590);
            btnRafraichir.Size = new System.Drawing.Size(100,35);
            btnRafraichir.Click += BtnRafraichir_Click;
            this.Controls.Add(btnRafraichir);

            btnModifier = new System.Windows.Forms.Button();
            btnModifier.Text = "Modifier";
            btnModifier.Location = new System.Drawing.Point(230,590);
            btnModifier.Size = new System.Drawing.Size(100,35);
            btnModifier.Click += BtnModifier_Click;
            this.Controls.Add(btnModifier);

            btnAnnuler = new System.Windows.Forms.Button();
            btnAnnuler.Text = "Annuler";
            btnAnnuler.Location = new System.Drawing.Point(340,590);
            btnAnnuler.Size = new System.Drawing.Size(100,35);
            btnAnnuler.Click += BtnAnnuler_Click;
            this.Controls.Add(btnAnnuler);

            this.ResumeLayout(false);
        }

        #endregion
    }
}