using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Platform.Controllers.Api;
using Platform.Core;
using Platform.Core.Components.Logging;
using Platform.Core.Models;
using Platform.Core.Models.Api.Responses;
using Platform.Core.Services;
using PlatformUnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PlatformUnitTests.Controllers.Api
{
    public class UsersControllerTests: IDisposable
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IAppLogger> _mockAppLogger;
        private Mock<IOptions<AppSettings>> _mockOptions;
        public UsersControllerTests()
        {
            var settings = new AppSettings();
            settings.JwtSecret = "testMustBeLongereThan128Bitsasdfwqiuednfenwiufwenufuin2938745329042079437udfiwoecjwncwi";

            _mockUserService = new Mock<IUserService>();
            _mockAppLogger = new Mock<IAppLogger>();
            _mockOptions = new Mock<IOptions<AppSettings>>();
            _mockOptions.Setup(f => f.Value).Returns(settings);

            
        }

        public void Dispose()
        {
            _mockUserService.Reset();
            _mockAppLogger.Reset();
            _mockOptions.Reset();
        }

        [Fact]
        public async Task GetAllListsAllUsers()
        {

            var userList = new List<User> { DataCreationHelpers.MakeUser(), DataCreationHelpers.MakeUser(), DataCreationHelpers.MakeUser() };


            _mockUserService.Setup(f => f.GetAll()).Returns(userList);
            var controller = new UsersController(_mockUserService.Object, _mockOptions.Object, _mockAppLogger.Object);

            var result = await controller.GetUsers();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var list = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(userList.Count, list.Count);
        }
        [Fact]
        public async Task GetOneUser()
        {
            _mockUserService.Setup(f => f.GetById(It.IsAny<Guid>())).Returns(DataCreationHelpers.MakeUser());
            var controller = new UsersController(_mockUserService.Object, _mockOptions.Object, _mockAppLogger.Object);

            var result = await controller.GetUser(Guid.NewGuid());
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var user = Assert.IsType<User>(okResult.Value);
            Assert.NotNull(user);
        }
        [Fact]
        public async Task AuthFails()
        {
            _mockUserService.Setup(f => f.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns<User>(null);
            var controller = new UsersController(_mockUserService.Object, _mockOptions.Object, _mockAppLogger.Object);

            var result = await controller.Authenticate(new Platform.Core.Models.Api.AuthenticationModel());
            var unAuth = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            var errorResponse = Assert.IsType<ErrorResponse>(unAuth.Value);
            Assert.NotNull(errorResponse);
            Assert.Equal("Username or password is incorrect", errorResponse.Message);
        }
        
        [Fact]
        public async Task AuthenticationSucceeds()
        {
            var u = DataCreationHelpers.MakeUser();
            _mockUserService.Setup(f => f.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(u);
            var controller = new UsersController(_mockUserService.Object, _mockOptions.Object, _mockAppLogger.Object);

            var result = await controller.Authenticate(new Platform.Core.Models.Api.AuthenticationModel());
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var authResp = Assert.IsType<AuthenticationResponse>(ok.Value);
            Assert.NotNull(authResp);

            Assert.Equal(u.Id, authResp.Id);
            Assert.Equal(u.Email, authResp.Email);
            Assert.NotNull(authResp.Token);
        }
    }
}
