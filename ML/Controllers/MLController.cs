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
        /// <summary>
        /// Trains a new model which can be subsequently predicted.
        /// </summary>
        /// <param name="request">Represents the input which will be used to create a binary model.</param>
        /// <returns>TrainingResponse object which contains the training result.</returns>
        /// <remarks>This method must be called at least one time before attempting to make a prediction.</remarks>
        [HttpPost]
        [Route("Train")]
        public ActionResult Train(TrainingRequest request)
        {
            //api/ml/train 
            var trainingResponse = _mlService.Train(request);
            return Ok(trainingResponse);
        }

        /// <summary>
        /// Attempts to make a prediction (or 'Score') against a model.
        /// </summary>
        /// <param name="request">Represents the input that is required to be predicted.</param>
        /// <returns>ScoringResponse object which contains the prediction result.</returns>
        /// <remarks>A model must first be trained using the <c>Train</c> method before a prediction can take place.</remarks>
        [HttpPost]
        [Route("Score")]
        [Route("Predict")]
        public ActionResult Score(ScoringRequest request)
        {
            var scoringResponse = _mlService.Predict(request);
            return Ok(scoringResponse);
        }

        /// <summary>
        /// Returns the current status of the singleton ML Service.
        /// </summary>
        /// <returns>ML Service Data including loaded model information and initialization information.</returns>
        [HttpGet]
        [Route("Status")]
        public MLStatusResponse Status()
        {
 
            return _mlService.Status();
        }

        /// <summary>
        /// Sample Code demonstrating a successful training and result.  This method can be called to quickly generate a model for prediction testing. 
        /// </summary>
        /// <returns>A Training Response using pre-defined data.</returns>
        [HttpGet]
        [Route("TrainSample")]
        public TrainingResponse TrainSample()
        {
           
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

        /// <summary>
        /// Attempts to load all known model files.
        /// </summary>
        /// <returns>List of loaded models</returns>
        [HttpGet]
        [Route("LoadModels")]
        public List<string> LoadModels()
        {
            return _mlService.LoadModels();
        }

        /// <summary>
        /// Attempts to dispose all loaded model files
        /// </summary>
        /// <returns>List of loaded models</returns>
        [HttpGet]
        [Route("DisposeModels")]
        public List<string> DisposeModels()
        {
            return _mlService.DisposeModels();
        }





    }
}