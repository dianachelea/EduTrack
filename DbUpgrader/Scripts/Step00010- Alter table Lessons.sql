USE CentricSummerPractice;

EXEC sp_rename 'SummerPractice.Lessons.Assignment_link', 'Assignment_file', 'COLUMN';

ALTER TABLE [SummerPractice].[Lessons]
ALTER COLUMN [Assignment_file] NVARCHAR(50);

ALTER TABLE [SummerPractice].[Lessons]
ADD CONSTRAINT FK_Assignment_file FOREIGN KEY (Assignment_file) REFERENCES [SummerPractice].[file](FileName);
