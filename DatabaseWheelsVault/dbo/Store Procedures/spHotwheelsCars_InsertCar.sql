CREATE PROCEDURE [dbo].[spHotwheelsCars_InsertCar]
	@modelName NVARCHAR(50),
	@seriesName NVARCHAR(50),
	@seriesNum NVARCHAR(50),
	@yearProduced NVARCHAR(50),
	@yearProducedNum NVARCHAR(50),
	@toyNum NVARCHAR(50),
	@photoUrl NVARCHAR(1000)
AS
begin
	set nocount on;
	insert into dbo.HotwheelsCars (modelName, seriesName, seriesNum, yearProduced, yearProducedNum, toyNum, photoUrl)
	values (@modelName, @seriesName, @seriesNum, @yearProduced, @yearProducedNum, @toyNum, @photoUrl);
end
