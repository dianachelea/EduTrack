USE CentricSummerPractice;

ALTER TABLE [SummerPractice].[Courses]
ALTER COLUMN [ImageData] NVARCHAR(50);

ALTER TABLE [SummerPractice].[Courses]
ADD CONSTRAINT FK_Image_data FOREIGN KEY (ImageData) REFERENCES [SummerPractice].[file](FileName);