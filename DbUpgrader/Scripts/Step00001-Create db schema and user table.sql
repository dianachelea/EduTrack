IF NOT EXISTS (SELECT * FROM master.sys.databases 
          WHERE name='CentricSummerPractice')
BEGIN
    CREATE DATABASE [CentricSummerPractice]
END

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'SummerPractice')
BEGIN
	EXEC('CREATE SCHEMA SummerPractice')
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'User' AND TABLE_SCHEMA = 'SummerPractice')
BEGIN
	CREATE TABLE [SummerPractice].[User](
		[Email]			nvarchar(50)	NOT NULL,
		[Username]			nvarchar(50)	NOT NULL,
		[Password]			nvarchar(256)	NOT NULL
	CONSTRAINT [PK_Email] PRIMARY KEY CLUSTERED ([Email] ASC) ON [PRIMARY]) ON [PRIMARY]
END