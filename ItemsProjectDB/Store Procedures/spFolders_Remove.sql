CREATE PROCEDURE [dbo].[spFolders_Remove]
	@folderId int
AS
begin

	set nocount on;
	delete from dbo.Items
	where Items.folderId = @folderId

	delete from dbo.Folders
	where Folders.Id = @folderId
		
end