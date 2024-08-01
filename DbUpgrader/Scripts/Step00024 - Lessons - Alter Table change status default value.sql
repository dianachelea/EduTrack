DECLARE @ConstraintName nvarchar(200)
SELECT @ConstraintName = Name FROM SYS.DEFAULT_CONSTRAINTS
WHERE PARENT_OBJECT_ID = OBJECT_ID('SummerPractice.Lessons')
AND PARENT_COLUMN_ID = (SELECT column_id FROM sys.columns
                        WHERE NAME = N'LessonStatus'
                        AND object_id = OBJECT_ID(N'SummerPractice.Lessons'))
IF @ConstraintName IS NOT NULL
EXEC('ALTER TABLE SummerPractice.Lessons DROP CONSTRAINT ' + @ConstraintName);

USE CentricSummerPractice;

ALTER TABLE SummerPractice.Lessons
DROP COLUMN LessonStatus;

USE CentricSummerPractice;

ALTER TABLE [SummerPractice].[Lessons]
ADD [LessonStatus] NVARCHAR(50) NOT NULL DEFAULT 'not_started';

