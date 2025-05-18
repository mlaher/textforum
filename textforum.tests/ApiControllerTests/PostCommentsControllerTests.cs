using AutoBogus;
using Bogus;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using textforum.api.Controllers;
using textforum.domain.interfaces;
using textforum.domain.models;
using textforum.logic.filters;

namespace textforum.tests.ApiControllerTests
{
    public class PostCommentsControllerTests
    {
        [Fact]
        public void AppAuth_ShouldAllow_AuthorisedRequests()
        {
            //Arrange
            var mockAppAuthService = getAppAuthenticationService();

            var filter = new AppAuthAttribute(mockAppAuthService.Object);

            var context = getAuthorizationFilterContext();

            //Act
            filter.OnAuthorization(context);
            var result = context.Result as ObjectResult;

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void AppAuth_ShouldReturnUnauthorised_WhenInvalidCredentialsProvided()
        {
            //Arrange
            var mockAppAuthService = getAppAuthenticationService();

            var filter = new AppAuthAttribute(mockAppAuthService.Object);

            var context = getAuthorizationFilterContext(new HeaderDictionary()
                    {
                        { "X-App-Token", "5678" },
                        { "X-Forwarded-For", "127.0.0.1" },
                        { "X-Machine-Name", "BadMachine" }
                    });

            //Act
            filter.OnAuthorization(context);
            var result = context.Result as JsonResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
            Assert.Equal("Unauthorized", JObject.FromObject(result?.Value ?? "")["message"]?.ToString());
        }

        [Fact]
        public async Task GetPostComments_ReturnsComments()
        {
            //Arrange
            var mockPostCommentService = getPostCommentServiceMock();
            var postComments = AutoFaker.Generate<PostComment>(3);

            mockPostCommentService.Setup(s => s.GetPostComments(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool?>())).ReturnsAsync(postComments);

            var controller = new PostCommentsController(mockPostCommentService.Object);

            controller.ControllerContext.HttpContext = getHttpContext();

            //Act
            var actionResult = await controller.GetPostComments("", "", "", "", 1, 1, 1);
            var result = actionResult.Result as OkObjectResult;
            var comments = (List<PostComment>?)result?.Value ?? new List<PostComment>();

            //Assert
            Assert.Equal(3, comments.Count);
        }

        [Fact]
        public async Task AddComment_ReturnsComment()
        {
            //Arrange
            var mockPostCommentService = getPostCommentServiceMock();
            var postComment = AutoFaker.Generate<PostComment>();
            mockPostCommentService.Setup(s => s.CreateComment(It.IsAny<PostComment>(), It.IsAny<string>())).ReturnsAsync(postComment);

            var controller = new PostCommentsController(mockPostCommentService.Object);

            controller.ControllerContext.HttpContext = getHttpContext();

            //Act
            var actionResult = await controller.AddComment("", "", "", "", postComment);
            var result = actionResult.Result as OkObjectResult;
            var comment = (PostComment?)result?.Value;

            //Assert
            Assert.NotNull(comment);
            Assert.IsType<PostComment>(comment);
            Assert.Equivalent(comment, postComment);
        }

        [Fact]
        public void AppAuth_ShouldReturnUnauthorized_WhenAppTokenIsMissing()
        {
            // Arrange
            var mockAppAuthService = getAppAuthenticationService();

            var filter = new AppAuthAttribute(mockAppAuthService.Object);

            var context = getAuthorizationFilterContext(new HeaderDictionary()
            {
                { "X-Forwarded-For", "127.0.0.1" },
                { "X-Machine-Name", "TestMachine" }
            });

            // Act
            filter.OnAuthorization(context);
            var result = context.Result as JsonResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
            Assert.Equal("Unauthorized", JObject.FromObject(result?.Value ?? "")["message"]?.ToString());
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnEmpty_WhenNoCommentsAvailable()
        {
            // Arrange
            var mockPostCommentService = getPostCommentServiceMock();
            mockPostCommentService.Setup(s => s.GetPostComments(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool?>()))
                                  .ReturnsAsync(new List<PostComment>());  // Simulate no comments available

            var controller = new PostCommentsController(mockPostCommentService.Object);
            controller.ControllerContext.HttpContext = getHttpContext();

            // Act
            var actionResult = await controller.GetPostComments("", "", "", "", 1, 1, 1);
            var result = actionResult.Result as OkObjectResult;
            var comments = (List<PostComment>?)result?.Value ?? new List<PostComment>();

            // Assert
            Assert.Empty(comments);  // Ensure no comments are returned
        }

        private HttpContext getHttpContext(HeaderDictionary? headerDictionary = null, Dictionary<object, object?>? requestItems = null)
        {
            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();

            if (headerDictionary == null)
            {
                headerDictionary = new HeaderDictionary()
                {
                    { "X-App-Token", "1234" },
                    { "X-Forwarded-For", "192.168.1.1" },
                    { "X-Machine-Name", "TestMachine" }
                };
            }

            if (requestItems == null)
            {
                requestItems = new Dictionary<object, object?>() { { "userid", "1" } };
            }

            mockRequest.Setup(req => req.Headers).Returns(headerDictionary);
            mockHttpContext.Setup(ctx => ctx.Request).Returns(mockRequest.Object);
            mockHttpContext.Setup(ctx => ctx.Items).Returns(requestItems);

            return mockHttpContext.Object;
        }

        private Mock<IPostCommentService> getPostCommentServiceMock()
        {
            var mockPostCommentService = new Mock<IPostCommentService>();

            return mockPostCommentService;
        }

        private Mock<IAppAuthenticationService> getAppAuthenticationService()
        {
            var mockAppAuthService = new Mock<IAppAuthenticationService>();

            mockAppAuthService.Setup(x => x.AuthenticateApp("1234", "192.168.1.1", "TestMachine", It.IsAny<string>())).Returns(true);

            return mockAppAuthService;
        }

        private AuthorizationFilterContext getAuthorizationFilterContext(HeaderDictionary? headerDictionary = null)
        {
            var context = new AuthorizationFilterContext(
                new ActionContext
                {
                    HttpContext = getHttpContext(headerDictionary),
                    RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                    ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
                },
                new List<IFilterMetadata>()
            );

            return context;
        }
    }
}
