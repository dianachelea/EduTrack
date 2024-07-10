USE CentricSummerPractice;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Feedback' AND TABLE_SCHEMA = 'SummerPractice')
BEGIN
    CREATE TABLE [SummerPractice].[Feedback](
        [Feedback_id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        [Feedback_name] nvarchar(50),
		[Feedback_email] nvarchar(100),
		[Feedback_description] nvarchar(1000),
		[Feedback_title] nvarchar(100),
		[Feedback_anonymus] BIT,
		[Feedback_stars] INT,
		CONSTRAINT CHK_Stars CHECK (Feedback_stars >=0 )
    );
END;
