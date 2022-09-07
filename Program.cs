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
        public List<CompareItem> compareItems = new List<CompareItem>();
        public static bool IsDone = false;
        
        static void Main(string[] args)
        {

            ////run this
            ///
            try
            {
                Console.Write("Input path: ");
                string input = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(input))
                {
                    Start2();
                }
                else
                    Start2(input);
            }
            catch
            {
                Console.WriteLine("Invalid directory");
            }
            
           // Start2();
            return;

            //Task thread = Task.Factory.StartNew(() => {
            //    while(!IsDone) { }
            //    Environment.Exit(0);
            //});


            //Console.Write("Input path: ");
            //string input = Console.ReadLine();
            //if (String.IsNullOrWhiteSpace(input))
            //{
            //    Start();
            //}
            //else
            //    Start(input);

        }

        

        static async void Start(string startFolder = @"F:\test")
        {  
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories).ToList();

          //  fileList = fileList.Take(2000);
            Console.WriteLine("Files Count: " + fileList.Count());
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var task = fileList.AsParallel().Select(async el =>
            {

                var hash = await GetHash(el.FullName);


                return new CompareItem() { Hash = hash, Path = el.FullName };
            });
            var result = await Task.WhenAll(task);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Getting Hash --- DONE -- {sw.Elapsed}" );

            var resultL = result.ToList().GroupBy(el => el.Hash).ToList();
            Console.WriteLine($"Grouping Copies --- DONE -- {sw.Elapsed}");


            Print(resultL);

            sw.Stop();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("Ex-Time: " + sw.Elapsed);
            IsDone = true;
        }


        /// <summary>
        /// test 2
        /// </summary>
        /// <param name="startFolder"></param>
        static void Start2(string startFolder = @"F:\Фото")
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories).ToList();

            //  fileList = fileList.Take(2000);
            Console.WriteLine("Files Count: " + fileList.Count());
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var result = fileList.AsParallel().GroupBy(el => el.Length).ToList();
            //  var result = await Task.WhenAll(task);


            var tasks = new List<Task>();

            result.ForEach(g =>
            {

                if (g.Count() > 1)
                {

                    if (tasks.Count() > 4)
                    {
                        Task.WaitAny(tasks.ToArray());
                    }

                    var ntasks = Task.Factory.StartNew(() =>
                    {
                        var fG = g.GroupBy(el => BitConverter.ToString(File.ReadAllBytes(el.FullName))).ToList();

                        fG.ForEach(g2 =>
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"--Original File {g2.First().FullName} --- {g2.Count() - 1} Copies");
                        });
                    });

                    tasks.Add(ntasks);
                }
            });

            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine($"Getting Hash --- DONE -- {sw.Elapsed}");

            //var resultL = result.ToList().GroupBy(el => GetHash(el.Hash)).ToList();
            //Console.WriteLine($"Grouping Copies --- DONE -- {sw.Elapsed}");


            // Print(resultL);

            Task.WaitAll(tasks.ToArray());

            sw.Stop();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("Ex-Time: " + sw.Elapsed);
           // IsDone = true;
        }


        public static void Print(List<IGrouping<string, CompareItem>> list)
        {
            list.ForEach(g =>
            {
                var groupe = g.ToList();
                var head = groupe.FirstOrDefault();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"--Original File {head.Path}");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"--- {groupe.Count - 1} Copies");

            });
        }



        public static async Task<string> GetHash(string path)
        {
            return BitConverter.ToString(File.ReadAllBytes(path));
        }

    }



    class CompareItem
    {
      //  public Guid ID { get; set; }
        public string Path { get; set; }
        public string Hash { get; set; }
        public bool IsMain { get; set; }
    }

    class CompareItem1
    {
      // public Guid ID { get; set; }
        public string Path { get; set; }
        //public string Hash { get; set; }
        public bool Touched { get; set; }
    }
}
