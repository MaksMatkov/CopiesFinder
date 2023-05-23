using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopiesFinderNext.Abstraction
{
    interface ICopyFinder
    {
        public bool Find(ProgressBar ProgresBar = null);
        public string GetResult();
        public void PrintResult();
        public int GetFilesCount();
        public string ParseString(List<FileInfo> g);

    }
}
