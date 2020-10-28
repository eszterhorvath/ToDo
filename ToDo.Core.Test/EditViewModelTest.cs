using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language.Flow;
using MvvmCross.Navigation;
using ToDo.Core.Models;
using ToDo.Core.Services;
using ToDo.Core.ViewModels;

namespace ToDo.Core.Test
{
    [TestClass]
    public class EditViewModelTest
    {
        private EditViewModel _viewModel;
        private Mock<IToDoService> _todoService;
        private Mock<IMvxNavigationService> _navigationService;
        private Mock<IUserDialogs> _userDialogService;

        [TestInitialize]
        public void SetUp()
        {
            _todoService = new Mock<IToDoService>();
            _navigationService = new Mock<IMvxNavigationService>();
            _userDialogService = new Mock<IUserDialogs>();
            
            _viewModel = new EditViewModel(_todoService.Object, _navigationService.Object, _userDialogService.Object);
        }

        [TestMethod]
        public async Task SaveTodoTest()
        {
            // ARRANGE
            var todo = new Models.ToDo()
            {
                Title = "Shopping",
                Description = "Buy wine",
                State = State.Done
            };
            _viewModel.ToDoItem = todo;

            _todoService.Setup(s => s.SaveTodoAsync(It.IsAny<Models.ToDo>())).ReturnsAsync(1);
            _navigationService.Setup(s => s.Close(_viewModel, default)).ReturnsAsync(true);

            // ACT
            await _viewModel.SaveTodoAsync();

            // ASSERT
            _todoService.Verify(s => s.SaveTodoAsync(It.IsAny<Models.ToDo>()), Times.Once);
            _navigationService.Verify(s => s.Close(_viewModel, default), Times.Once);
        }

        [TestMethod]
        public async Task SaveTodoWithoutTitleTest()
        {
            // ARRANGE
            var todo = new Models.ToDo()
            {
                Title = "",
                Description = "Buy wine",
                State = State.Done
            };
            _viewModel.ToDoItem = todo;

            _userDialogService.Setup(s => s.Alert(It.IsAny<string>(), default, default)).Returns(Mock.Of<IDisposable>());

            // ACT
            await _viewModel.SaveTodoAsync();

            // ASSERT
            _todoService.Verify(s => s.SaveTodoAsync(It.IsAny<Models.ToDo>()), Times.Never);
            _navigationService.Verify(s => s.Close(_viewModel, default), Times.Never);
            _userDialogService.Verify(s => s.Alert(It.IsAny<string>(), default, default), Times.Once);
        }
    }
}
