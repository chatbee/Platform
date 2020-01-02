using Chatbee.ML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbee.ML
{
    public interface IService
    {
        Task Train(TrainingRequest message);
        Task Predict(ScoringRequest message);
    }
}
