/******************************************************************************
 * Nom du fichier : model.cs
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
using VMS.TPS.Common.Model.API;
using System.Linq;
using System.Collections.Generic;
using System;
using VMS.TPS.Common.Model.Types;

namespace EQD2_Calculator

{
    internal class model
    {
        private ScriptContext _context;
        private Interface _interface;
        private bool _allowBED = false;
        private bool _allowEQD2_Voxel = false;
        private double _alphaBeta;
        private double _DoseFr;
        private int _Nbfraction;

        public model(ScriptContext context)
        {
            _context = context;
            _interface = new Interface(this);
            _DoseFr = context.ExternalPlanSetup.DosePerFraction.Dose;
            _Nbfraction = (int)context.ExternalPlanSetup.NumberOfFractions;
            checkthat();
            EndMessage end = new EndMessage();
            end.ShowDialog();
        }

        internal void checkthat()
        {

            if (_context.Patient == null)
            {
                MessageBox.Show("Merci de charger un patient");
                return;
            }

            if (_context.StructureSet == null)
            {
                MessageBox.Show("Merci de charger un groupe de structures");
                return;
            }

            if (_context.Image == null)
            {
                MessageBox.Show("Merci de charger une image");
                return;
            }
            Log.Write($"Patient chargé : {_context.Patient.Name}\n");
            Log.Write($"Début du calcul des plans EQD2,BED,...");
            GenerateVoxelData();
        }

        private void GenerateVoxelData()
        {
            foreach (var plan in _context.Course.ExternalPlanSetups.ToList())
            {
                if (plan.Id.ToUpper().Equals("EQD2_VOXEL") ? true : plan.Id.ToUpper().Equals("EQD2") ? true : plan.Id.ToUpper().Equals("BED") ? true : false)
                {
                    Log.Write($"Plan supprimé : {plan.Id}");
                    _context.Course.RemovePlanSetup(plan);
                }
            }

            var exPlan = _context.ExternalPlanSetup;
            var copy = (ExternalPlanSetup)_context.Course.CopyPlanSetup(_context.PlanSetup);
            var copy_EQD2s = allowEQD2_Voxel ? (ExternalPlanSetup)_context.Course.CopyPlanSetup(_context.PlanSetup) : null; ;
            var copy_BED = allowBED ? (ExternalPlanSetup)_context.Course.CopyPlanSetup(_context.PlanSetup) : null;
            Log.Write($"Plans copiés avec succès\n");

            copy.Id = _context.Course.ExternalPlanSetups.Any(x => x.Id.Equals("EQD2")) ? "EQD2_" + DateTime.Now.ToString("MMddHHmm") : "EQD2";
            copy.Name = "EQD2";
            copy.Comment = $"EQD2 automatique généré par le script EQD2_Calculator\n" +
                $"α/ß = {_interface.AlphaBeta.Text}\nDose par fraction = 2\nNombre de fraction = {_Nbfraction}\nDose totale prescription EQD2 = {Math.Round(EQD2(_context.PlanSetup.TotalDose.Dose), 3)} Gy\n" +
                $"La validation de ce plan repose sur la responsabilité de l'utilisateur\n";
            Log.Write(copy.Comment);


            if (allowEQD2_Voxel)
            {
                copy_EQD2s.Id = _context.Course.ExternalPlanSetups.Any(x => x.Id.Equals("EQD2_Voxel")) ? "EQD2_Voxel_" + DateTime.Now.ToString("MMddHHmm") : "EQD2_Voxel";
                copy_EQD2s.Name = "EQD2_Voxel";
                copy_EQD2s.Comment = $"EQD2_Voxel automatique généré par le script EQD2_Calculator\n" +
                $"α/ß = {_interface.AlphaBeta.Text}\nDose par fraction = variable\nNombre de fraction = {_Nbfraction}\nDose totale prescription EQD2 = {Math.Round(EQD2(_context.PlanSetup.TotalDose.Dose), 3)} Gy\n" +
                $"La validation de ce plan repose sur la responsabilité de l'utilisateur\n";
                Log.Write(copy_EQD2s.Comment);
            }
            if (allowBED)
            {
                copy_BED.Id = _context.Course.ExternalPlanSetups.Any(x => x.Id.Equals("BED")) ? "BED_" + DateTime.Now.ToString("MMddHHmm") : "BED";
                copy_BED.Name = "BED";
                copy_BED.Comment = $"BED automatique généré par le script EQD2_Calculator\n" +
                $"α/ß = {_interface.AlphaBeta.Text}\nDose par fraction = {Math.Round(double_BED(_Nbfraction * _DoseFr)/_Nbfraction, 3)}\nNombre de fraction = {_Nbfraction}\nDose totale prescription BED = {Math.Round(double_BED(_Nbfraction * _DoseFr), 3)} Gy\n" +
                $"La validation de ce plan repose sur la responsabilité de l'utilisateur\n";
                Log.Write(copy_BED.Comment);
            }

            var oldDose = exPlan.Dose;

            List<Beam> beams = copy.Beams.ToList();
            List<Beam> beams_EQD2s = allowEQD2_Voxel ? copy_EQD2s.Beams.ToList() : null;
            List<Beam> beams_bed = allowBED ? copy_BED.Beams.ToList() : null;

            for (int i = 0; i < beams.Count; i++)
            {
                copy.RemoveBeam(beams[i]);
                if (allowEQD2_Voxel) copy_EQD2s.RemoveBeam(beams_EQD2s[i]);
                if (allowBED) copy_BED.RemoveBeam(beams_bed[i]);
            }
            Log.Write($"Beams supprimés des plans copiés\n");

            EvaluationDose copiedDose = copy.CopyEvaluationDose(oldDose);
            EvaluationDose copiedDose_EQD2s = allowEQD2_Voxel ? copy_EQD2s.CopyEvaluationDose(oldDose) : null;
            EvaluationDose copiedDose_BED = allowBED ? copy_BED.CopyEvaluationDose(oldDose) : null;

            int Xsize = copiedDose.XSize;
            int Ysize = copiedDose.YSize;
            int Zsize = copiedDose.ZSize;

            var origin = copiedDose.Origin;
            var resX = copiedDose.XRes;
            var resY = copiedDose.YRes;
            var resZ = copiedDose.ZRes;

            int[,,] doseMatrix = GetDoseVoxelsFromDose(copiedDose);
            double maxDoseVal = GetMaxDoseVal(copiedDose, copy);
            Tuple<int, int> minMaxDose = GetMinMaxValues(doseMatrix, Xsize, Ysize, Zsize);

            double rescaleFactor = maxDoseVal / minMaxDose.Item2;
            Log.Write($"Rescale Factor = {rescaleFactor}\n");

            var progressWindow = new ProgressWindow(Zsize);
            progressWindow.Show();

            for (int k = 0; k < Zsize; k++)
            {
                progressWindow.UpdateProgress((int)(((double)(k + 1) / Zsize) * 100));

                int[,] plane = new int[Xsize, Ysize];
                int[,] plane_EQD2s = new int[Xsize, Ysize];
                int[,] plane_BED = new int[Xsize, Ysize];

                for (int i = 0; i < Xsize; i++)
                {
                    for (int j = 0; j < Ysize; j++)
                    {
                        try
                        {
                            var x_mm = origin.x + i * resX;
                            var y_mm = origin.y + j * resY;
                            var z_mm = origin.z + k * resZ;
                            var position = new VVector(x_mm, y_mm, z_mm);

                            // Cette ligne est utile pour calculer l'EQD2 général et non voxel par voxel (facteur fixe lié à la prescritpion)
                            //plane[i, j] = (int)(EQD2(doseMatrix[k, i, j], rescaleFactor));

                            if (allowEQD2_Voxel) plane_EQD2s[i, j] = (int)(EQD2(doseMatrix[k, i, j], rescaleFactor) * ((_Nbfraction * _DoseFr) / copy_EQD2s.TotalDose.Dose));
                            if (allowBED) plane_BED[i, j] = (int)(BED(doseMatrix[k, i, j], rescaleFactor));
                        }
                        catch
                        { }
                    }
                }
                //Cette ligne applique les résultats du calcul précédent coupe par coupe
                //copiedDose.SetVoxels(k, plane);
                if (allowEQD2_Voxel) copiedDose_EQD2s.SetVoxels(k, plane_EQD2s);
                if (allowBED) copiedDose_BED.SetVoxels(k, plane_BED);
            }
            copy.SetPrescription(1, new DoseValue(Math.Round(EQD2(_context.PlanSetup.TotalDose.Dose), 3), "Gy"), 1);

            Log.Write($"Plan EQD2 généré avec succès : {copy.Id}\nPrescription modifiée pour 2Gy/fraction");
            if (allowEQD2_Voxel) Log.Write($"Plan EQD2_Voxel généré avec succès : {copy_EQD2s.Id}");
            if (allowBED) Log.Write($"Plan BED généré avec succès : {copy_BED.Id}\n");
            progressWindow.Close();
        }

        #region EQD2
        internal double EQD2(double Dose)
        {
            return Dose * ((_alphaBeta + _DoseFr) / (2 + _alphaBeta));
        }
        internal double EQD2(double Dose, double scaling)
        {
            return Convert.ToInt32(Dose * (_alphaBeta + Dose * scaling / _Nbfraction) / (2 + _alphaBeta));
        }
        internal double double_EQD2(double Dose, double scaling)
        {
            return (double)Dose * ((_alphaBeta + Dose * scaling / _Nbfraction)) / (2 + _alphaBeta);
        }
        internal double double_EQD2(double Dose)
        {
            return (double)Dose * ((_alphaBeta + (Dose / _Nbfraction) / (2 + _alphaBeta)));
        }
        #endregion

        #region BED
        internal double BED(int dose, double scaling)
        {
            return Convert.ToInt32(dose * (1 + (scaling * dose) / (_Nbfraction * _alphaBeta)));
        }
        internal double double_BED(int dose, double scaling)
        {
            return (double)(dose * (1 + scaling * dose / (_Nbfraction * _alphaBeta)));
        }
        internal double double_BED(double dose)
        {
            return (double)(dose * (1 + (dose / (_Nbfraction * _alphaBeta))));
        }
        #endregion

        #region Multiplication par alpha beta
        internal int MultiplyByAlphaBeta(int dose, double alphabeta, double scaling)
        {
            return Convert.ToInt32(dose * alphabeta);
        }
        #endregion

        #region Calcul du facteur de rescaling et récupération de la dose par voxel
        internal Tuple<int, int> GetMinMaxValues(int[,,] array, int Xsize, int Ysize, int Zsize)
        {
            int min = Int32.MaxValue;
            int max = 0;

            for (int i = 0; i < Xsize; i++)
            {
                for (int j = 0; j < Ysize; j++)
                {
                    for (int k = 0; k < Zsize; k++)
                    {
                        int temp = array[k, i, j];

                        if (temp > max) max = temp;
                        else if (temp < min) min = temp;
                    }
                }
            }
            return Tuple.Create(min, max);
        }

        public int[,,] GetDoseVoxelsFromDose(Dose dose)
        {
            int Xsize = dose.XSize;
            int Ysize = dose.YSize;
            int Zsize = dose.ZSize;

            int[,,] doseMatrix = new int[Zsize, Xsize, Ysize];

            for (int k = 0; k < Zsize; k++)
            {
                int[,] plane = new int[Xsize, Ysize];
                dose.GetVoxels(k, plane);

                for (int i = 0; i < Xsize; i++)
                {
                    for (int j = 0; j < Ysize; j++)
                    {
                        doseMatrix[k, i, j] = plane[i, j];
                    }
                }
            }
            return doseMatrix;
        }
        public double GetMaxDoseVal(Dose dose, ExternalPlanSetup plan)
        {
            DoseValue maxDose = dose.DoseMax3D;
            double maxDoseVal = maxDose.Dose;

            if (maxDose.IsRelativeDoseValue)
            {
                if (plan.TotalDose.Unit == DoseValue.DoseUnit.cGy)
                {
                    maxDoseVal = maxDoseVal * plan.TotalDose.Dose / 10000.0;
                }
                else
                {
                    maxDoseVal = maxDoseVal * plan.TotalDose.Dose / 100.0;
                }
            }

            if (maxDose.Unit == DoseValue.DoseUnit.cGy)
            {
                maxDoseVal = maxDoseVal / 100.0;
            }
            return maxDoseVal;
        }
        #endregion

        #region Get and Set
        internal double AlphaBeta
        {
            get { return _alphaBeta; }
            set { _alphaBeta = value; }
        }

        internal bool allowBED
        {
            get { return _allowBED; }
            set { _allowBED = value; }
        }

        internal bool allowEQD2_Voxel
        {
            get { return _allowEQD2_Voxel; }
            set { _allowEQD2_Voxel = value; }
        }
        #endregion
    }
}
