IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_InsertMainImgRoadTest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_InsertMainImgRoadTest]
GO

	Create procedure [dbo].[Con_InsertMainImgRoadTest]

@RTId NUMERIC,
@MainImgPath VARCHAR(100)

as

UPDATE Con_RoadTest SET MainImgPath=@MainImgPath Where ID=@RTId