CREATE PROCEDURE [dbo].[spItems_EditItem]
	@itemId int,
	@newName nvarchar(50),
	@newReleaseDate nvarchar(10),
	@newCollectionName nvarchar(50)
AS
begin
	set nocount on;
	update dbo.Items
	set modelName = @newName, modelReleaseDate = @newReleaseDate, collectionName = @newCollectionName
	where Id = @itemId
end
