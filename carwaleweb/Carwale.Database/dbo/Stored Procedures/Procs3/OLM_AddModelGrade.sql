IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AddModelGrade]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AddModelGrade]
GO

	-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 15 July 2013
-- Description : Save Audi Model Grade  Details 
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AddModelGrade]
(
@ModelId int,
@GradeId int,
@FeatureId int,
@IsUpdate BIT,
@Id int,
@Status int OUTPUT
)
AS
BEGIN
      IF @IsUpdate <> 0
		  BEGIN  
		  
			   SELECT AGF.Id FROM OLM_AudiBE_Model_GradeFeatures AS AGF WITH(NOLOCK) WHERE AGF.ModelId=@ModelId AND AGF.GradeId=@GradeId AND AGF.FeatureId=@FeatureId AND AGF.Id <> @Id
				IF @@ROWCOUNT <> 0
				BEGIN
				  SET @Status=0
				  END 
				ELSE
				  BEGIN    
				UPDATE OLM_AudiBE_Model_GradeFeatures SET ModelId=@ModelId,FeatureId=@FeatureId,GradeId=@GradeId WHERE Id=@Id  
				SET @Status=2 
			  END
		  END      
      ELSE
		  BEGIN
			  SELECT AGF.Id FROM OLM_AudiBE_Model_GradeFeatures AS AGF WITH(NOLOCK) WHERE AGF.ModelId=@ModelId AND AGF.GradeId=@GradeId AND AGF.FeatureId=@FeatureId
			IF @@ROWCOUNT <> 0
				BEGIN
				  SET @Status=0
				END 
			 ELSE
			    BEGIN    
				   INSERT INTO OLM_AudiBE_Model_GradeFeatures(ModelId,GradeId,FeatureId) VALUES(@ModelId,@GradeId,@FeatureId)
				   SET @Status=1
			     END
		  END	  
 END
