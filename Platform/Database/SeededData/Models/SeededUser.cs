using Platform.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform.Database.SeededData.Models
{
    public class SeededUser : User
    {
        public string Password { get; set; }
    }
}
