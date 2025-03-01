CREATE PROCEDURE [dbo].[spItems_AddHotWheelsModel]
	@folderId int,
	@modelName nvarchar(100),
	@seriesName nvarchar(100),
	@seriesNum nvarchar(15),
	@yearProduced nvarchar(10),
	@yearProducedNum nvarchar(15),
	@toyNum nvarchar(15),
	@photoURL nvarchar(1000),
	@isCustom bit
AS
begin
	set nocount on;
	declare @defaultFolderId int;
	declare @addedItemId int;

	select @defaultFolderId = Folders.Id
	from Folders
	where isDefault = 1	

	if not exists(
	select 1 from dbo.Items
	where modelName = @modelName
	and seriesName = @seriesName
	and seriesNum = @seriesNum
	and yearProduced = @yearProduced
	and toyNum = @toyNum
	and photoUrl = @photoURL
	and isCustom = 0 )
	begin
		insert into dbo.Items (modelName, seriesName, seriesNum, yearProduced, yearProducedNum, toyNum, photoURL, isCustom, quantity)
		values(@modelName, @seriesName, @seriesNum, @yearProduced, @yearProducedNum, @toyNum, @photoURL, @isCustom, 1)
		set @addedItemId = SCOPE_IDENTITY();

		insert into dbo.FolderItems (folderId, itemId)
		values (@defaultFolderId, @addedItemId)

		if (@defaultFolderId != @folderId)
		begin
			insert into dbo.FolderItems (folderId, itemId)
			values (@folderId, @addedItemID)
		end
	end
	else
	begin
		update dbo.Items
		set quantity = quantity + 1
		where modelName = @modelName
		and seriesName = @seriesName
		and seriesNum = @seriesNum
		and yearProduced = @yearProduced
		and toyNum = @toyNum
		and photoUrl = @photoURL
		and isCustom = 0
	end
end