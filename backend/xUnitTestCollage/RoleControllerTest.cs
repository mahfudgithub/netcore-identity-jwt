using FakeItEasy;
using hidayah_collage.Controllers;
using hidayah_collage.Interface;
using hidayah_collage.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace xUnitTestCollage
{
    public class RoleControllerTest
    {
        private readonly Mock<IRole> roleService;
        public RoleControllerTest()
        {
            roleService = new Mock<IRole>();
        }

        private WebResponse webResponses()
        {
            WebResponse webResponsesData = new WebResponse() { status = true };

            return webResponsesData;
        }
        [Fact]
        public async Task WhenGetAllofRole()
        {
            //Arrange
            //var dataIStore = A.Fake<IRole>();
            //var controller = new RoleController(dataIStore);

            // var mockRoleClient = new Mock<IRole>();
            //mockRoleClient.Setup(x => x.GetAllRole());

            //var controller = new RoleController(dataIStore);
            var webResponse = webResponses();
            //var actionResult = await controller.GetAllRole();
            roleService.Setup(x => x.GetAllRole()).ReturnsAsync(webResponses);

            //Act
            var controller = new RoleController(roleService.Object);
            var result = await controller.GetAllRole();
            //var actionResult =  await controller.GetAllRole();
            //mockRoleClient.Verify(c => c.GetAllRole(), Moq.Times.Once);
            //var result = actionResult.Value;

            //Assert
            //mockRoleClient.Verify(c => c.GetAllRole(), Moq.Times.Once);
            //Assert.IsType<OkObjectResult>(result);
            Assert.True(webResponse.Equals(result));
            //var result = actionResult.Result as OkObjectResult;
        }
    }
}
