using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbee.ML.Models
{
    public class TrainingRequest
    {
        public List<Input> Dataset { get; set; } = new List<Input>();
        public bool LoadAfterTraining { get; set; } = false;
        public bool ReturnModelData { get; set; } = false;
    }
}
