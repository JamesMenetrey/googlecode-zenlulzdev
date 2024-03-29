﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;
using System.IO;
using Styx.Helpers;

namespace zeHunter.Classes
{
    /// <summary>
    /// Gère les mise à jour de la CC.
    /// </summary>
    class Updater
    {
        /// <summary>
        /// Url du serveur SubVersion.
        /// </summary>
        static public string strSvn { get { return "https://zenlulzdev.googlecode.com/svn/trunk/HonorBuddy/CustomClasses/zeHunter/"; } }
        /// <summary>
        /// Indique l'emplacement du dossier du plugin.
        /// </summary>
        static public string strPluginPath { get { return "CustomClasses\\zeHunter\\"; } }
        /// <summary>
        /// Indique si une nouvelle mise à jour est disponible.
        /// </summary>
        static public bool isAvailable { get { return !string.IsNullOrEmpty(getLatestVersion()); } }


        /// <summary>
        /// Vérifie si le plugin est dans sa dernière version.
        /// </summary>
        /// <returns>Retourne une chaîne de caractères avec le numéro de version de la dernière version.</returns>
        static public string getLatestVersion()
        {
            try
            {
                //Détermine la dernière version du plugin
                XDocument xConfig = XDocument.Load(strSvn + "Config/Update.xml");
                String strLatestVersion = xConfig.Element("update").Element("version").Value;
                //Vérifie si la version actuelle est différente
                if (zeHunter.version.ToString() != strLatestVersion)
                    return strLatestVersion;
                else
                    return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Installe la dernière version du plugin.
        /// </summary>
        /// <remarks>
        /// Ce code a été inspiré de l'addon ProfileChanger.
        /// </remarks>
        /// <returns>Si la mise à jour est déjà installé ou s'est installée correctement, retourne true.</returns>
        static public bool installLatestVersion()
        {
            //Vérifie qu'il y ait une mise à jour
            if (Classes.Updater.isAvailable)
            {
                //Instancie un browser
                WebClient client = new WebClient();
                //Charge le fichier de mise à jour
                XDocument xConfig = XDocument.Load(strSvn + "Config/Update.xml");
                //Il faut que le dossier du plugin existe
                try
                {
                    if (Directory.Exists(Path.Combine(Logging.ApplicationPath, strPluginPath)))
                    {
                        //Met à jour les fichiers sources
                        foreach (XElement node in xConfig.Element("update").Element("content").Elements("file"))
                        {
                            //Vérifie si le fichier en question se trouve dans un dossier
                            if (node.Value.Contains("\\"))
                            {
                                //Vérifie si le dossier existe, dans le cas contraire, le crée
                                if (!Directory.Exists(Path.Combine(Logging.ApplicationPath, strPluginPath) + node.Value.Substring(0, node.Value.IndexOf("\\"))))
                                    Directory.CreateDirectory(Path.Combine(Logging.ApplicationPath, strPluginPath) + node.Value.Substring(0, node.Value.IndexOf("\\")));
                            }
                            //Télécharge le fichier
                            try
                            {
                                client.DownloadFile(strSvn + node.Value.Replace('\\', '/'), Path.Combine(Logging.ApplicationPath, strPluginPath) + node.Value);
                            }
                            catch (Exception ex)
                            {
                                throw new ApplicationException(string.Format("Error when downloading the file '{0}'\nError: {1}", strSvn + node.Value.Replace('\\', '/'), ex.Message));
                            }
                            
                        }
                    }
                    //Affiche un message de réussite
                    System.Windows.Forms.MessageBox.Show("Update completed ! You need to restart HonorBuddy to let changes take effect.", "ZeHunter Updater",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    //Affiche un message d'erreur
                    System.Windows.Forms.MessageBox.Show("Error during ZeHunter is updating :\n" + ex.Message, "ZeHunter Updater",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                //Affiche un message que le plugin est déjà à jour
                System.Windows.Forms.MessageBox.Show("You already have the latest version of this CustomClass !", "ZeHunter Updater", 
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            return true;
        }
    }
}
