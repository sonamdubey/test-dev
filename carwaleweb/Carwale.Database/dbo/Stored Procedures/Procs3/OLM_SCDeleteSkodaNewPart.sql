IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SCDeleteSkodaNewPart]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SCDeleteSkodaNewPart]
GO

	CREATE PROCEDURE [dbo].[OLM_SCDeleteSkodaNewPart]
(
@PartId VARCHAR(500),
@Status BIT OUTPUT
)
AS
BEGIN
   
	  BEGIN      
		DELETE FROM  OLM_SCPartsMaster  WHERE Id IN(SELECT * FROM list_to_tbl(@PartId))
		SET @Status =1     
	  END      
	
 END
 