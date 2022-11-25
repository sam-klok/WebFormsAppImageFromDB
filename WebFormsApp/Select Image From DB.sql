use [LifelongLearning]

Declare @ImageID as int = 1

SELECT
	 [ID]
    ,[Name]
    ,[Type]
    ,[Hash]
    ,[PicData]
FROM [dbo].[Picture]
WHERE ID = @ImageID