using Chatbee.ML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbee.ML
{
    public interface IMLService
    {
        TrainingResponse Train(TrainingRequest message);
        ScoringResponse Predict(ScoringRequest message);
        MLStatusResponse Status();
    }
}
