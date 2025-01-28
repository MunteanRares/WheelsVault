CREATE PROCEDURE [dbo].[spFolders_GetLast]

AS
begin
	set nocount on;

	select *
	from dbo.Folders
	where Folders.Id = (select MAX(Folders.Id) from dbo.Folders)

end
