IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_ExecMeetingDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_ExecMeetingDetails]
GO

	-- =============================================
-- Author	:	Sachin Bharti(25th March 2014)
-- Description	:	Get FieldExecutive meeting details
--Modified By:Komal Manjare on(1-December-2015)
--Desc:New parameters of Decision makers fetched
-- =============================================

CREATE PROCEDURE [dbo].[DCRM_ExecMeetingDetails] 
	
	@DealerType	INT,
	@DAY	INT = NULL,
	@MONTH	INT,
	@YEAR	INT,
	@OprUserId	INT = NULL,
	@StateId	INT = NULL,
	@CityId		INT = NULL,
	@DealerId	NUMERIC(18,0) = NULL,
	@MeetingMode	SMALLINT = NULL
AS
BEGIN
	
	SET NOCOUNT ON;

	IF @DealerType = 1
		BEGIN
			SELECT D.Organization,OU.UserName,DSM.MeetingDate,D.MobileNo,C.Name AS CityName , DSM.ActionComments ,DSM.ActionTakenOn ,
			DSM.DecisionMakerName,DSM.DecisionMakerDesignation,DSM.DecisionMakerPhoneNo,DSM.DecisionMakerEmail,
			CASE WHEN ISNULL(DSM.MeetDecisionMaker,0) = 1 THEN 'Yes'  WHEN ISNULL(DSM.MeetDecisionMaker,0) = 0 THEN 'No' END AS MeetDescisionMaker ,
			CASE WHEN ISNULL(DSM.MeetingType,0) = 1 THEN 'Sales Meeting' WHEN ISNULL(DSM.MeetingType,0) = 2 THEN  'ServiceMeeting' END AS MeetingType, 
			DRD.Description AS RateMeeting ,DMM.Name AS MeetingMode,
			CASE WHEN ISNULL(DSM.DealerType,0) = 1 THEN 'SalesDealer' WHEN ISNULL(DSM.MeetingType,0) = 2 THEN  'OEMDealer' END AS DealerType 
			FROM DCRM_SalesMeeting DSM(NOLOCK)  
			INNER JOIN Dealers D (NOLOCK) ON D.ID = DSM.DealerId  
			INNER JOIN Cities C (NOLOCK) ON C.ID = D.CityId  
			INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = DSM.ActionTakenBy 
			LEFT JOIN DCRM_RateDealerMeeting DRD(NOLOCK) ON DRD.ID = DSM.RateDealerMeeting 
			LEFT JOIN DCRM_MeetingModes DMM(NOLOCK) ON DMM.Id = DSM.MeetingMode
			WHERE 
			MONTH(DSM.MeetingDate) = @MONTH AND YEAR(DSM.MeetingDate) = @YEAR AND
			(@DAY IS NULL OR DAY(DSM.MeetingDate) = @DAY) AND 
			DSM.DealerType = 1	AND 
			(@OprUserId IS NULL OR DSM.ActionTakenBy = @OprUserId) AND
			(@StateId IS NULL OR D.StateId = @StateId)	AND
			(@CityID IS NULL OR D.CityId = @CityID)		AND
			(@DealerId IS NULL OR D.ID = @DealerId)		AND
			(@MeetingMode IS NULL OR DSM.MeetingMode = @MeetingMode)
			ORDER BY DSM.MeetingDate DESC 
		END
	IF @DealerType = 2
		BEGIN
			SELECT NCD.Name AS Organization,OU.UserName,DSM.MeetingDate,NCD.Mobile AS MobileNo,C.Name AS CityName , DSM.ActionComments ,DSM.ActionTakenOn ,  
			DSM.DecisionMakerName,DSM.DecisionMakerDesignation,DSM.DecisionMakerPhoneNo,DSM.DecisionMakerEmail,			
			CASE WHEN ISNULL(DSM.MeetDecisionMaker,0) = 1 THEN 'Yes'  WHEN ISNULL(DSM.MeetDecisionMaker,0) = 0 THEN 'No' END AS MeetDescisionMaker ,
			CASE WHEN ISNULL(DSM.MeetingType,0) = 1 THEN 'Sales Meeting' WHEN ISNULL(DSM.MeetingType,0) = 2 THEN  'ServiceMeeting' END AS MeetingType, 
			DRD.Description AS RateMeeting ,DMM.Name AS MeetingMode,
			CASE WHEN ISNULL(DSM.DealerType,0) = 1 THEN 'SalesDealer' WHEN ISNULL(DSM.MeetingType,0) = 2 THEN  'OEMDealer' END AS DealerType 
			FROM DCRM_SalesMeeting DSM(NOLOCK)  
			INNER JOIN NCS_Dealers NCD (NOLOCK) ON NCD.ID = DSM.DealerId  
			INNER JOIN Cities C (NOLOCK) ON C.ID = NCD.CityId 
			INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = DSM.ActionTakenBy 
			LEFT JOIN DCRM_RateDealerMeeting DRD(NOLOCK) ON DRD.ID = DSM.RateDealerMeeting 
			LEFT JOIN DCRM_MeetingModes DMM(NOLOCK) ON DMM.Id = DSM.MeetingMode
			WHERE 
			MONTH(DSM.MeetingDate) = @MONTH AND YEAR(DSM.MeetingDate) = @YEAR AND
			(@DAY IS NULL OR DAY(DSM.MeetingDate) = @DAY) AND 
			DSM.DealerType = 2 AND 
			(@OprUserId IS NULL OR DSM.ActionTakenBy = @OprUserId) AND
			(@StateId IS NULL OR C.StateId = @StateId)	AND
			(@CityID IS NULL OR NCD.CityId = @CityID)	AND
			(@DealerId IS NULL OR NCD.ID = @DealerId)	AND
			(@MeetingMode IS NULL OR DSM.MeetingMode = @MeetingMode)
			ORDER BY DSM.MeetingDate DESC 
		END
END
