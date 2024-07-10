USE CentricSummerPractice;

ALTER TABLE [SummerPractice].[User]
ADD Last_Name NVARCHAR(50) NOT NULL DEFAULT 'Temp_LastName',
    First_name NVARCHAR(50) NOT NULL DEFAULT 'Temp_FirstName',
    Phone_number CHAR(10) NOT NULL DEFAULT '0000000000';



ALTER TABLE [SummerPractice].[User]
ADD CONSTRAINT UQ_Phone_number UNIQUE (Phone_number);
