IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_GradesByVersion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_GradesByVersion]
GO

	
-- =============================================  
-- Author:  Supriya K.  
-- Create date: 27/7/13  
-- Description: To fetch grades of versions for particular modelId
-- Modifier: Vaibhav  K (1-8-2013) Removed fueltype constarint from the query to get all version grades for thar model
-- =============================================  
CREATE PROCEDURE [dbo].[OLM_AudiBE_GradesByVersion]  
 @TransactionId INTEGER  
AS  
BEGIN  
 SET NOCOUNT ON;  
 SELECT vg.VersionId,vg.GradeId  
 FROM  
 OLM_AudiBE_Transactions t  
 LEFT JOIN OLM_AudiBE_Models m  
 ON t.ModelId=m.Id AND m.IsActive=1  
 LEFT JOIN OLM_AudiBE_Versions v  
 ON m.Id=v.ModelId AND v.IsActive=1  
 LEFT JOIN OLM_AudiBE_VersionGrades vg  
 ON v.Id=vg.VersionId AND vg.IsActive=1  
 LEFT JOIN OLM_AudiBE_Version_Specs vs  
 ON v.id=vs.VersionId AND vs.specId=1   
 WHERE t.Id=@TransactionId --AND vs.value=t.FuelTypeId  
END  
