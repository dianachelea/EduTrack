--this should be included in a different script in order to run properly
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Feedback' AND COLUMN_NAME = 'Feedback_date' AND TABLE_SCHEMA = 'SummerPractice')
BEGIN
    ALTER TABLE [SummerPractice].[Feedback]
    ADD [Feedback_date] DATETIME DEFAULT GETDATE();
END;