using Platform.Core.Exceptions;
using Platform.Core.Services;
using Platform.Database;
using PlatformUnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PlatformUnitTests.Core.Services
{
    public class UserServiceTests : IDisposable
    {
        private PlatformDbContext _db { get; set; }
        private UserService service { get; set; }
        public UserServiceTests()
        {
            _db = DatabaseHelpers.GetDatabase("UserService");
            service = new UserService(_db);
        }
        public void Dispose()
        {
            _db.Dispose();
        }

        [Fact]
        public void CreateUserFailsWithNoPassword()
        {
            var u = DataCreationHelpers.MakeUser();

            Assert.Throws<ArgumentException>(delegate
            {
                service.Create(u, null);
            });
            Assert.Empty(_db.Users);
        }
        [Fact]
        public void CreateUserFailsWithShortPassword()
        {
            var u = DataCreationHelpers.MakeUser();

            Assert.Throws<ArgumentException>(delegate
            {
                service.Create(u, "1234567");
            });
            Assert.Empty(_db.Users);
        } 
        [Fact]
        public void CreateUserFailsWithDuplicates()
        {
            var u = DataCreationHelpers.MakeUser();

            _db.Users.Add(u);
            _db.SaveChanges();
            Assert.Throws<PlatformException>(delegate
            {
                service.Create(u, "123456789");
            });
            Assert.Single(_db.Users);
        }
        [Fact]
        public void CreateUserInsertsANewRecord()
        {
            var u = DataCreationHelpers.MakeUser();

            var result = service.Create(u, "123456789");

            Assert.Equal(u.Id, result.Id);
            Assert.Single(_db.Users);
            var fromDb = _db.Users.Find(u.Id);
            Assert.NotNull(fromDb);

            Assert.Equal(result, fromDb);
        }
        [Fact]
        public void AuthenticateReturnsNullWhenEmailIsNull()
        {
            Assert.Null(service.Authenticate(null, "123456789"));
        }
        [Fact]
        public void AuthenticateReturnsNullWhenPasswordIsNull()
        {
            Assert.Null(service.Authenticate("test@test.com", null));
        }
        [Fact]
        public void AuthenticateReturnsNullWhenUserDoesNotExist()
        {
            Assert.Null(service.Authenticate("test@test.com", "123456789"));
        }
        [Fact]
        public void AuthenticateReturnsNullWhenpasswordIsWrong()
        {
            var u = DataCreationHelpers.MakeUser();
            service.Create(u, "123456789");

            Assert.Null(service.Authenticate("test@test.com", "wrongPassword"));
        }
        [Fact]
        public void AuthenticateReturnsAUser()
        {
            var u = DataCreationHelpers.MakeUser();
            service.Create(u, "123456789");

            Assert.Null(service.Authenticate("test@test.com", "1123456789"));
        }
        [Fact]
        public void DeleteDeletesAUser()
        {
            var u = DataCreationHelpers.MakeUser();
            service.Create(u, "123456789");
            Assert.Single(_db.Users);
            service.Delete(u.Id);
            Assert.Empty(_db.Users);
        }
        [Fact]
        public void DeleteDoesNothingIfIncorrectId()
        {
            var u = DataCreationHelpers.MakeUser();
            service.Create(u, "123456789");
            Assert.Single(_db.Users);
            service.Delete(Guid.NewGuid());
            Assert.Single(_db.Users);
        }
        [Fact]
        public void GetAllReturnsAList()
        {
            var u = DataCreationHelpers.MakeUser();
            service.Create(u, "123456789");
            var u2 = DataCreationHelpers.MakeUser(email: "test2@test.com");
            service.Create(u2, "123456789");
            var u3 = DataCreationHelpers.MakeUser(email: "test3@test.com");
            service.Create(u3, "123456789");

            var users = service.GetAll();

            Assert.Equal(3, users.Count());
        }
        [Fact]
        public void GetByIdReturnsTheCorrectOne()
        {
            var u = DataCreationHelpers.MakeUser();
            service.Create(u, "123456789");
            var u2 = DataCreationHelpers.MakeUser(email: "test2@test.com");
            service.Create(u2, "123456789");
            var u3 = DataCreationHelpers.MakeUser(email: "test3@test.com");
            service.Create(u3, "123456789");

            Assert.NotNull(service.GetById(u.Id));
        }
        [Fact]
        public void UpdateThrowsIfUserDoesntExist()
        {
            var u = DataCreationHelpers.MakeUser();

            Assert.Throws<PlatformException>(delegate
            {
                service.Update(u, "1234567");
            });
            Assert.Empty(_db.Users);
        }
        [Fact]
        public void UpdateUpdatesEmail()
        {
            var u = DataCreationHelpers.MakeUser();
            service.Create(u, "123456789");

            u.Email = "testing123@test.com";
            service.Update(u);
            var savedUser = _db.Users.Find(u.Id);
            Assert.Equal(u.Email, savedUser.Email);
            Assert.NotEqual(savedUser.UpdatedAt, savedUser.CreatedAt);
        }
        [Fact]
        public void UpdateUpdatesPassword()
        {
            var u = DataCreationHelpers.MakeUser();
            service.Create(u, "123456789");
            var savedUser = _db.Users.Find(u.Id);

            service.Update(u, "ILikeePieeee");
            var savedUser2 = _db.Users.Find(u.Id);
            Assert.Equal(savedUser.PasswordHash,savedUser2.PasswordHash);
        }
    }
}
