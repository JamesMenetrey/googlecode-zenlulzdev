using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Styx;
using Styx.Helpers;
using zeHunter.Classes;
using System.ComponentModel;

namespace zeHunter
{
    public class zeHunterSettings : Settings
    {
        public static readonly zeHunterSettings Instance = new zeHunterSettings();

        public zeHunterSettings()
            : base(Path.Combine(Logging.ApplicationPath, string.Format(@"Settings/zeHunter/zeHunter-Settings-{0}.xml", StyxWoW.Me.Name)))
        {
        }

        //Détails des techniques de la CC
        [CategoryAttribute("Miscellaneous"), DescriptionAttribute("Show more information about the CustomClass gameplay (only for debug recommanded because it's spamful. :D)")]
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool Verbose { get; set; }

        //Régénération du bot
        [CategoryAttribute("Player"), DescriptionAttribute("If enabled, the player feeds (set below when it has to).")]
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool Feed { get; set; }
        [CategoryAttribute("Player"), DescriptionAttribute("Set the number of percent when the player has to feed.")]
        [Setting, Styx.Helpers.DefaultValue(30)]
        public int FeedPourcent { get; set; }

        //Utilisation de Venin de veuve
        [CategoryAttribute("Combat"), DescriptionAttribute("If enabled, always apply Widow Venom on the target.")]
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool WidowVenom { get; set; }

        //Numéro du pet à utiliser
        [CategoryAttribute("Pet"), DescriptionAttribute("Set which pet the player has to use. From 1 to 5.")]
        [Setting, Styx.Helpers.DefaultValue(1)]
        public int PetNumber { get; set; }

        //Utilisation du 1er bijou
        [CategoryAttribute("Trinket"), DescriptionAttribute("Set if the player can use the first trinket.")]
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool Trinket1Use { get; set; }
        [CategoryAttribute("Trinket"), DescriptionAttribute("Set the type of the trinket. Burst=Trinket increasing the damages; Mobility=Trinket removing all effects which cause loss of control your character; Survability=Trinket which helps you to survive")]
        [Setting, Styx.Helpers.DefaultValue(Descriptors.TrinketType.Mobility)]
        public Descriptors.TrinketType Trinket1Type { get; set; }

        //Utilisation du 2ème bijou
        [CategoryAttribute("Trinket"), DescriptionAttribute("Set if the player can use the second trinket.")]
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool Trinket2Use { get; set; }
        [CategoryAttribute("Trinket"), DescriptionAttribute("Set the type of the trinket. Burst=Trinket increasing the damages; Mobility=Trinket removing all effects which cause loss of control your character; Survability=Trinket which helps you to survive")]
        [Setting, Styx.Helpers.DefaultValue(Descriptors.TrinketType.Burst)]
        public Descriptors.TrinketType Trinket2Type { get; set; }
    }
}
