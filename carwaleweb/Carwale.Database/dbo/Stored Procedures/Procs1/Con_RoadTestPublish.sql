IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_RoadTestPublish]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_RoadTestPublish]
GO

	
CREATE PROCEDURE [dbo].[Con_RoadTestPublish]
@ID NUMERIC

as

UPDATE Con_RoadTest SET IsPublished=1 Where ID=@ID

