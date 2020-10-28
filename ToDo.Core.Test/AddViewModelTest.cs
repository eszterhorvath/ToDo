using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ToDo.Core.Models;
using ToDo.Core.Services;
using ToDo.Core.ViewModels;

namespace ToDo.Core.Test
{
    [TestClass]
    public class AddViewModelTest
    {
        private AddViewModel _viewModel;
        private Mock<IToDoService> _todoService;
        private Mock<IMvxNavigationService> _navigationService;
        private Mock<IUserDialogs> _userDialogService;

        [TestInitialize]
        public void SetUp()
        {
            _todoService = new Mock<IToDoService>();
            _navigationService = new Mock<IMvxNavigationService>();
            _userDialogService = new Mock<IUserDialogs>();

            _viewModel = new AddViewModel(_todoService.Object, _navigationService.Object, _userDialogService.Object);
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
    }
}
