IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_DeleteAudiFeature]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_DeleteAudiFeature]
GO

	-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 15 July 2013
-- Description : Insert Audi Details 
-- =============================================


CREATE PROCEDURE [dbo].[OLM_DeleteAudiFeature]
(
@Id VARCHAR(500),
@Status BIT OUTPUT
)
AS
BEGIN
   
	  BEGIN      
		DELETE FROM  OLM_AudiBE_Features  WHERE Id IN(SELECT * FROM list_to_tbl(@Id))
		SET @Status =1     
	  END      
	
 END
