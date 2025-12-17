/******************************************************************************
 * Nom du fichier : interface.xaml.cs
 * Auteur         : LACAZE Killian
 * Date de création : [03/12/2025]
 * Description    : [Brève description du contenu ou de l'objectif du code]
 *
 * Droits d'auteur © [2025], [LACAZE.K].
 * Tous droits réservés.
 * 
 * Ce code a été développé exclusivement par LACAZE Killian. Toute utilisation de ce code 
 * est soumise aux conditions suivantes :
 * 
 * 1. L'utilisation de ce code est autorisée uniquement à titre personnel ou professionnel, 
 *    mais sans modification de son contenu.
 * 2. Toute redistribution, copie, ou publication de ce code sans l'accord explicite 
 *    de l'auteur est strictement interdite.
 * 3. L'utilisateur assume la responsabilité de l'utilisation de ce code dans ses propres projets.
 * 4. L'utilisateur assume la responsabilité de l'utilisation de ce code.
 * 
 * CE CODE EST FOURNI "EN L'ÉTAT", SANS AUCUNE GARANTIE, EXPRESSE OU IMPLICITE. 
 * L'AUTEUR DÉCLINE TOUTE RESPONSABILITÉ POUR TOUT DOMMAGE OU PERTE RÉSULTANT 
 * DE L'UTILISATION DE CE CODE.
 *
 * Toute utilisation non autorisée ou attribution incorrecte de ce code est interdite.
 ******************************************************************************/

using System.Windows;

namespace EQD2_Calculator
{
    /// <summary>
    /// Logique d'interaction pour Interface.xaml
    /// </summary>
    public partial class Interface : Window
    {
        private model _m;
        internal Interface(model m)
        {
            InitializeComponent();
            _m = m;
            AlphaBeta.Text = "2";
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ShowDialog();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
                _m.allowEQD2_Voxel = true;
            Log.Write($"Utilisateur {System.Environment.UserName} a activé le calcul EQD2 voxelisé");
        }
        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
                _m.allowEQD2_Voxel = false;
            Log.Write($"Utilisateur {System.Environment.UserName} a désactivé le calcul EQD2 voxelisé");
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            _m.allowBED = true;
            Log.Write($"Utilisateur {System.Environment.UserName} a activé le calcul BED");
        }
        private void CheckBox_UnChecked_1(object sender, RoutedEventArgs e)
        {
            _m.allowBED = false;
            Log.Write($"Utilisateur {System.Environment.UserName} a désactivé le calcul BED");
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                _m.AlphaBeta = double.Parse(AlphaBeta.Text.ToString());
            }
            catch
            {
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
