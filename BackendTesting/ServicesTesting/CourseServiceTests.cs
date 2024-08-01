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

public class CourseServiceTests
{
    private readonly ICoursesRepository _courseRepository;
    private readonly CourseService _courseService;

    public CourseServiceTests()
    {
        _courseRepository = Substitute.For<ICoursesRepository>();
        _courseService = new CourseService(_courseRepository);
    }

    [Fact]
    public void GetCourse_ShouldReturnCourse_WhenCourseExists()
    {
        // Arrange
        var courseName = "TestCourse";
        var courses = new List<Course>
        {
            new Course { Name = "TestCourse" }
        };
        _courseRepository.GetCourse(courseName).Returns(courses);

        // Act
        var result = _courseService.GetCourse(courseName);

        // Assert
        result.Should().BeEquivalentTo(courses.First());
    }

    [Fact]
    public void GetCoursePresentation_ShouldReturnCourseInfoPage_WhenCourseExists()
    {
        // Arrange
        var courseName = "TestCourse";
        var courseInfoPages = new List<CourseInfoPage>
        {
            new CourseInfoPage { Name = "TestCourse" }
        };
        _courseRepository.GetCourseForPage(courseName).Returns(courseInfoPages);

        // Act
        var result = _courseService.GetCoursePresentation(courseName);

        // Assert
        result.Should().BeEquivalentTo(courseInfoPages.First());
    }

    [Fact]
    public void GetAllStudentsEnrolled_ShouldReturnStudents_WhenStudentsExist()
    {
        // Arrange
        var courseName = "TestCourse";
        var teacherEmail = "teacher@example.com";
        var students = new List<Student>
        {
            new Student { Email = "student1@example.com" },
            new Student { Email = "student2@example.com" }
        };
        _courseRepository.GetStudentsEnrolledInCourse(courseName, teacherEmail).Returns(students);

        // Act
        var result = _courseService.GetAllStudentsEnrolled(courseName, teacherEmail);

        // Assert
        result.Should().BeEquivalentTo(students);
    }

    [Fact]
    public void GetCourseLessons_ShouldReturnLessons_WhenLessonsExist()
    {
        // Arrange
        var courseName = "TestCourse";
        var teacherEmail = "teacher@example.com";
        var lessons = new List<Lesson>
        {
            new Lesson { Name = "Lesson1" },
            new Lesson { Name = "Lesson2" }
        };
        _courseRepository.GetCourseLessons(courseName, teacherEmail).Returns(lessons);

        // Act
        var result = _courseService.GetCourseLessons(courseName, teacherEmail);

        // Assert
        result.Should().BeEquivalentTo(lessons);
    }

    [Fact]
    public async Task EnrollToCourse_ShouldReturnTrue_WhenEnrollmentIsSuccessful()
    {
        // Arrange
        var courseName = "TestCourse";
        var studentEmail = "student@example.com";
        _courseRepository.EnrollStudent(courseName, studentEmail).Returns(1);

        // Act
        var result = await _courseService.EnrollToCourse(courseName, studentEmail);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task EnrollToCourse_ShouldReturnFalse_WhenEnrollmentFails()
    {
        // Arrange
        var courseName = "TestCourse";
        var studentEmail = "student@example.com";
        _courseRepository.EnrollStudent(courseName, studentEmail).Returns(0);

        // Act
        var result = await _courseService.EnrollToCourse(courseName, studentEmail);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetStudentAttendance_ShouldReturnAttendance_WhenRecordsExist()
    {
        // Arrange
        var courseName = "TestCourse";
        var studentEmail = "student@example.com";
        var attendanceRecords = new List<Attendance>
        {
            new Attendance { LessonName = "Lesson1", Attended = true },
            new Attendance { LessonName = "Lesson2", Attended = false }
        };
        _courseRepository.GetStudentAttendance(courseName, studentEmail).Returns(attendanceRecords);

        // Act
        var result = _courseService.GetStudentAttendance(courseName, studentEmail);

        // Assert
        result.Should().BeEquivalentTo(attendanceRecords);
    }
}
