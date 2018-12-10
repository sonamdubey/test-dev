IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetCallsOfDisEngageDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetCallsOfDisEngageDealers]
GO
	-- =============================================
-- Author	:	Sachin Bharti(8th Jan 2014)
-- Description	:	Get calls data for Disengaged dealers for back office users only
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetCallsOfDisEngageDealers] --3
	@UserId		INT
AS
BEGIN
	
	SET NOCOUNT ON;
		
	SELECT  DISTINCT DC.Id AS CallId, DC.DealerId, DC.ScheduleDate, DC.LastCallDate,ISNULL(DSD.LeadStatus,0) As IsActive,ISNULL(DAT.AlertType,'')AS AlertType,
	D.ExpiryDate AS PackageExpiryDate,DC.Subject, OU.UserName, D.Organization AS Dealer, C.Name AS City, D.MobileNo ,D.ContactPerson 
	,CASE ISNULL(D.Status,0) WHEN 'False' THEN 1 ELSE 0 END AS Status, DC.CallType AS CallType, DCT.Name AS CallTypeDesc, DC.LastComment
	, DTY.Name AS RenewalType,TD.DealerType,CASE ISNULL(D.IsDealerDeleted,0) WHEN 'True' THEN 1 ELSE 0 END AS IsDealerDeleted,
	CASE
		WHEN DED.IsTillDayRenewal = 1				THEN	'B1'  
		WHEN DED.IsTillDayToNextMonthRenewal = 1	THEN	'B2' 
		WHEN DED.IsLastMonthRenewal = 1				THEN	'B3' 
		WHEN DED.IsLastMonthRenewal = 0 AND DED.IsTillDayToNextMonthRenewal = 0 AND DED.IsTillDayRenewal = 0  THEN 'B4' END AS BucketType,
	CASE 
		WHEN DED.IsLastMonthRenewal = 0 AND DED.IsTillDayToNextMonthRenewal = 0 AND DED.IsTillDayRenewal = 0 THEN 
			(CONVERT(VARCHAR,'1')+CONVERT(VARCHAR,'1')+CONVERT(VARCHAR,'1')+CONVERT(VARCHAR,DED.IsGetResponse)+
			CONVERT(VARCHAR,DED.IsCarUploaded)+CONVERT(VARCHAR,DED.IsSuccessfullFieldVisit)+CONVERT(VARCHAR,DED.IsCallConnected))
		ELSE 
			(CONVERT(VARCHAR,DED.IsLastMonthRenewal)+CONVERT(VARCHAR,DED.IsTillDayToNextMonthRenewal)+CONVERT(VARCHAR,DED.IsTillDayRenewal)+
			CONVERT(VARCHAR,DED.IsGetResponse)+CONVERT(VARCHAR,DED.IsCarUploaded)+CONVERT(VARCHAR,DED.IsSuccessfullFieldVisit)+CONVERT(VARCHAR,DED.IsCallConnected)) END AS NewOne
	FROM DCRM_Calls DC (NOLOCK) 
	INNER JOIN  Dealers D (NOLOCK) ON DC.DealerId = D.ID 
	INNER JOIN	DCRM_DisEngagedDealers DED ON DED.DealerId = D.Id
	INNER JOIN	dcrm_adm_userdealers DAU ON DAU.DealerId = D.Id
	LEFT JOIN DCRM_AlertTypes DAT ON DAT.Id = DC.AlertId 
	LEFT JOIN DCRM_SalesDealer DSD ON D.ID = DSD.DealerId AND DSD.LeadStatus = 1  
	LEFT JOIN DCRM_ADM_DealerTypes DTY ON DSD.DealerType = DTY.Id 
	INNER JOIN Cities AS C WITH (NOLOCK) ON C.ID = D.CityId  
	INNER JOIN OprUsers AS OU WITH (NOLOCK) ON DC.UserId = OU.Id 
	LEFT JOIN DCRM_CallTypes DCT WITH (NOLOCK) ON DCT.Id = DC.CallType 
	LEFT JOIN TC_DealerType TD WITH (NOLOCK) ON TD.TC_DealerTypeId = D.TC_DealerTypeId 
	WHERE CONVERT(DATE,DC.ScheduleDate) <= CONVERT(DATE,GETDATE())
	AND DC.UserId = @UserId AND ActionTakenId = 2 AND DAU.RoleId = 4 -- For Back office users only
	ORDER BY (	
		CASE WHEN DED.IsLastMonthRenewal = 0 AND DED.IsTillDayToNextMonthRenewal = 0 AND DED.IsTillDayRenewal = 0 THEN 
		(CONVERT(VARCHAR,'1')+CONVERT(VARCHAR,'1')+CONVERT(VARCHAR,'1')+CONVERT(VARCHAR,DED.IsGetResponse)+CONVERT(VARCHAR,DED.IsCarUploaded)+CONVERT(VARCHAR,DED.IsSuccessfullFieldVisit)+CONVERT(VARCHAR,DED.IsCallConnected))
		ELSE 
		(CONVERT(VARCHAR,DED.IsLastMonthRenewal)+CONVERT(VARCHAR,DED.IsTillDayToNextMonthRenewal)+CONVERT(VARCHAR,DED.IsTillDayRenewal)+
		CONVERT(VARCHAR,DED.IsGetResponse)+CONVERT(VARCHAR,DED.IsCarUploaded)+CONVERT(VARCHAR,DED.IsSuccessfullFieldVisit)+CONVERT(VARCHAR,DED.IsCallConnected)) END
	) ASC
END
