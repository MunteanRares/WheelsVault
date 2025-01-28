CREATE PROCEDURE [dbo].[spItems_CreateItem]
	@FolderId int,
	@ModelName nvarchar(50),
	@ModelReleaseDate nvarchar(50),
	@CollectionName nvarchar(50)
	
AS
begin
	set nocount on;
	insert into dbo.Items (folderId, modelName, collectionName, modelReleaseDate) 
	values (@FolderId, @ModelName, @CollectionName, @ModelReleaseDate)

end
