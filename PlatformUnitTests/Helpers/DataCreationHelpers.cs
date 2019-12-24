using Platform.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformUnitTests.Helpers
{
    public static class DataCreationHelpers
    {
        public static User MakeUser(string fName = "test", string lName = "user", string email = "test@test.com")
        {

            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = fName,
                LastName = lName,
                Email = email
            };
        }
    }
}
