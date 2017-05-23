using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUWPer.Models
{
    public class BeatInfo
    {
        public string FileName { get; private set; }
        public string FilePath { get; private set; }

        public BeatInfo(string name, string path)
        {
            FileName = name;
            FilePath = path;
        }
    }
}
