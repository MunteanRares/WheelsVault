CREATE PROCEDURE [dbo].[spItems_GetUnique]
	@modelName nvarchar(100),
	@seriesName nvarchar(100),
	@seriesNum nvarchar(15),
	@yearProduced nvarchar(15),
	@yearProducedNum nvarchar(15),
	@toyNum nvarchar(15),
	@photoURL nvarchar(1000),
	@isCustom bit
AS
begin
	set nocount on;
	select [Id], [modelName], [seriesName], [seriesNum], [yearProduced], [yearProducedNum], [toyNum], [photoUrl], [isCustom], [quantity]
	from dbo.Items
	where modelName = @modelName
	and seriesName = @seriesName
	and seriesNum = @seriesNum
	and yearProduced = @yearProduced
	and yearProducedNum = @yearProducedNum
	and toyNum = @toyNum
	and photoUrl = @photoURL
	and isCustom = @isCustom
end