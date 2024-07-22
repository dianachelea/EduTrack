IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE WHERE TABLE_NAME = 'Feedback' AND CONSTRAINT_NAME = 'CHK_Feedback_Category' AND TABLE_SCHEMA = 'SummerPractice')
BEGIN
    ALTER TABLE [SummerPractice].[Feedback]
    ADD CONSTRAINT CHK_Feedback_Category CHECK ([Feedback_category] IN ('Content Quality', 'User Experience', 'Technical Performance', 'Educational Tools', 'Assessment and Feedback'));
END;