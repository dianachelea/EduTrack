IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'User' AND TABLE_SCHEMA = 'SummerPractice')
BEGIN
IF NOT EXISTS (SELECT * FROM sys.columns WHERE [name]=N'Role' AND [object_id]=OBJECT_ID(N'[SummerPractice].[User]'))
BEGIN
	ALTER TABLE [SummerPractice].[User]
	ADD [Role] nvarchar(50) NOT NULL
END
END