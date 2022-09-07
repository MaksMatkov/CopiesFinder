using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    class Program
    {
        public List<CompareItem> compareItems = new List<CompareItem>();
        public static bool IsDone = false;
        
        static void Main(string[] args)
        {

            Task thread = Task.Factory.StartNew(() => {
                while(!IsDone) { }
                Environment.Exit(0);
            });


            Console.Write("Input path: ");
            string input = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(input))
            {
                Start();
            }
            else
                Start(input);
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


            resultL.ForEach(g =>
            {
                var groupe = g.ToList();
                var head = groupe.FirstOrDefault();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"--Original File {head.Path}");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"--- {groupe.Count - 1} Copies");

            });



            sw.Stop();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("Ex-Time: " + sw.Elapsed);

            IsDone = true;
        }



        public static async Task<string> GetHash(string path)
        {
            return BitConverter.ToString(File.ReadAllBytes(path));
        }

        //public static string GetFileHash(string fileName)
        //{
        //    string fileHash = string.Empty;

        //    try
        //    {
        //        using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        //        {
        //            fileHash = Encoding.UTF8.GetString(new SHA1Managed().ComputeHash(fileStream));
        //        }
        //    }
        //    catch (FileNotFoundException ex)
        //    {
        //        // Handle your exceptions
        //        ex.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle your exceptions
        //        ex.ToString();
        //    }

        //    return fileHash;
        //}

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
