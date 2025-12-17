/******************************************************************************
 * Nom du fichier : Log.cs
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


using System;
using System.IO;
using System.Reflection;

namespace EQD2_Calculator
{
    static internal class Log
    {
        static readonly string _path = Assembly.GetExecutingAssembly().Location.ToString();
        static readonly string _file = "EQD2_log.txt";
        static readonly string _filePath = System.IO.Path.Combine(_file, _path);

        static internal void Write(string message)
        {
            File.AppendAllText(_file, message + Environment.NewLine);
        }
    }
}
