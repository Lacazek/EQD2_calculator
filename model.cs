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
        ScriptContext _context;
        private double _AlphaBeta { get; set; }

        public model(ScriptContext context)
        {
            _context = context;
            checkthat();
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
            GenerateVoxelData();
        }

        private void GenerateVoxelData()
        {
            var exPlan = _context.ExternalPlanSetup;
            var copy = (ExternalPlanSetup)_context.Course.CopyPlanSetup(_context.PlanSetup);
            copy.Id = _context.Course.ExternalPlanSetups.Any(x => x.Id.Equals("EQD2")) ? "EQD2_" + DateTime.Now.ToString("MMddHHmm") : "EQD2";
            copy.Name = "EQD2";
            copy.Comment = "EQD2 Automatique";
            var oldDose = exPlan.Dose;

            List<Beam> beams = copy.Beams.ToList();
            for (int i = 0; i < beams.Count; i++)
            {
                copy.RemoveBeam(beams[i]);
            }

            EvaluationDose copiedDose = copy.CopyEvaluationDose(oldDose);

            int Xsize = copiedDose.XSize;
            int Ysize = copiedDose.YSize;
            int Zsize = copiedDose.ZSize;

            var origin = copiedDose.Origin;
            var resX = copiedDose.XRes;
            var resY = copiedDose.YRes;
            var resZ = copiedDose.ZRes;

            //double rescalefactor = copiedDose.DoseMax3D.Dose / EQD2(oldDose.DoseMax3D.Dose);
            double rescalefactor = 1.154 / 48.1051025;

            var progressWindow = new ProgressWindow(Zsize);
            progressWindow.Show();

            for (int k = 0; k < Zsize; k++)
            {
                progressWindow.UpdateProgress((int)(((double)(k + 1) / Zsize) * 100));

                int[,] plane = new int[Xsize, Ysize];

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

                            double dose = EQD2(copiedDose.GetDoseToPoint(position).Dose * 100);

                            plane[i, j] = (int)(dose/rescalefactor);

                        }

                        catch
                        { }
                    }
                }
                copiedDose.SetVoxels(k, plane);

            }
            progressWindow.Close();
        }

        internal double EQD2(double Dose)
        {
            /* if (Type == "cible")
                 _AlphaBeta = 2;
             else
                 _AlphaBeta = 2;*/
            _AlphaBeta = 2;
            return (double)(Dose * ((_AlphaBeta + (Dose / _context.PlanSetup.NumberOfFractions)) / (2 + _AlphaBeta)));
        }

    }
}
