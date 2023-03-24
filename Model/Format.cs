using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSharingApp.Model
{
    public record Format
    {
        public string Profile { get; set; }
        public string StartTrim { get; set; }
        public string EndTrim { get; set; }
        
    }
}
