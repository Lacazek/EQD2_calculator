using System.Windows;
using VMS.TPS.Common.Model.API;
using System.Linq;
using System.Collections.Generic;
using System;
using VMS.TPS.Common.Model.Types;
using System.Runtime.Remoting.Contexts;

namespace EQD2_Calculator

{
    internal class model
    {
        private ScriptContext _context;
        private Interface _interface;
        private bool _allowBED = false;
        private bool _allowEQD2s = false;
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
            foreach (var plan in _context.Course.ExternalPlanSetups.ToList())
            {
                if (plan.Id.ToUpper().Contains("EQD2") ? true : plan.Id.ToUpper().Contains("BED") ? true : false)
                    _context.Course.RemovePlanSetup(plan);
            }

            var exPlan = _context.ExternalPlanSetup;
            var copy = (ExternalPlanSetup)_context.Course.CopyPlanSetup(_context.PlanSetup);
            var copy_EQD2s = allowEQD2s ? (ExternalPlanSetup)_context.Course.CopyPlanSetup(_context.PlanSetup) : null; ;
            var copy_BED = allowBED ? (ExternalPlanSetup)_context.Course.CopyPlanSetup(_context.PlanSetup) : null;

            copy.Id = _context.Course.ExternalPlanSetups.Any(x => x.Id.Equals("EQD2")) ? "EQD2_" + DateTime.Now.ToString("MMddHHmm") : "EQD2";
            copy.Name = "EQD2";
            copy.Comment = "EQD2 Automatique";
            //_context.Course.ExternalPlanSetups.First(x => x.Name.ToUpper().Equals("EQD2")).SetPrescription(1, new DoseValue(double_EQD2strict(copy.TotalDose.Dose, 1), DoseValue.DoseUnit.Gy), 1);

            if (allowEQD2s)
            {
                copy_EQD2s.Id = _context.Course.ExternalPlanSetups.Any(x => x.Id.Equals("EQD2s")) ? "EQD2s_" + DateTime.Now.ToString("MMddHHmm") : "EQD2s";
                copy_EQD2s.Name = "EQD2_strict";
                copy_EQD2s.Comment = "EQD2_strict Automatique";
                _context.Course.ExternalPlanSetups.First(x => x.Name.ToUpper().Equals("EQD2_STRICT")).SetPrescription(1, new DoseValue(double_EQD2strict(copy_EQD2s.TotalDose.Dose, 1), DoseValue.DoseUnit.Gy), 1);
            }
            if (allowBED)
            {
                copy_BED.Id = _context.Course.ExternalPlanSetups.Any(x => x.Id.Equals("BED")) ? "BED_" + DateTime.Now.ToString("MMddHHmm") : "BED";
                copy_BED.Name = "BED";
                copy_BED.Comment = "BED Automatique";
                //_context.Course.ExternalPlanSetups.First(x => x.Name.ToUpper().Equals("BED")).SetPrescription(1, new DoseValue(double_BED((int)copy_BED.TotalDose.Dose, 1), DoseValue.DoseUnit.Gy), 1);
            }

            var oldDose = exPlan.Dose;

            List<Beam> beams = copy.Beams.ToList();
            List<Beam> beams_EQD2s = allowEQD2s ? copy_EQD2s.Beams.ToList() : null;
            List<Beam> beams_bed = allowBED ? copy_BED.Beams.ToList() : null;

            for (int i = 0; i < beams.Count; i++)
            {
                copy.RemoveBeam(beams[i]);
                if (allowEQD2s) copy_EQD2s.RemoveBeam(beams_EQD2s[i]);
                if (allowBED) copy_BED.RemoveBeam(beams_bed[i]);
            }

            EvaluationDose copiedDose = copy.CopyEvaluationDose(oldDose);
            EvaluationDose copiedDose_EQD2s = allowEQD2s ? copy_EQD2s.CopyEvaluationDose(oldDose) : null;
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

                            plane[i, j] = (int)(EQD2(doseMatrix[k, i, j], rescaleFactor));
                            if (allowEQD2s) plane_EQD2s[i, j] = (int)(EQD2strict(doseMatrix[k, i, j], rescaleFactor) * ((_Nbfraction*_DoseFr)/copy_EQD2s.TotalDose.Dose));
                            if (allowBED) plane_BED[i, j] = (int)(BED(doseMatrix[k, i, j], rescaleFactor));
                        }
                        catch
                        { }
                    }
                }
                copiedDose.SetVoxels(k, plane);
                if (allowEQD2s) copiedDose_EQD2s.SetVoxels(k, plane_EQD2s);
                if (allowBED) copiedDose_BED.SetVoxels(k, plane_BED);
            }
            progressWindow.Close();
        }

        internal double EQD2(double Dose, double scaling)
        {
            return Convert.ToInt32(Dose * ((_alphaBeta + Dose * scaling / _Nbfraction)) / (2 + _alphaBeta));
        }

        internal double EQD2strict(double Dose, double scaling)
        {
            return Convert.ToInt32(Dose * ((_alphaBeta + _DoseFr) / (2 + _alphaBeta)));
        }
        internal double double_EQD2strict(double Dose, double scaling)
        {
            return (double)Dose * ((_alphaBeta + _DoseFr) / (2 + _alphaBeta));
        }
        internal double BED(int dose, double scaling)
        {
            return Convert.ToInt32(dose * (1 + dose * scaling / (_Nbfraction * _alphaBeta)));
        }
        internal double double_BED(int dose, double scaling)
        {
            return (double)(dose * (1 + dose * scaling / (_Nbfraction * _alphaBeta)));
        }

        internal int MultiplyByAlphaBeta(int dose, double alphabeta, double scaling)
        {
            return Convert.ToInt32(dose * alphabeta);
        }

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

        internal bool allowEQD2s
        {
            get { return _allowEQD2s; }
            set { _allowEQD2s = value; }
        }
    }
}
