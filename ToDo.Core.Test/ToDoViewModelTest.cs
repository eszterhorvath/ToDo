using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvvmCross.Navigation;
using MvvmCross.Tests;
using ToDo.Core.Models;
using ToDo.Core.Services;
using ToDo.Core.ViewModels;

namespace ToDo.Core.Test
{
    [TestClass]
    public class ToDoViewModelTest
    {
        private ToDoViewModel _viewModel;
        private Mock<IToDoService> _todoService;
        private Mock<IMvxNavigationService> _navigationService;
        private Mock<IUserDialogs> _userDialogService;

        [TestInitialize]
        public void SetUp()
        {
            _todoService = new Mock<IToDoService>();
            _navigationService = new Mock<IMvxNavigationService>();
            _userDialogService = new Mock<IUserDialogs>();

            _viewModel = new ToDoViewModel(_todoService.Object, _navigationService.Object, _userDialogService.Object);
        }

        [TestMethod]
        public async Task WhenInitializingToDoViewModel_LoadsTodosFromDB()
        {
            // ARRANGE
            var todos = new List<Models.ToDo>()
            {
                new Models.ToDo()
                {
                    Title = "Shopping",
                    Description = "Buy wine",
                    State = State.Pending
                },
                new Models.ToDo()
                {
                    Title = "Learning",
                    Description = "Math",
                    State = State.Pending
                }
            };

            _todoService.Setup(s => s.GetTodosAsync()).ReturnsAsync(todos);

            // ACT
            await _viewModel.Initialize();

            // ASSERT
            _todoService.Verify(s => s.GetTodosAsync(), Times.Once);

            foreach (var todo in todos)
            {
                Assert.IsTrue(_viewModel.ToDos.Contains(todo));
            }
        }

        [TestMethod]
        public async Task WhenChangingStateOfATodo_ChangesTheStateAndReloadsTheTodos()
        {
            // ARRANGE
            var todoItem = new Models.ToDo()
            {
                State = State.Pending
            };

            _todoService.Setup(s => s.GetTodosAsync()).ReturnsAsync(new List<Models.ToDo>());

            // ACT
            await _viewModel.ChangeState(todoItem);

            // ASSERT
            _todoService.Verify(s => s.GetTodosAsync(), Times.Once);

            Assert.AreEqual(State.Done, todoItem.State);
        }

        [TestMethod]
        public async Task WhenAddingNewTodo_NavigatesToAddViewModel()
        {
            // ARRANGE
            _navigationService.Setup(
                s => s.Navigate<AddViewModel>(null, default)).ReturnsAsync(true);

            // ACT
            await _viewModel.AddNewTodo();

            // ASSERT
            _navigationService.Verify(
                s => s.Navigate<AddViewModel>(null, default), Times.Once);
        }

        [TestMethod]
        public async Task WhenEditingATodo_NavigatesToEditViewModel()
        {
            // ARRANGE
            var todoItem = new Models.ToDo()
            {
                Title = "Shopping",
                Description = "Buy wine",
                State = State.Pending
            };

            _navigationService.Setup(
                s => s.Navigate<EditViewModel, Models.ToDo>(todoItem, null, default)).ReturnsAsync(true);

            // ACT
            await _viewModel.EditTodo(todoItem);

            // ASSERT
            _navigationService.Verify(
                s => s.Navigate<EditViewModel, Models.ToDo>(todoItem, null, default), Times.Once);
        }

        [TestMethod]
        public async Task WhenRemovingATodo_IfConfirmed_RemovesTheSelectedTodoAndReloadsTheTodos()
        {
            // ARRANGE
            var todoItem = new Models.ToDo()
            {
                Id = 1,
                Title = "Shopping",
                Description = "Buy wine",
                State = State.Pending
            };

            _viewModel.ToDos = new ObservableCollection<Models.ToDo>
            {
                todoItem
            };

            _userDialogService.Setup(s => s.ConfirmAsync(It.IsAny<ConfirmConfig>(), default)).ReturnsAsync(true);
            _todoService.Setup(s => s.DeleteTodo(It.IsAny<Models.ToDo>())).ReturnsAsync(1);
            _todoService.Setup(s => s.GetTodosAsync()).ReturnsAsync(new List<Models.ToDo>());

            // ACT
            await _viewModel.RemoveTodo(todoItem);

            // ASSERT
            _userDialogService.Verify(s => s.ConfirmAsync(It.IsAny<ConfirmConfig>(), default), Times.Once);
            _todoService.Verify(s => s.DeleteTodo(It.IsAny<Models.ToDo>()), Times.Once);
            _todoService.Verify(s => s.GetTodosAsync(), Times.Once());
        }

        [TestMethod]
        public async Task WhenRemovingATodo_IfNotConfirmed_NotRemovesTheSelectedTodo()
        {
            // ARRANGE
            var todoItem = new Models.ToDo()
            {
                Title = "Shopping",
                Description = "Buy wine",
                State = State.Pending
            };

            _userDialogService.Setup(s => s.ConfirmAsync(It.IsAny<ConfirmConfig>(), default)).ReturnsAsync(false);
            _todoService.Setup(s => s.DeleteTodo(It.IsAny<Models.ToDo>())).ReturnsAsync(1);
            _todoService.Setup(s => s.GetTodosAsync()).ReturnsAsync(new List<Models.ToDo>());


            // ACT
            await _viewModel.RemoveTodo(todoItem);

            // ASSERT
            _userDialogService.Verify(s => s.ConfirmAsync(It.IsAny<ConfirmConfig>(), default), Times.Once);
            _todoService.Verify(s => s.DeleteTodo(It.IsAny<Models.ToDo>()), Times.Never);
            _todoService.Verify(s => s.GetTodosAsync(), Times.Never);
        }
    }
}
