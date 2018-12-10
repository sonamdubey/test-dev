IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_VersionsByFuelType_Specs_GradesFeatures]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_VersionsByFuelType_Specs_GradesFeatures]
GO

	
-- =============================================
-- Author:		Supriya Khartode
-- Create date: 26/7/13
-- Description: To fetch only versions for fueltype selected in transaction & its all specifications
--				To fetch only grades & its features for transaction selected
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AudiBE_VersionsByFuelType_Specs_GradesFeatures] 
	@TransactionId INTEGER
AS
BEGIN
	SET NOCOUNT ON;

	exec OLM_AudiBE_VersionsByFuelType_Specs @TransactionId
	
	exec OLM_AudiBE_GradesFeatures @TransactionId
	
END

