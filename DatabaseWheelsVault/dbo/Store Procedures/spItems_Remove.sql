CREATE PROCEDURE [dbo].[spItems_Remove]
	@itemId int,
	@folderId int
AS
begin
	set nocount on;
	declare @defaultFolderId int;
	select @defaultFolderId = Folders.Id
	from Folders
	where Folders.isDefault = 1


	if (@defaultFolderId = @folderId)
	begin
		delete from Items
		where Items.Id = @itemId
	end
	else
	begin
		delete from FolderItems
		where FolderItems.folderId = @folderId and FolderItems.itemId = @itemId
	end	

end
