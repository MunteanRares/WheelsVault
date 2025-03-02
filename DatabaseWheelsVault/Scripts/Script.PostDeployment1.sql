﻿/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

if not exists (select 1 from Folders)
begin
    insert into Folders (name, isDefault)
    values ('All Cars', 1)
end

if not exists(select 1 from Appsettings)
begin
    insert into AppSettings(isDbPopulated)
    values(0)
end