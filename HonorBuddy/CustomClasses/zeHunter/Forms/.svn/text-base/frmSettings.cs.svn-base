using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zeHunter.Classes;

namespace zeHunter.Forms
{
    public partial class frmSettings : Form
    {
        #region Constructor
        public frmSettings()
        {
            //Charge les composants graphiques
            InitializeComponent();
            //Récupère les dernières configurations de la CC
            zeHunterSettings.Instance.Load();
            //Lie le tableau de propriétés avec l'objet contenant les configurations de la CC
            pgHunter.SelectedObject = zeHunterSettings.Instance;

            //Vérifie si une mise à jour est disponible
            if (Updater.isAvailable)
            {
                //Complète les labels avec les numéros de versions
                this.labUpdateCurrentVersion.Text = zeHunter.version.ToString();
                this.labUpdateNewVersion.Text = Updater.getLatestVersion();
                //Affiche l'onglet de mise à jour
                this.tabControl.SelectedTab = tabUpdate;
            }
            else
            {
                //N'affiche pa sl'onglet de mise à jour
                this.tabControl.TabPages.Remove(tabUpdate);
            }
            
            //Nomme la fenêtre avec le numéro de version
            this.Text = String.Format("ZeHunter Ver.{0} - Settings", zeHunter.version);

            //Focus le bouton Cancel
            this.btnCancel.Focus();
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Handle du bouton "Save and Close".
        /// </summary>
        private void btnSaveAndExit_Click(object sender, EventArgs e)
        {
            //Vérifie que le numéro du pet soit entre 1 et 5.
            if (zeHunterSettings.Instance.PetNumber < 1 || zeHunterSettings.Instance.PetNumber > 5)
            {
                MessageBox.Show("The pet number must be between 1 and 5.", "ZeHunter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Sauvegarde les configuration
            ((zeHunterSettings)pgHunter.SelectedObject).Save();
            //Ferme la fenêtre
            this.Close();
        }
        /// <summary>
        /// Handle du bouton "Close".
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            //Ferme la fenêtre
            this.Close();
        }
        /// <summary>
        /// Handle du bouton Update.
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Met à jour la CustomClass
            Updater.installLatestVersion();
        }
        /// <summary>
        /// Handle du bouton Patchnote.
        /// </summary>
        private void btnPatchnote_Click(object sender, EventArgs e)
        {
            //Ouvre le patchnote dans le navigateur Internet
            System.Diagnostics.Process.Start(Updater.strSvn + "Patchnote.txt");
        }
        /// <summary>
        /// Handle du lien de donation.
        /// </summary>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Ouvre la page de donation
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=X74QZ47LZNJEE");
        }
        #endregion
    }
}
