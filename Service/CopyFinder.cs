using CopiesFinder.Abstraction;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CopiesFinder.Service
{
    public class CopyFinder : ICopyFinder
    {
        private string _path;
        private int _maxTaskCount;

        private IEnumerable<System.IO.FileInfo> _filaeList;
            
        private int filesCount = 0;
        
        private TimeSpan executeTime;

        private List<string> _output = new List<string>();

        public CopyFinder(string path, int maxTaskCount = 3)
        {
            _path = path;
            _maxTaskCount = maxTaskCount;
        }

        public bool Find()
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(_path);
                _filaeList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                filesCount = _filaeList.Count();
                sw.Start();
                Console.WriteLine("Files count: " + filesCount);
                Console.WriteLine("Processing...");

                var result = _filaeList.AsParallel().GroupBy(el => el.Length).ToList();
                var tasks = new List<Task>();

                result.ForEach(g =>
                {
                    //if one file in group it haven`t copies 
                    if (g.Count() > 1)
                    {
                        //Check if task list is full
                        if (tasks.Count() > _maxTaskCount)
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
                                    var fG = g.GroupBy(el => CalculateMD5(el.FullName)).ToList();

                                    _output.AddRange(fG.Where(g2 => g2.Count() > 1).Select(el => ParseString(el.ToList())));
                                }
                                catch
                                {
                                    ///file can be so big and string of bites is out of range So I added converting to Int64
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"!!!So big file: {(g != null && g.Count() > 0 ? g.FirstOrDefault().FullName : "No File")} -- Try use .ToInt64");

                                    var fG = g.GroupBy(el => BitConverter.ToInt64(File.ReadAllBytes(el.FullName))).ToList();

                                    _output.AddRange(fG.Where(g2 => g2.Count() > 1).Select(el => ParseString(el.ToList())));
                                }
                            }
                            catch
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"!!!Broken file: {(g != null && g.Count() > 0 ? g.FirstOrDefault().FullName : "No File")}");
                            }
                        });

                        //Add new Task to list
                        tasks.Add(newTasks);
                    }
                });

                Task.WaitAll(tasks.ToArray());
                sw.Stop();
                executeTime = sw.Elapsed;
            }
            catch
            {
                return false;
            }

            return true;
        }

        public string GetResult()
        {
            return String.Join("\n", _output);
        }

        public string ParseString(List<FileInfo> g)
        {
            return "\n<------------------------->NEW\n" + String.Join("\n", g.Select(el => el.FullName)) + $"\n<-------------------------Count: {g.Count()}\n";
        }

        public void PrintResult()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(GetResult());
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Execute time: " + executeTime.ToString());
        }

        private static string CalculateMD5(string filename)
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

        public int GetFilesCount() => filesCount;
    }
}
