using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopiesFinder.Abstraction
{
    interface ICopyFinder
    {
        public bool Find();
        public string GetResult();
        public void PrintResult();
        public int GetFilesCount();
        public string ParseString(List<FileInfo> g);

    }
}
