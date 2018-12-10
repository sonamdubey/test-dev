IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetMFCCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetMFCCities]
GO

	-- =============================================
-- Author	:	Sachin Bharti(4th Aug 2014)
-- Description	:	Get mahindra mapped cities 
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetMFCCities]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT C.Name AS City 
	FROM DCRM_MFCMappedCities MFC(NOLOCK) 
    INNER JOIN Cities C (NOLOCK) ON C.ID = MFC.CityID 
	WHERE IsActive = 1 AND ( ISNULL(MFC.LeadsSent,0) <= ISNULL(MFC.ProcurementLeads,0)) 
END
