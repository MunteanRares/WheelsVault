CREATE PROCEDURE [dbo].[spFolders_GetAll]

AS
begin
	set nocount on;
	select *
	from Folders
end
