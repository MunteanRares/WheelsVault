CREATE PROCEDURE [dbo].[Items_AddToFolderIfNotDefault]
	@folderId int,
	@itemId int
AS
begin
	set nocount on;

	declare @defaultFolderId int;
	select @defaultFolderId = Folders.Id
	from Folders
	where isDefault = 1	
	
	if not exists(
	select 1 from dbo.FolderItems
	where itemId = @itemId and folderId = @folderId)
	begin
		if (@defaultFolderId != @folderId)
		begin
			insert into dbo.FolderItems (folderId, itemId)
			values (@folderId, @itemId)
		end
	end
end
