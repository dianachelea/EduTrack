using Application.Interfaces;
using Application.Services;
using Domain;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class LessonInventoryServiceTests
{
    private readonly ILessonRepository _lessonRepository;
    private readonly LessonInventoryService _lessonInventoryService;

    public LessonInventoryServiceTests()
    {
        _lessonRepository = Substitute.For<ILessonRepository>();
        _lessonInventoryService = new LessonInventoryService(_lessonRepository);
    }

    [Fact]
    public async Task GetAllLessons_ShouldReturnLessons_WhenCourseNameIsValid()
    {
        // Arrange
        var courseName = "TestCourse";
        var lessons = new List<LessonDisplay>
        {
            new LessonDisplay { Name = "Lesson1", Lesson_Description = "Description1" },
            new LessonDisplay { Name = "Lesson2", Lesson_Description = "Description2" }
        };
        _lessonRepository.GetLessons(courseName).Returns(lessons);

        // Act
        var result = await _lessonInventoryService.GetAllLessons(courseName);

        // Assert
        result.Should().BeEquivalentTo(lessons);
    }

    [Fact]
    public async Task GetLesson_ShouldReturnLesson_WhenLessonExists()
    {
        // Arrange
        var courseName = "TestCourse";
        var lessonTitle = "TestLesson";
        var lesson = new Lesson { Name = "TestLesson", Description = "Test Description" };
        _lessonRepository.GetLesson(courseName, lessonTitle).Returns(lesson);

        // Act
        var result = await _lessonInventoryService.GetLesson(courseName, lessonTitle);

        // Assert
        result.Should().BeEquivalentTo(lesson);
    }

    [Fact]
    public async Task UpdateLesson_ShouldReturnTrue_WhenUpdateIsSuccessful()
    {
        // Arrange
        var lessonTitle = "TestLesson";
        var lesson = new Lesson { Name = "UpdatedLesson", Description = "Updated Description" };
        _lessonRepository.UpdateLesson(lessonTitle, lesson).Returns(true);

        // Act
        var result = await _lessonInventoryService.UpdateLesson(lessonTitle, lesson);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task AddLesson_ShouldReturnTrue_WhenAddIsSuccessful()
    {
        // Arrange
        var courseTitle = "TestCourse";
        var teacherEmail = "teacher@test.com";
        var lesson = new Lesson { Name = "NewLesson", Description = "New Description" };
        _lessonRepository.AddLesson(courseTitle, teacherEmail, lesson).Returns(true);

        // Act
        var result = await _lessonInventoryService.AddLesson(courseTitle, teacherEmail, lesson);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteLesson_ShouldReturnTrue_WhenDeleteIsSuccessful()
    {
        // Arrange
        var courseName = "TestCourse";
        var lessonTitle = "TestLesson";
        _lessonRepository.DeleteLesson(courseName, lessonTitle).Returns(true);

        // Act
        var result = await _lessonInventoryService.DeleteLesson(courseName, lessonTitle);

        // Assert
        result.Should().BeTrue();
    }
}
