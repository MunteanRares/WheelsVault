CREATE PROCEDURE [dbo].[spFolders_EditName]
	@folderName nvarchar(50),
	@folderId int

AS
begin
	set nocount on;
	update dbo.Folders
	set name = @folderName
	where Id = @folderId
end