using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform.Core
{
    public class AppSettings
    {
        public string JwtSecret { get; set; }
        public bool UseSeededData { get; set; }
    }
}
