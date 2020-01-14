using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML;
using Chatbee.ML.Models;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Chatbee.ML
{
    public class MLService : IMLService
    {
        public DateTime ServiceLoaded { get; set; }
        public Dictionary<string, ChatbeeModel> LoadedModels { get; set; } = new Dictionary<string, ChatbeeModel>();
        public MLService()
        {
            ServiceLoaded = DateTime.Now;
        }

        /// <summary>
        /// Trains a new machine learning model and returns the file name
        /// </summary>
        /// <returns></returns>
        public TrainingResponse Train(TrainingRequest request)
        {
            //Todo: Load dataset here optionally?

            if (request.Dataset.Count == 0)
            {
                throw new Exception("No data loaded");
            }


            var response = new TrainingResponse();
            response.Request = request;

            response.Started = DateTime.Now;

            //log dataset as metadata
            //var jsonDataset = Newtonsoft.Json.JsonConvert.SerializeObject(request.Dataset);

            //create ml context for training
            var mlContext = new MLContext(seed: 0);
            var dataView = mlContext.Data.LoadFromEnumerable(request.Dataset);

            //create test train split
            DataOperationsCatalog.TrainTestData dataSplitView = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            IDataView trainData = dataSplitView.TrainSet;
            IDataView testData = dataSplitView.TestSet;

            //create pipeline for training
            IEstimator<ITransformer> mlPipeline = BuildTrainingPipeline(mlContext);

            //train model
            ITransformer mlModel = TrainModel(mlContext, trainData, mlPipeline);

            response.Finished = DateTime.Now;

            //create chatbee model
            var chatBeeModel = new ChatbeeModel();
            chatBeeModel.Model = mlModel;
            chatBeeModel.DataSet = request.Dataset;

            //save model and return file name
            var modelFileName = chatBeeModel.SaveModelToFile(mlContext, trainData.Schema, "Data\\Models\\");

            //set filename for response
            response.ModelName = modelFileName;

            //load if indicated
            if (request.LoadAfterTraining)
            {
                if (LoadedModels.ContainsKey(modelFileName))
                    LoadedModels.Remove(modelFileName);

                LoadedModels.Add(modelFileName, chatBeeModel);
            }

            //return file data if requested
            if (request.ReturnFileData)
            {
                response.ModelData = chatBeeModel.ModelFileData;
            }

            //set result
            response.Result = "Training Completed Successfully";

            //return model file name
            return response;
        }

        public ScoringResponse Predict(ScoringRequest request)
        {
            var response = new ScoringResponse();
            //Todo: Enable Exact Match attempt?



            var modelFileName = request.ModelName;
            var utterance = request.Utterance;

            MLContext mlContext = new MLContext();
            ChatbeeModel chatbeeModel;

            //load model if not loaded
            if (!LoadedModels.ContainsKey(modelFileName))
            {
                chatbeeModel = new ChatbeeModel().LoadModelFromFile(mlContext, modelFileName);
                LoadedModels.Add(modelFileName, chatbeeModel); 
            }

            ChatbeeModel loadedModel = LoadedModels[modelFileName];
            chatbeeModel = loadedModel;


            //attempt to find any exact matches before going to ml
            foreach (var item in chatbeeModel.DataSet)
            {
                if (item.Utterance == request.Utterance)
                {
                    //return for exact match
                    Output exactMatchOutput = new Output();
                    exactMatchOutput.ExactMatch = true;
                    exactMatchOutput.Prediction = item.Label;
                    response.Result = exactMatchOutput;
                    return response;
                }
              
            }


            var prediction = mlContext.Model.CreatePredictionEngine<Input, Output>(chatbeeModel.Model);
            var input = new Input() { Utterance = utterance };

            Output result = prediction.Predict(input);
            result.ExactMatch = false;

            //loop
            if (chatbeeModel.DataSet != null && chatbeeModel.DataSet.Count > 0)
            {
                for (int scr = 0; scr < result.Score.Length; scr++)
                {
                    var detailedScore = new ScoreDetail();
                    var datasetItem = chatbeeModel.DataSet[scr];
                    detailedScore.Label = datasetItem.Label;
                    detailedScore.Utterance = datasetItem.Utterance;
                    detailedScore.score = result.Score[scr];
                    result.Details.Add(detailedScore);
                }

               //todo: process score details
            }

            response.Result = result;

            return response;
        }

        public MLStatusResponse Status()
        {
            var response = new MLStatusResponse();
            response.LoadedModels = string.Join(", ", LoadedModels.Keys);
            response.TotalModelsLoaded = LoadedModels.Count;
            response.ServiceLoaded = ServiceLoaded;
            return response;
        }
           

        public void LoadModels()
        {
            throw new NotImplementedException();
        }
        public void UnloadModels()
        {
            throw new NotImplementedException();
        }
        public void ReloadModels()
        {
            throw new NotImplementedException();
        }

        #region MLdotNet
        public static IEstimator<ITransformer> BuildTrainingPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var dataProcessPipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", "Label")
                                      .Append(mlContext.Transforms.Text.FeaturizeText("Statement_tf", "Statement"))
                                      .Append(mlContext.Transforms.CopyColumns("Features", "Statement_tf"))
                                      .Append(mlContext.Transforms.NormalizeMinMax("Features", "Features"))
                                      .AppendCacheCheckpoint(mlContext);

            // Set the training algorithm
            var trainer = mlContext.MulticlassClassification.Trainers.OneVersusAll(mlContext.BinaryClassification.Trainers.AveragedPerceptron(labelColumnName: "Label", numberOfIterations: 10, featureColumnName: "Features"), labelColumnName: "Label")
                                      .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            return trainingPipeline;
        }
        public static ITransformer TrainModel(MLContext mlContext, IDataView trainingDataView, IEstimator<ITransformer> pipeline)
        {
            return pipeline.Fit(trainingDataView);
        }

        #endregion
    }
}
