CREATE PROCEDURE [dbo].[spFolders_GetById]
	@folderId int
AS
begin

	set nocount on;
	select [Id], [name]
	from dbo.Folders
	where Folders.Id = @folderId

end
