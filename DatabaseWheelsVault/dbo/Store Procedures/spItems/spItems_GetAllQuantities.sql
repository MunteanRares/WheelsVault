CREATE PROCEDURE [dbo].[spItems_GetAllQuantities]

AS
begin
	set nocount on;
	select coalesce(Sum(quantity),0) from dbo.Items
end
