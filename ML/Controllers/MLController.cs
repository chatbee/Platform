using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Chatbee.ML.Models;

namespace Chatbee.ML.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MLController : ControllerBase
    {
        private readonly IMLService _mlService;

        public MLController(IMLService mlService)
        {
            _mlService = mlService;
        }
        [HttpPost]
        [Route("Train")]
        public ActionResult Train(TrainingRequest request)
        {
            //api/ml/train 
            var trainingResponse = _mlService.Train(request);
            return Ok(trainingResponse);
        }

        [HttpPost]
        [Route("Score")]
        [Route("Predict")]
        public ActionResult Score(ScoringRequest request)
        {
            //request.ModelName
            //request.Utterance
            //api/ml/score && //api/ml/predict
            var scoringResponse = _mlService.Predict(request);
            return Ok(scoringResponse);
        }

        [HttpGet]
        [Route("Status")]
        public MLStatusResponse Status()
        {
            //api/ml/status
            return _mlService.Status();
        }

        [HttpGet]
        [Route("TrainSample")]
        public TrainingResponse TrainSample()
        {
            //api/ml/trainsample
            //create training request
            var req = new TrainingRequest();

            //create dataset
            var MLInputs = new List<Input>();
            MLInputs.Add(new Input() { Utterance = "hi", Label = "#greet" });
            MLInputs.Add(new Input() { Utterance = "hello", Label = "#greet" });
            MLInputs.Add(new Input() { Utterance = "hey", Label = "#greet" });
            MLInputs.Add(new Input() { Utterance = "whats up", Label = "#greet" });
            MLInputs.Add(new Input() { Utterance = "bye", Label = "#farewell" });
            MLInputs.Add(new Input() { Utterance = "later", Label = "#farewell" });
            MLInputs.Add(new Input() { Utterance = "goodbye", Label = "#farewell" });
            MLInputs.Add(new Input() { Utterance = "ttyl", Label = "#farewell" });
            MLInputs.Add(new Input() { Utterance = "yes", Label = "#confirm" });
            MLInputs.Add(new Input() { Utterance = "ya", Label = "#confirm" });
            MLInputs.Add(new Input() { Utterance = "yasssss", Label = "#confirm" });
            MLInputs.Add(new Input() { Utterance = "yes", Label = "#confirm" });
            MLInputs.Add(new Input() { Utterance = "y", Label = "#confirm" });
            MLInputs.Add(new Input() { Utterance = "no", Label = "#decline" });
            MLInputs.Add(new Input() { Utterance = "nah", Label = "#decline" });
            MLInputs.Add(new Input() { Utterance = "n", Label = "#decline" });
            MLInputs.Add(new Input() { Utterance = "narp", Label = "#decline" });
            MLInputs.Add(new Input() { Utterance = "nope", Label = "#decline" });
            req.Dataset = MLInputs;

            //update properties for request
            req.LoadAfterTraining = true;
            req.ReturnModelData = true;

            //pass in request for training
            var response = _mlService.Train(req);
            return response;


        }
            




    }
}