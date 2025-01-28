CREATE PROCEDURE [dbo].[spItems_GetAll]

AS
begin
	set nocount on;
	select [Id], [folderId], [modelName], [modelReleaseDate], [collectionName]
	from dbo.Items;

end
