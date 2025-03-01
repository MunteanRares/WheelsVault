CREATE PROCEDURE [dbo].[spFolders_GetById]
	@folderId int
AS
begin
	set nocount on;

	if exists (select 1 from FolderItems where folderId = @folderId)
	begin
		select Folders.*
		from Folders
		join FolderItems on Folders.Id = FolderItems.folderId
		where Folders.Id = @folderId
	end
	else
	begin
		select *
		from dbo.Folders 
		where Id = @folderId
	end
	
end
