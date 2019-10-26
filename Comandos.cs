using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComandosPrinter = System.Console;
using ComandosReader = System.Console;

using static ComandosXLibrary.CommandRealizer;




namespace ComandosX
{
    class Comandos
    {

        public static void Main(string[] args)
        {
            
           new Comandos(execPath: @"C:\", Title: "ComandosX", isRuning: true)
                .Comandos_Exec();

        }

        
        public string ComandosTitle;
        public string ComandosExecPath;
        private bool ComandosIsRuning;




        public Comandos(string execPath, string Title, bool isRuning)
        {

            
            ComandosExecPath = execPath;
            ComandosTitle = (ComandosReader.Title = Title);
            ComandosIsRuning = isRuning;

        }





        #region // Comandos Exec Function

        public void Comandos_Exec()
        {
            

            while (ComandosIsRuning)
            {

                

                ComandosPrinter.Write($"\n{ComandosExecPath}>> ");
                var ComandosCommand = ComandosReader.ReadLine();




                #region // Clear console and Show help information

                if (ComandosCommand == "cl")
                    Console.Clear();

                if (ComandosCommand == "help")
                    _GetHelpInformation();

                #endregion






                #region // List Directory

                if (ComandosCommand.StartsWith("ls"))
                {
                    if (ComandosCommand.Contains("/ctime = visible"))
                    {
                        Element.CreationTime.visible = true;

                    }

                    if (ComandosCommand.Contains("/ctime = hidden"))
                    {
                        Element.CreationTime.visible = false;
                    }

                    _ListDirectory(Path: ComandosExecPath);
                    
                }

                #endregion







                #region // Change Directory

                if (ComandosCommand.StartsWith("cd"))
                {
                    var mustPath = ComandosCommand.Substring(ComandosCommand.IndexOf("d") + 2);
                    var changedPath = _ChangeDirectory(ComandosExecPath, mustPath, ComandosCommand );
                    ComandosExecPath = changedPath;
                }

                #endregion





                #region // Set Console Color

                if (ComandosCommand.StartsWith("color"))
                {
                    var mustColor = 
                        ComandosCommand.Substring(ComandosCommand.IndexOf("r") + 2);

                    _SetConsoleColor(Convert.ToInt32(mustColor));

                }

                #endregion





                #region // Download Nircmd


                if (ComandosCommand.StartsWith("download nircmd"))
                   _DownloadNircmd();
                    

                #endregion





                #region // Run Nircmd


                if (ComandosCommand.StartsWith("nircmd"))
                    _StartNirmcd();


                #endregion





                #region // List local drives

                if (ComandosCommand.StartsWith("drives"))
                    _ListDrives();


                #endregion



            }


        }

        #endregion
    }
}
