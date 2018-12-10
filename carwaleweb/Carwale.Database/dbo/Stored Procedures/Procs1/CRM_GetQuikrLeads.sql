IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetQuikrLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetQuikrLeads]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 24 Mar,2015
-- Description : To get quickr leads
-- EXEC CRM_GetQuikrLeads 
-- =============================================
CREATE PROCEDURE [dbo].[CRM_GetQuikrLeads] 
	@RequiredDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	SELECT CD.AdId, CD.Id CarId,C.Name AS City, A.Name AS Area,CASE WHEN CD.IsSurveyDone = 1 THEN 'Inspection Done' ELSE 'Inspection Pending' END AS 'Status of inspection',
		   CASE WHEN CD.IsSurveyDone = 1 THEN 'http://www.autobiz.in/absure/CarCertificate.aspx?carId=' + CONVERT(VARCHAR, CD.Id) ELSE '' END AS 'Report URL'
	FROM AbSure_CarDetails CD WITH(NOLOCK)
	LEFT JOIN Cities C WITH(NOLOCK) ON C.ID = CD.OwnerCityId
	LEFT JOIN Areas A WITH(NOLOCK) ON A.ID = CD.OwnerAreaId
	WHERE AdId IS NOT NULL
	AND CONVERT(DATE,EntryDate) = CONVERT(DATE,@RequiredDate)
	AND DealerId = 11392 
END
