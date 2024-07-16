USE CentricSummerPractice;

ALTER TABLE [SummerPractice].[Courses]
ADD Learning_topics NVARCHAR(500) NOT NULL DEFAULT 'Nimic' ,
	Perequisites NVARCHAR(500) NOT NULL DEFAULT 'Nimic' ;

