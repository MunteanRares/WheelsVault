CREATE PROCEDURE [dbo].[spFolders_CreateFolder]
	@folderName nvarchar(50)
AS
begin
	set nocount on;
	
	insert into dbo.Folders (name)
	values (@folderName)

end
