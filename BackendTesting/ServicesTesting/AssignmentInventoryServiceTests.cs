using Application.Interfaces;
using Application.Services;
using Domain;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class AssignmentInventoryServiceTests
{
    private readonly IAssignmentsRepository _assignmentsRepository;
    private readonly AssignmentInventoryService _assignmentInventoryService;

    public AssignmentInventoryServiceTests()
    {
        _assignmentsRepository = Substitute.For<IAssignmentsRepository>();
        _assignmentInventoryService = new AssignmentInventoryService(_assignmentsRepository);
    }

    [Fact]
    public async Task GetAssignment_ShouldReturnAssignments_WhenAssignmentsExist()
    {
        // Arrange
        var courseName = "TestCourse";
        var lessonTitle = "TestLesson";
        var assignments = new List<AssignmentDo>
        {
            new AssignmentDo { Assignment_name = "Assignment1", Assignment_description = "Description1" },
            new AssignmentDo { Assignment_name = "Assignment2", Assignment_description = "Description2" }
        };
        _assignmentsRepository.GetAssignment(courseName, lessonTitle).Returns(assignments);

        // Act
        var result = await _assignmentInventoryService.GetAssignment(courseName, lessonTitle);

        // Assert
        result.Should().BeEquivalentTo(assignments);
    }

    [Fact]
    public async Task GetAssignment_ShouldThrowException_WhenAssignmentsDoNotExist()
    {
        // Arrange
        var courseName = "TestCourse";
        var lessonTitle = "TestLesson";
        _assignmentsRepository.GetAssignment(courseName, lessonTitle).Returns((IEnumerable<AssignmentDo>)null);

        // Act
        Func<Task> act = async () => await _assignmentInventoryService.GetAssignment(courseName, lessonTitle);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("No assignments");
    }

    //[Fact]
    //public async Task AddAssignment_ShouldReturnTrue_WhenAddIsSuccessful()
    //{
    //    // Arrange
    //    var courseName = "TestCourse";
    //    var lessonTitle = "TestLesson";
    //    var assignmentData = new AssignmentDo { Assignment_name = "Assignment1", Assignment_description = "Description1" };
    //    var fileName = "file.txt";
    //    _assignmentsRepository.AddAssignment(courseName, lessonTitle, assignmentData, fileName).Returns(true);

    //    // Act
    //    var result = await _assignmentInventoryService.AddAssignment(courseName, lessonTitle, assignmentData, fileName);

    //    // Assert
    //    result.Should().BeTrue();
    //}

    [Fact]
    public async Task GetStudentAssignments_ShouldReturnAssignments_WhenAssignmentsExist()
    {
        // Arrange
        var courseName = "TestCourse";
        var studentEmail = "student@example.com";
        var assignments = new List<List<AssignmentDo>>
        {
            new List<AssignmentDo>
            {
                new AssignmentDo { Assignment_name = "Assignment1", Assignment_description = "Description1" }
            }
        };
        _assignmentsRepository.GetStudentAssignments(courseName, studentEmail).Returns(assignments);

        // Act
        var result = await _assignmentInventoryService.GetStudentAssignments(courseName, studentEmail);

        // Assert
        result.Should().BeEquivalentTo(assignments);
    }

    [Fact]
    public async Task GetStudentAssignments_ShouldThrowException_WhenNoAssignmentsExist()
    {
        // Arrange
        var courseName = "TestCourse";
        var studentEmail = "student@example.com";
        _assignmentsRepository.GetStudentAssignments(courseName, studentEmail).Returns((IEnumerable<List<AssignmentDo>>)null);

        // Act
        Func<Task> act = async () => await _assignmentInventoryService.GetStudentAssignments(courseName, studentEmail);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("No students solved this assignment");
    }

    //[Fact]
    //public async Task EditAssignment_ShouldReturnTrue_WhenEditIsSuccessful()
    //{
    //    // Arrange
    //    var courseName = "TestCourse";
    //    var lessonTitle = "TestLesson";
    //    var assignmentData = new AssignmentDo { Assignment_name = "Assignment1", Assignment_description = "UpdatedDescription" };
    //    var fileName = "file.txt";
    //    _assignmentsRepository.EditAssignment(courseName, lessonTitle, assignmentData, fileName).Returns(true);

    //    // Act
    //    var result = await _assignmentInventoryService.EditAssignment(courseName, lessonTitle, assignmentData, fileName);

    //    // Assert
    //    result.Value.Should().BeTrue();
    //}

    [Fact]
    public async Task DeleteAssignment_ShouldReturnTrue_WhenDeleteIsSuccessful()
    {
        // Arrange
        var courseName = "TestCourse";
        var lessonTitle = "TestLesson";
        _assignmentsRepository.DeleteAssignment(courseName, lessonTitle).Returns(true);

        // Act
        var result = await _assignmentInventoryService.DeleteAssignment(courseName, lessonTitle);

        // Assert
        result.Value.Should().BeTrue();
    }
}
