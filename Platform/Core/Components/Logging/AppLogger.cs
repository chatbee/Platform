using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform.Core.Components.Logging
{
    public interface IAppLogger
    {
        void e(Exception ex);
    }
    public class AppLogger : IAppLogger
    {
        private readonly ILogger _logger;
        public AppLogger()
        {
            _logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.RollingFile("").CreateLogger();
        }

        public void e(Exception ex)
        {
            _logger.Error(ex, "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
        }
    }
}
