IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AddGroup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AddGroup]
GO

	
-- =============================================
-- Author      : Rahul Kumar
-- Create date : 3st Oct 2013
-- Description : Insert GroupName, modelId into  OLM_SkodaFeatureGroup
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AddGroup]
(	
    @Id  BIGINT,
    @GroupName VARCHAR(100),	
	@ModelId BIGINT,
	@Status		INT OUTPUT	
)	
 AS
	
BEGIN

         SELECT OSFG.Id FROM OLM_SkodaFeatureGroup AS OSFG WITH(NOLOCK) WHERE Name = @GroupName AND ModelId =  @ModelId
		 IF @@ROWCOUNT <> 0
		    BEGIN
		      SET @Status = 0
		    END

	     ELSE if @Id <> -1
             BEGIN
			      UPDATE  OLM_SkodaFeatureGroup SET Name = @GroupName,  ModelId =  @ModelId
			      WHERE ID = @Id
			      SET @Status = 1 
		     END
		ELSE
	         BEGIN
                 INSERT INTO OLM_SkodaFeatureGroup(Name, ModelId,IsActive) 
			     VALUES(@GroupName, @ModelId,1)		
	             SET @Status = 1 
			 END
END
		