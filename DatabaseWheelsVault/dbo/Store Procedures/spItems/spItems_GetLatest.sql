CREATE PROCEDURE [dbo].[spItems_GetLatest]
	@currentYear int,
	@lastYearHw int
AS
begin
	set nocount on;
	select * from dbo.HotwheelsCars
	where HotwheelsCars.yearProduced in (@currentYear, @lastYearHw)
end