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
                Console.Write("Input path: ");
                string input = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(input))
                {
                    Start();
                }
                else
                    Start(input);
            }
            catch
            {
                Console.WriteLine("Invalid directory");
            }
        }

        /// <summary>
        /// Get Files Copies Using multi-tasks
        /// </summary>
        /// <param name="startFolder"> folder with files </param>
        static void Start(string startFolder = @"F:\test")
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories).ToList();

            //  fileList = fileList.Take(2000);
            Console.WriteLine("Files Count: " + fileList.Count());
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // getting groups with same size
            var result = fileList.AsParallel().GroupBy(el => el.Length).ToList();
            var tasks = new List<Task>();

            result.ForEach(g =>
            {
                //if one file in group it haven`t copies 
                if (g.Count() > 1)
                {
                    //Check if task list is full
                    if (tasks.Count() > MaxTaskCount)
                    {
                        Task.WaitAny(tasks.ToArray());
                    }

                    //Add new async Task
                    var newTasks = Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            try
                            {
                               // var fG = g.GroupBy(el => BitConverter.ToString(File.ReadAllBytes(el.FullName))).ToList();
                                var fG = g.GroupBy(el => CalculateMD5(el.FullName)).ToList();

                                fG.ForEach(g2 =>
                                {
                                    if (g2.Count() > 1)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine($"File Name:  {g2.First().Name} --> {g2.Count() - 1} Copies");
                                    }
                                });
                            }
                            catch
                            {
                                ///file can be so big and string of bites is out of range So I added converting to Int64
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"!!!So big file: {(g != null && g.Count() > 0 ? g.FirstOrDefault().FullName : "No File")} -- Try use .ToInt64");

                                var fG = g.GroupBy(el => BitConverter.ToInt64(File.ReadAllBytes(el.FullName))).ToList();

                                fG.ForEach(g2 =>
                                {
                                    if (g2.Count() > 1)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine($"File Name:  {g2.First().Name} --> {g2.Count() - 1} Copies");
                                    }
                                });
                            }
                        }
                        catch
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"!!!Broken file: {(g != null && g.Count() > 0 ? g.FirstOrDefault().FullName : "No File" )}");
                        }
                    });

                    //Add new Task to list
                    tasks.Add(newTasks);
                }
            });

            //Wait when all task finished
            Task.WaitAll(tasks.ToArray());
            sw.Stop();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("Ex-Time: " + sw.Elapsed);
        }

        public static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
