using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform.Core.Components.Logging
{
    public class Logger
    {
        public Serilog.Core.Logger CreateLogger()
        {
            string fileName = $"{DateTime.UtcNow}.txt";

            return new LoggerConfiguration().MinimumLevel.Debug().WriteTo.RollingFile("").CreateLogger();
        }
    }
}
