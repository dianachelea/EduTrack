USE CentricSummerPractice;

ALTER TABLE [SummerPractice].[Courses]
ADD [TeacherEmail] NVARCHAR(50) NOT NULL DEFAULT 'nimic@nimic.com' ;

ALTER TABLE [SummerPractice].[Courses]
ADD CONSTRAINT FK_Teacher_email FOREIGN KEY (TeacherEmail) REFERENCES [SummerPractice].[User](Email);
