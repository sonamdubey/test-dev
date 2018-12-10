IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_UpdateMainImgRoadTest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_UpdateMainImgRoadTest]
GO

	Create procedure [dbo].[Con_UpdateMainImgRoadTest]
@RTId NUMERIC,
@MainImgPath VARCHAR(100)

as
			if EXISTS (select ID from Con_RoadTest where ID=@RTId and 
MainImgpath=@MainImgPath)
				BEGIN
					Update Con_RoadTest set MainImgpath=''
				END