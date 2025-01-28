CREATE PROCEDURE [dbo].[spItems_GetByFolderId]
	@folderId int
AS
begin
	set nocount on;
	select [Id], [folderId], [modelName], [modelReleaseDate], [collectionName]
	from dbo.Items 
	where dbo.Items.folderId = @folderId
end