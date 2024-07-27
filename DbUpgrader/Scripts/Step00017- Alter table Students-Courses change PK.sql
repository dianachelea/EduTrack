USE CentricSummerPractice;

DROP TABLE [SummerPractice].[Students-Courses];

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Students-Courses' AND TABLE_SCHEMA = 'SummerPractice')
BEGIN
    CREATE TABLE [SummerPractice].[Students-Courses](
        [Relation_id]  UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        [Course_id] UNIQUEIDENTIFIER NOT NULL,
        [Email] NVARCHAR(50) NOT NULL,
        FOREIGN KEY (Course_id) REFERENCES [SummerPractice].[Courses](Course_id),
        FOREIGN KEY (Email) REFERENCES [SummerPractice].[User](Email)
    );
END;

