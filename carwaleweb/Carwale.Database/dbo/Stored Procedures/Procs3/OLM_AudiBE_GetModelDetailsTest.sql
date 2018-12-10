IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_GetModelDetailsTest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_GetModelDetailsTest]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 28 July 2013
-- Description: Proc to fetch all model details.
-- Modifier:	Vaibhav K (10-8-2013)
--				Also get the details for upholstery so added on new sp
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AudiBE_GetModelDetailsTest]
	@TransactionId BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	EXEC OLM_AudiBE_GetModelFuelType @TransactionId
	
	EXEC OLM_AudiBE_VersionsByModel @TransactionId	
	
	EXEC OLM_AudiBE_GradesFeaturesTest @TransactionId
	
	EXEC OLM_AudiBE_GetVersionGrades @TransactionId
	
	EXEC OLM_AudiBE_GetModelColors @TransactionId
	
	EXEC OLM_AudiBE_GetModelUpholsteryColors @TransactionId
	
END
