using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary> 
/// Handles IO Functionality used by the Chat Platform 
/// </summary> 
namespace Platform.Core.Components.IO
{
    /// <summary> 
    /// Exposes static methods for accessing folders used by ChatPlatform. 
    /// </summary> 
    public class PlatformFolders
    {
        private readonly IWebHostEnvironment _env;
        public PlatformFolders(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary> 
        /// Returns a folder path to a specific ChatPlatform folder that is used by the ChatPlatform service 
        /// </summary> 
        /// <remarks> 
        /// If the directory does not exist, it will be automatically created. 
        /// </remarks> 
        /// <param name="folder">FolderName enumeration which represents the required folder.</param> 
        /// <returns>A full path to the requested folder.</returns> 
        public string GetFolderPath(FolderName folder, FolderType folderType = FolderType.Standard)
        {

            //get base path 
            string basePath = System.IO.Path.Combine(_env.ContentRootPath);

            //create var to store folder path 
            string folderPath = "";

            //switch based on folder requested 
            switch (folder)
            {
                case FolderName.BasePath:
                    folderPath = basePath;
                    break;
                //contains configs for services 
                case FolderName.ServerConfigs:
                    folderPath = System.IO.Path.Combine(basePath, "Configs");
                    break;
                case FolderName.ProfilePictures:
                    if (_env.EnvironmentName == "Development")
                    {
                        folderPath = System.IO.Path.Combine(basePath, "ClientApp", "src", "assets", "user", "photos");
                    }
                    else
                    {
                        folderPath = System.IO.Path.Combine(basePath, "wwwroot", "assets", "user", "photos");
                    }

                    break;
                case FolderName.BotDeployed:
                    folderPath = System.IO.Path.Combine(basePath, "Data", "BotConfigurations", "Deployed");
                    break;
                case FolderName.BotStaging:
                    folderPath = System.IO.Path.Combine(basePath, "Data", "BotConfigurations", "Staging");
                    break;
                case FolderName.TrainingRequests:
                    folderPath = System.IO.Path.Combine(basePath, "Data", "TrainingRequests");
                    break;
                case FolderName.ApplicationLogs:
                    folderPath = System.IO.Path.Combine(basePath, "Data", "Logs");
                    break;
                case FolderName.uploadedBots:
                    folderPath = System.IO.Path.Combine(basePath, "Data", "Uploads");
                    break;
                case FolderName.ScriptExecution:
                    folderPath = System.IO.Path.Combine(basePath, "Data", "ScriptExecution");
                    break;
                case FolderName.DataSeed:
                    folderPath = System.IO.Path.Combine(basePath, "Database", "SeededData");
                    break;
                default:
                    //occurs when enum is added and used without a switch definition 
                    throw new NotImplementedException("FolderName not implemented: " + folder.ToString());
            }
            if (folderType == FolderType.Backup)
            {
                folderPath = System.IO.Path.Combine(folderPath, "Backups");
            }

            //create folder if it does not exist? 
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            //return to user 
            return folderPath;


        }
        /// <summary> 
        /// Specifies enumerated constants used to retrieve directory paths to ChatPlatform folders. 
        /// </summary> 
        public enum FolderName
        {
            /// <summary> 
            /// The folder that contains underlying configurations consumed by the WebAPI backend. 
            /// </summary> 
            ServerConfigs,
            /// <summary> 
            /// The folder that contains Application Logs 
            /// </summary> 
            ApplicationLogs,
            /// <summary> 
            /// The folder where profile pictures are stored for use by ChatPlatform. 
            /// </summary> 
            ProfilePictures,
            /// <summary> 
            /// The folder where the bot configurations are for staging. 
            /// </summary> 
            BotStaging,
            /// <summary>
            /// The folder where the bot configurations are for Dpeloyed.
            /// </summary>
            BotDeployed,
            TrainingRequests,
            BasePath,
            uploadedBots,
            ScriptExecution,
            DataSeed,
        }

        /// <summary> 
        /// Specifies enumerated constants used to retrieve either the regular or backup folders. 
        /// </summary> 
        public enum FolderType
        {
            // <summary> 
            /// The regular folder for the foldername. 
            /// </summary> 
            Standard,
            // <summary> 
            /// The backup folder for the foldername. 
            /// </summary> 
            Backup
        }
    }
}
