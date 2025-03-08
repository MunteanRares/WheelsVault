CREATE PROCEDURE [dbo].[spFolders_GetDefault]
	
AS
begin
	set nocount on;
	select * from dbo.Folders
	where Folders.isDefault = 1;
end