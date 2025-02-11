CREATE PROCEDURE [dbo].[spFolders_Remove]
	@folderId int
AS
begin
	set nocount on;

	delete from Folders
	where Folders.Id = @folderId
	
end