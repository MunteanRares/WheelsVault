CREATE PROCEDURE [dbo].[spItems_RemoveAllFromFolder]
	@itemId int
AS
begin
	set nocount on;
	delete from dbo.Items
	where Items.Id = @itemId
end
