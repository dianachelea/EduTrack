using Application.Interfaces;
using Application.Services;
using Domain;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class AssignmentServiceTests
{
    private readonly IGradesRepository _gradesRepository;
    private readonly IFileRepository _fileRepository;
    private readonly AssignmentService _assignmentService;

    public AssignmentServiceTests()
    {
        _gradesRepository = Substitute.For<IGradesRepository>();
        _fileRepository = Substitute.For<IFileRepository>();
        _assignmentService = new AssignmentService(_gradesRepository, _fileRepository);
    }

    [Fact]
    public async Task SolveAssignment_ShouldThrowException_WhenSolutionAlreadyExists()
    {
        // Arrange
        var courseName = "TestCourse";
        var lessonTitle = "TestLesson";
        var studentEmail = "student@example.com";
        var solution = new AssignmentSolutionDo { Solution_title = "Solution1", Solution = "Content" };
        var fileName = "file.txt";
        _gradesRepository.GetStudentSolution(courseName, lessonTitle, studentEmail).Returns(new List<AssignmentSolutionDo> { solution });

        // Act
        Func<Task> act = async () => await _assignmentService.SolveAssignment(courseName, lessonTitle, studentEmail, solution, fileName);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Already sent solution");
    }

    ////[Fact]
    ////public async Task SolveAssignment_ShouldReturnTrue_WhenSolutionIsAdded()
    ////{
    ////    // Arrange
    ////    var courseName = "TestCourse";
    ////    var lessonTitle = "TestLesson";
    ////    var studentEmail = "student@example.com";
    ////    var solution = new AssignmentSolutionDo { Solution_title = "Solution1", Solution = "Content" };
    ////    var fileName = "file.txt";
    ////    _gradesRepository.GetStudentSolution(courseName, lessonTitle, studentEmail).Returns(new List<AssignmentSolutionDo>());
    ////    _gradesRepository.SolveAssignment(courseName, lessonTitle, studentEmail, solution, fileName).Returns(true);

    ////    // Act
    ////    var result = await _assignmentService.SolveAssignment(courseName, lessonTitle, studentEmail, solution, fileName);

    ////    // Assert
    ////    result.Should().BeTrue();
    ////}

    [Fact]
    public async Task GetStudentSolution_ShouldReturnSolutions_WhenSolutionsExist()
    {
        // Arrange
        var courseName = "TestCourse";
        var lessonTitle = "TestLesson";
        var studentEmail = "student@example.com";
        var solutions = new List<AssignmentSolutionDo>
        {
            new AssignmentSolutionDo { Solution_title = "Solution1", Solution = "Content" }
        };
        _gradesRepository.GetStudentSolution(courseName, lessonTitle, studentEmail).Returns(solutions);

        // Act
        var result = await _assignmentService.GetStudentSolution(courseName, lessonTitle, studentEmail);

        // Assert
        result.Should().BeEquivalentTo(solutions);
    }

    [Fact]
    public async Task GetStudentSolution_ShouldThrowException_WhenNoSolutionsExist()
    {
        // Arrange
        var courseName = "TestCourse";
        var lessonTitle = "TestLesson";
        var studentEmail = "student@example.com";
        _gradesRepository.GetStudentSolution(courseName, lessonTitle, studentEmail).Returns((IEnumerable<AssignmentSolutionDo>)null);

        // Act
        Func<Task> act = async () => await _assignmentService.GetStudentSolution(courseName, lessonTitle, studentEmail);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("No solution");
    }

    [Fact]
    public async Task GradeAssignment_ShouldReturnTrue_WhenGradeIsSuccessful()
    {
        // Arrange
        var courseName = "TestCourse";
        var lessonTitle = "TestLesson";
        var grade = 95.0;
        var studentEmail = "student@example.com";
        _gradesRepository.GradeAssignment(courseName, lessonTitle, grade, studentEmail).Returns(true);

        // Act
        var result = await _assignmentService.GradeAssignment(courseName, lessonTitle, grade, studentEmail);

        // Assert
        result.Should().BeTrue();
    }
}
