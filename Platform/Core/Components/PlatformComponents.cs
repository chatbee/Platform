using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.NodeServices;
using Platform.Core.Components.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform.Core.Components
{
    /// <summary>
    /// Initializes instances of Componet classes used by ChatPlatform.
    /// </summary>
    /// <remarks>
    ///  Consists of componennts such as IO, Logging, and Conversation Management.
    /// </remarks>
    public static class PlatformComponents
    {
        /// <value>
        /// Gets or Sets an Initialized logger Instance.
        /// </value>
        public static Serilog.Core.Logger Logger { get; set; }
        public static IWebHostEnvironment Env { get; private set; }
        public static INodeServices NodeServices { get; private set; }
        /// <summary>
        /// Initializes Platform Components for use in the platform.
        /// </summary>
        /// <remarks>
        /// Retrieves ComponentSettings and calls the initilization methods on the underyling Component classes.
        /// </remarks>
        /// <param name="env">Provides information about the web hosting environment the platform application is running in.</param>
        public static void InitializeComponents(IWebHostEnvironment env, INodeServices nodeServices)
        {
            Env = env;
            NodeServices = nodeServices;
        }

    }
}
