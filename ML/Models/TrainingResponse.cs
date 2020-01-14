using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbee.ML.Models
{
    public class TrainingResponse 
    {
        public TrainingRequest Request { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        public string Result { get; set; }
        public string ModelName { get; set; }
        public ChatbeeModel ModelData { get; set; }
    }
}
