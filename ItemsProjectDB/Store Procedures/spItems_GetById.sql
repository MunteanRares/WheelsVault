CREATE PROCEDURE [dbo].[spItems_GetById]
	@itemId int
AS
begin
	set nocount on;

	select *
	from dbo.Items
	where Items.Id = @itemId

end
