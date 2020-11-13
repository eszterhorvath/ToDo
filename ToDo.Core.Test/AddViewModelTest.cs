using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvvmCross.Navigation;
using Plugin.Media.Abstractions;
using ToDo.Core.Models;
using ToDo.Core.Services;
using ToDo.Core.ViewModels;
using Xamarin.Essentials;

namespace ToDo.Core.Test
{
    [TestClass]
    public class AddViewModelTest
    {
        private AddViewModel _viewModel;
        private Mock<IToDoService> _todoService;
        private Mock<IMvxNavigationService> _navigationService;
        private Mock<IUserDialogs> _userDialogService;
        private Mock<IComputerVisionService> _computerVisionService;
        private Mock<IMediaService> _mediaService;

        [TestInitialize]
        public void SetUp()
        {
            _todoService = new Mock<IToDoService>();
            _navigationService = new Mock<IMvxNavigationService>();
            _userDialogService = new Mock<IUserDialogs>();
            _computerVisionService = new Mock<IComputerVisionService>();
            _mediaService = new Mock<IMediaService>();

            _viewModel = new AddViewModel(_todoService.Object, _navigationService.Object, _userDialogService.Object,
                _computerVisionService.Object, _mediaService.Object);
        }

        [TestMethod]
        public async Task WhenAddingATodo_WithValidTitle_SavesTodoToDBAndNavigatesBack()
        {
            // ARRANGE
            var todo = new Models.ToDo()
            {
                Title = "Shopping",
                Description = "Buy wine"
            };
            _viewModel.ToDoItem = todo;

            _userDialogService.Setup(s => s.Alert(It.IsAny<string>(), default, default)).Returns(Mock.Of<IDisposable>());
            _todoService.Setup(s => s.SaveTodoAsync(It.IsAny<Models.ToDo>())).ReturnsAsync(1);
            _navigationService.Setup(s => s.Close(_viewModel, default)).ReturnsAsync(true);

            // ACT
            await _viewModel.AddTodo();

            // ASSERT
            _userDialogService.Verify(s => s.Alert(It.IsAny<string>(), default, default), Times.Never);
            _todoService.Verify(s => s.SaveTodoAsync(It.IsAny<Models.ToDo>()), Times.Once);
            _navigationService.Verify(s => s.Close(_viewModel, default), Times.Once);


            Assert.AreEqual(State.Pending, todo.State);
        }

        [TestMethod]
        public async Task WhenAddingATodo_WithoutTitle_ShowsAlertAndDoesNotAddTheTodo()
        {
            // ARRANGE
            var todo = new Models.ToDo()
            {
                Description = "Buy wine"
            };
            _viewModel.ToDoItem = todo;

            _userDialogService.Setup(s => s.Alert(It.IsAny<string>(), default, default)).Returns(Mock.Of<IDisposable>());
            _todoService.Setup(s => s.SaveTodoAsync(It.IsAny<Models.ToDo>())).ReturnsAsync(1);
            _navigationService.Setup(s => s.Close(_viewModel, default)).ReturnsAsync(true);

            // ACT
            await _viewModel.AddTodo();

            // ASSERT
            _userDialogService.Verify(s => s.Alert(It.IsAny<string>(), default, default), Times.Once);
            _todoService.Verify(s => s.SaveTodoAsync(It.IsAny<Models.ToDo>()), Times.Never);
            _navigationService.Verify(s => s.Close(_viewModel, default), Times.Never);


            Assert.AreEqual(State.Pending, todo.State);
        }

        [TestMethod]
        public async Task WhenTakingAPicture_PhotoAndAnalysisAppearsOnScreen()
        {
            // ARRANGE
            var testPhoto = new MediaFile("", () => new MemoryStream(new byte[] {}));
            _mediaService.Setup(s => s.CapturePhoto()).ReturnsAsync(testPhoto);

            _computerVisionService.Setup(s => s.GetImageSummary(testPhoto)).ReturnsAsync("Recognized Summary");
            _computerVisionService.Setup(s => s.RecognizeText(testPhoto)).ReturnsAsync("Recognized Text");
            _computerVisionService.Setup(s => s.RecognizeTags(testPhoto)).ReturnsAsync(new List<ImageTag>());
            _computerVisionService.Setup(s => s.RecognizeObjects(testPhoto)).ReturnsAsync(new List<DetectedObject>());
            _computerVisionService.Setup(s => s.RecognizeBrands(testPhoto)).ReturnsAsync(new List<DetectedBrand>());
            _computerVisionService.Setup(s => s.RecognizeFaces(testPhoto)).ReturnsAsync(new List<FaceDescription>());

            // ACT
            await _viewModel.TakePhoto();

            // ASSERT
            _mediaService.Verify(s => s.CapturePhoto(), Times.Once());
            Assert.IsNotNull(_viewModel.ImageSource);

            _computerVisionService.Verify(s => s.GetImageSummary(testPhoto), Times.Once());
            _computerVisionService.Verify(s => s.RecognizeText(testPhoto), Times.Once());
            _computerVisionService.Verify(s => s.RecognizeTags(testPhoto), Times.Once());
            _computerVisionService.Verify(s => s.RecognizeObjects(testPhoto), Times.Once());
            _computerVisionService.Verify(s => s.RecognizeBrands(testPhoto), Times.Once());
            _computerVisionService.Verify(s => s.RecognizeFaces(testPhoto), Times.Once());
        }
    }
}
