CREATE PROCEDURE [dbo].[Items_AddHotWheelsModel]
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

	insert into dbo.Items (modelName, seriesName, seriesNum, yearProduced, yearProducedNum, toyNum, photoURL, isCustom)
	values(@modelName, @seriesName, @seriesNum, @yearProduced, @yearProducedNum, @toyNum, @photoURL, @isCustom)
	set @addedItemId = SCOPE_IDENTITY();

	insert into dbo.FolderItems (folderId, itemId)
	values (@defaultFolderId, @addedItemId)
end