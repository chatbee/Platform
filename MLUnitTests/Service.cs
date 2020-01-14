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
        public void TrainAndPredictLoadedModel()
        {

            //create sample training request from data
            var req = CreateSampleTrainingRequest();

            //update properties for request
            req.LoadAfterTraining = true;
            req.ReturnModelData = true;

            //pass in request for training
            var mlService = new MLService();
            var response = mlService.Train(req);
            
            //response should not be null
            Assert.NotNull(response);

            //model was loaded after training
            Assert.NotEmpty(mlService.LoadedModels);

            //file data was not returned
            Assert.NotNull(response.ModelData);

            //predict statements for testing

            //test exact matches
            var scoringRequest = new ScoringRequest();
            scoringRequest.ModelName = response.ModelName;
            scoringRequest.Utterance = "hi";

            var scoringResponse = mlService.Predict(scoringRequest);

            Assert.Equal("#greet", scoringResponse.Result.Prediction);
            Assert.True(scoringResponse.Result.ExactMatch);

            scoringRequest.Utterance = "bye";
            scoringResponse = mlService.Predict(scoringRequest);

            Assert.Equal("#farewell", scoringResponse.Result.Prediction);
            Assert.True(scoringResponse.Result.ExactMatch);

            //test machine learning matches

            scoringRequest.Utterance = "hallo";
            scoringResponse = mlService.Predict(scoringRequest);

            Assert.Equal("#greet", scoringResponse.Result.Prediction);
            Assert.False(scoringResponse.Result.ExactMatch);

        }

        [Fact]
        public void LoadAfterTraining()
        {
            var mlService = new MLService();

            //create sample training request from data
            var req = CreateSampleTrainingRequest();

            //update properties for request
            req.LoadAfterTraining = true;

            //pass in request for training
            var response = mlService.Train(req);

            //test loaded model
            Assert.NotEmpty(mlService.LoadedModels);
        }
    }
}
