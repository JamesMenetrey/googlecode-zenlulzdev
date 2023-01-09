using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Classe gérant les déplacements du joueur.
    /// </summary>
    public class Movement
    {
        /// <summary>
        /// Objet pointant sur les propriétés du bot.
        /// </summary>
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        /// <summary>
        /// Pointeur sur l'objet gérant les variables de personnalisation du bot.
        /// </summary>
        private static zeHunterSettings vars = zeHunterSettings.Instance;
        /// <summary>
        /// Destination du déplacement actuel du joueur.
        /// </summary>
        private static WoWPoint WptDestination;
        /// <summary>
        /// Distance à laquelle le joueur doit se placer le joueur par rapport à la destination.
        /// </summary>
        private static float fltDistance;
        /// <summary>
        /// Indique si le joueur peut bouger.
        /// </summary>
        private static bool blnCanMove = true;
        /// <summary>
        /// Raison de la dernière méthode appelée.
        /// </summary>
        private static string strReason;

        /// <summary>
        /// Indique si le joueur peut bouger.
        /// </summary>
        public static bool CanMove
        {
            get
            {
                return blnCanMove;
            }
        }

        /// <summary>
        /// Déplace le joueur à la destination spécifiée.
        /// </summary>
        /// <param name="destination">Destination spécifiée.</param>
        /// <param name="reason">Raison du déplacement.</param>
        public static void Move(WoWPoint destination, string reason)
        {
            Move(destination, 0, reason);
        }

        /// <summary>
        /// Déplace le joueur à la position de l'unité spécifiée, à la distance spécifiée.
        /// </summary>
        /// <param name="unite">Unité spécifiée.</param>
        /// <param name="distance">Distance spécifiée.</param>
        /// <param name="reason">Raison du déplacement.</param>
        public static void Move(WoWUnit unit, float distance, string reason)
        {
            Move(unit.Location, distance, reason);
        }

        /// <summary>
        /// Déplace le joueur à la position spécifiée, à la distance spécifiée.
        /// </summary>
        /// <param name="point">Destination spécifiée.</param>
        /// <param name="distance">Distance spécifiée.</param>
        /// <param name="reason">Raison du déplacement.</param>
        public static void Move(WoWPoint point, float distance, string reason)
        {
            //Copie les variables localement
            WptDestination = point;
            fltDistance = distance;
            strReason = reason;
            //Déplace le joueur
            Move();
        }

        /// <summary>
        /// Déplace le joueur à la position et à la distance enregistrée.
        /// </summary>
        private static void Move()
        {
            //Vérifie Output.wLog le déplacement est nécessaire
            if (blnCanMove)
            {
                //Déplace le joueur
                Navigator.MoveTo(WoWMovement.CalculatePointFrom(WptDestination, fltDistance));
                //Log le mouvement
                if (vars.Verbose)
                    Output.wLogVerbose("Moving to {0} [Reason: {1}]", WoWMovement.CalculatePointFrom(WptDestination, fltDistance).ToString(), strReason);
            }
        }

        /// <summary>
        /// Se déplace à distance de la cible.
        /// </summary>
        /// <param name="reason">Raison du déplacement.</param>
        public static void MoveAway(string reason)
        {
            Movement.Move(Me.CurrentTarget, (float)Descriptors.Distance.LongRanged, reason);
        }

        /// <summary>
        /// Interrompt et empêche les mouvements du joueur.
        /// </summary>
        /// <param name="reason">Raison du déplacement.</param>
        public static void StopMoving(string reason)
        {
            blnCanMove = false;
            WoWMovement.MoveStop();
            if (vars.Verbose)
                Output.wLogVerbose("StopMoving [Reason: {0}]", reason);
        }

        /// <summary>
        /// Rétablit la possibilité des mouvements du joueur.
        /// </summary>
        /// <param name="reason">Raison du déplacement.</param>
        public static void StartMoving(string reason)
        {
            blnCanMove = true;
            Move();
            if (vars.Verbose)
                Output.wLogVerbose("StartMoving [Reason: {0}]", reason);
        }

        /// <summary>
        /// Effectue un saut (à l'aide de la touche esapce du clavier).
        /// </summary>
        /// <param name="reason">Raison du déplacement.</param>
        public static void Jump(string reason)
        {
            //Si le joueur n'est pas déjà en l'air
            if (!Me.IsFalling)
            {
                WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend);
                Output.wLogVerbose("Jumping [Reason: {0}]", reason);
            }
            else
            {
                Output.wLogVerbose("Cannot jump; already in air [Reason: {0}]", reason);
            }
        }

        /// <summary>
        /// Oblige le joueur à faire face à l'unité spécifiée et attend que ce soit réellement le cas.
        /// </summary>
        /// <param name="unit">unité spécifiée.</param>
        /// <param name="reason">Raison du déplacement.</param>
        public static void StartFace(WoWUnit unit, string reason)
        {
            //Interdit les déplacements
            if(!blnCanMove)
                StopMoving(reason);

            //Fait face à l'unité
            WoWMovement.ConstantFace(unit.Guid);
            //Déclare le moment
            DateTime dtTimeOut = DateTime.Now;

            //Message en verbose
            Output.wLogVerbose("StartFace [Reason: {0}]", reason);

            //Attend que le joueur se tourne en direction de sa cible pour autant qu'elle soit vivante ave cun timeout de 2 sec
            while (!Me.IsFacing(unit.Location) && !unit.Dead && (DateTime.Now - dtTimeOut).Seconds < 2)
            {
                Output.wLogVerbose("Awaiting facing...");
                Thread.Sleep(1);
                ObjectManager.Update();
            }
        }

        /// <summary>
        /// Rompt l'obligation de faire face à l'unité.
        /// </summary>
        /// <param name="reason">Raison du déplacement.</param>
        public static void StopFace(string reason)
        {
            //Arrête le facing
            WoWMovement.StopFace();

            //Autorise les déplacements
            if (blnCanMove)
                StartMoving(reason);

            //Message en verbose
            Output.wLogVerbose("StopFace [Reason: {0}]", reason);
        }
    }
}
