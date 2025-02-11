CREATE PROCEDURE [dbo].[spItems_GetByFolderId]
	@folderId int
AS
begin
	set nocount on;
	select Items.*
	from Items
	join FolderItems on Items.Id = FolderItems.itemId
	where FolderItems.folderId = @folderId
end
