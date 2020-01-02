using System;
using Xunit;
using Chatbee.ML;
using Chatbee.ML.Models;
using System.Collections.Generic;

namespace Chatbee.ML.Tests
{
    public class Service
    {
        private TrainingRequest CreateSampleTrainingRequest()
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

            return req;
        }

        [Fact]
        public void TrainAndPredict()
        {

            //create sample training request from data
            var req = CreateSampleTrainingRequest();

            //update properties for request
            req.LoadAfterTraining = true;
            req.ReturnFileData = true;

            //pass in request for training
            var response = ML.Service.Train(req);
            
            //response should not be null
            Assert.NotNull(response);

            //model was loaded after training
            Assert.NotEmpty(ML.Service.LoadedModels);

            //file data was not returned
            Assert.NotNull(response.ModelData);

            //predict statements for testing
            var scoringRequest = new ScoringRequest();
            scoringRequest.ModelName = response.ModelName;
            scoringRequest.Utterance = "hi";

            var scoringResponse = ML.Service.Predict(scoringRequest);

            Assert.Equal("#greet", scoringResponse.Result.Prediction);

            scoringRequest.Utterance = "bye";
            scoringResponse = ML.Service.Predict(scoringRequest);

            Assert.Equal("#farewell", scoringResponse.Result.Prediction);

        }

        [Fact]
        public void LoadAfterTraining()
        {

            //create sample training request from data
            var req = CreateSampleTrainingRequest();

            //update properties for request
            req.LoadAfterTraining = true;

            //pass in request for training
            var response = ML.Service.Train(req);

            //test loaded model
            Assert.NotEmpty(ML.Service.LoadedModels);
        }
    }
}
