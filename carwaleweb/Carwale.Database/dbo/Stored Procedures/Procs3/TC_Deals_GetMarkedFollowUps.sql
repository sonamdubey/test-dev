IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_GetMarkedFollowUps]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_GetMarkedFollowUps]
GO

	
-- =============================================
-- Author:		Vinayak
-- Create date: 07/01/2016
-- Description:	Getting the follow up calls
-- EXEC [dbo].[TC_Deals_GetMarkedFollowUps]
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_GetMarkedFollowUps]
	-- Add the parameters for the stored procedure here
	@UserId INT = - 1
AS
BEGIN
	SELECT DISTINCT TA.ScheduledOn AS [ScheduleAt]
		,TC.CustomerName AS [Name]
		,TC.Mobile AS [ContactNumber]
		,[Role] = CASE 
			WHEN TA.NextCallTo = 1
				THEN 'DEALER EXECUTIVE'
			WHEN TA.NextCallTo = 2
				THEN 'USER'
			END
		,[Organization] = CASE 
			WHEN TA.NextCallTo = 1
				THEN D.Organization
			WHEN TA.NextCallTo = 2
				THEN ''
			END
		,TA.LastCallComment AS [LastComment]
		,TA.LastCallDate AS [LastContactedAt]
		,[Actions] = CASE 
			WHEN TA.NextCallTo = 1
				THEN 'Call Dealer (DCRM)'
			WHEN TA.NextCallTo = 2
				THEN 'CALL USER(AB)'
			END
		,D.ID AS [DealerId]
		,TN.CityId AS [CityId]
		,TI.TC_InquiriesLeadId AS [LeadId]
		,TI.TC_CustomerId AS [CustomerId]
		,TI.TC_UserId AS [UserId]
		,[Subject] = ''
	FROM TC_NewCarInquiries TN WITH (NOLOCK)
	INNER JOIN TC_InquiriesLead TI WITH (NOLOCK) ON TN.TC_InquiriesLeadId = TI.TC_InquiriesLeadId
	INNER JOIN TC_CustomerDetails TC WITH (NOLOCK) ON TI.TC_CustomerId = TC.Id
	INNER JOIN TC_ActiveCalls TA WITH (NOLOCK) ON TA.TC_LeadId = TI.TC_LeadId
	INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = TI.BranchId
	INNER JOIN TC_Deals_Dealers TDD WITH (NOLOCK) ON D.ID = TDD.DealerId
		AND TDD.IsDealerDealActive = 1
	INNER JOIN DCRM_ADM_UserDealers DAU WITH (NOLOCK) ON DAU.DealerId = D.ID
		AND (
			DAU.UserId = @UserId
			OR @UserId = - 1
			)
		AND DAU.RoleId = 7
	LEFT JOIN TC_Deals_StockVIN VIN WITH (NOLOCK) ON TN.TC_DealsStockVINId = VIN.TC_DealsStockVINId
	LEFT JOIN TC_Deals_Stock STK WITH (NOLOCK) ON VIN.TC_Deals_StockId = STK.Id
	LEFT JOIN TC_Deals_StockStatus STKS WITH (NOLOCK) ON VIN.STATUS = STKS.Id
	WHERE TN.TC_InquirySourceId = 134
		AND STKS.Id NOT IN (
			4
			,5
			,8
			,9
			,10
			)
		AND D.TC_DealerTypeId IN (
			2
			,3
			)
		AND D.IsDealerActive = 1
		AND TA.CallType NOT IN (
			1
			,2
			)
	
	UNION
	
	SELECT DISTINCT DC.ScheduleDate AS [ScheduleAt]
		,[Name] = ''
		,[ContactNumber] = ''
		,[Role] = ''
		,D.Organization AS [Organization]
		,DC.Comments AS [LastComment]
		,DC.LastCallDate AS [LastContactedAt]
		,[Actions] = 'Call Dealer (DCRM)'
		,D.ID AS [DealerId]
		,[CityId] = NULL
		,[LeadId] = NULL
		,[CustomerId] = NULL
		,DC.UserId AS [UserId]
		,DC.Subject AS [Subject]
	FROM DCRM_Calls DC WITH (NOLOCK)
	INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = DC.DealerId
	INNER JOIN DCRM_ADM_UserDealers DAU WITH (NOLOCK) ON DAU.DealerId = D.ID
		AND (
			DAU.UserId = @UserId
			OR @UserId = - 1
			)
		AND DAU.RoleId = 7
	INNER JOIN TC_Deals_Dealers TDD WITH (NOLOCK) ON D.ID = TDD.DealerId
		AND TDD.IsDealerDealActive = 1
	WHERE DC.ActionTakenId = 2
		AND D.TC_DealerTypeId IN (
			2
			,3
			)
		AND D.IsDealerActive = 1
END
