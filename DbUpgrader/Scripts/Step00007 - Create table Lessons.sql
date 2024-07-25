USE CentricSummerPractice;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Lessons' AND TABLE_SCHEMA = 'SummerPractice')
BEGIN
    CREATE TABLE [SummerPractice].[Lessons](
        [Lesson_id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID() ,
        [Course_id] UNIQUEIDENTIFIER,
		[Lesson_name] NVARCHAR(50) NOT NULL,
		[Lesson_description] NVARCHAR(2000) ,
		[Assignment_name] NVARCHAR(100) ,
		[Assignment_description] NVARCHAR(1000) ,
		[Assignment_link] NVARCHAR(500) ,
		[Assignment_preview] NVARCHAR(500) , 
		FOREIGN KEY (Course_id) REFERENCES [SummerPractice].[Courses](Course_id)
    );
END;
