/******************************************************************************
 * Nom du fichier : main.cs
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
 * 3. L'auteur assume la responsabilité de l'utilisation de ce code dans ses propres projets. * 3. L'utilisateur assume la responsabilité de l'utilisation de ce code dans ses propres projets.
 * 4. L'utilisateur assume la responsabilité de l'utilisation de ce code. 
 *  
 * CE CODE EST FOURNI "EN L'ÉTAT", SANS AUCUNE GARANTIE, EXPRESSE OU IMPLICITE. 
 * L'AUTEUR DÉCLINE TOUTE RESPONSABILITÉ POUR TOUT DOMMAGE OU PERTE RÉSULTANT 
 * DE L'UTILISATION DE CE CODE.
 *
 * Toute utilisation non autorisée ou attribution incorrecte de ce code est interdite.
 ******************************************************************************/

using VMS.TPS.Common.Model.API;
using System.Diagnostics;
using EQD2_Calculator;
using System.Reflection;
using System;

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
            Stopwatch stopwatch = new Stopwatch();
            Log.Write($"Début : {DateTime.Now}");
            stopwatch.Start();
            try
            {
                model _model = new model(context);
                stopwatch.Stop();
            }
            catch {}
            Log.Write($"Script exécuté par l'utilisateur {System.Environment.UserName} en {stopwatch.ElapsedMilliseconds/1000} s");
            Log.Write($"Fin : {DateTime.Now}\n");
            Log.Write("-----------------------------------------------------\n\n");
        }
    }
}
