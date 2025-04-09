using System.Collections.Generic;
using System.Linq;
using VMS.TPS.Common.Model.API;

namespace EQD2_Calculator
{
    internal class VoxelData
    {

        private Structure Structure { get; set; }  // Structure associée
        private List<int> X { get; set; }                // Coordonnée X
        private List<int> Y { get; set; }                // Coordonnée Y
        private List<int> Z { get; set; }                // Coordonnée Z
        private List<double> UH { get; set; }             // Unité Hounsfield
        private List<float> Dose { get; set; }           // Dose en Gray
        private double _AlphaBeta { get; set; }

        private ScriptContext _context;

        public VoxelData(ScriptContext context)
        {
            _context = context;
        }

        internal double EQD2(double Dose)
        {
            /* if (Type == "cible")
                 _AlphaBeta = 2;
             else
                 _AlphaBeta = 2;*/
            _AlphaBeta = 2;
            return Dose = (double)(Dose * ((_AlphaBeta + Dose / _context.PlanSetup.NumberOfFractions) / ((2 + _AlphaBeta) * _context.PlanSetup.NumberOfFractions)));
        }

        internal double ReverseEQD2(double Dose)
        {
            /* if (Type == "cible")
     _AlphaBeta = 2;
 else
     _AlphaBeta = 2;*/
            return Dose = (double)((double)(Dose * (_AlphaBeta + 2)) / (_AlphaBeta + (Dose / _context.PlanSetup.NumberOfFractions)));
        }










        internal void AddVoxelToList(Structure structure, int x, int y, int z, double hu, float dose)
        {
            Structure = structure;
            X.Add(x);
            Y.Add(y);
            Z.Add(z);
            UH.Add(hu);
            Dose.Add(dose);
        }

        internal void EQD2(Dose DoseMatrix)
        {
            for (int i = 0; i < Dose.Count; i++)
            {
                Dose[i] = (float)(Dose[i] * ((2 + (Dose.Max() / _context.PlanSetup.TotalDose.Dose) / 4) * _context.PlanSetup.TotalDose.Dose));
            }

        }

        internal double EQD2(double Dose, string Type)
        {
            /* if (Type == "cible")
                 _AlphaBeta = 2;
             else
                 _AlphaBeta = 2;*/
            _AlphaBeta = 2;
            //return Dose = (float)(Dose * ((2 + (Dose.Max() / _context.PlanSetup.TotalDose.Dose) / 4) * _context.PlanSetup.TotalDose.Dose));
            return Dose = (double)(Dose * ((_AlphaBeta + Dose / _context.PlanSetup.NumberOfFractions) / ((2 + _AlphaBeta) * _context.PlanSetup.NumberOfFractions)));
        }

        internal void ReverseEQD2(Dose DoseMatrix)
        {
            foreach (var dose in Dose)
            {
                for (int i = 0; i < Dose.Count; i++)
                {
                    Dose[i] = (float)(Dose[i] / ((1 + (Dose.Max() / _context.PlanSetup.TotalDose.Dose) / 4) * _context.PlanSetup.TotalDose.Dose));
                }
            }
        }

        public float GetMaxDose
        {
            get { return Dose.Max(); }
        }
        public float GetMinDose
        {
            get { return Dose.Min(); }
        }
        public float GetDose(int x, int y, int z)
        {
            return Dose.Max();
        }
        public List<int> GetX()
        {
            return X;
        }
        public List<int> GetY()
        {
            return Y;
        }
        public List<int> GetZ()
        {
            return Z;
        }

    }
}
