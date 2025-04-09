using VMS.TPS.Common.Model.API;
using System.Diagnostics;
using EQD2_Calculator;
using System.Reflection;

// This line is necessary to "write" in database
[assembly: ESAPIScript(IsWriteable = true)]
[assembly: AssemblyVersion("2.0.0.1")]

namespace VMS.TPS
{
    public class Script
    {
        public Script()   //Constructor
        { }

        public void Execute(ScriptContext context)
        {
            context.Patient.BeginModifications();
            #region Verification de l'existence des données et déclaration des variables
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            model _model = new model(context);         
            stopwatch.Stop();
            #endregion
        }
    }
}
