USE CentricSummerPractice;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Courses' AND TABLE_SCHEMA = 'SummerPractice')
BEGIN
    CREATE TABLE [SummerPractice].[Courses](
        [Course_id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        [Name_course] NVARCHAR(100) NOT NULL,
        [Description] NVARCHAR(2000) NOT NULL,
        [Preview] NVARCHAR(1000) NOT NULL,
        [ImageData] VARBINARY(MAX) NULL, 
        [Category] NVARCHAR(50) NOT NULL,
        [Difficulty] NVARCHAR(50) NOT NULL, 
        [Time] INT NOT NULL
    );
END;
