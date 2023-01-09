using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace zeHunter.Classes
{
    /// <summary>
    /// Gère les flux de sorties.
    /// </summary>
    class Output
    {
        /// <summary>
        /// Méthode écrivant du texte dans le console Log.
        /// </summary>
        public static void wLog(string format, params object[] args)
        {
            Logging.Write(Color.Cyan, "[ZeHunter]: " + format, args);
        }
        /// <summary>
        /// Méthode écrivant du texte dans le console Log en tant qu'erreur.
        /// </summary>
        public static void wLogError(string format, params object[] args)
        {
            Logging.Write(Color.Red, "[ZeHunter]: " + format, args);
        }
        /// <summary>
        /// Méthode écrivant du texte dans le console Debug.
        /// </summary>
        public static void wLogDebug(string format, params object[] args)
        {
            Logging.WriteDebug(Color.Cyan, "[ZeHunter]: " + format, args);
        }
        /// <summary>
        /// Méthode écrivant du texte du texte de debug de la CC.
        /// </summary>
        public static void wLogVerbose(string format, params object[] args)
        {
            //Vérifie que le mode Verbose soit bien activé
            if(zeHunterSettings.Instance.Verbose)
                Logging.Write(Color.Cyan, "[ZeHunter]: " + format, args);
        }
    }
}
