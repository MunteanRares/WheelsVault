CREATE PROCEDURE [dbo].[spItems_GetLast]

AS
begin
	set nocount on;
	select TOP 1 dbo.Items.*
	from dbo.Items
	join FolderItems on Items.Id = FolderItems.itemId
	order by Items.Id DESC;
end
