USE CentricSummerPractice;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Attendance' AND TABLE_SCHEMA = 'SummerPractice')
BEGIN
    CREATE TABLE [SummerPractice].[Attendance](
        [Email] NVARCHAR(50),
        [Lesson_id] UNIQUEIDENTIFIER,
        [Attendance_verify] BIT,
        FOREIGN KEY (Email) REFERENCES [SummerPractice].[User](Email),
        FOREIGN KEY (Lesson_id) REFERENCES [SummerPractice].[Lessons](Lesson_id)
    );
END;
