CREATE TABLE [dbo].[HotwheelsCars]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [modelName] NVARCHAR(50) NOT NULL, 
    [seriesName] NVARCHAR(50) NOT NULL, 
    [seriesNum] NVARCHAR(50) NULL, 
    [yearProduced] NVARCHAR(50) NOT NULL, 
    [yearProducedNum] NVARCHAR(50) NULL, 
    [toyNum] NVARCHAR(50) NULL, 
    [photoUrl] NVARCHAR(1000) NOT NULL 
)
