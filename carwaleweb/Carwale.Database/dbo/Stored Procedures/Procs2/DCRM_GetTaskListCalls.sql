IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetTaskListCalls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetTaskListCalls]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(7th Jan 2014)
-- Description	:	Get all call data for DCRM Tasklist
-- Modified by :	Sachin Bharti(22th July 2014) Added two parameters for DealerId and Dealer IsActive
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetTaskListCalls] 
	
	@UserId		INT,
	@ScheduleDate	DATETIME = NULL,
	@DealerID	INT = NULL,
	@IsActive	VARCHAR(5) = NULL
	
AS
	BEGIN
		
		--SET NOCOUNT ON;
	
		SELECT DISTINCT DC.Id AS CallId, DC.DealerId, DC.ScheduleDate, DC.LastCallDate,ISNULL(DSD.LeadStatus,0) As IsActive,ISNULL(DAT.AlertType,'')AS AlertType,
		D.ExpiryDate AS PackageExpiryDate,DC.Subject, OU.UserName, D.Organization AS Dealer, C.Name AS City, D.MobileNo ,D.ContactPerson,
		CASE ISNULL(D.Status,0) WHEN 'False' THEN 1 ELSE 0 END AS Status,
		CASE ISNULL(D.Status,0) WHEN 'FALSE' THEN 'Active' ELSE 'InActive' END AS CallStatus,
		DC.CallType AS CallType, DCT.Name AS CallTypeDesc, DC.LastComment
		, DTY.Name AS RenewalType,TD.DealerType,CASE ISNULL(D.IsDealerDeleted,0) WHEN 'True' THEN 1 ELSE 0 END AS IsDealerDeleted
		FROM DCRM_Calls  AS DC WITH (NOLOCK) 
		INNER JOIN  Dealers AS D WITH (NOLOCK) ON DC.DealerId = D.ID 
		LEFT JOIN DCRM_AlertTypes DAT WITH (NOLOCK)   ON DAT.Id = DC.AlertId 
		LEFT JOIN DCRM_SalesDealer DSD WITH (NOLOCK)  ON D.ID = DSD.DealerId AND DSD.LeadStatus = 1  
		LEFT JOIN DCRM_ADM_DealerTypes DTY WITH (NOLOCK)   ON DSD.DealerType = DTY.Id 
		INNER JOIN Cities AS C WITH (NOLOCK) ON C.ID = D.CityId  
		INNER JOIN OprUsers AS OU WITH (NOLOCK) ON DC.UserId = OU.Id 
		LEFT JOIN DCRM_CallTypes DCT WITH (NOLOCK) ON DCT.Id = DC.CallType 
		LEFT JOIN TC_DealerType TD WITH (NOLOCK) ON TD.TC_DealerTypeId = D.TC_DealerTypeId 
		WHERE UserId = @UserId AND 
		DC.ScheduleDate <= GETDATE()
		--( CONVERT(Date, DC.ScheduleDate) <= CONVERT(Date,GETDATE()))
		AND  ActionTakenId = 2
		AND (@DealerID IS NULL OR  DC.DealerId = @DealerID)
		AND (@IsActive IS NULL OR D.Status = @IsActive)
		ORDER BY DC.ScheduleDate Desc
	END


