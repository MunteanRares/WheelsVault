CREATE PROCEDURE [dbo].[spItems_GetAll]

AS
begin
	set nocount on;
	select *
	from Items
end
