IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AddSubGroup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AddSubGroup]
GO

	-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 1st Oct 2013
-- Description : Insert SubgroupName, modelId GroupId into  OLM_SkodaFeatureSubGroup
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AddSubGroup]
(	
    @Id  BIGINT,
    @SubGroupName VARCHAR(100),	
	@ModelId BIGINT,
	@GroupId BIGINT,	
	@Status		INT OUTPUT	
)	
 AS
	
BEGIN
  IF @Id <> -1
	  BEGIN
		   SELECT SFSG.Id FROM  OLM_SkodaFeatureSubGroup AS SFSG WITH(NOLOCK) WHERE  Name = @SubGroupName AND   GroupId = @GroupId AND ModelId =  @ModelId AND ID <> @Id
		   IF @@ROWCOUNT <> 0
			  BEGIN
				 SET @Status = 0 
			  END
		   ELSE
			  BEGIN
				 UPDATE  OLM_SkodaFeatureSubGroup SET Name = @SubGroupName,  GroupId = @GroupId, ModelId =  @ModelId
				 WHERE ID = @Id
				 SET @Status = 1
			   END		
	   END
	
  ELSE
	
	 BEGIN
		   SELECT SFSG.Id FROM  OLM_SkodaFeatureSubGroup AS SFSG WITH(NOLOCK) WHERE  Name = @SubGroupName AND  GroupId = @GroupId AND ModelId =  @ModelId 
		    IF @@ROWCOUNT <> 0
			  BEGIN
				 SET @Status = 0 
			  END
		   ELSE
			  BEGIN
				  INSERT INTO OLM_SkodaFeatureSubGroup(Name,GroupId, ModelId,IsActive) 
				  VALUES(@SubGroupName, @GroupId, @ModelId,1)			 
				  SET @Status = 1 
				  PRINT @STATUS
			  END
		
	 END
					
END

