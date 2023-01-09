/*----------------------------------------------------
 * ZeHunter - Developped by ZenLulz (on thebuddyforum.com)
 * WoW Version Supported : 4.3.4.15595
 * SVN : https://zenlulzdev.googlecode.com/svn/trunk/HonorBuddy/CustomClasses/ZeHunter
 * Note : This is a free CustomClass, and could not be sold, or repackaged.
 * Version : 0.4 (Beta)
 ----------------------------------------------------*/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using zeHunter.Classes;

namespace zeHunter
{
    class zeHunter : CombatRoutine
    {
        //Normal Stuff.
        public override sealed string Name { get { return string.Format("ZeHunter Ver.{0} by {1}", version.ToString(), this.Author); } }
        public override WoWClass Class { get { return WoWClass.Hunter; } }
        public override bool WantButton { get { return true; } }
        public static Version version { get { return new Version(0, 4); } }
        public string Author { get { return "ZenLulz"; } }

        /// <summary>
        /// Objet pointant sur les propri�t�s du bot.
        /// </summary>
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        /// <summary>
        /// Pointeur sur l'objet g�rant les variables de personnalisation du bot.
        /// </summary>
        private zeHunterSettings vars = zeHunterSettings.Instance;
        /// <summary>
        /// D�finit si le chasseur est � la distance maximum de sa cible dans la partie Combat.
        /// </summary>
        private bool blnDistanceMax = false;
        /// <summary>
        /// Indique le moment ou la derni�re technique pour quitter le CaC a �t� effectu�.
        /// </summary>
        private DateTime dtLastExitMelee = DateTime.MinValue;
        /// <summary>
        /// D�termine depuis combien de temps la cible est au CaC.
        /// </summary>
        private DateTime dtWithinMelee = DateTime.MinValue;
        /// <summary>
        /// D�termine depuis combien de temps la cible est hors port�e.
        /// </summary>
        private DateTime dtTooFarAway = DateTime.MinValue;
        private ulong ulngLastTarget = 0;

        public override void OnButtonPress()
        {
            Forms.frmSettings config = new Forms.frmSettings();
            config.ShowDialog();
        }

        public override void Initialize()
        {
            base.Initialize();
            //Charge les personnalisations utilisateur
            vars.Load();
            //Bind l'�v�nement lors de l'appui sur le bouton Start du bot
            BotEvents.OnBotStart += new BotEvents.OnBotStartDelegate(BotEvents_OnBotStart);
            //V�rifie qu'une mise � jour est disponible
            if (Updater.isAvailable)
                Output.wLog("A new version of ZeHunter is available. To update it, open the settings of this CustomClass. New version: {0}, current version: {1}", Updater.getLatestVersion(), zeHunter.version);
        }

        void BotEvents_OnBotStart(EventArgs args)
        {
            //V�rifie le niveau du personnage, si celui-ci est inf�rieur au niveau 83, la CC ne fonctionnera pas !
            if (Me.Level < 83)
            {
                Output.wLogError("Your hunter cannot use this CustomClass because it doesn't reach the minimum level (83).");
                Styx.Logic.BehaviorTree.TreeRoot.Stop();
            }
            //Charge les personnalisations utilisateur
            vars.Load();
            //Met les variables de date � jour
            dtLastExitMelee = DateTime.Now;
            dtTooFarAway = DateTime.Now;
            dtWithinMelee = DateTime.Now;
        }

        #region Rest
        public override bool NeedRest
        {
            get
            {
                //D�termine si le bot a besoin de regen
                if (vars.Feed)
                {
                    if (Me.HealthPercent <= vars.FeedPourcent)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        public override void Rest()
        {
            //Mange !
            Styx.Logic.Common.Rest.Feed();
        }
        #endregion

        #region Pull
        public override void Pull()
        {
            //V�rifie que nous ayons bien une cible
            if (!Target.isValid())
                return;
            //Si la cible est hors de port�e (sous-entendu derri�re un objet), nous nous rapprochons
            if (!Me.CurrentTarget.InLineOfSight)
            {
                //Log
                Output.wLogVerbose("The target is not in light in sight, moving closer...");
                Movement.Move(Me.CurrentTarget, (int)Descriptors.Distance.CaC, "(Pull) Not InLightOfSight");
                return;
            }

            //Si le joueur est d�j� en train de caster et que la cible est � port�e, on ne fait rien
            if (Me.IsCasting)
            {
                if (Me.CurrentTarget.Distance >= 5 && Me.CurrentTarget.Distance <= 40)
                    return;
                else
                    SpellManager.StopCasting();
            }

            //Si la distance entre le joueur et l'ennemi est d'au moins 30m et qu'il dispose de plus de 88% de vie
            if (Me.CurrentTarget.Distance >= 20f && Me.CurrentFocus >= Abilities.GetSpell(Descriptors.Spells.AimedShot).PowerCost && Me.CurrentTarget.HealthPercent > 88)
            {
                if (!Me.Combat && !Me.PetInCombat && Me.CurrentFocus >= Abilities.GetSpell(Descriptors.Spells.Camouflage).PowerCost + Abilities.GetSpell(Descriptors.Spells.AimedShot).PowerCost)
                {
                    //Utilise Dissimulation
                    Abilities.Cast(Descriptors.Spells.Camouflage);
                }
                //Interrompt le d�placement du bot
                WoWMovement.MoveStop();
                //Utilise Vis�e
                Abilities.Cast(Descriptors.Spells.AimedShot);
            }

            //Ordonne l'attaque du pet
            if (Abilities.PetIsAlive())
                Abilities.PetAttack();
            //Active l'attaque automatique
            Abilities.StartAutoAttack();
        }
        #endregion

        #region Pull Buffs

        public override bool NeedPullBuffs { get { return false; } }

        public override void PullBuff() { }

        #endregion

        #region Pre Combat Buffs

        public override bool NeedPreCombatBuffs { get { return false; } }

        public override void PreCombatBuff()
        {
            return;
        }

        #endregion

        #region Combat Buffs

        public override bool NeedCombatBuffs { get { return false; } }

        public override void CombatBuff()
        {

        }

        #endregion

        #region Heal

        public override bool NeedHeal
        {
            get
            {
                //Est-ce que le familier a besoin de soins ?
                if (Abilities.PetIsAlive() && !Me.Pet.HasAura(Abilities.GetSpell(Descriptors.Spells.MendPet).Name))
                {
                    //Est-ce que le familier est � port�e de heal ?
                    if (Me.Pet.InLineOfSight && Me.Pet.Distance <= Abilities.GetSpell(Descriptors.Spells.MendPet).MaxRange)
                    {
                        //V�rifie que la vie du pet ne soit pas en dessous de 60%
                        if (Me.Pet.HealthPercent <= 60)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        //Ordonne au familier de revenir
                        Abilities.PetFollow();
                        return false;
                    }
                }
                else
                    return false;
            }
        }

        public override void Heal()
        {
            //Soigne le familier
            Abilities.PetMend();
        }

        #endregion

        #region Combat

        public override void Combat()
        {
            //Nous allons tout d'abord v�rifier si notre cible est valide
            //V�rifie si la cible est existante, qu'elle est �gal � cette sauvegard�e et qu'elle n'est pas plus loin que 40m
            if (Me.CurrentTarget != null && Me.CurrentTarget.Guid == ulngLastTarget && Me.CurrentTarget.Distance > 40)
            {
                //si cela fait plus ou �gal � 7 secondes que la cible est trop loin
                if ((DateTime.Now - dtTooFarAway).TotalSeconds >= 5)
                {
                    //Message de log
                    Output.wLog("The target is not in light of sight since 5 seconds");
                    //Reset la cible sauv�e
                    ulngLastTarget = 0;
                    //Change de cible si une autre est disponible
                    Target.TargetNext();
                }
            }
            else
            {
                //Reset le moment o� la cible est valide
                dtTooFarAway = DateTime.Now;
                //Sauvegarde la cible actuelle
                ulngLastTarget = Me.CurrentTarget.Guid;
            }

            //S'il n'y a personne � attaquer, nous quittons la m�thode
            if (!Target.isValid())
                return;

            //Enregistre la dur�e pendant laquelle la cible se trouve au CaC
            if (!Me.CurrentTarget.IsWithinMeleeRange)
                dtWithinMelee = DateTime.Now;

            //----------------------------------
            // Gestion du d�placement
            //----------------------------------
            //R�cup�re le type de la cible
            string strTargetType = Lua.GetReturnVal<string>("return UnitClassification(\"target\")", 0);
            bool blnIsEliteOrWorldboss = strTargetType == "elite" || strTargetType == "worldboss";
            //Si la cible est hors de port�e (sous-entendu derri�re un objet), nous nous rapprochons
            if (!Me.CurrentTarget.InLineOfSight)
            {
                //Log
                Output.wLogVerbose("The target is not in light in sight, moving closer...");
                Movement.Move(Me.CurrentTarget, (int)Descriptors.Distance.CaC, "Not InLightOfSight");
                return;
            }
            else
            {
                //S'il s'agit d'une unit� autre qu'un �lite ou qu'un worldboss
                if(!blnIsEliteOrWorldboss)
                {
                    //V�rifie que la cible soit toujours snare
                    if (Abilities.NeedSnare(Me.CurrentTarget))
                    {
                        //Pour commencer, nous mettons la cible dans le filet de l'araign�e
                        if (!WoWSpell.FromId((int)Descriptors.PetSpells.Web).Cooldown)
                            Abilities.PetUse(Descriptors.PetSpells.Web);
                        //Sinon nous la snarons avec nos autres techniques
                        else
                            Abilities.Snare(Me.CurrentTarget);
                    }
                }

                //D�place le chasseur � une port�e minimum (10y pour les �lites/worldbosses ou 30y pour les autres)
                if (blnIsEliteOrWorldboss && ((Me.CurrentTarget.Distance >= (double)Descriptors.Distance.SoftRanged) && (Me.CurrentTarget.Distance <= (double)Descriptors.Distance.LongRanged))
                    || !blnIsEliteOrWorldboss && ((Me.CurrentTarget.Distance >= (double)Descriptors.Distance.Ranged) && (Me.CurrentTarget.Distance <= (double)Descriptors.Distance.LongRanged)))
                {
                    //Arr�te le mouvement: nous sommes assez loin
                    if(Movement.CanMove)
                        Movement.StopMoving("Max Distance Reached");
                    blnDistanceMax = true;
                    //Se tourne en direction de la cible
                    if (!Me.IsFacing(Me.CurrentTarget))
                        Me.CurrentTarget.Face();
                }
                else
                {
                    if (!Movement.CanMove)
                        Movement.StartMoving("Kitting Core");
                    Movement.MoveAway("Kitting Core");
                    blnDistanceMax = false;
                }
            }

            
            //Silence la cible
            if (Me.CurrentTarget.IsCasting && Me.CurrentTarget.CastingSpell.PowerType == WoWPowerType.Mana)
                Abilities.Silence(Me.CurrentTarget);
            
            //Attaque du familier si la cible n'est pas contr�l�e et que le familier n'a pas besoin d'�tre soign�
            if (Target.isControlled().TotalMilliseconds == 0 && !NeedHeal)
                Abilities.PetAttack();

            //V�rifie que le joueur ne soit pas stun plus de 2 secondes
            if (Target.isStunned(Me).TotalSeconds > 2 || Target.isControlled(Me).TotalSeconds > 2)
            {
                if (!TrinketsRacial.Use(Descriptors.TrinketType.Mobility))
                {
                    //Utilise Rugissement de sacrifice du pet
                    Abilities.PetUse(Descriptors.PetSpells.RoarOfSacrifice, Me);
                }
            }
            
            //V�rifie que le joueur n'a pas d'effet de ralentissement
            if (Target.isSlowed(Me) || Target.isRooted(Me).TotalMilliseconds > 0)
            {
                //Dans le cas contraire, nous utilisons l'appel du ma�tre
                Abilities.Cast(Descriptors.Spells.MastersCall);
            }

            //Active le syst�me de d�fense si la cible est au cac
            if (Me.CurrentTarget.IsWithinMeleeRange)
            {
                //Pose un pi�ge givrant ou de serpents
                if (!WoWSpell.FromId((int)Descriptors.TrapsGround.Ice).Cooldown)
                    Traps.Use(Descriptors.TrapsGround.Ice);
                else if (!WoWSpell.FromId((int)Descriptors.TrapsGround.Snake).Cooldown)
                    Traps.Use(Descriptors.TrapsGround.Snake);

                //Si le d�sengagement n'est pas sous cooldown
                if (!Abilities.GetSpell(Descriptors.Spells.Disengage).Cooldown)
                {
                    //Utilisation de la technique
                    Abilities.Disengage();
                    //Enregistrement du moment
                    dtLastExitMelee = DateTime.Now;
                }
                //Dans le cas o� d�sengagement est sous CD ou que celle-ci a �t� utilis�e il y a 2 secondes ou plus 
                else if ((DateTime.Now - dtLastExitMelee).TotalSeconds >= 2 && !Abilities.GetSpell(Descriptors.Spells.ScatterShot).Cooldown)
                {
                    //Utilisation de la fl�che de dispertion
                    if (Abilities.Cast(Descriptors.Spells.ScatterShot))
                    {
                        Output.wLogVerbose("Scatter Shot to avoid melee");
                        //Enregistrement du moment
                        dtLastExitMelee = DateTime.Now;
                        //Patiente au maximum 3 secondes pendant que la cible est contr�l�e
                        DateTime dtScatterShot = DateTime.Now;
                        while (Target.isControlled().TotalMilliseconds > 0 && (DateTime.Now - dtScatterShot).TotalSeconds < 3)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
                //Dans le cas o� d�sangagement et la fl�che de dispertion ont d�j� �t� utilis�s et que la cible est plus de 3 secondes au cac
                else if ((DateTime.Now - dtLastExitMelee).TotalSeconds >= 3 && !Abilities.GetSpell(Descriptors.Spells.Deterrence).Cooldown
                            && (DateTime.Now - dtWithinMelee).TotalSeconds >= 3)
                {
                    if (Abilities.Use(Descriptors.Spells.Deterrence))
                    {
                        //Log
                        Output.wLogVerbose("Deterrence to avoid melee");
                        //Enregistrement du moment
                        dtLastExitMelee = DateTime.Now;
                    }
                }
            }
            
            //D�but de la rotation du chasseur
            //Si la cible est � distance
            if (Descriptors.GetRange(Me.CurrentTarget) == Descriptors.Distance.Ranged || Descriptors.GetRange(Me.CurrentTarget) == Descriptors.Distance.LongRanged)
            {
                //----------------------------------
                // Utilisation de la prep
                //----------------------------------
                //Si le cooldown de tir rapide est actif (et que le buff n'est pas actif), que disengage est sous cd et que scatter shot aussi et => ON PREP
                if (!Abilities.GetSpell(Descriptors.Spells.Readiness).Cooldown && 
                    Abilities.GetSpell(Descriptors.Spells.RapidFire).Cooldown &&
                    Abilities.GetSpell(Descriptors.Spells.ScatterShot).Cooldown &&
                    (Abilities.GetSpell(Descriptors.Spells.Disengage).Cooldown ||
                    Abilities.GetSpell(Descriptors.Spells.SilencingShot).Cooldown))
                {
                    //Utilisation de Promptitude
                    Abilities.Use(Descriptors.Spells.Readiness);
                }

                
                //----------------------------------
                // Utilisation des CDs (burst)
                //----------------------------------
                //Burst d�s que le cooldown de Tir Rapide est disponible
                if (!Abilities.GetSpell(Descriptors.Spells.RapidFire).Cooldown)
                    Abilities.Burst();

                //----------------------------------
                // Utilisation des procs
                //----------------------------------
                //Vis�e !
                if (Target.hasAura(Me, Descriptors.Auras.AimedShotProc))
                {
                    Movement.StartFace(Me.CurrentTarget, "Aimed Shot Proc");
                    Output.wLog("Got an Aimed Shot proc !");
                    Abilities.Cast(Descriptors.Spells.AimedShot, !blnDistanceMax);
                    Movement.StopFace("Aimed Shot Proc");
                    return;
                }

                //----------------------------------
                // Utilisation de tir mortel
                //----------------------------------
                if (Me.CurrentTarget.HealthPercent <= 20 && !Abilities.GetSpell(Descriptors.Spells.KillShot).Cooldown)
                    Abilities.Cast(Descriptors.Spells.KillShot, !blnDistanceMax);

                //----------------------------------
                // Utilisation de Tir de la Chim�re
                //----------------------------------
                //Si le cooldown du tir de la chim�re est disponible et que nous avons assez de foca pour l'utiliser
                if (Abilities.GetSpell(Descriptors.Spells.ChimeraShot).CooldownTimeLeft.TotalMilliseconds == 0 &&
                        Me.CurrentFocus >= Abilities.GetSpell(Descriptors.Spells.ChimeraShot).PowerCost)
                {
                    //Utilise le tir de la chim�re
                    if(Abilities.Cast(Descriptors.Spells.ChimeraShot, !blnDistanceMax))
                        return;
                }
                //Si le cooldown est up mais nous n'avons pas assez de foca, utilisons le tir assur�
                else if (Abilities.GetSpell(Descriptors.Spells.ChimeraShot).CooldownTimeLeft.TotalMilliseconds == 0)
                {
                    if(Abilities.SteadyShot(Me.CurrentTarget, !blnDistanceMax))
                        return;
                }


                //----------------------------------
                // Utilisation de Morsure du serpent
                //----------------------------------
                //Si la cible n'a pas le DOT, nous l'appliquons
                if (Abilities.GetSpell(Descriptors.Spells.SerpentSting).CanCast && !Me.CurrentTarget.HasAura((int)Descriptors.Spells.SerpentSting))
                {
                    if (Abilities.Cast(Descriptors.Spells.SerpentSting, !blnDistanceMax))
                        return;
                }

                //----------------------------------
                // Utilisation de Venin de veuve
                //----------------------------------
                //Si la cible n'a pas le debuff, nous l'appliquons
                if (Abilities.GetSpell(Descriptors.Spells.WidowVenom).CanCast && !Me.CurrentTarget.HasAura((int)Descriptors.Spells.WidowVenom))
                {
                    if (Abilities.Cast(Descriptors.Spells.WidowVenom, !blnDistanceMax))
                        return;
                }

                //----------------------------------
                // Utilisation de tir assur� (pour buff)
                //----------------------------------
                //Si un Tir assur� a d�j� �t� lanc� une fois, on profite pour en lancer un deuxi�me
                if (Abilities.blnFirstSteadyShot)
                {
                    if(Abilities.SteadyShot(Me.CurrentTarget, !blnDistanceMax))
                        return;
                }

                //----------------------------------
                // Utilisation de Tir des arcanes
                //----------------------------------
                if (Abilities.Cast(Descriptors.Spells.ArcaneShot, !blnDistanceMax))
                    return;

                //----------------------------------
                // Utilisation de tir assur�
                //----------------------------------
                Abilities.SteadyShot(Me.CurrentTarget, !blnDistanceMax);
            }
        }

        #endregion

        #region Pulse
        public override void Pulse()
        {
            //Si le pet n'est pas pr�sent, que nous sommes vivant et que nous ne combattons pas, nous l'invoquons
            if(StyxWoW.IsInWorld && !Abilities.PetIsAlive() && !Me.Mounted && !Me.Dead && !Me.IsGhost && !Me.Combat)
                Abilities.PetCall();
            //Si le joueur n'est pas en combat et que les mouvements sont interrompus, nous les restaurons
            if (!Me.Combat && !Movement.CanMove)
                Movement.Move(Me.Location, "End of combat");
        }
        #endregion
    }   
}
