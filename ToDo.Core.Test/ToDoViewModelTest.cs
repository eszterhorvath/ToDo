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

            _viewModel.SelectedTodo = todoItem;
            _todoService.Setup(s => s.SaveTodoAsync(todoItem)).ReturnsAsync(1);

            // ACT
            await _viewModel.ChangeState();

            // ASSERT
            _todoService.Verify(s => s.SaveTodoAsync(It.IsAny<Models.ToDo>()), Times.Once);

            Assert.AreEqual(State.Done, _viewModel.SelectedTodo.State);
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

            _viewModel.SelectedTodo = todoItem;
            _navigationService.Setup(
                s => s.Navigate<EditViewModel, Models.ToDo>(todoItem, null, default)).ReturnsAsync(true);

            // ACT
            await _viewModel.EditTodo();

            // ASSERT
            _navigationService.Verify(
                s => s.Navigate<EditViewModel, Models.ToDo>(todoItem, null, default), Times.Once);
        }

        [TestMethod]
        public async Task WhenRemovingATodo_IfConfirmed_DeletesTheSelectedTodoAndReloadsTheTodos()
        {
            // ARRANGE
            var todoItem = new Models.ToDo()
            {
                Id = 1,
                Title = "Shopping",
                Description = "Buy wine",
                State = State.Pending
            };

            _viewModel.SelectedTodo = todoItem;
            _viewModel.ToDos = new ObservableCollection<Models.ToDo>
            {
                todoItem
            };

            _todoService.Setup(s => s.DeleteTodo(It.IsAny<Models.ToDo>())).ReturnsAsync(1);

            // ACT
            await _viewModel.DeleteTodo();

            // ASSERT
            _todoService.Verify(s => s.DeleteTodo(It.IsAny<Models.ToDo>()), Times.Once);
            Assert.IsFalse(_viewModel.ToDos.Contains(todoItem));
        }

        [TestMethod]
        public async Task WhenSearchingATodo_SearchesTheTextInDB()
        {
            // ARRANGE
            string searched = "searched text";
            _viewModel.SearchedString = searched;

            _viewModel.ToDos = new ObservableCollection<Models.ToDo>
            {
                new Models.ToDo()
                {
                    Title = "Shopping",
                    Description = "Buy wine",
                    State = State.Pending
                }
            };

            _todoService.Setup(s => s.SearchTodo(searched)).ReturnsAsync(new List<Models.ToDo>());

            // ACT
            await _viewModel.SearchTodos(searched);

            // ASSERT
            _todoService.Verify(s => s.SearchTodo(searched), Times.Once);
        }
    }
}
