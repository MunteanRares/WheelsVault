CREATE TABLE [dbo].[Items]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [folderId] INT NOT NULL, 
    [modelName] NVARCHAR(100) NOT NULL, 
    [modelReleaseDate] NVARCHAR(10) NOT NULL, 
    [collectionName] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_Items_Folders] FOREIGN KEY (folderId) REFERENCES Folders(Id)
)
