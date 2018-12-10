IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SCInsertSkodaNewPart]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SCInsertSkodaNewPart]
GO

	

-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 13 Jun 2013
-- Description : Insert Skoda  New Part Details 
-- =============================================
CREATE PROCEDURE [dbo].[OLM_SCInsertSkodaNewPart]
(
@PartName VARCHAR(50),
@PartNo  VARCHAR(50),
@PartType  SmallInt,
@IsUpdate BIT,
@PartId BIGINT
)
AS
BEGIN
	 IF  @IsUpdate =1   
		  BEGIN      
			UPDATE OLM_SCPartsMaster SET PartDescription=@PartName,PartNumber=@PartNo,PartType=@PartType WHERE Id=@PartId      
		  END      
	 ELSE  IF @IsUpdate =0   
		  BEGIN    
			  INSERT INTO OLM_SCPartsMaster(PartDescription,PartNumber,PartType) VALUES(@PartName,@PartNo,@PartType)
		  END
 END
