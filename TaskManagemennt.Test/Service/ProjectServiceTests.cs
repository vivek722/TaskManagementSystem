using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TaskManagementSystem.Model;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.services;
using TaskManagementSystem.ResponseDto;
using Xunit;

namespace TaskManagemennt.Test.Services
{
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _repoMock;
        private readonly ProjectService _service;

        public ProjectServiceTests()
        {
            _repoMock = new Mock<IProjectRepository>();
            _service = new ProjectService(_repoMock.Object);
        }

        [Fact]
        public async Task AddProject_DelegatesToRepository()
        {
            var p = new projectModel { Name = "P1" };
            _repoMock.Setup(r => r.AddProject(p)).ReturnsAsync(true);

            var res = await _service.AddProject(p);

            Assert.True(res);
            _repoMock.Verify(r => r.AddProject(p), Times.Once);
        }

        [Fact]
        public async Task GetAllProjects_ReturnsList()
        {
            var list = new List<projectModel> { new projectModel { Name = "X" } };
            _repoMock.Setup(r => r.GetAllProjects()).ReturnsAsync(list);

            var res = await _service.GetAllProjects();

            Assert.Same(list, res);
        }

        [Fact]
        public async Task GetAllProjectsWithTask_ReturnsList()
        {
            var list = new List<projectModel> { new projectModel { Name = "XT" } };
            _repoMock.Setup(r => r.GetAllProjectsWithTask()).ReturnsAsync(list);

            var res = await _service.GetAllProjectsWithTask();

            Assert.Single(res);
        }

        [Fact]
        public async Task GetSpecificProjectsWithTask_ReturnsModel()
        {
            var p = new projectModel { Id = 3, Name = "S" };
            _repoMock.Setup(r => r.GetSpecificProjectsWithTask(3)).ReturnsAsync(p);

            var res = await _service.GetSpecificProjectsWithTask(3);

            Assert.Equal(3, res.Id);
        }

        [Fact]
        public async Task MemberDetails_ReturnsMemberDetails()
        {
            var members = new List<MemberDetails> { new MemberDetails { EmployeeName = "E1", Email = "e@x.com" } };
            _repoMock.Setup(r => r.MemberDetails(2)).ReturnsAsync(members);

            var res = await _service.MemberDetails(2);

            Assert.Single(res);
        }

        [Fact]
        public async Task GetProjectWithOverDueTask_ReturnsModel()
        {
            var p = new projectModel { Id = 7 };
            _repoMock.Setup(r => r.GetProjectWithOverDueTask(7)).ReturnsAsync(p);

            var res = await _service.GetProjectWithOverDueTask(7);

            Assert.Equal(7, res.Id);
        }

        [Fact]
        public async Task UpdateProject_DelegatesToRepo()
        {
            var p = new projectModel { Name = "U" };
            _repoMock.Setup(r => r.UpdateProject(5, p)).ReturnsAsync(true);

            var res = await _service.UpdateProject(5, p);

            Assert.True(res);
            _repoMock.Verify(r => r.UpdateProject(5, p), Times.Once);
        }

        [Fact]
        public async Task DeleteProject_DelegatesToRepo()
        {
            _repoMock.Setup(r => r.DeleteProject(9)).ReturnsAsync(false);
            var res = await _service.DeleteProject(9);
            Assert.False(res);
        }
    }
}