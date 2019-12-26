using Microsoft.EntityFrameworkCore;
using Platform.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformUnitTests.Helpers
{
    public class DatabaseHelpers
    {
        public static PlatformDbContext GetDatabase(string name) {
            var builder = new DbContextOptionsBuilder<PlatformDbContext>().UseInMemoryDatabase(name);
            var o = builder.Options;

            var db = new PlatformDbContext(o);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return db;
        }
    }
}
