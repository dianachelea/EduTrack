USE CentricSummerPractice;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Grade' AND TABLE_SCHEMA = 'SummerPractice')
BEGIN
    CREATE TABLE [SummerPractice].[Grade](
        [Email] NVARCHAR(50),
        [Lesson_id] UNIQUEIDENTIFIER,
        [FileName] NVARCHAR(50) NOT NULL,
        [Grade] INT NOT NULL,
        CONSTRAINT CHK_Grade CHECK (Grade >= 0 AND Grade <= 10), 
        FOREIGN KEY (Email) REFERENCES [SummerPractice].[User](Email),
        FOREIGN KEY (Lesson_id) REFERENCES [SummerPractice].[Lessons](Lesson_id),
        FOREIGN KEY (FileName) REFERENCES [SummerPractice].[file](FileName)
    );
END;
