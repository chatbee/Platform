using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbee.ML.Models
{
    public class Output
    {
        public bool ExactMatch { get; set; }
        [ColumnName("PredictedLabel")]
        public string Prediction { get; set; }
        public float[] Score { get; set; }

    }
}
