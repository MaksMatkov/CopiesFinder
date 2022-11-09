using CopiesFinder.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task1
{
    class Program
    {
        public static int MaxTaskCount = 4;

        static void Main(string[] args)
        {
            try
            {
                Console.Write("'CopyFinder' ---> Input directory path: ");
                string input = Console.ReadLine();
                if (!String.IsNullOrWhiteSpace(input))
                {
                    CopyFinder cf = new CopyFinder(input);
                    cf.Find();
                    cf.PrintResult();
                }
                else
                    Console.WriteLine("Invalid directory");
            }
            catch
            {
                Console.WriteLine("Invalid directory");
            }
            finally
            {
                Console.WriteLine("Press enter to close...");
                Console.ReadLine();
            }
        }
    }
}
