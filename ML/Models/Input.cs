using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbee.ML.Models
{

        public class Input
        {
            [ColumnName("Statement")]
            public string Utterance { get; set; }
            [ColumnName("Label")]
            public string Label { get; set; }

        }
    
}
