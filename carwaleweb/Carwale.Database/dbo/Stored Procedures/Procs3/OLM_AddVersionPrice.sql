IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AddVersionPrice]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AddVersionPrice]
GO

	-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 27 July 2013
-- Description : Save Audi Version Price  Details 
-- Updated By : Vinay Kumar Prajapti 15th May 2015 Added IsActive Field ...
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AddVersionPrice]
(
@GradeId INT,
@VersionId INT,
@StateId INT,
@Price BIGINT,
@IsUpdate BIT,
@Id INT,
@Status INT OUTPUT
)
AS
BEGIN
      IF @IsUpdate <> 0
		  BEGIN  
		  
			   SELECT AVG.Id FROM OLM_AudiBE_VersionPrices AS AVG WITH(NOLOCK) WHERE  AVG.GradeId=@GradeId AND AVG.VersionId=@VersionId AND AVG.StateId=@StateId AND AVG.Id <> @Id AND AVG.IsActive=1
				IF @@ROWCOUNT <> 0
				BEGIN
				  SET @Status=0
				  END 
				ELSE
				  BEGIN    
				UPDATE OLM_AudiBE_VersionPrices SET VersionId=@VersionId,GradeId=@GradeId,StateId=@StateId,Price=@Price WHERE Id=@Id  
				SET @Status=2 
			  END
		  END      
      ELSE
		  BEGIN
			 SELECT AVG.Id FROM OLM_AudiBE_VersionPrices AS AVG WITH(NOLOCK) WHERE  AVG.GradeId=@GradeId AND AVG.VersionId=@VersionId AND  AVG.StateId=@StateId AND AVG.IsActive=1				
			IF @@ROWCOUNT <> 0
				BEGIN
				  SET @Status=0
				END 
			 ELSE
			    BEGIN    
				   INSERT INTO OLM_AudiBE_VersionPrices(VersionId,GradeId,StateId,Price) VALUES(@VersionId,@GradeId,@StateId,@Price)
				   SET @Status=1
			     END
		  END	  
 END

 