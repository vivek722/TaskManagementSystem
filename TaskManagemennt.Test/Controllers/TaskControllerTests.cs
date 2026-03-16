using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagement.Domain.interfaces;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.DataResponseModel;
using TaskManagementSystem.DtoModels;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;
using TaskManagementSystem.services;
using Xunit;

namespace TaskManagemennt.Test.Controllers
{
    public class TaskControllerTests
    {
        private readonly Mock<ITaskServices> _taskServiceMock;
        private readonly Mock<ISubTaskService> _subTaskServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAiService> _aiServiceMock;
        private readonly TaskController _controller;

        public TaskControllerTests()
        {
            _taskServiceMock = new Mock<ITaskServices>();
            _subTaskServiceMock = new Mock<ISubTaskService>();
            _mapperMock = new Mock<IMapper>();
            _aiServiceMock = new Mock<IAiService>();

            _controller = new TaskController(
                _taskServiceMock.Object,
                _subTaskServiceMock.Object,
                _mapperMock.Object,
                _aiServiceMock.Object
            );
        }

        [Fact]
        public async Task GetAllTasks_ReturnsOk_WhenTasksExist()
        {
            var tasks = new List<TaskManage> { new TaskManage { Id = 1, Name = "T1" }, new TaskManage { Id = 2, Name = "T2" } };

            _taskServiceMock.Setup(s => s.GetAllTask()).ReturnsAsync(tasks);

            var result = await _controller.getAllTasks();

            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<List<TaskManage>>>(ok.Value);
            Assert.Equal(2, model.data.Count);
            Assert.Equal("getAllTasks successfully", model.Message);
        }

        [Fact]
        public async Task GetTaskById_ReturnsOk_WhenExists()
        {
            var task = new TaskManage { Id = 10, Name = "MyTask" };

            _taskServiceMock.Setup(s => s.GetByIdTask(10)).ReturnsAsync(task);

            var result = await _controller.getTaskbyId(10);

            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<TaskManage>>(ok.Value);
            Assert.Equal(10, model.data.Id);
            Assert.Equal("getTaskbyId successfully", model.Message);
        }

        [Fact]
        public async Task AddTasks_ReturnsOk_WhenValid_WithSubtasks()
        {
            var dto = new TaskDto
            {
                Name = "New",
                Description = "desc",
                subTaskManegs = new List<SubTaskManegdto> { new SubTaskManegdto { Name = "s1", Description = "sd" } }
            };

            var mappedTask = new TaskManage { Id = 100, Name = "New", Description = "desc-summarized" };
            var mappedSub = new SubTaskManeg { Id = 5, Name = "s1", Description = "sd-summarized" };

            _aiServiceMock.Setup(a => a.GenerateSummary(It.IsAny<string>())).ReturnsAsync((string s) => s + "-summarized");
            _mapperMock.Setup(m => m.Map<TaskManage>(It.IsAny<TaskDto>())).Returns(mappedTask);
            _mapperMock.Setup(m => m.Map<SubTaskManeg>(It.IsAny<SubTaskManegdto>())).Returns(mappedSub);

            _taskServiceMock.Setup(t => t.AddTask(It.IsAny<TaskManage>())).ReturnsAsync(mappedTask);
            _subTaskServiceMock.Setup(s => s.AddTask(It.IsAny<SubTaskManeg>())).ReturnsAsync(true);

            var result = await _controller.AddTasks(dto);

            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<TaskManage>>(ok.Value);
            Assert.Equal(100, model.data.Id);
            Assert.Equal("AddTasks successfully", model.Message);

            _subTaskServiceMock.Verify(s => s.AddTask(It.Is<SubTaskManeg>(st => st.TaskManageid == mappedTask.Id)), Times.Once);
        }

        [Fact]
        public async Task DeleteTask_ReturnsOk_WhenDeleted()
        {
            _taskServiceMock.Setup(s => s.DeleteTask(1)).ReturnsAsync(true);

            var result = await _controller.DeleteTask(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<bool>>(ok.Value);
            Assert.True(model.data);
            Assert.Equal("DeleteTask successfully", model.Message);
        }

        [Fact]
        public async Task UpdateTask_ReturnsOk_WhenUpdated_WithSubtasks()
        {
            var dto = new TaskDto
            {
                Name = "U",
                subTaskManegs = new List<SubTaskManegdto> { new SubTaskManegdto {  Name = "s2" } }
            };

            var mapped = new TaskManage { Name = "U" };

            _mapperMock.Setup(m => m.Map<TaskManage>(It.IsAny<TaskDto>())).Returns(mapped);
            _mapperMock.Setup(m => m.Map<SubTaskManeg>(It.IsAny<SubTaskManegdto>())).Returns(new SubTaskManeg { Id = 2 });
            _taskServiceMock.Setup(s => s.UpdateTask(3, It.IsAny<TaskManage>())).ReturnsAsync(true);
            _subTaskServiceMock.Setup(s => s.UpdateTask(2, It.IsAny<SubTaskManeg>())).ReturnsAsync(true);

            var result = await _controller.UpdateTask(3, dto);

            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<TaskManage>>(ok.Value);
            Assert.Equal("UpdateTask successfully", model.Message);
            _subTaskServiceMock.Verify(s => s.UpdateTask(2, It.IsAny<SubTaskManeg>()), Times.Once);
        }
    }
}