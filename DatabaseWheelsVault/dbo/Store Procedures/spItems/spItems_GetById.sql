CREATE PROCEDURE [dbo].[spItems_GetById]
	@itemId int
AS
begin
	set nocount on;
	select Items.*
	from Items
	join FolderItems on Items.Id = FolderItems.itemId
	where Items.Id = @itemId
end