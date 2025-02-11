CREATE TABLE [dbo].[Items]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [modelName] NVARCHAR(100) NOT NULL, 
    [modelReleaseDate] NVARCHAR(10) NOT NULL, 
    [collectionName] NVARCHAR(50) NOT NULL
)
