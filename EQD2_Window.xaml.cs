using HelixToolkit.Wpf;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;


namespace EQD2_Calculator
{
    /// <summary>
    /// Logique d'interaction pour EQD2_Window.xaml
    /// </summary>
    /*public partial class EQD2_Window : Window
    {

        public EQD2_Window()
        {
            //InitializeComponent();

            // Exemple de matrice de doses (3x3x3 pour la démonstration)
            DoseValue[,,] transformedDoseMatrix = GenerateExampleDoseMatrix();

            // Créer une scène 3D pour afficher la matrice
            ShowDoseMatrixIn3D(transformedDoseMatrix);
        }

        private void ShowDoseMatrixIn3D(DoseValue[,,] transformedDoseMatrix)
        {
            // Créer une collection de géométries pour les voxels
            var modelGroup = new Model3DGroup();

            // Récupérer les dimensions de la matrice
            int sizeX = transformedDoseMatrix.GetLength(0);
            int sizeY = transformedDoseMatrix.GetLength(1);
            int sizeZ = transformedDoseMatrix.GetLength(2);

            // Parcourir la matrice et créer des cubes pour chaque voxel
            for (int z = 0; z < sizeZ; z++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    for (int x = 0; x < sizeX; x++)
                    {
                        double doseValue = transformedDoseMatrix[x, y, z].Dose; // Récupérer la valeur de dose

                        // Définir la couleur du voxel en fonction de la dose
                        var color = GetColorForDose(doseValue);

                        // Créer un cube pour chaque voxel avec une taille fixe
                        var cube = new BoxVisual3D
                        {
                            Center = new Point3D(x, y, z),
                            //Size = 1,
                            Material = new DiffuseMaterial(new SolidColorBrush(color))
                        };

                        modelGroup.Children.Add(cube.Content); // Ajouter le cube à la scène 3D
                    }
                }
            }

            // Ajouter la scène 3D au contrôle HelixViewport3D
            var model = new ModelVisual3D
            {
                Content = modelGroup
            };
            var helixViewport = new HelixViewport3D();
            helixViewport.Children.Add(model);
        }

        // Fonction pour générer un exemple de matrice de doses (tu peux remplacer cela par ta propre matrice)
        private DoseValue[,,] GenerateExampleDoseMatrix()
        {
            int sizeX = 3, sizeY = 3, sizeZ = 3;
            var doseMatrix = new DoseValue[sizeX, sizeY, sizeZ];

            // Remplir la matrice avec des valeurs de doses exemple
            for (int z = 0; z < sizeZ; z++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    for (int x = 0; x < sizeX; x++)
                    {
                        doseMatrix[x, y, z] = new DoseValue(x * y * z + 1); // Valeur d'exemple
                    }
                }
            }

            return doseMatrix;
        }

        // Fonction pour obtenir une couleur en fonction de la dose (exemple : bleu pour faible dose, rouge pour forte dose)
        private System.Windows.Media.Color GetColorForDose(double dose)
        {
            byte red = (byte)(Math.Min(255, dose * 50));   // Fortes doses = plus de rouge
            byte green = (byte)(Math.Min(255, 255 - dose * 50)); // Faibles doses = plus de vert
            return System.Windows.Media.Color.FromRgb(red, green, 0);  // Rendre une couleur RGB
        }
    }

    // Classe pour encapsuler une valeur de dose
    public class DoseValue
    {
        public double Dose { get; set; }
        public DoseValue(double dose)
        {
            Dose = dose;
        }
    }*/
}

