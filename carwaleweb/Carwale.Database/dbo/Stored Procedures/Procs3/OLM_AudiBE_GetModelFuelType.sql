IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_GetModelFuelType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_GetModelFuelType]
GO

	
-- =============================================================================================
-- Author:		Ashish G. Kamble
-- Create date: 28 July 2013
-- Description:	SP to get the model fuel type
-- exec [OLM_AudiBE_GetModelFuelType] 133
-- Modified By Supriya On 5/12/2013  to add v.isActive filter
-- =============================================================================================
CREATE PROCEDURE [dbo].[OLM_AudiBE_GetModelFuelType] @TransactionId BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT vs.Value AS FuelTypeId
		,CASE vs.Value
			WHEN 1
				THEN 'Diesel'
			ELSE 'Petrol'
			END AS FuelType
	FROM OLM_AudiBE_Transactions t
	LEFT JOIN OLM_AudiBE_Versions v ON t.ModelId = v.ModelId 
	LEFT JOIN OLM_AudiBE_Version_Specs vs ON v.Id = vs.VersionId
	WHERE vs.SpecId = 1 
		AND t.Id = @TransactionId AND v.isActive=1
END
