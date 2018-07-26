using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace SngPropMod
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                string[] lstFiles = args.Where(x => x != args[0]).ToArray();

                if (args.Length > 0)
                {
                    if (args[0] == "File")
                        Application.Run(new frmFileProp(lstFiles));
                    else if (args[0] == "Dir")
                    {
                        lstFiles = Directory.GetFiles(lstFiles[0], "*.mp3");
                        if (lstFiles.Length != 0)
                            Application.Run(new frmFileProp(lstFiles, true));
                    }

                }
                else
                {
                    //Register mp3 file menu item
                    RegistryKey keyMp3 = Registry.ClassesRoot.OpenSubKey(@"SystemFileAssociations\.mp3\shell\Edit audio properties", true);

                    if (keyMp3 == null)
                    {
                        keyMp3 = Registry.ClassesRoot.CreateSubKey(@"SystemFileAssociations\.mp3\shell\Edit audio properties", RegistryKeyPermissionCheck.ReadWriteSubTree);
                        RegistryKey command = keyMp3.CreateSubKey("command");
                        command.SetValue("", "\"" + Application.ExecutablePath + "\"" + " \"File\" \"%1\"");
                        keyMp3.SetValue("Icon", "\"" + Application.ExecutablePath + "\"");
                    }
                    else
                    {
                        RegistryKey command = keyMp3.CreateSubKey("command");
                        command.SetValue("", "\"" + Application.ExecutablePath + "\"" + " \"File\" \"%1\"");
                        keyMp3.SetValue("Icon", "\"" + Application.ExecutablePath + "\"");
                    }

                    //Register directory menu item
                    RegistryKey keyDir = Registry.ClassesRoot.OpenSubKey(@"Directory\shell\Edit all mp3 files", true);

                    if (keyDir == null)
                    {
                        keyDir = Registry.ClassesRoot.CreateSubKey(@"Directory\shell\Edit all mp3 files", RegistryKeyPermissionCheck.ReadWriteSubTree);
                        RegistryKey command = keyDir.CreateSubKey("command");
                        command.SetValue("", "\"" + Application.ExecutablePath + "\"" + " \"Dir\" \"%1\"");
                    }
                    else
                    {
                        RegistryKey command = keyDir.CreateSubKey("command");
                        command.SetValue("", "\"" + Application.ExecutablePath + "\"" + " \"Dir\" \"%1\"");
                    }

                    MessageBox.Show("Application installed successfully", "Success");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fatal error");
            }
        }
    }
}
