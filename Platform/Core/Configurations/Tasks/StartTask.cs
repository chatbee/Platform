using Chatbees.Engine.Configurations.Tasks;
using Chatbees.Engine.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform.Core.Configurations.Tasks
{
    public class StartTask : IStartTask
    {
        public string Name { get; set; }
        public Guid TaskId { get; set; }
        public string TaskType { get; set; }
        public string Description { get; set; }
        public Exception TaskException { get; set; }
        public string NextTaskId { get; set; }

        public string ExecuteTask(JobContext context)
        {
            throw new NotImplementedException();
        }
    }
}
