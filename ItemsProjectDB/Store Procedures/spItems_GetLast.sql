CREATE PROCEDURE [dbo].[spItems_GetLast]

AS
begin
	set nocount on;
	
	select [Id], [folderId], [modelName], [modelReleaseDate], [collectionName]
	from dbo.Items
	where Items.Id = (select MAX(Items.Id) from dbo.Items)

end
