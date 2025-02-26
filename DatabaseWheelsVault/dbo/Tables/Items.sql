CREATE TABLE [dbo].[Items]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [modelName] NVARCHAR(100) NOT NULL, 
    [seriesName] NVARCHAR(100) NOT NULL, 
    [seriesNum] NVARCHAR(15) NULL, 
    [yearProduced] NVARCHAR(10) NOT NULL, 
    [yearProducedNum] NVARCHAR(50) NULL, 
    [toyNum] NVARCHAR(15) NULL, 
    [photoUrl] NVARCHAR(1000) NULL, 
    [isCustom] BIT NOT NULL
)
