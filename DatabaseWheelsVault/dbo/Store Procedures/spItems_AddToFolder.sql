CREATE PROCEDURE [dbo].[spItems_AddToFolder]
	@selectedItemId int,
	@selectedFolderId int
AS
begin
	set nocount on;
	insert into dbo.FolderItems (FolderItems.FolderId, FolderItems.ItemId)
	values (@selectedFolderId, @selectedItemId)
end
