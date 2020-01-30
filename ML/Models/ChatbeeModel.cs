using Microsoft.ML;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbee.ML.Models
{
    public class ChatbeeModel
    {
        public List<Input> DataSet { get; set; } = new List<Input>();
        [JsonIgnore]
        public ITransformer Model { get; set; }
        public string ModelFileData { get; set; }

       public string SaveModelToFile(MLContext mlContext, DataViewSchema schema, string path)
        {
            //define temp model path
            string tempModelPath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".zip";

            //save model to temp file
            mlContext.Model.Save(Model, schema, tempModelPath);

            //read zip file bytes
            var modelZip = File.ReadAllBytes(tempModelPath);

            //convert to base64 string
            var base64String = Convert.ToBase64String(modelZip);
            ModelFileData = base64String;

            //create chatbees model file
            var modelName = Guid.NewGuid().ToString() + ".cbmdl";

            var chatbeeModelPath = path + modelName;

            //serialize class
            var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(this);

            //write serialized class
            File.WriteAllText(chatbeeModelPath, serializedData);

            //delete model from temp path
            File.Delete(tempModelPath);

            //return file name
            return chatbeeModelPath;
        }

        public ChatbeeModel LoadModelFromFile(MLContext mlContext, string modelFileName)
        {
            if (!System.IO.File.Exists(modelFileName))
            {
                //model was not found!
                throw new Exception($"Model not found at '{modelFileName}'");
            }
            else
            {
                //add to loaded models for faster future responses
                var modelInfo = System.IO.File.ReadAllText(modelFileName);
                var chatBeeModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ChatbeeModel>(modelInfo);
                var modelBytes = Convert.FromBase64String(chatBeeModel.ModelFileData);
                Stream stream = new MemoryStream(modelBytes);
                chatBeeModel.Model = mlContext.Model.Load(stream, out DataViewSchema inputSchema);
                return chatBeeModel;
            }

        }
    }
}
