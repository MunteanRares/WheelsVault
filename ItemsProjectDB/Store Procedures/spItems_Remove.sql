CREATE PROCEDURE [dbo].[spItems_Remove]
	@itemId int,
	@folderId int,
	@modelName nvarchar(50),
	@modelReleaseDate nvarchar(10),
	@collectionName nvarchar(50)

AS
begin
	set nocount on;

	declare @topFolderId int;
	set @topFolderId = (select top 1 Id from dbo.Folders)
	
	if @folderId = @topFolderId
	begin
		delete from dbo.Items
		where Items.collectionName = @collectionName and Items.modelName = @modelName and Items.modelReleaseDate = @modelReleaseDate
	end
	else
	begin
		delete from dbo.Items
		where Items.Id = @itemId and Items.folderId = folderId
	end
end
