CREATE PROCEDURE [dbo].[spItems_RemoveById]
	@itemId int
AS
begin
set nocount on;
	
	delete from dbo.Items
	where Items.Id = @itemId

end
