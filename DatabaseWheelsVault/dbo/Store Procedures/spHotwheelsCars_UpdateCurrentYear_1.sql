CREATE PROCEDURE [dbo].[spHotwheelsCars_UpdateCurrentYear]
	@modelName nvarchar(50),
	@seriesName nvarchar(50),
	@photoUrl nvarchar(1000),
	@seriesNum nvarchar(50),
	@yearProduced nvarchar(50),
	@yearProducedNum nvarchar(50),
	@toyNum nvarchar(50)
AS
begin
	set nocount on;
	if not exists 
	(select * from HotWheelsCars
	where modelName = @modelName and seriesName = @seriesName and seriesNum = @seriesNum and yearProduced = @yearProduced and yearProducedNum = @yearProducedNum and toyNum = @toyNum)
	begin
		insert into HotWheelsCars (modelName, seriesName, photoUrl, seriesNum, yearProduced, yearProducedNum, toyNum)
		values (@modelName, @seriesName, @photoUrl, @seriesNum, @yearProduced, @yearProducedNum, @toyNum)
	end
	
end