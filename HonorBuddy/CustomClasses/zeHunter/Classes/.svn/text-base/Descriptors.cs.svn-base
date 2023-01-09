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
    /// Définit les constantes pour un chasseur.
    /// </summary>
    public class Descriptors
    {
        #region Enumerations
        /// <summary>
        /// Enum des spécialisation du chasseur.
        /// </summary>
        public enum Spec
        {
            BeastMastery,
            Marksmanship,
            Survival
        }

        /// <summary>
        /// Enumération des techniques du chasseur.
        /// </summary>
        public enum Spells
        {
            AimedShot = 19434,
            ArcaneShot = 3044,
            ArcaneTorrent = 80483,
            AspectOfCheetah = 5118,
            AspectOfFox = 82661,
            AspectOfHawk = 13165,
            AspectOfPack = 13159,
            AspectOfWild = 20043,
            CallPet1 = 883,
            CallPet2 = 83242,
            CallPet3 = 83243,
            CallPet4 = 83244,
            CallPet5 = 83245,
            Camouflage = 51753,
            ChimeraShot = 53209,
            ConcussiveShot = 5116,
            Deterrence = 19263,
            Disengage = 781,
            FeignDeath = 5384,
            KillShot = 53351,
            MastersCall = 53271,
            MendPet = 136,
            RapidFire = 3045,
            Readiness = 23989,
            RevivePet = 982,
            SerpentSting = 1978,
            ScatterShot = 19503,
            SilencingShot = 34490,
            SteadyShot = 56641,
            WidowVenom = 82654,
            WindClip = 2974
        }

        /// <summary>
        /// Enumération des techniques du familier du chasseur.
        /// </summary>
        public enum PetSpells
        {
            CallOfTheWild = 53434,
            RoarOfRecovery = 53517,
            RoarOfSacrifice = 53480,
            Web = 4167
        }

        /// <summary>
        /// Enumération de buffs.
        /// </summary>
        public enum Auras
        {
            AimedShotProc = 82926,
            AspectOfCheetah = 5118,
            AspectOfFox = 82661,
            AspectOfHawk = 13165,
            AspectOfPack = 13159,
            AspectOfWild = 20043,
            DivineShield = 642,
            Deterrence = 67801,
            FeignDeath = 5384,
            HandOfFreedom = 1044,
            HandOfProtection = 1022,
            MastersCall = 53271,
            PillarOfFrost = 51271,
            Throwdown = 85388
        }

        /// <summary>
        /// Enumération des aspects du chasseur.
        /// </summary>
        public enum Aspects
        {
            AspectOfCheetah = 5118,
            AspectOfFox = 82661,
            AspectOfHawk = 13165,
            AspectOfPack = 13159,
            AspectOfWild = 20043
        }

        /// <summary>
        /// Enumération des pièges à terre du chasseur.
        /// </summary>
        public enum TrapsGround
        {
            Explosive = 13813,
            Freezing = 1499, //Glaçon
            Ice = 13809, //Pattinoire
            Immolation = 13795,
            Snake = 34600
        }

        /// <summary>
        /// Enumération des différents types de portée du chasseur.
        /// </summary>
        public enum Distance
        {
            None,
            CaC = 4,
            SoftRanged = 10,
            Ranged = 30,
            LongRanged = 40,
            DeathlyRanged = 45,
            TooFarAway = 46
        }

        /// <summary>
        /// Enumération des différents types de bijoux.
        /// </summary>
        public enum TrinketType
        {
            None,
            Burst,
            Mobility,
            Survivability
        }

        /// <summary>
        /// Enumération des différents techniques raciales.
        /// </summary>
        public enum Racials
        {
            ArcaneShot = 3044, //Elfe de sang
            Berserking = 26297, //Troll
            BloodFury = 20572, //Orc
            Cannibalize = 20577, //Mort vivant
            EscapeArtist = 20589, //Gnome
            EveryManForHimself = 59752, //Humain
            GiftOfTheNaaru = 59543, //Draenei
            RocketBarrage = 69041, //Goblin
            RocketJump = 69070, //Goblin
            Shadowmeld = 58984, //Elfe de la nuit
            Stoneform = 20594, //Nain
            WarStromp = 20549, //Tauren
            WillOfTheForsaken = 7744 //Mort vivant
        }
        #endregion

        #region Methodes
        /// <summary>
        /// Détermine le type de distance qui sépare le chasseur de l'unité spécifiée.
        /// </summary>
        /// <param name="unit">Unité spécifiée.</param>
        /// <returns>Enum Distance.</returns>
        public static Distance GetRange(WoWUnit unit)
        {
            //Si l'unité existe
            if (unit != null)
            {
                if (unit.Distance <= (double)Distance.CaC)
                    return Distance.CaC;
                else if (unit.Distance <= (double)Distance.SoftRanged)
                    return Distance.SoftRanged;
                else if (unit.Distance <= (double)Distance.Ranged)
                    return Distance.Ranged;
                else if (unit.Distance <= (double)Distance.LongRanged)
                    return Distance.LongRanged;
                else if (unit.Distance <= (double)Distance.DeathlyRanged)
                    return Distance.DeathlyRanged;
                else
                    return Distance.TooFarAway;
            }
            else
                return Distance.None;
        }
        #endregion
    }
}
