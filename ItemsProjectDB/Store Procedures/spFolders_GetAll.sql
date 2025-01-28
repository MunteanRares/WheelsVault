CREATE PROCEDURE [dbo].[spFolders_GetAll]
AS
begin
set nocount on;

	select [Id], [name]
	from dbo.Folders

end
