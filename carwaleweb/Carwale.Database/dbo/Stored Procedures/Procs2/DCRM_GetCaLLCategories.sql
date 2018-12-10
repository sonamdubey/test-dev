IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetCaLLCategories]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetCaLLCategories]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(18th 2014)
-- Description	:	Get call categories for DCRM_Calls
-- Modifier 1 : Rucira Patil on 14th Oct 2014 (Added IsActive constraint )
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetCaLLCategories]
	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT Id AS Value , CallCategories FROM DCRM_CallCategories WITH (NOLOCK) WHERE IsActive = 1
END

