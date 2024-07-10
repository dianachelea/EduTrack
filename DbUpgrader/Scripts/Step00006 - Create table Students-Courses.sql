USE CentricSummerPractice;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Students-Courses' AND TABLE_SCHEMA = 'SummerPractice')
BEGIN
    CREATE TABLE [SummerPractice].[Students-Courses](
        [Course_id] UNIQUEIDENTIFIER NOT NULL,
        [Email] NVARCHAR(50) PRIMARY KEY NOT NULL,
        FOREIGN KEY (Course_id) REFERENCES [SummerPractice].[Courses](Course_id),
        FOREIGN KEY (Email) REFERENCES [SummerPractice].[User](Email)
    );
END;