CREATE PROCEDURE [dbo].[spHotwheelsCars_FindCarByText]
	@searchhwText nvarchar(100)
AS
begin
	set nocount on;

	select *
	from dbo.HotwheelsCars
	where HotwheelsCars.modelName like '%' + @searchhwText + '%' or
		HotwheelsCars.seriesName like '%' + @searchhwText + '%' or
		HotwheelsCars.yearProduced like '%' + @searchhwText + '%' or
		HotwheelsCars.toyNum like '%' + @searchhwText + '%' or
		HotwheelsCars.yearProducedNum like '%' + @searchhwText + '%'
end