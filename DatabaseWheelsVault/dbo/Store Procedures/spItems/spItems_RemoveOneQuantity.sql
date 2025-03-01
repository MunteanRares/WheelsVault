CREATE PROCEDURE [dbo].[spItems_RemoveOneQuantity]
	@itemId int
AS
begin
	set nocount on;
	update dbo.Items
	set Items.quantity = quantity - 1
	where Items.Id = @itemId
end