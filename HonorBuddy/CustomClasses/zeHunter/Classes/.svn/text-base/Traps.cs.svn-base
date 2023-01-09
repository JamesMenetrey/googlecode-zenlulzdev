using System;
using System.Collections.Generic;
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
    /// Classe gérant les pièges du chasseur.
    /// </summary>
    public class Traps
    {
        /// <summary>
        /// Utilise un piège du chasseur au sol.
        /// </summary>
        /// <param name="trapID">Type du piège.</param>
        /// <returns>Si le piège a correctement été placé.</returns>
        public static bool Use(Descriptors.TrapsGround trapID)
        {
            //Détermine la technique
            WoWSpell trap = WoWSpell.FromId((int)trapID);

            //Vérifie que le piège peut être placé
            if(SpellManager.CanCast(trap) && !trap.Cooldown && !StyxWoW.GlobalCooldown)
            {
                //Utilise le piège
                if(SpellManager.Cast(trap))
                {
                    //Log
                    Output.wLog("Trap used on ground : {0}", trap.Name);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}
