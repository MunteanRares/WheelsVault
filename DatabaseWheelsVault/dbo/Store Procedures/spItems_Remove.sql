CREATE PROCEDURE [dbo].[spItems_Remove]
	@itemId int
AS
begin
	set nocount on;
	delete from Items
	where Items.Id = @itemId
end
