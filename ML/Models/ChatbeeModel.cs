using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbee.ML.Models
{
    public class ChatbeeModel
    {
        public ITransformer Model { get; set; }
        public string Metadata { get; set; }

        public ChatbeeModel(ITransformer model, string meta = "")
        {
            this.Model = model;
            this.Metadata = meta;
        }

        public List<Input> GetDataset()
        {
            if (!string.IsNullOrEmpty(Metadata))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Input>>(Metadata);
            }
            else
            {
                throw new Exception("Metadata is not currently loaded");
            }

        }
    }
}
