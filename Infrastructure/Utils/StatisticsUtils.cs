using Application.Interfaces;
using Dapper;
using Domain;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    public class StatisticsUtils : IStatistics
    {
        private readonly IDatabaseContext _databaseContext;
        public StatisticsUtils(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public async Task<StatisticsDO> GetStudentStats(string email)
        {
            if (email == null) { throw new ArgumentNullException("email"); }
            
            var query = @"
                        WITH TotalLessons AS (
                            SELECT 
                                Course_id, 
                                COUNT(Lesson_id) AS TotalLessonCount
                            FROM 
                                [CentricSummerPractice].[SummerPractice].[Lessons]
                            GROUP BY 
                                Course_id
                        ),
                        AttendedLessons AS (
                            SELECT 
                                L.Course_id,
                                A.Email,
                                COUNT(A.Lesson_id) AS AttendedLessonCount
                            FROM 
                                [CentricSummerPractice].[SummerPractice].[Attendance] A
                            INNER JOIN 
                                [CentricSummerPractice].[SummerPractice].[Lessons] L
                                ON A.Lesson_id = L.Lesson_id
                            WHERE 
                                A.Attendance_verify = 1
                            GROUP BY 
                                L.Course_id,
                                A.Email
                        )
                        SELECT 
                            C.Name_course,
                            (AL.AttendedLessonCount * 100.0 / TL.TotalLessonCount) AS AttendancePercentage
                        FROM 
                            AttendedLessons AL
                        INNER JOIN 
                            TotalLessons TL
                            ON AL.Course_id = TL.Course_id
                        INNER JOIN 
                            [CentricSummerPractice].[SummerPractice].[Courses] C
                            ON AL.Course_id = C.Course_id
                        WHERE 
                            AL.Email = @Email
                        ORDER BY 
                            C.Name_course,
                            AL.Email;
                        ";
            var parameters = new DynamicParameters();
            parameters.Add("Email", email, DbType.String);

            var connection = _databaseContext.GetDbConnection();
            var allCoursesCompleted = await connection.QueryAsync<(string courseName, double percentage)>(query, parameters, _databaseContext.GetDbTransaction());
            var coursesCompleted = allCoursesCompleted.Average(item => item.percentage);

            query = @"WITH TotalAssignments AS (
                        SELECT 
                            L.Course_id, 
                            A.Email,
                            COUNT(A.Lesson_id) AS TotalAssignmentCount
                        FROM 
                            [CentricSummerPractice].[SummerPractice].[Lessons] L
                        INNER JOIN 
                            [CentricSummerPractice].[SummerPractice].[Attendance] A
                            ON L.Lesson_id = A.Lesson_id
                        WHERE 
                            A.Email = @Email
                        GROUP BY 
                            L.Course_id,
                            A.Email
                    ),

                    -- Calculate the number of completed assignments (grade >= 5) per course for the student
                    CompletedAssignments AS (
                        SELECT 
                            L.Course_id,
                            G.Email,
                            COUNT(G.Lesson_id) AS CompletedAssignmentCount
                        FROM 
                            [CentricSummerPractice].[SummerPractice].[Lessons] L
                        INNER JOIN 
                            [CentricSummerPractice].[SummerPractice].[Grade] G
                            ON L.Lesson_id = G.Lesson_id
                        WHERE 
                            G.Email = @Email
                            AND 
                            G.Grade >= 5
                        GROUP BY 
                            L.Course_id,
                            G.Email
                    )
                    -- Calculate the percentage of completed assignments for each course for the student
                    SELECT 
                        C.Name_course,
                        (COALESCE(CA.CompletedAssignmentCount, 0) * 100.0 / COALESCE(TA.TotalAssignmentCount, 1)) AS CompletedAssignmentPercentage
                    FROM 
                        TotalAssignments TA
                    LEFT JOIN 
                        CompletedAssignments CA
                        ON TA.Course_id = CA.Course_id
                        AND TA.Email = CA.Email
                    INNER JOIN
                        [CentricSummerPractice].[SummerPractice].[Courses] C
                        ON TA.Course_id = C.Course_id
                    WHERE
                        TA.Email = @Email
                    ORDER BY 
                        C.Name_course,
                        TA.Email;";

            var allAssignmentsCompleted = await connection.QueryAsync<(string courseName, double percentage)>(query, parameters, _databaseContext.GetDbTransaction());
            var assignmentsCompleted = allAssignmentsCompleted.Average(item => item.percentage);

            query = @"SELECT 
                        C.Name_course,
                        AVG(G.Grade) AS MeanGrade
                    FROM 
                        [CentricSummerPractice].[SummerPractice].[Grade] G
                    INNER JOIN 
                        [CentricSummerPractice].[SummerPractice].[Lessons] L
                        ON G.Lesson_id = L.Lesson_id
                    INNER JOIN 
                        [CentricSummerPractice].[SummerPractice].[Courses] C
                        ON L.Course_id = C.Course_id
                    WHERE 
                        G.Email = @Email
                    GROUP BY 
                        L.Course_id,
                        C.Name_course
                    ORDER BY 
                        L.Course_id;";

            var allCoursesGrades = await connection.QueryAsync<(string courseName, double grade)>(query, parameters, _databaseContext.GetDbTransaction());
            var coursesGrade = allCoursesGrades.Average(item => item.grade);

            var statistics = new StatisticsDO
            {
                CoursesCompleted = (coursesCompleted % 1 >= 0.5) 
                                    ? (int)Math.Ceiling(coursesCompleted) 
                                    : (int)Math.Floor(coursesCompleted),
                AllCoursesCompleted = allCoursesCompleted.ToDictionary(
                                                                    pair => pair.courseName,
                                                                    pair => (int)(pair.percentage)
                                                                    ),
                AssignmentsCompleted = (assignmentsCompleted % 1 >= 0.5) 
                                    ? (int)Math.Ceiling(assignmentsCompleted) 
                                    : (int)Math.Floor(assignmentsCompleted),
                AllAssignmentsCompleted = allAssignmentsCompleted.ToDictionary(
                                                                    pair => pair.courseName,
                                                                    pair => (int)(pair.percentage)
                                                                    ),
                CoursesGrade = (coursesGrade % 1 >= 0.5)
                            ? (int)Math.Ceiling(coursesGrade)
                            : (int)Math.Floor(coursesGrade),
                AllCoursesGrades = allCoursesGrades.ToDictionary(
                                                                    pair => pair.courseName,
                                                                    pair => (int)(pair.grade)
                                                                    )
            };

            return statistics;
        }
        public async Task<StatisticsDO> GetTeacherStats(string email)
        {
            if (email == null) {throw new ArgumentNullException("email"); }

            var query = @"-- Calculate the total number of lessons per course
                        WITH TotalLessons AS (
                            SELECT 
                                L.Course_id, 
                                COUNT(L.Lesson_id) AS TotalLessonCount
                            FROM 
                                [CentricSummerPractice].[SummerPractice].[Lessons] L
                            GROUP BY 
                                L.Course_id
                        ),
                        -- Get the courses managed by the specified teacher
                        TeacherCourses AS (
                            SELECT 
                                SC.Course_id,
                                C.Name_course
                            FROM 
                                [CentricSummerPractice].[SummerPractice].[Students-Courses] SC
                            INNER JOIN 
                                [CentricSummerPractice].[SummerPractice].[Courses] C
                                ON SC.Course_id = C.Course_id
                            WHERE 
                                SC.Email = @Email
                        ),
                        -- Calculate the number of attended lessons per course
                        AttendedLessons AS (
                            SELECT 
                                L.Course_id,
                                COUNT(A.Email) AS AttendedLessonCount
                            FROM 
                                [CentricSummerPractice].[SummerPractice].[Lessons] L
                            LEFT JOIN 
                                [CentricSummerPractice].[SummerPractice].[Attendance] A
                                ON L.Lesson_id = A.Lesson_id
                            GROUP BY 
                                L.Course_id
                        )
                        -- Calculate the attendance percentage for each course managed by the specified teacher
                        SELECT 
                            --TC.Course_id,
                            TC.Name_course,
                            (ISNULL(AL.AttendedLessonCount, 0) * 100.0 / TL.TotalLessonCount) AS AttendancePercentage
                        FROM 
                            TeacherCourses TC
                        INNER JOIN 
                            TotalLessons TL
                            ON TC.Course_id = TL.Course_id
                        LEFT JOIN 
                            AttendedLessons AL
                            ON TC.Course_id = AL.Course_id
                        ORDER BY 
                            TC.Course_id;";

            var parameters = new DynamicParameters();
            parameters.Add("Email", email);

            var connection = _databaseContext.GetDbConnection();
            var allCoursesCompleted = await connection.QueryAsync<(string courseName, double percentage)>(query, parameters, _databaseContext.GetDbTransaction());
            var coursesCompleted = allCoursesCompleted.Average(item => item.percentage);

            query = @"WITH TeacherCourses AS (
                        SELECT 
                            SC.Course_id,
                            C.Name_course
                        FROM 
                            [CentricSummerPractice].[SummerPractice].[Students-Courses] SC
                        INNER JOIN 
                            [CentricSummerPractice].[SummerPractice].[Courses] C
                            ON SC.Course_id = C.Course_id
                        WHERE 
                            SC.Email = @Email
                    ),
                    TotalAssignments AS (
                        SELECT 
                            L.Course_id,
                            COUNT(G.Grade) AS TotalAssignmentCount
                        FROM 
                            [CentricSummerPractice].[SummerPractice].[Grade] G
                        INNER JOIN 
                            [CentricSummerPractice].[SummerPractice].[Lessons] L
                            ON G.Lesson_id = L.Lesson_id
                        GROUP BY 
                            L.Course_id
                    ),
                    PassedAssignments AS (
                        SELECT 
                            L.Course_id,
                            COUNT(G.Grade) AS PassedAssignmentCount
                        FROM 
                            [CentricSummerPractice].[SummerPractice].[Grade] G
                        INNER JOIN 
                            [CentricSummerPractice].[SummerPractice].[Lessons] L
                            ON G.Lesson_id = L.Lesson_id
                        WHERE 
                            G.Grade >= 5
                        GROUP BY 
                            L.Course_id
                    )
                    SELECT 
                        --TC.Course_id,
                        TC.Name_course,
                        (ISNULL(PA.PassedAssignmentCount, 0) * 100.0 / ISNULL(TA.TotalAssignmentCount, 1)) AS PassingGradePercentage
                    FROM 
                        TeacherCourses TC
                    LEFT JOIN 
                        TotalAssignments TA
                        ON TC.Course_id = TA.Course_id
                    LEFT JOIN 
                        PassedAssignments PA
                        ON TC.Course_id = PA.Course_id
                    ORDER BY 
                        TC.Course_id;";

            var allAssignmentsCompleted = await connection.QueryAsync<(string courseName, double percentage)>(query, parameters, _databaseContext.GetDbTransaction());
            var assignmentsCompleted = allAssignmentsCompleted.Average(item => item.percentage);

            query = @"WITH TeacherCourses AS (
                        SELECT 
                            SC.Course_id,
                            C.Name_course
                        FROM 
                            [CentricSummerPractice].[SummerPractice].[Students-Courses] SC
                        INNER JOIN 
                            [CentricSummerPractice].[SummerPractice].[Courses] C
                            ON SC.Course_id = C.Course_id
                        WHERE 
                            SC.Email = @Email
                    )
                    SELECT 
                        --TC.Course_id,
                        TC.Name_course,
                        CAST(AVG(CAST(G.Grade AS FLOAT)) AS DECIMAL(10, 2)) AS MeanGrade
                    FROM 
                        TeacherCourses TC
                    INNER JOIN 
                        [CentricSummerPractice].[SummerPractice].[Lessons] L
                        ON TC.Course_id = L.Course_id
                    INNER JOIN 
                        [CentricSummerPractice].[SummerPractice].[Grade] G
                        ON L.Lesson_id = G.Lesson_id
                    GROUP BY 
                        TC.Course_id,
                        TC.Name_course
                    ORDER BY 
                        TC.Course_id;";

            var allCoursesGrades = await connection.QueryAsync<(string courseName, double grade)>(query, parameters, _databaseContext.GetDbTransaction());
            var coursesGrade = allCoursesGrades.Average(item => item.grade);

            var statistics = new StatisticsDO
            {
                CoursesCompleted = (coursesCompleted % 1 >= 0.5) 
                                    ? (int)Math.Ceiling(coursesCompleted) 
                                    : (int)Math.Floor(coursesCompleted),
                AllCoursesCompleted = allCoursesCompleted.ToDictionary(
                                                                    pair => pair.courseName,
                                                                    pair => (int)(pair.percentage)
                                                                    ),
                AssignmentsCompleted = (assignmentsCompleted % 1 >= 0.5)
                            ? (int)Math.Ceiling(assignmentsCompleted)
                            : (int)Math.Floor(assignmentsCompleted),
                AllAssignmentsCompleted = allAssignmentsCompleted.ToDictionary(
                                                                    pair => pair.courseName,
                                                                    pair => (int)(pair.percentage)
                                                                    ),
                CoursesGrade = (coursesGrade % 1 >= 0.5)
                            ? (int)Math.Ceiling(coursesGrade)
                            : (int)Math.Floor(coursesGrade),
                AllCoursesGrades = allCoursesGrades.ToDictionary(
                                                                    pair => pair.courseName,
                                                                    pair => (int)(pair.grade)
                                                                    )
            };

            return statistics;
        }
    }
}
