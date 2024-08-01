using Application.Interfaces;
using Application.Services;
using Domain;
using FluentAssertions;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class LessonServiceTests
{
    private readonly ILessonRepository _lessonRepository;
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly LessonInventoryService _lessonInventoryService;
    private readonly LessonService _lessonService;

    public LessonServiceTests()
    {
        _lessonRepository = Substitute.For<ILessonRepository>();
        _attendanceRepository = Substitute.For<IAttendanceRepository>();
        _lessonInventoryService = Substitute.For<LessonInventoryService>(_lessonRepository);
        _lessonService = new LessonService(_lessonRepository, _attendanceRepository, _lessonInventoryService);
    }

    [Fact]
    public async Task ChangeStatus_ShouldReturnTrue_WhenChangeIsSuccessful()
    {
        // Arrange
        var courseName = "TestCourse";
        var lessonTitle = "Lesson1";
        var status = "Completed";
        _lessonRepository.ChangeStatus(courseName, lessonTitle, status).Returns(true);

        // Act
        var result = await _lessonService.ChangeStatus(courseName, lessonTitle, status);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task MakeAttendance_ShouldReturnTrue_WhenAttendanceIsSuccessful()
    {
        // Arrange
        var courseName = "TestCourse";
        var lessonTitle = "Lesson1";
        var students = new List<Student> { new Student { FirstName = "Student1", Email = "student1@example.com" } };
        _attendanceRepository.MakeAttendance(courseName, lessonTitle, students).Returns(true);

        // Act
        var result = await _lessonService.MakeAttendance(courseName, lessonTitle, students);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetAttendance_ShouldReturnAttendanceList()
    {
        // Arrange
        var courseName = "TestCourse";
        var lessonTitle = "Lesson1";
        var attendanceList = new List<Attendance>
        {
            new Attendance { StudentEmail = "Student1", Attended = true },
            new Attendance { StudentEmail = "Student2", Attended = false }
        };
        _attendanceRepository.GetAttendance(courseName, lessonTitle).Returns(attendanceList);

        // Act
        var result = await _lessonService.GetAttendance(courseName, lessonTitle);

        // Assert
        result.Should().BeEquivalentTo(attendanceList);
    }

    [Fact]
    public async Task GetSAttendance_ShouldReturnStudentAttendanceList()
    {
        // Arrange
        var courseName = "TestCourse";
        var email = "student1@example.com";
        var attendanceList = new List<Attendance>
        {
            new Attendance { StudentEmail = "Student1", Attended = true }
        };
        _attendanceRepository.GetStudentAttendance(courseName, email).Returns(attendanceList);

        // Act
        var result = await _lessonService.GetSAttendance(courseName, email);

        // Assert
        result.Should().BeEquivalentTo(attendanceList);
    }
}
