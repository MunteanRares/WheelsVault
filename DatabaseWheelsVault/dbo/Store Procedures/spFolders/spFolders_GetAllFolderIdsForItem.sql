CREATE PROCEDURE [dbo].[spFolders_GetAllFolderIdsForItem]
	@selectedItemID int
AS
begin
	set nocount on;
	select [folderId]
	from FolderItems
	where FolderItems.itemId = @selectedItemID

end