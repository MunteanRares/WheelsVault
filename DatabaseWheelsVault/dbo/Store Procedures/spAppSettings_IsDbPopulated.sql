CREATE PROCEDURE [dbo].[spAppSettings_IsDbPopulated]
	
AS
begin
	set nocount on;
	select top 1 [isDbPopulated]
	from dbo.AppSettings;
end