IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Feedback' AND COLUMN_NAME = 'Feedback_category' AND TABLE_SCHEMA = 'SummerPractice')
BEGIN
    ALTER TABLE [SummerPractice].[Feedback]
    ADD [Feedback_category] nvarchar(50);
END;

--this should be included in a different script in order to run properly
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE WHERE TABLE_NAME = 'Feedback' AND CONSTRAINT_NAME = 'CHK_Feedback_Category' AND TABLE_SCHEMA = 'SummerPractice')
BEGIN
    ALTER TABLE [SummerPractice].[Feedback]
    ADD CONSTRAINT CHK_Feedback_Category CHECK ([Feedback_category] IN ('Content Quality', 'User Experience', 'Technical Performance', 'Educational Tools', 'Assessment and Feedback'));
END;

--this should be included in a different script in order to run properly
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Feedback' AND COLUMN_NAME = 'Feedback_date' AND TABLE_SCHEMA = 'SummerPractice')
BEGIN
    ALTER TABLE [SummerPractice].[Feedback]
    ADD [Feedback_date] DATETIME DEFAULT GETDATE();
END;