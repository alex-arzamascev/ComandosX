using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

using ComandosPrinter = System.Console;
using ComandosReader = System.Console;
using ComandosColorChooser = System.Console;

using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ComandosXLibrary
{
    public class CommandRealizer
    {



        private static int counter = 0;

        private static LinkedList<string> changedDirectoriesList
            = new LinkedList<string>();

        private static Dictionary<int, LinkedListNode<string>> directoriesNodesList
            = new Dictionary<int, LinkedListNode<string>>();




        static CommandRealizer()
        {

            changedDirectoriesList.AddFirst(@"C:\");
            directoriesNodesList.Add(counter, changedDirectoriesList.First);
           
        }



        public struct Element
        {
            public struct CreationTime
            {
                public static bool visible;

            }

        }




        #region // Get help informaion Function

            
            
        public static void _GetHelpInformation() 
        {
            ComandosPrinter.WriteLine();
            ComandosPrinter.WriteLine
                (
                
                "\t\t\t\tComands\n" + 
                "\t\t#######################################"
                
                );

            new List<string> 
            {
            
                "ls         : Show directories and files",
                "cd         : Change directory",
                "color      : Change Console Color",
                "nircmd     : Download nircmd", 
                "drives     : List local drives"
            
            }.ForEach((comands) => ComandosPrinter.WriteLine("\t\t" + comands));      

        }
        

        #endregion





        #region // List Directory Function


        public static void _ListDirectory(string Path)
        {
     
            try
            {
                if (string.IsNullOrWhiteSpace(Path))
                {
                    throw new ArgumentException("", nameof(Path));
                }

                else
                {

                    DirectoryInfo directoryInfo;
                    FileInfo fileInfo;


                    #region // Directories and files

                    var directories =
                        Directory.GetDirectories(path: Path, "*");
                    var files =
                        Directory.GetFiles(path: Path, "*");

                    #endregion


                    ComandosPrinter.WriteLine("\n\t\tTotal directories {0}, files {1}", directories.Length, files.Length); 
                    ComandosPrinter.WriteLine();
                    

                    foreach (var file in files)
                    {
                        fileInfo = new FileInfo(file);
                        string creationTime;

                        creationTime = Element.CreationTime.visible == false
                        ? ""
                        : fileInfo.CreationTime.ToString();


                        ComandosPrinter.WriteLine("\t" + creationTime + "\t<File>\t" + fileInfo.Name);

                    }


                    foreach (var directory in directories)
                    {
                        directoryInfo = new DirectoryInfo(directory);
                        string creationTime;

                        creationTime = Element.CreationTime.visible == false
                        ? ""
                        : directoryInfo.CreationTime.ToString();

                        ComandosPrinter.WriteLine("\t" + creationTime + "\t<Dir>\t" + directoryInfo.Name);

                    }

                }

            }
            catch (ArgumentException)
            {
                ComandosPrinter.WriteLine("Path cannot be empty\nPlease enter the path");
            }
        }


        #endregion







        #region // List local drives Function

        public static void _ListDrives()
        {

            #region // Local drives

            var localDrives =
                DriveInfo.GetDrives();

            #endregion


            Array.ForEach<DriveInfo>(localDrives, (drive) =>
            {

                if (drive.IsReady)
                {
                    var totalDriveSize = (drive.TotalSize / 1024 / 1024 / 1024).ToString("N1");
                    var totalFreeSpace = (drive.TotalFreeSpace / 1024 / 1024 / 1024).ToString("N1");

                    string volLabel = null;
                    volLabel = string.IsNullOrEmpty(drive.VolumeLabel) 
                    ? "volume is empty" 
                    : drive.VolumeLabel; 

                    var driveInfo = 
                    $"\n\t\t|------> Volume label - {volLabel} " +
                    $"\n\t\t|------> File system - {drive.DriveFormat}" +
                    $"\n\t\t|------> Total size - {totalDriveSize} GB" +
                    $"\n\t\t|------> Free space - {totalFreeSpace} GB";
                    
                    ComandosPrinter.Write($"\n\t\tVolume - {drive.Name}");
                    ComandosPrinter.WriteLine(driveInfo);

                }

            });

        }

        #endregion







        #region // Change Directory Function

        public static string _ChangeDirectory(string currentPath, string mustPath, string comandosComand)
        {

            string changedPath = "";

            if (!comandosComand.Contains(@"..\") && !comandosComand.Contains(@"\.."))
            {

                changedPath = Path.Combine(currentPath, mustPath.Trim());

                if (changedDirectoriesList.Contains(changedPath))
                {
                    changedPath = Path.Combine(currentPath, mustPath.Trim());
                    ++counter;
                }

                else 
                {

                    try
                    {
                        directoriesNodesList.Add(++counter, changedDirectoriesList.AddLast(changedPath));
                    }

                    catch (ArgumentException)
                    {

                        DeleteAllNodes();

                    }

                    void DeleteAllNodes()
                    {
                        
                        foreach (KeyValuePair<int, LinkedListNode<string>> nodes in directoriesNodesList)
                        {
                            changedDirectoriesList.Remove(nodes.Value);
                        }

                        counter = 0;

                        directoriesNodesList.Clear();
                        directoriesNodesList.Add(counter, changedDirectoriesList.AddFirst(@"C:\"));

                        directoriesNodesList.Add(++counter, changedDirectoriesList.AddLast(changedPath));

                    }

                    

                }

            }


            else
            {

                if (comandosComand.Contains(@"..\"))
                {

                    --counter;
                    directoriesNodesList.TryGetValue(counter, out LinkedListNode<string> listNode);

                    changedPath = listNode.Value;

                }


                if (comandosComand.Contains(@"\.."))
                {
                    ++counter;
                    directoriesNodesList.TryGetValue(counter, out LinkedListNode<string> listNode);

                    changedPath = listNode.Value;
                }

            }

            return changedPath;
        }

        #endregion







        #region // Set ConsoleColor Function

        public static void _SetConsoleColor(int mustColor)
        {
            var consoleColors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));
            var colorsList = new Dictionary<int, ConsoleColor>();

            var colors = consoleColors.GetEnumerator();
  
            int counter = 0;
            while (colors.MoveNext())
            {
                colorsList.Add(counter++, (ConsoleColor)colors.Current);
            }

            colorsList.TryGetValue(mustColor, out ConsoleColor color);

            ComandosColorChooser.ForegroundColor = color;
            
        }

        #endregion







        #region // Download Nircmd Function
        
        // Export the native Windows API Library
        // Call native MessageBox Function

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr h, string s, string m, int type);


        public static void _DownloadNircmd()
        {

            var nirCmdPath = @"C:\nircmd\";
            var nirCmdZipName = @"nircmd.zip";
            var nirCmdBacupDirectory = @"C:\NircmdRestore\";
            var nircmdBacupZip = @"C:\NircmdRestore\NircmdRestore.zip";


            try {
                
                var nircmdZipFile = Path.Combine(nirCmdPath, nirCmdZipName);
                var nirCmdDirectory = new DirectoryInfo(nirCmdPath);


                if (nirCmdDirectory.Exists)
                {
                    var nircmdFiles = new List<string>(Directory.GetFiles(nirCmdPath)
                                      .Select(file => file)
                                      .Where(file => file.Contains("nir"))
                                      .ToList());


                    if (nircmdFiles.Count == 3) ComandosPrinter.WriteLine("You have nircmd");
                    else
                    {
                        ComandosPrinter.WriteLine
                        (
                          "\n" +
                          "You don't have nircmd\n\n" +
                          "1) Recover from the archive\n" +
                          "2) Download from official website"

                        );

                        switch (Convert.ToInt32(ComandosReader.ReadLine()))
                        {

                            case 1:
                                ZipFile.ExtractToDirectory(nircmdBacupZip, nirCmdPath);
                                break;
                            case 2:
                                download(MoveNirCmdZip.No);
                                break;
                        };
                    }

                    
                }

                else download(MoveNirCmdZip.Yes); 


                void download(MoveNirCmdZip move)
                {
                    Task.Run(function: async () =>
                    {

                        WebClient webClient = new WebClient();
                        webClient.DownloadFileAsync
                        (
                            new Uri("https://www.nirsoft.net/utils/nircmd.zip"),
                            Directory.CreateDirectory(nirCmdPath).FullName + nirCmdZipName
                        );


                        webClient.DownloadFileCompleted += (sender, AsyncCompletedArgs) =>
                        {


                            if (move.HasFlag(MoveNirCmdZip.No))
                            {
                                ZipFile.ExtractToDirectory(nircmdZipFile, nirCmdPath);
                                new FileInfo(nircmdZipFile).Delete();
                            }


            
                            if (move.HasFlag(MoveNirCmdZip.Yes))
                            {
                                ZipFile.ExtractToDirectory(nircmdZipFile, nirCmdPath);

                                var nirZip = new FileInfo(nircmdZipFile)
                                {
                                    Attributes = FileAttributes.Hidden
                                };

                                var bacupDir = Directory.CreateDirectory(nirCmdBacupDirectory);
                                nirZip.MoveTo(nircmdBacupZip);
                                

                                bacupDir.Attributes = FileAttributes.Hidden;
                            }


                            MessageBox((IntPtr)0, $"Nircmd is downloaded and unpacked to {nirCmdPath}", "Message", 0);

                        };


                    });

                }

            }catch (UnauthorizedAccessException ex) 
            {
                ComandosPrinter.WriteLine(ex.Message + "\nOpen the program with administrator rights" ); 
                 
            }

        }

        #endregion







        #region // Run Nircmd Function

        public static void _StartNirmcd()
        {

            using var nircmd = new Process();
            nircmd.StartInfo.FileName = @"C:\nircmd\nircmd.exe";
            

            ComandosPrinter.WriteLine();
            ComandosPrinter.Write("nircmd>>");

            string nircmdComand;
            while ((nircmdComand = ComandosReader.ReadLine()) != "exit")
            {
                ComandosPrinter.Write("nircmd>>");
                nircmd.StartInfo.Arguments = nircmdComand;
                nircmd.Start();

            }

        }

          #endregion


    }
}
