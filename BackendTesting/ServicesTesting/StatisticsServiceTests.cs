using Application.Interfaces;
using Application.Services;
using Domain;
using FluentAssertions;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

public class StatisticsServiceTests
{
    private readonly IStatistics _statistics;
    private readonly StatisticsService _statisticsService;

    public StatisticsServiceTests()
    {
        _statistics = Substitute.For<IStatistics>();
        _statisticsService = new StatisticsService(_statistics);
    }

    [Fact]
    public async Task GetStudentStats_ShouldReturnStats_WhenEmailIsValid()
    {
        // Arrange
        var email = "student@example.com";
        var stats = new StatisticsDO { CoursesCompleted = 80, AssignmentsCompleted = 90, CoursesGrade = 85 };
        _statistics.GetStudentStats(email).Returns(Task.FromResult(stats));

        // Act
        var result = await _statisticsService.GetStudentStats(email);

        // Assert
        result.Should().BeEquivalentTo(stats);
    }

    //[Fact]
    //public async Task GetStudentStats_ShouldThrowArgumentNullException_WhenEmailIsNull()
    //{
    //    // Act
    //    Func<Task> act = async () => await _statisticsService.GetStudentStats(null);

    //    // Assert
    //    await act.Should().ThrowAsync<ArgumentNullException>().WithMessage("email");
    //}

    [Fact]
    public async Task GetStudentStats_ShouldThrowArgumentException_WhenEmailIsInvalid()
    {
        // Act
        Func<Task> act = async () => await _statisticsService.GetStudentStats("invalid-email");

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Email is invalid");
    }

    [Fact]
    public async Task GetTeacherStats_ShouldReturnStats_WhenEmailIsValid()
    {
        // Arrange
        var email = "teacher@example.com";
        var stats = new StatisticsDO { CoursesCompleted = 70, AssignmentsCompleted = 75, CoursesGrade = 80 };
        _statistics.GetTeacherStats(email).Returns(Task.FromResult(stats));

        // Act
        var result = await _statisticsService.GetTeacherStats(email);

        // Assert
        result.Should().BeEquivalentTo(stats);
    }

    //[Fact]
    //public async Task GetTeacherStats_ShouldThrowArgumentNullException_WhenEmailIsNull()
    //{
    //    // Act
    //    Func<Task> act = async () => await _statisticsService.GetTeacherStats(null);

    //    // Assert
    //    await act.Should().ThrowAsync<ArgumentNullException>().WithMessage("email");
    //}

    [Fact]
    public async Task GetTeacherStats_ShouldThrowArgumentException_WhenEmailIsInvalid()
    {
        // Act
        Func<Task> act = async () => await _statisticsService.GetTeacherStats("invalid-email");

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Email is invalid");
    }
}
