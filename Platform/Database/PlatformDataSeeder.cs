using Microsoft.AspNetCore.Hosting;
using Platform.Core.Components.IO;
using Platform.Core.Models;
using Platform.Core.Services;
using Platform.Database.SeededData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform.Database
{
    public class PlatformDataSeeder
    {
        private readonly PlatformDbContext _context;
        private readonly string _folderPath;
        public PlatformDataSeeder(PlatformDbContext dbContext, IWebHostEnvironment env)
        {
            this._context = dbContext;
            this._folderPath = new PlatformFolders(env).GetFolderPath(PlatformFolders.FolderName.DataSeed);
        }
        private bool SeedUsers()
        {
            if (_context.Users.Any()) //already seeded
            {
                return true;
            }
            var path = System.IO.Path.Combine(_folderPath, "users.json");

            var contents = System.IO.File.ReadAllText(path);

            var users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SeededUser>>(contents);

            var userService = new UserService(_context);

            foreach (var user in users)
            {
                userService.Create(user, user.Password);
            }
            return true;
        }
        public bool SeedAll()
        {
            return SeedUsers();
        }
    }
}
