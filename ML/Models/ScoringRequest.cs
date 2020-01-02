using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbee.ML.Models
{
    public class ScoringRequest
    {
        public string ModelName { get; set; }
        public string Utterance { get; set; }
    }
}
