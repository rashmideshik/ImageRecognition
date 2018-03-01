using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageRecognition
{
    class ConsoleOutputWriter : IOutputWriter
    {
        public void WriteOutput(string value)
        {
            Console.WriteLine(value);
        }
    }
}
