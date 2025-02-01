CREATE PROCEDURE [dbo].[spItems_CreateItem]
	@FolderId int,
	@ModelName nvarchar(50),
	@ModelReleaseDate nvarchar(50),
	@CollectionName nvarchar(50)
	
AS
begin
	set nocount on;

	declare @TopFolderId int;
	set @TopFolderId = (select top 1 Id from dbo.Folders)

	if @FolderId != @TopFolderId
	begin
		insert into dbo.Items (folderId, modelName, collectionName, modelReleaseDate) 
		values (@FolderId, @ModelName, @CollectionName, @ModelReleaseDate),
			   (@TopFolderId, @ModelName, @CollectionName, @ModelReleaseDate);
	end

	else
	begin
		insert into dbo.Items (folderId, modelName, collectionName, modelReleaseDate) 
		values (@FolderId, @ModelName, @CollectionName, @ModelReleaseDate);
	end

end
