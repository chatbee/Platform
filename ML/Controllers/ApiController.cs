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
    public class ApiController : ControllerBase
    {
      
        public ActionResult Train(TrainingRequest request)
        {
            var trainingResponse = Service.Train(request);
            return Ok(trainingResponse);
        }

        public ActionResult Score()
        {
            var resp = new TrainingResponse();
            return Ok(resp);
        }


    }
}