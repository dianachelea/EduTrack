IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Feedback' AND COLUMN_NAME = 'Feedback_category' AND TABLE_SCHEMA = 'SummerPractice')
BEGIN
    ALTER TABLE [SummerPractice].[Feedback]
    ADD [Feedback_category] nvarchar(50);
END;