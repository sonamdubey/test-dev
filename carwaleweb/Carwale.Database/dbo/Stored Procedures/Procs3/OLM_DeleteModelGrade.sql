IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_DeleteModelGrade]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_DeleteModelGrade]
GO

	-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 15 July 2013
-- Description : Delete Audi Model Grade  Details 
-- =============================================

 CREATE PROCEDURE [dbo].[OLM_DeleteModelGrade]
(
@Id VARCHAR(500),
@Status BIT OUTPUT
)
AS
BEGIN
   
	  BEGIN      
		DELETE FROM  OLM_AudiBE_Model_GradeFeatures  WHERE Id IN(SELECT * FROM list_to_tbl(@Id))
		SET @Status =1     
	  END      
	
 END
