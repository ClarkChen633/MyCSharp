using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Diagnostics;

namespace ExecuteCMD
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hellow ExecuteCMD ----- Main");
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);

            try
            {
                Process firstProcess = new Process();
                //Change file name for the process you want to start.
                firstProcess.StartInfo.FileName = "Test01.exe";
                firstProcess.EnableRaisingEvents = true;

                firstProcess.Start();
                firstProcess.WaitForExit();

                if (firstProcess.ExitCode != 0)
                {
                    //Put code here for when errors occur in the first process.
                }
                else //Code for when ExitCode == 0
                {
                    Process secondProcess = new Process();
                    //Change file name for the process you want to start.
                    secondProcess.StartInfo.FileName = "Test02.exe";
                    secondProcess.EnableRaisingEvents = true;

                    secondProcess.Start();
                    secondProcess.WaitForExit();

                    if (secondProcess.ExitCode != 0)
                    {
                        //Put code here for when errors occur in the first process.
                    }
                    else //Code for when ExitCode == 0
                    {
                        //Most likely this will be blank so that this program exits.
                    }
                }
            }
            //You should write your own catches.
            //This one is for no filename provided.
            catch (InvalidOperationException)
            {
                //Put code here for error help.
            }
        }
    }
}
