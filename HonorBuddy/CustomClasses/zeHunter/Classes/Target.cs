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
    /// Gestion des cibles.
    /// </summary>
    public class Target
    {
        /// <summary>
        /// Objet pointant sur les propriétés du bot.
        /// </summary>
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        /// <summary>
        /// Liste des effets où le joueur perd la maîtrise de soi.
        /// </summary>
        private static List<WoWSpellMechanic> controlMechanic = new List<WoWSpellMechanic>()
        {
            WoWSpellMechanic.Charmed,
            WoWSpellMechanic.Disoriented,
            WoWSpellMechanic.Fleeing,
            WoWSpellMechanic.Frozen,
            WoWSpellMechanic.Incapacitated,
            WoWSpellMechanic.Polymorphed,
            WoWSpellMechanic.Sapped
        };
        /// <summary>
        /// Liste des effets où le joueur est ralenti.
        /// </summary>
        private static List<WoWSpellMechanic> slowMechanic = new List<WoWSpellMechanic>()
        {
            WoWSpellMechanic.Dazed,
            WoWSpellMechanic.Shackled,
            WoWSpellMechanic.Slowed,
            WoWSpellMechanic.Snared
        };
        /// <summary>
        /// Liste des effets où le joueur est cloué sur place.
        /// </summary>
        private static List<WoWSpellMechanic> rootMechanic = new List<WoWSpellMechanic>()
        {
            WoWSpellMechanic.Rooted
        };
        /// <summary>
        /// Liste des effets où le joueur est stun.
        /// </summary>
        private static List<WoWSpellMechanic> stunMechanic = new List<WoWSpellMechanic>()
        {
            WoWSpellMechanic.Stunned,
            WoWSpellMechanic.Frozen,
            WoWSpellMechanic.Fleeing,
            WoWSpellMechanic.Horrified
        };

        /// <summary>
        /// Vérifie si l'unité spécifiée est valide.
        /// </summary>
        /// <param name="unit">Unité spécifiée.</param>
        public static bool isValid(WoWUnit unit)
        {
            //1. L'unité ne doit pas être null
            //2. L'unité doit être valide
            //3. L'unité ne doit pas être morte
            //4. L'unité peut être un pet seulement s'il n'appartient pas à un joueur
            return unit != null && unit.IsValid && !unit.Dead && ((unit.IsPet && !unit.OwnedByUnit.IsPlayer) || !unit.IsPet);
        }

        /// <summary>
        /// Vérifie si la cible est valide.
        /// </summary>
        public static bool isValid()
        {
            return isValid(Me.CurrentTarget) && Me.GotTarget;
        }

        /// <summary>
        /// Indique si l'unité spécifiée est ralentie.
        /// </summary>
        /// <param name="unit">Unité spécifiée.</param>
        /// <returns>Retourne le plus grand temps restant du contrôle.</returns>
        public static bool isSlowed(WoWUnit unit)
        {
            //Détecte un ralentissement du personnage par rapport à sa vitesse normale
            if (unit.MovementInfo.RunSpeed < 4.5)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Indique si la cible est ralentie.
        /// </summary>
        /// <returns>Retourne le plus grand temps restant du contrôle.</returns>
        public static bool isSlowed()
        {
            return isSlowed(Me.CurrentTarget);
        }

        /// <summary>
        /// Indique si l'unité spécifiée est rooted.
        /// </summary>
        /// <param name="unit">Unité spécifiée.</param>
        /// <returns>Retourne le plus grand temps restant du contrôle.</returns>
        public static TimeSpan isRooted(WoWUnit unit)
        {
            TimeSpan tsTimeRemaining = new TimeSpan(0, 0, 0);
            foreach (WoWAura aura in unit.Auras.Values)
            {
                //Vérifie que l'aura actuelle contient un effet néfaste
                if (rootMechanic.Contains(WoWSpell.FromId(aura.SpellId).Mechanic) || Target.isStunned(Me).Milliseconds > 0)
                {
                    if (tsTimeRemaining < aura.TimeLeft)
                        tsTimeRemaining = aura.TimeLeft;
                }
            }
            return tsTimeRemaining;
        }

        /// <summary>
        /// Indique si la cible est rooted.
        /// </summary>
        /// <returns>Retourne le plus grand temps restant du contrôle.</returns>
        public static TimeSpan isRooted()
        {
            return isRooted(Me.CurrentTarget);
        }

        /// <summary>
        /// Indique si l'unité spécifiée est stun.
        /// </summary>
        /// <param name="unit">Unité spécifiée.</param>
        /// <returns>Retourne le plus grand temps restant du contrôle.</returns>
        public static TimeSpan isStunned(WoWUnit unit)
        {
            TimeSpan tsTimeRemaining = new TimeSpan(0, 0, 0);
            foreach (WoWAura aura in unit.Auras.Values)
            {
                //Vérifie que l'aura actuelle contient un effet néfaste
                if (stunMechanic.Contains(WoWSpell.FromId(aura.SpellId).Mechanic))
                {
                    if (tsTimeRemaining < aura.TimeLeft)
                        tsTimeRemaining = aura.TimeLeft;
                }
            }
            return tsTimeRemaining;
        }

        /// <summary>
        /// Indique si la cible est stun.
        /// </summary>
        /// <returns>Retourne le plus grand temps restant du contrôle.</returns>
        public static TimeSpan isStunned()
        {
            return isStunned(Me.CurrentTarget);
        }

        /// <summary>
        /// Indique si l'unité spécifiée est controlée.
        /// </summary>
        /// <param name="unit">Unité spécifiée.</param>
        /// <returns>Retourne le plus grand temps restant du contrôle.</returns>
        public static TimeSpan isControlled(WoWUnit unit)
        {
            TimeSpan tsTimeRemaining = new TimeSpan(0, 0, 0);
            foreach (WoWAura aura in unit.Auras.Values)
            {
                //Vérifie que l'aura actuelle contient un effet néfaste
                if (controlMechanic.Contains(WoWSpell.FromId(aura.SpellId).Mechanic) || aura.SpellId == (int)Descriptors.Spells.ScatterShot)
                {
                    if (tsTimeRemaining < aura.TimeLeft)
                        tsTimeRemaining = aura.TimeLeft;
                }
            }
            return tsTimeRemaining;
        }

        /// <summary>
        /// Indique si la cible est controlée.
        /// </summary>
        /// <returns>Retourne le plus grand temps restant du contrôle.</returns>
        public static TimeSpan isControlled()
        {
            return isControlled(Me.CurrentTarget);
        }

        /// <summary>
        /// Change de cible et prend la cible la plus proche qui n'est pas de crowd control.
        /// </summary>
        public static void TargetNext()
        {
            //Ordonne au familier de revenir
            Abilities.PetFollow();
            if (zeHunterSettings.Instance.Verbose)
                Output.wLog("Trying to target someone else");
            //Cible le joueur possédant le pet ciblé
            if (Me.GotTarget && Me.CurrentTarget.IsPet && Me.CurrentTarget.OwnedByUnit.IsPlayer && Descriptors.GetRange(Me.CurrentTarget.OwnedByUnit) != Descriptors.Distance.TooFarAway)
                Me.CurrentTarget.OwnedByUnit.Target();
            //Sinon si la liste des joueurs en combat pour HB possède au moins un ennemi
            else if (Targeting.Instance.TargetList.Count > 1)
            {
                Targeting.Instance.TargetList.OrderBy(u => u.Distance).FirstOrDefault(u => isControlled(u).TotalMilliseconds == 0).Target();
                if (isValid())
                    Output.wLog("Switching target to {0}", Me.CurrentTarget);
            }
        }

        /// <summary>
        /// Vérifie si l'unité spécifiée possède l'aura spécifiée.
        /// </summary>
        /// <param name="unit">Unité spécifiée.</param>
        /// <param name="auraID">Aura spécifiée.</param>
        public static bool hasAura(WoWUnit unit, Descriptors.Auras auraID)
        {
            if(unit.Auras.Values.Count(a => a.SpellId == (int)auraID) > 0)
                return true;
            else 
                return false;
        }

        /// <summary>
        /// Vérifie si la cible possède l'aura spécifiée.
        /// </summary>
        /// <param name="auraID">Aura spécifiée.</param>
        public static bool hasAura(Descriptors.Auras auraID)
        {
            return hasAura(Me.CurrentTarget, auraID);
        }
    }
}
