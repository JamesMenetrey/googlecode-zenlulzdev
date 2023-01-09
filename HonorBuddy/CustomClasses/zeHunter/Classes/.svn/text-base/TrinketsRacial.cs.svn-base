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
    /// Classe gérant les bijoux et les raciaux du joueur.
    /// </summary>
    public class TrinketsRacial
    {
        /// <summary>
        /// Objet pointant sur les propriétés du bot.
        /// </summary>
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        /// <summary>
        /// Pointeur sur l'objet gérant les variables de personnalisation du bot.
        /// </summary>
        private static zeHunterSettings vars = zeHunterSettings.Instance;

        private static Dictionary<Descriptors.Racials, Descriptors.TrinketType> RacialsTypes = new Dictionary<Descriptors.Racials, Descriptors.TrinketType>()
        {
            {Descriptors.Racials.Berserking, Descriptors.TrinketType.Burst},
            {Descriptors.Racials.BloodFury, Descriptors.TrinketType.Burst},
            {Descriptors.Racials.EscapeArtist, Descriptors.TrinketType.Mobility},
            {Descriptors.Racials.EveryManForHimself, Descriptors.TrinketType.Mobility},
            {Descriptors.Racials.GiftOfTheNaaru, Descriptors.TrinketType.Survivability},
            {Descriptors.Racials.RocketBarrage, Descriptors.TrinketType.Burst},
            {Descriptors.Racials.WarStromp, Descriptors.TrinketType.Survivability},
            {Descriptors.Racials.WillOfTheForsaken, Descriptors.TrinketType.Mobility}
        };

        /// <summary>
        /// Utilise le ou les trinkets par rapport à leur effets.
        /// </summary>
        /// <param name="type">Type du ou des trinkets à utiliser.</param>
        public static bool Use(Descriptors.TrinketType type)
        {
            //Si un bijou ou un talent racial a été utilisé, cette valeur sera vrai
            bool blnUsed = false;

            //Utilisation des techniques raciales
            foreach (KeyValuePair<Descriptors.Racials, Descriptors.TrinketType> racialType in RacialsTypes.Where(r => r.Value == type))
            {
                //Détermine si la technique peut être lancée
                if (SpellManager.CanCast((int)racialType.Key) && !WoWSpell.FromId((int)racialType.Key).Cooldown)
                {
                    //Utilisation de la technique
                    if (SpellManager.Cast((int)racialType.Key))
                    {
                        //Log
                        Output.wLog("Racial used : {0}", WoWSpell.FromId((int)racialType.Key).Name);
                        blnUsed = true;
                    }
                }
            }

            //Utilisation du premier trinket
            if (vars.Trinket1Use && vars.Trinket1Type == type)
            {
                if (Me.Inventory.Equipped.Trinket1 != null && Me.Inventory.Equipped.Trinket1.Cooldown <= 0)
                {
                    if(Me.Inventory.Equipped.Trinket1.Use())
                    {
                        blnUsed = true;
                        //Log
                        Output.wLog("Trinket 1 used : {0}", Me.Inventory.Equipped.Trinket1.Name);
                    }
                }
            }
            //Utilisation du deuxième trinket
            if (vars.Trinket2Use && vars.Trinket2Type == type)
            {
                if (Me.Inventory.Equipped.Trinket2 != null && Me.Inventory.Equipped.Trinket2.Cooldown <= 0)
                {
                    if (Me.Inventory.Equipped.Trinket2.Use())
                    {
                        blnUsed = true;
                        //Log
                        Output.wLog("Trinket 2 used : {0}", Me.Inventory.Equipped.Trinket1.Name);
                    }
                }
            }

            //Utilisation d'éventuels enchantements pour burst
            if (type == Descriptors.TrinketType.Burst)
            {
                if (Me.Inventory.Equipped.Hands != null && Me.Inventory.Equipped.Hands.Usable && Me.Inventory.Equipped.Hands.CooldownTimeLeft.TotalSeconds == 0)
                {
                    //Jump Shot -- Effectue un saut
                    Movement.Jump("Use Enchantment");
                    //Se tourne en direction de la cible et force le ciblage
                    if (Me.CurrentTarget != null)
                    {
                        Movement.StartFace(Me.CurrentTarget, "Use Enchantment");
                    }
                    //Utilisation de l'enchantement sur les gants
                    if (Me.Inventory.Equipped.Hands.Use())
                    {
                        blnUsed = true;
                        //Log
                        Output.wLog("Hand enchant used : {0}", Me.Inventory.Equipped.Hands.Name);
                    }
                    //Interrompt le facing
                    Movement.StopFace("Use Enchantment");
                    //Jump Shot -- Restaure le mouvement interrompu
                }
            }
        
            //Retourne si quelque chose a pu être exécuté
            return blnUsed;
        }
    }
}
