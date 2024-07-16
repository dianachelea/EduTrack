USE CentricSummerPractice;

CREATE TABLE [SummerPractice].[Tokens] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Token] NVARCHAR(100) NOT NULL,
    [UserEmail] NVARCHAR(100) NOT NULL,
    [ExpirationDate] DATETIME NOT NULL
);
