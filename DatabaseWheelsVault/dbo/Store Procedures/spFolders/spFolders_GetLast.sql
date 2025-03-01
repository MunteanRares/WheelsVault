CREATE PROCEDURE [dbo].[spFolders_GetLast]

AS
begin
	set nocount on;
	select top 1 Folders.*
	from Folders
	order by Folders.Id desc
end
