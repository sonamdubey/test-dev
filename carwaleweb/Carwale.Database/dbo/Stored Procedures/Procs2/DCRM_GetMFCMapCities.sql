IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetMFCMapCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetMFCMapCities]
GO

	-- =============================================
-- Author	:	Sachin Bharti(9th July)
-- Description	:	Get all the mapped cities for Mahindra first choice leads pushing
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetMFCMapCities]
	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT	MC.ID,C.Name AS City,
			OU.UserName ,
			CONVERT(VARCHAR(50),MC.UpdatedOn,0) AS UpdatedOn ,
			MC.IsActive ,
			ISNULL(MC.ProcurementLeads,0) AS ProcurementLeads,
			ISNULL(MC.LeadsSent,0) AS LeadsSend
			FROM DCRM_MFCMappedCities MC(NOLOCK)
			INNER	JOIN Cities C(NOLOCK) ON C.ID = MC.CityID
			INNER	JOIN OprUsers OU(NOLOCK) ON OU.Id = MC.UpdatedBy   
			ORDER BY  IsActive DESC , City , UpdatedOn
END
