CREATE PROCEDURE [dbo].[spItems_CreateItem]
	@FolderId int,
	@ModelName nvarchar(50),
	@ModelReleaseDate nvarchar(10),
	@CollectionName nvarchar(50)
AS
begin 
	set nocount on;

	declare @addedItemID int;
	
	insert into dbo.Items (modelName, modelReleaseDate, collectionName)
	values(@ModelName, @ModelReleaseDate, @CollectionName)
	set @addedItemID = SCOPE_IDENTITY()

	insert into dbo.FolderItems (folderId, itemId)
	values (@FolderId, @addedItemID)
end