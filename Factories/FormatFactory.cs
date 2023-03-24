using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using PhotoSharingApp.Model;
using Microsoft.Extensions.Primitives;

namespace PhotoSharingApp.Factories
{
    public class FormatFactory
    {
        private Format format = new Format();
        public FormatFactory(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            if(reader.ReadLine() != null)
            {
                string line = reader.ReadLine();
                string[] values = new string[3];
                values = line.Split(";");
                format.Profile = values[0];
                format.StartTrim = values[1];
                format.EndTrim = values[2];
            }
        }
        public Format getFormat() => format;
    }
}

          

            
  