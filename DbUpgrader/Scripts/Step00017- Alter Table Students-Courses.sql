USE CentricSummerPractice;

ALTER TABLE [SummerPractice].[Students-Courses]
ADD CONSTRAINT UK_course_email 
UNIQUE ([Course_id], [Email]);