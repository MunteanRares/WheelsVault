CREATE PROCEDURE [dbo].[spItems_CreateItem]
	@FolderId int,
	@ModelName nvarchar(100),
	@ModelReleaseDate nvarchar(10),
	@CollectionName nvarchar(50),
	@IsCustom bit
AS
begin 
	set nocount on;

	declare @addedItemID int;
	declare @defaultFolderId int;
	
	insert into dbo.Items (modelName, seriesName, yearProduced, isCustom)
	values(@ModelName, @CollectionName, @ModelReleaseDate, @IsCustom)
	set @addedItemID = SCOPE_IDENTITY()

	select @defaultFolderId = Folders.Id
	from Folders
	where isDefault = 1

	insert into dbo.FolderItems (folderId, itemId)
	values (@defaultFolderId, @addedItemID)

	if (@defaultFolderId != @FolderId)
	begin
		insert into dbo.FolderItems (folderId, itemId)
		values (@FolderId, @addedItemID)
	end
end