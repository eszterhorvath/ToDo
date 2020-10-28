using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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

        [TestInitialize]
        public void SetUp()
        {
            _todoService = new Mock<IToDoService>();
            _navigationService = new Mock<IMvxNavigationService>();

            _viewModel = new ToDoViewModel(_todoService.Object, _navigationService.Object);
        }

        [TestMethod]
        public async Task InitializeTest()
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
        public async Task ChangeStateTest()
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
        public async Task AddNewTodoTest()
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
        public async Task EditTodoTest()
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
    }
}
