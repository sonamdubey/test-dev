IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetConvertedDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetConvertedDealers]
GO
	-- =============================================
-- Author	:	Sachin Bharti(4th April 2014)
-- Description	:	Get Package details and dealer details of converted dealers
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetConvertedDealers]
	
	@StateId	INT = NULL,
	@CityId		INT = NULL,
	@DealerId	NUMERIC(18,0) = NULL

AS
BEGIN
	
	SET NOCOUNT ON;

    SELECT DISTINCT D.ID AS DealerId, DSD.PitchingProduct, PK.Name AS PackageName,PK.Id AS PackageId,
	D.Organization ,DSD.PitchDuration,DSD.EntryDate,DSD.ClosingDate ,IPC.Id AS InqPntId,
	DSD.ClosingAmount , DSD.EntryDate ,DSD.ClosingProbability,DSD.DealerType,
	C.ID AS CityId , D.StateId AS StateId,DSM.DealerType ,
	CASE WHEN DSM.DealerType = 1 THEN 'SalesDealer' WHEN DSM.DealerType = 2 THEN 'OEMDealers' END AS MeetingType,
	DSD.ClosingDate , OU.UserName AS ActionTakenBy,C.Name AS CityName
	FROM DCRM_SalesDealer DSD(NOLOCK)
	INNER JOIN Dealers D(NOLOCK) ON D.ID = DSD.DealerId
	INNER JOIN DCRM_SalesMeeting DSM(NOLOCK) ON DSM.DealerId = DSD.DealerId 
	AND DSD.ClosingProbability = 100 AND DSD.ClosedMeetingId IS NOT NULL 
	INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = DSM.ActionTakenBy
	INNER JOIN Packages PK(NOLOCK) ON PK.Id = DSD.PitchingProduct
	INNER JOIN InquiryPointCategory IPC(NOLOCK) ON IPC.Id = PK.InqPtCategoryId AND IPC.GroupType IN(2,3,4)
	INNER JOIN Cities C (NOLOCK) ON C.ID = D.CityId
	WHERE 
	DSD.LeadStatus = 2 AND D.Status = 1 ANd
	(@CityId IS NULL OR D.CityId = @CityId )
	AND (@StateId IS NULL OR D.StateId = @StateId )
	AND (@DealerId IS NULL OR D.ID = @DealerId)

END
