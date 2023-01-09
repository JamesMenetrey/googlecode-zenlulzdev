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
    /// Classe gérant les tehcniques du chasseur.
    /// </summary>
    public static class Abilities
    {
        /// <summary>
        /// Définit la spécialisation du chasseur.
        /// </summary>
        private static Descriptors.Spec CurrentSpec { get { return Descriptors.Spec.Marksmanship; } }
        /// <summary>
        /// Objet pointant sur les propriétés du bot.
        /// </summary>
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        /// <summary>
        /// Pointeur sur l'objet gérant les variables de personnalisation du bot.
        /// </summary>
        private static zeHunterSettings vars = zeHunterSettings.Instance;
        /// <summary>
        /// Indique si le chasseur a lancé le premier tir assuré.
        /// </summary>
        public static bool blnFirstSteadyShot = false;

        #region Methodes gérant les techniques complexes
        /// <summary>
        /// Vérifie si l'unité spécifiée a besoin/peut être ralentie.
        /// </summary>
        /// <param name="unit">Cible spécifiée.</param>
        public static bool NeedSnare(WoWUnit unit)
        {
            if (Target.isValid(unit))
            {
                if (Target.isSlowed(unit))
                    return false;
                else if (unit.HasAura(GetSpell(Descriptors.Auras.DivineShield).Name))
                    return false;
                else if (unit.HasAura(GetSpell(Descriptors.Auras.HandOfFreedom).Name))
                    return false;
                else if (unit.HasAura(GetSpell(Descriptors.Auras.HandOfProtection).Name))
                    return false;
                else if (unit.HasAura(GetSpell(Descriptors.Auras.MastersCall).Name))
                    return false;
                else if (unit.HasAura(GetSpell(Descriptors.Auras.PillarOfFrost).Name))
                    return false;
                else if (unit.IsWithinMeleeRange && Target.isControlled(unit).TotalMilliseconds > 0) //Dans le cas où l'unité est au cac et est controlée
                    return false;

                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Snare l'unité spécifiée.
        /// </summary>
        /// <param name="unit">Unité spécifiée.</param>
        public static void Snare(WoWUnit unit)
        {
            //Détermine la distance entre le chasseur et l'unité
            switch (Descriptors.GetRange(unit))
            {
                case Descriptors.Distance.CaC:
                    Cast(Descriptors.Spells.WindClip); //Useless d'autres contrôles tels que les pièges sont prévus
                    break;
                case Descriptors.Distance.Ranged:
                    if (Cast(Descriptors.Spells.ConcussiveShot))
                    {
                        //Logging
                        Output.wLog("{0} is now snared", unit.Name);
                    }
                    break;
                case Descriptors.Distance.LongRanged:
                    if(Cast(Descriptors.Spells.ConcussiveShot))
                    {
                        //Logging
                        Output.wLog("{0} is now snared", unit.Name);
                    }
                    break;
            }
        }

        /// <summary>
        /// Silence l'unité spécifiée.
        /// </summary>
        /// <param name="unit">Unité spécifiée.</param>
        public static void Silence(WoWUnit unit)
        {
            //Indique si l'unité a été silence
            bool blnIsSilenced = false;

            //Silence en fonction de la portée
            //Utilise torrent arcanique s'il s'agit d'un elfe de sang
            if (unit.IsWithinMeleeRange && Me.Race == WoWRace.BloodElf)
                if (Cast(Descriptors.Spells.ArcaneShot))
                    blnIsSilenced = true;
            if (unit.Distance >= 5 && unit.Distance <= 35 && !blnIsSilenced)
                if (Cast(Descriptors.Spells.SilencingShot))
                    blnIsSilenced = true;
            if (unit.Distance <= 20 && !blnIsSilenced)
                if (Cast(Descriptors.Spells.ScatterShot))
                {
                    blnIsSilenced = true;
                    //Redémarre l'attaque sur la cible
                    StartAutoAttack();
                }
            //Si la technique n'est pas un sort de heal et que l'unité nous cible, tentative de Feint la mort
            if (!blnIsSilenced && WoWSpell.FromId(unit.CastingSpellId).SpellEffect1.EffectType != WoWSpellEffectType.Heal && Me.CurrentTarget.IsTargetingMeOrPet)
            {
                if (FeignDeath())
                    blnIsSilenced = true;
            }

            //Si la cible a bien été interrompue, nous attendons un peu et nous mettons à jour le gestionnaire d'objet
            if (blnIsSilenced)
            {
                System.Threading.Thread.Sleep(50);
                ObjectManager.Update();
            }
        }

        public static bool FeignDeath()
        {
            //conserve la cible
            WoWUnit unit = Me.CurrentTarget;

            //Arrête de marcher
            Movement.StopMoving("Feign Death");
            //Feint la mort
            if (SpellManager.Cast((int)Descriptors.Spells.FeignDeath))
            {
                //Attend un peu
                System.Threading.Thread.Sleep(1000);
                //Bouge pour sortir du FD
                if (vars.Verbose)
                    Output.wLogVerbose("Exiting Feign Death...");
                Movement.StartMoving("End Feign Death");
                Movement.Jump("Ending Feign Death");
                //Redémarre l'attaque sur la cible
                if (unit != null)
                    StartAutoAttack();
                return true;
            }
            //Bouge
            Movement.StartMoving("End Feign Death");
            return false;
        }

        /// <summary>
        /// Utilise tir assuré contre l'unité spécifiée.
        /// </summary>
        /// <param name="unit">Unité spécifiée.</param>
        /// <returns>Retourne vrai si Tir Assuré à été lancé.</returns>
        public static bool SteadyShot(WoWUnit unit, bool blnJumpShot)
        {
            //Utilise le tir assuré
            if (Cast(Descriptors.Spells.SteadyShot, unit, blnJumpShot, false))
            {
                try
                {
                    //----------------------------------------
                    // Pendant l'incantation de tir assuré
                    //----------------------------------------
                    //Patiente jusqu'à que le cast soit proche de la fin ou que l'unité soit au CaC
                    while (Me.IsCasting && Me.CurrentCastTimeLeft.TotalMilliseconds > 600 && !unit.IsWithinMeleeRange)
                        System.Threading.Thread.Sleep(10);

                    //----------------------------------------
                    // Se retourne pour terminer le tir assuré
                    //----------------------------------------
                    //Fait un saut si l'option est activée
                    if (blnJumpShot)
                        Movement.Jump("Steady Shot");
                    //Se tourne en direction de la cible et force le ciblage
                    Movement.StartFace(unit, "Steady Shot");
                    //Patiente le temps que le tir parte
                    while (Me.IsCasting)
                        System.Threading.Thread.Sleep(10);
                    //Annule le facing
                    Movement.StopFace("Steady Shot");

                    //Si le chasseur a déjà lancé une première fois Tir assuré, c'est que c'est la deuxième !
                    if (blnFirstSteadyShot)
                        blnFirstSteadyShot = false;
                    else
                        blnFirstSteadyShot = true;

                    //Tout c'est bien passé
                    return true;
                }
                catch (Exception)
                {
                }
                finally
                {
                    //Annulation du facing
                    Movement.StopFace("Steady Shot");
                }
            }
            return false;
        }

        /// <summary>
        /// Effectue un désengagement sur la cible en effectuant un saut.
        /// </summary>
        public static void Disengage()
        {
            //Vérifie que désengagement pour être lancé
            if (GetSpell(Descriptors.Spells.Disengage).CanCast && !GetSpell(Descriptors.Spells.Disengage).Cooldown)
            {
                //Saute
                Movement.Jump("Disengage");
                //Se positionne comme la cible (en terme de facing)
                float fltFace = WoWMathHelper.CalculateNeededFacing(Me.CurrentTarget.Location, Me.Location);
                Me.SetFacing(fltFace);
                //Attend que le joueur ne soit plus tourné vers la cible
                DateTime dtTimeOut = DateTime.Now;
                while (Me.IsFacing(Me.CurrentTarget.Location) && !Me.CurrentTarget.Dead  && (DateTime.Now - dtTimeOut).Seconds < 2)
                    System.Threading.Thread.Sleep(10);
                //Utilise désengagement
                if (SpellManager.Cast((int)Descriptors.Spells.Disengage))
                {
                    //log
                    Output.wLog("Disengage");
                }
            }
        }

        /// <summary>
        /// Active toutes les techniques augmentant les dégâts du joueur.
        /// </summary>
        public static void Burst()
        {
            Output.wLog("Let's Burst !");
            //Utilise Tir Rapide
            Use(Descriptors.Spells.RapidFire);
            //Utilise le rugissement de récupération du familier (gain de foca)
            PetUse(Descriptors.PetSpells.RoarOfRecovery);
            //Utilise l'appel du fauve (augmente les dégâts)
            PetUse(Descriptors.PetSpells.CallOfTheWild);
            //Utilisation des trinkets et raciaux
            TrinketsRacial.Use(Descriptors.TrinketType.Burst);
        }

        #endregion

        #region Methodes lies a la gestion des techniques
        /// <summary>
        /// Tente d'utiliser une technique du joueur sur une unité spécifiée.
        /// </summary>
        /// <param name="spellID">Technique à utiliser.</param>
        /// <param name="unit">unité spécifiée.</param>
        /// <param name="jumpShot">Utilise la technique de Jump Shot.</param>
        /// <param name="waitGCD">Indique s'il est nécessaire d'attendre la fin du GCD</param>
        /// <returns>Technique exécutée ou non.</returns>
        public static bool Cast(Descriptors.Spells spellID, WoWUnit unit, bool jumpShot, bool waitGCD)
        {
            //Si le joueur est déjà en train de caster, nous annulons l'action
            if (Me.IsCasting || Target.hasAura(Me, Descriptors.Auras.Deterrence))
                return false;
            
            //Détermine la technique grace à son ID
            WoWSpell spell = WoWSpell.FromId((int)spellID);
            
            //Vérifie si le chasseur peut utiliser cette technique
            //Exception sur tir assuré car CanCast retoure false lorsque le chasseur est en mouvement avec l'aspect du renard
            if ((SpellManager.CanCast(spell) || spellID == Descriptors.Spells.SteadyShot) && !StyxWoW.GlobalCooldown)
            {
                //Gère le cas où la cible n'est pas ciblée (meurt, vanish, etc.)
                try
                {
                    //Log
                    Output.wLogVerbose("Trying to cast: {0} on {1}", spell.Name, unit.Name);

                    //Change d'aspect
                    //Utilise l'aspect du faucon ou l'aspect du renard dans le cas d'un tir assuré
                    if (spellID == Descriptors.Spells.SteadyShot)
                        SwapAspect(Descriptors.Aspects.AspectOfFox);
                    else
                        SwapAspect(Descriptors.Aspects.AspectOfHawk);

                    //Jump Shot -- Effectue un saut
                    if (jumpShot)
                    {
                        Movement.Jump("Casting");
                    }
                    //Se tourne en direction de la cible et force le ciblage
                    Movement.StartFace(unit, "StartCasting");
                    //Utilise la technique
                    if (SpellManager.Cast(spell, unit))
                    {
                        //Log
                        if(zeHunterSettings.Instance.Verbose)
                            Output.wLog("Casted: {0} on {1} (jump: {2})", spell.Name, unit.Name, jumpShot);
                        else
                            Output.wLog("Casted: {0} on {1}", spell.Name, unit.Name);
                        //CHASSEUR -- Si la technique est difféente de Tir assuré, nous réinitialisons sa première fois
                        if (spellID != Descriptors.Spells.SteadyShot)
                            blnFirstSteadyShot = false;

                        return true;
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    //Interrompt le facing
                    Movement.StopFace("StopCasting");
                    //Patiente que le GCD se termine
                    if(waitGCD)
                        System.Threading.Thread.Sleep(SpellManager.GlobalCooldownLeft);
                }
            }
            
            return false;
        }

        /// <summary>
        /// Tente d'utiliser une technique du joueur sur la cible actuelle (sans Jump Shot).
        /// </summary>
        /// <param name="spell">Technique à utiliser.</param>
        /// <returns>Technique exécutée ou non.</returns>
        public static bool Cast(Descriptors.Spells spellID)
        {
            return Cast(spellID, Me.CurrentTarget, false, true);
        }

        /// <summary>
        /// Tente d'utiliser une technique du joueur sur la cible actuelle.
        /// </summary>
        /// <param name="spell">Technique à utiliser.</param>
        /// <param name="blnJumpShot">Utilise la technique de Jump Shot.</param>
        /// <returns>Technique exécutée ou non.</returns>
        public static bool Cast(Descriptors.Spells spellID, bool blnJumpShot)
        {
            return Cast(spellID, Me.CurrentTarget, blnJumpShot, true);
        }

        /// <summary>
        /// Tente d'utiliser une technique du joueur, sans intéraction avec un joueur.
        /// </summary>
        /// <param name="spell">Technique à utiliser.</param>
        /// <returns>Technique exécutée ou non.</returns>
        public static bool Use(WoWSpell spell)
        {
            //Si le joueur est déjà en train de caster, nous annulons l'action
            if (Me.IsCasting)
                return false;

            //Si la technique peut être utilisée
            if (SpellManager.CanCast(spell))
            {
                //Lance la technique
                if (SpellManager.Cast(spell))
                {
                    Output.wLog("Used : {0}", spell.Name);
                    //Patiente que le GCD se termine
                    System.Threading.Thread.Sleep(SpellManager.GlobalCooldownLeft);
                    return true;
                }
                else
                    return false;

            }
            else
                return false;
        }

        /// <summary>
        /// Tente d'utiliser une technique du joueur, sans intéraction avec un joueur.
        /// </summary>
        /// <param name="spell">Technique à utiliser.</param>
        /// <returns>Technique exécutée ou non.</returns>
        public static bool Use(Descriptors.Spells spellID)
        {
            //Détermine la technique grâce à son ID
            return Use(WoWSpell.FromId((int)spellID));

        }

        /// <summary>
        /// Tente d'utiliser une technique du familier du joueur, sans intéraction avec un joueur.
        /// </summary>
        /// <param name="spell">Technique à utiliser.</param>
        public static void PetUse(Descriptors.PetSpells spellPetID)
        {
            //Si le joueur possède un pet
            if (Me.GotAlivePet)
            {
                foreach (WoWPetSpell spell in Me.PetSpells.Where(s => s.ToString() == WoWSpell.FromId((int)spellPetID).Name))
                {
                    if (!spell.Cooldown)
                    {
                        Lua.DoString("CastPetAction({0});", spell.ActionBarIndex + 1);
                        Output.wLog("Pet Used : {0}", spell.Spell.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Tente d'utiliser une technique du familier du joueur, sans intéraction avec un joueur.
        /// </summary>
        /// <param name="spell">Technique à utiliser.</param>
        /// <param name="unit">Unité spécifiée.</param>
        public static void PetUse(Descriptors.PetSpells spellPetID, WoWUnit unit)
        {
            //Si le joueur possède un pet
            if (Me.GotAlivePet)
            {
                foreach (WoWPetSpell spell in Me.PetSpells.Where(s => s.ToString() == WoWSpell.FromId((int)spellPetID).Name))
                {
                    if (!spell.Cooldown)
                    {
                        Lua.DoString("CastPetAction({0}, '{1}');", spell.ActionBarIndex + 1, unit.Name);
                        Output.wLog("Pet Used : {0}", spell.Spell.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Retourne les informations concernant une technique.
        /// </summary>
        /// <param name="spellID">Enumération de techniques.</param>
        public static WoWSpell GetSpell(Descriptors.Spells spellID)
        {
            return WoWSpell.FromId((int)spellID);
        }

        /// <summary>
        /// Retourne les informations concernant une aura.
        /// </summary>
        /// <param name="auraID">Enumération de techniques.</param>
        public static WoWSpell GetSpell(Descriptors.Auras auraID)
        {
            return WoWSpell.FromId((int)auraID);
        }

        /// <summary>
        /// Change d'aspect.
        /// </summary>
        /// <param name="aspectID">Aspect spécifié.</param>
        public static void SwapAspect(Descriptors.Aspects aspectID)
        {
            //Log
            if (!Me.Auras.ContainsKey(WoWSpell.FromId((int)aspectID).Name))
                Output.wLog("Swapping aspect to {0}", WoWSpell.FromId((int)aspectID).Name);
            //Tant que l'aspect n'a pas été appliqué au chasseur
            while (!Me.Auras.ContainsKey(WoWSpell.FromId((int)aspectID).Name) && !Me.Dead)
            {
                SpellManager.Cast((int)aspectID);
            }
        }

        /// <summary>
        /// Patiente le temps que le joueur incante. Interrompt l'incantation si le joueur entre en combat.
        /// </summary>
        public static void WaitWhileCastingExceptCombat()
        {
            do
            {
                //Patiente 100ms
                System.Threading.Thread.Sleep(100);
                //Si le joueur entre en combat, on interrompt l'incantation
                if (Me.IsActuallyInCombat)
                    SpellManager.StopCasting();
            } while (Me.IsCasting);
        }

        /// <summary>
        /// Démarre l'auto attaque.
        /// </summary>
        public static void StartAutoAttack()
        {
            //Si l'attaque automatique n'est pas démarrée
            if (!Me.IsAutoAttacking)
                Me.ToggleAttack();
        }

        /// <summary>
        /// Arrête l'auto attaque.
        /// </summary>
        public static void StopAutoAttack()
        {
            //Si l'attaque automatique est démarrée
            if (Me.IsAutoAttacking)
                Me.ToggleAttack();
        }
        #endregion

        #region Methodes de pet
        /// <summary>
        /// Attaque du pet sur une cible définie.
        /// </summary>
        public static void PetAttack()
        {
            //Vérifie que le pet soit vivant
            if(PetIsAlive())
            {
                //Attaque du pet
                if (Me.Pet.CurrentTarget != Me.CurrentTarget)
                {
                    Lua.DoString("PetAttack()");
                    Output.wLogVerbose("Pet is now attacking {0}", Me.CurrentTarget.Name);
                }
            }
        }
        /// <summary>
        /// Vérifie si le pet est vivant.
        /// </summary>
        /// <returns>Retourne un booléen.</returns>
        public static bool PetIsAlive()
        {
            if (Me.GotAlivePet)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Appel le pet. Si celui-ci est mort, le ressucite.
        /// </summary>
        public static void PetCall()
        {
            //Vérifie que le pet ne soit pas déjà présent
            if (!PetIsAlive())
            {
                //Invoque le pet définit par l'utilisateur
                switch (vars.PetNumber)
                {
                    case 1:
                        Use(Descriptors.Spells.CallPet1);
                        break;
                    case 2:
                        Use(Descriptors.Spells.CallPet2);
                        break;
                    case 3:
                        Use(Descriptors.Spells.CallPet3);
                        break;
                    case 4:
                        Use(Descriptors.Spells.CallPet4);
                        break;
                    case 5:
                        Use(Descriptors.Spells.CallPet5);
                        break;
                }
                Output.wLog("Calling pet {0}", vars.PetNumber);
                //Patiente le temps que le pet apparaisse en jeu
                System.Threading.Thread.Sleep(2000);
                //Vérifie que le pet soit vivant
                ObjectManager.Update();
                if (!Me.GotAlivePet)
                {
                    Movement.StopMoving("Invoke Pet");
                    Output.wLog("Reviving pet {0}", vars.PetNumber);
                    //Ressucite le pet
                    Use(Descriptors.Spells.RevivePet);
                    //Patiente le temps que le pet soit vivant
                    WaitWhileCastingExceptCombat();
                    Movement.StartMoving("Invoke Pet");
                }
            }
        }
        /// <summary>
        /// Soigne le familier.
        /// </summary>
        public static void PetMend()
        {
            //Vérifie que le familier soit invoqué
            if (PetIsAlive())
            {
                //Vérifie que le buff ne soit pas déjà appliqué
                if (!Me.Pet.HasAura(GetSpell(Descriptors.Spells.MendPet).Name))
                {
                    Use(Descriptors.Spells.MendPet);
                    Output.wLog("Mending Pet");
                }
            }
        }
        /// <summary>
        /// Ordonne au familier de suivre son maître.
        /// </summary>
        public static void PetFollow()
        {
            if (Me.GotAlivePet)
                Lua.DoString("PetFollow()");
        }
        #endregion
    }
}
