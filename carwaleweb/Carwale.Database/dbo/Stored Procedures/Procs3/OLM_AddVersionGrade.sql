IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AddVersionGrade]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AddVersionGrade]
GO

	-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 25 July 2013
-- Description : Save Audi Version Grade  Details 
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AddVersionGrade]
(
@GradeId int,
@VersionId int,
@IsUpdate BIT,
@Id int,
@Status int OUTPUT
)
AS
BEGIN
      IF @IsUpdate <> 0
		  BEGIN  
		  
			   SELECT AVG.Id FROM OLM_AudiBE_VersionGrades AS AVG WITH(NOLOCK) WHERE  AVG.GradeId=@GradeId AND AVG.VersionId=@VersionId AND AVG.Id <> @Id
				IF @@ROWCOUNT <> 0
				BEGIN
				  SET @Status=0
				  END 
				ELSE
				  BEGIN    
				UPDATE OLM_AudiBE_VersionGrades SET VersionId=@VersionId,GradeId=@GradeId WHERE Id=@Id  
				SET @Status=2 
			  END
		  END      
      ELSE
		  BEGIN
			 SELECT AVG.Id FROM OLM_AudiBE_VersionGrades AS AVG WITH(NOLOCK) WHERE  AVG.GradeId=@GradeId AND AVG.VersionId=@VersionId				
			IF @@ROWCOUNT <> 0
				BEGIN
				  SET @Status=0
				END 
			 ELSE
			    BEGIN    
				   INSERT INTO OLM_AudiBE_VersionGrades(VersionId,GradeId) VALUES(@VersionId,@GradeId)
				   SET @Status=1
			     END
		  END	  
 END
