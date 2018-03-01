using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageRecognition
{
    class FileOutputWriter : IOutputWriter
    {
        public void WriteOutput(string value)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\temp\log.txt", true))
            {
                file.WriteLine(value);
            }
        }
    }
}
