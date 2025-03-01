CREATE PROCEDURE [dbo].[spAppSettings_SetDbToPopulated]
	
AS
begin
	set nocount on;
	update top (1) dbo.Appsettings
	set isDbPopulated = 1
end
