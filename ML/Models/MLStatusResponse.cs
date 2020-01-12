using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbee.ML.Models
{
    public class MLStatusResponse
    {
        public int TotalModelsLoaded { get; set; }
        public string LoadedModels { get; set; }
        public DateTime ServiceLoaded { get; set; }
    }
}
