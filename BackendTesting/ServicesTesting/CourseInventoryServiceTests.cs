using Application.Interfaces;
using Application.Services;
using Domain;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class CourseInventoryServiceTests
{
    private readonly ICoursesRepository _courseRepository;
    private readonly CourseInventoryService _courseInventoryService;

    public CourseInventoryServiceTests()
    {
        _courseRepository = Substitute.For<ICoursesRepository>();
        _courseInventoryService = new CourseInventoryService(_courseRepository);
    }

    [Fact]
    public async Task AddCourse_ShouldReturnTrue_WhenCourseIsAdded()
    {
        // Arrange
        var email = "teacher@example.com";
        var course = new Course { Name = "TestCourse" };
        _courseRepository.GetTeacherCourses(email).Returns(new List<string>());
        _courseRepository.AddCourse(course).Returns(true);

        // Act
        var result = await _courseInventoryService.AddCourse(email, course);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task AddCourse_ShouldThrowException_WhenCourseAlreadyExists()
    {
        // Arrange
        var email = "teacher@example.com";
        var course = new Course { Name = "TestCourse" };
        _courseRepository.GetTeacherCourses(email).Returns(new List<string> { "TestCourse" });

        // Act
        Func<Task> act = async () => await _courseInventoryService.AddCourse(email, course);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Course already exists");
    }

    [Fact]
    public async Task DeleteCourse_ShouldReturnTrue_WhenCourseIsDeleted()
    {
        // Arrange
        var email = "teacher@example.com";
        var courseName = "TestCourse";
        _courseRepository.GetTeacherCourses(email).Returns(new List<string> { courseName });
        _courseRepository.DeleteCourse(email, courseName).Returns(true);

        // Act
        var result = await _courseInventoryService.DeleteCourse(email, courseName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteCourse_ShouldThrowException_WhenCourseDoesNotExist()
    {
        // Arrange
        var email = "teacher@example.com";
        var courseName = "TestCourse";
        _courseRepository.GetTeacherCourses(email).Returns(new List<string>());

        // Act
        Func<Task> act = async () => await _courseInventoryService.DeleteCourse(email, courseName);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Course does not exist");
    }

    //[Fact]
    //public void GetAllCourses_ShouldReturnAllCourses_WhenNoFilterIsApplied()
    //{
    //    // Arrange
    //    var filter = new CourseFilter();
    //    var courses = new List<CourseDisplay>
    //    {
    //        new CourseDisplay { Name = "TestCourse1" },
    //        new CourseDisplay { Name = "TestCourse2" }
    //    };
    //    _courseRepository.GetAllCourses().Returns(courses);

    //    // Act
    //    var result = _courseInventoryService.GetAllCourses(filter);

    //    // Assert
    //    result.Should().BeEquivalentTo(courses);
    //}

    [Fact]
    public async Task UpdateCourse_ShouldReturnTrue_WhenCourseIsUpdated()
    {
        // Arrange
        var email = "teacher@example.com";
        var courseName = "TestCourse";
        var course = new Course { Name = "UpdatedCourse" };
        _courseRepository.GetTeacherCourses(email).Returns(new List<string> { courseName });
        _courseRepository.UpdateCourse(email, courseName, course).Returns(true);

        // Act
        var result = await _courseInventoryService.UpdateCourse(email, courseName, course);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateCourse_ShouldReturnFalse_WhenCourseDoesNotExist()
    {
        // Arrange
        var email = "teacher@example.com";
        var courseName = "TestCourse";
        var course = new Course { Name = "UpdatedCourse" };
        _courseRepository.GetTeacherCourses(email).Returns(new List<string>());

        // Act
        var result = await _courseInventoryService.UpdateCourse(email, courseName, course);

        // Assert
        result.Should().BeFalse();
    }
}
