CREATE TABLE [dbo].[FolderItems]
(
	[folderId] INT NOT NULL, 
    [itemId] INT NOT NULL,
	PRIMARY KEY (folderId, itemId),
	FOREIGN KEY (folderId) REFERENCES Folders(Id) ON DELETE CASCADE,
	FOREIGN KEY (itemId) REFERENCES Items(Id) ON DELETE CASCADE	
	
)
