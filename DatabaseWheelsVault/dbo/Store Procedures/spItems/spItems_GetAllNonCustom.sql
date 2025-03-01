CREATE PROCEDURE [dbo].[spItems_GetAllNonCustom]
	
AS
begin
	set nocount on;
	select *
	from dbo.Items
	where Items.isCustom = 0
end
