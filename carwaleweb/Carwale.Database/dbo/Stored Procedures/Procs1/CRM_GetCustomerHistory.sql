IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetCustomerHistory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetCustomerHistory]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 14 Apr 2014
-- Description:	To get CRM Customer Leads history (Current & Previous) & Follow Ups & Listed used cars
-- Execution:	EXEC CRM_GetCustomerHistory 15
-- =============================================
CREATE PROCEDURE [dbo].[CRM_GetCustomerHistory]
	-- Add the parameters for the stored procedure here
	@CRMCustomerId			NUMERIC(18,0)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	DECLARE 
		@LeadId	BIGINT = -1,
		@CallId BIGINT = -1

	--To get last connected call with the customer and its lead
	SELECT TOP 1 @LeadId = CC.LeadId, @CallId = CC.Id
	FROM CRM_Leads CL WITH (NOLOCK)
	JOIN CRM_Calls CC WITH (NOLOCK) ON CL.ID = CC.LeadId AND CC.IsActionTaken = 1	
	JOIN CRM_SubDisposition SD WITH (NOLOCK) ON CC.DispType = SD.Id --AND SD.IsConnected = 1
	WHERE CNS_CustId = @CRMCustomerId
	ORDER BY CC.ActionTakenOn DESC
	
	--If exist then fetch all the details for that lead
	IF @CallId <> -1 AND @LeadId <> -1
	BEGIN
		--Get last connected call details on that last connected call
		SELECT CC.Id AS CallId, CC.ActionTakenOn AS LastConnectedOn,
		OU.UserName AS SpokeTo, CC.ActionComments AS Comments,
		CC.LeadId
		FROM CRM_Calls CC WITH (NOLOCK)
		JOIN OprUsers OU WITH (NOLOCK) ON CC.ActionTakenBy = OU.Id
		WHERE CC.Id = @CallId

		--To get details of all the cars of that lead along with its assignment details
		SELECT CBD.ID AS CBDId, (CMK.Name + ' ' + CMO.Name + ' ' + CV.Name) AS CarName, 
		ISNULL(CBD.IsDealerAssigned,0) IsDealerAssigned, ISNULL(CBD.IsDeleted,0) IsDeleted,CBD.UpdatedOn CBDDate, SD.SubDisposition AS LastConnectedStatus,
		CDA.Id AS CDAId, CDA.UpdatedOn AS UpdatedDate, ND.Name AS DealerName,
		PQL.Id AS PQId, ISNULL(PQL.IsPQRequested,0) IsPQRequested, PQL.PQRequestDate, ISNULL(PQL.IsPQNotRequired,0) IsPQNotRequired,
		ISNULL(PQL.IsPQCompleted,0) IsPQCompleted, PQL.PQCompleteDate, PQL.PQCompletedEventOn,
		--CASE ISNULL(PQL.IsPQRequested,0) WHEN 0 THEN '' ELSE '' END
		CASE ISNULL(PQL.IsPQRequested,0) 
		WHEN 0 THEN '' 
		ELSE 
			CASE ISNULL(PQL.IsPQCompleted,0) 
			WHEN 1 THEN 'PQ completed on ' + CONVERT(VARCHAR(19),PQL.PQCompletedEventOn)
			ELSE 
				CASE ISNULL(PQL.IsPQNotRequired,0) 
				WHEN 1 THEN 'PQ not required tagged on ' + CONVERT(VARCHAR(19),PQL.PQCompletedEventOn)
				ELSE 'PQ requested but not completed' 
				END 
			END 
		END AS PQStatement,
		TDL.Id AS TDId, ISNULL(TDL.IsTDRequested,0) IsTDRequested, TDL.TDRequestDate, ISNULL(TDL.IsTDCompleted,0) IsTDCompleted, 		
		TDL.TDCompleteDate, TDL.TDCompletedEventOn, ISNULL(TDL.ISTDNotPossible,0) ISTDNotPossible, ISNULL(TDL.IsTDDirect,0) IsTDDirect, TDL.TDComment AS TDComments,
		CASE ISNULL(TDL.IsTDDirect,0) 
		WHEN 1 THEN 'TD completed directly on ' + CONVERT(VARCHAR(19),TDL.TDCompletedEventOn)
		ELSE 
			CASE ISNULL(TDL.IsTDRequested,0) 
			WHEN 0 THEN '' 
			ELSE 
				CASE ISNULL(TDL.IsTDCompleted,0) 
				WHEN 1 THEN 'TD completed on ' + CONVERT(VARCHAR(19),TDL.TDCompletedEventOn)
				ELSE 
					CASE ISNULL(TDL.ISTDNotPossible,0) 
					WHEN 1 THEN 'TD not possible tagged on ' + CONVERT(VARCHAR(19),TDL.TDCompletedEventOn)
					ELSE 'TD requested but not completed' 
					END 
				END 
			END 
		END AS TDStatement,
		BL.Id AS BLId, ISNULL(BL.IsBookingRequested,0) IsBookingRequested, BL.BookingRequestDate, ISNULL(BL.IsBookingCompleted,0) IsBookingCompleted, 
		BL.BookingCompleteDate, BL.BookingCompletedEventOn, ISNULL(BL.IsPriorBooking,0) IsPriorBooking, ISNULL(BL.IsBookingNotPossible,0) IsBookingNotPossible, BL.Comments AS BLComments,
		CASE ISNULL(BL.IsBookingRequested,0) 
		WHEN 0 THEN ''
		ELSE 
			CASE ISNULL(BL.IsPriorBooking,0) 
			WHEN 1 THEN 'Prior Booking' 
			ELSE 
				CASE ISNULL(BL.IsBookingNotPossible,0) 
				WHEN 1 THEN 'Booking not possible tagged on ' + CONVERT(VARCHAR(19),BL.BookingCompletedEventOn)
				ELSE 
					CASE ISNULL(BL.IsBookingCompleted,0) 
					WHEN 1 THEN 'Booking completed tagged on ' + CONVERT(VARCHAR(19),BL.BookingCompletedEventOn)
					ELSE '' 
					END 
				END
			END
		END AS BookingStatement
		FROM CRM_CarBasicData CBD WITH (NOLOCK)
		JOIN CarVersions CV WITH (NOLOCK) ON CBD.VersionId = CV.ID
		JOIN CarModels CMO WITH (NOLOCK) ON CV.CarModelId = CMO.ID
		JOIN CarMakes CMK WITH (NOLOCK) ON CMO.CarMakeId = CMK.ID
		LEFT JOIN CRM_CarDealerAssignment CDA WITH (NOLOCK) ON CBD.ID = CDA.CBDId
		LEFT JOIN NCS_Dealers ND WITH (NOLOCK) ON CDA.DealerId = ND.ID
		LEFT JOIN CRM_SubDisposition SD WITH (NOLOCK) ON CDA.LastConnectedStatus = SD.Id
		LEFT JOIN CRM_CarPQLog PQL WITH (NOLOCK) ON CBD.ID = PQL.CBDId
		LEFT JOIN CRM_CarTDLog TDL WITH (NOLOCK) ON CBd.ID = TDl.CBDId
		LEFT JOIN CRM_CarBookingLog BL WITH (NOLOCK) ON CBD.ID = BL.CBDId
		WHERE CBD.LeadId = @LeadId --AND CBD.IsDealerAssigned = 1 AND CBD.IsDeleted <> 1
		ORDER BY CDA.UpdatedOn DESC

		--SELECT CBD.ID AS CBDId, (CMK.Name + ' ' + CMO.Name + ' ' + CV.Name) AS CarName, 
		--CBD.IsDealerAssigned, CBD.IsDeleted
		--FROM CRM_CarBasicData CBD
		--JOIN CarVersions CV ON CBD.VersionId = CV.ID
		--JOIN CarModels CMO ON CV.CarModelId = CMO.ID
		--JOIN CarMakes CMK ON CMO.CarMakeId = CMK.ID
		--WHERE CBD.LeadId = @LeadId AND CBD.IsDealerAssigned = 0

		--To get follow ups for that lead
		SELECT TOP 10 OU.UserName AS ActionTakenBy, GRP.Name AS GroupName,
		CC.ActionTakenOn, CC.ActionComments
		FROM CRM_Calls CC WITH (NOLOCK)
		JOIN CRM_Leads CL WITH (NOLOCK) ON CC.LeadId = CL.ID
		JOIN CRM_ADM_FLCGroups GRP WITH (NOLOCK) ON CL.GroupId = GRP.Id
		JOIN OprUsers OU WITH (NOLOCK) ON CC.ActionTakenBy = OU.Id
		WHERE CL.ID = @LeadId AND
		CC.IsActionTaken = 1
		ORDER BY CC.ActionTakenOn DESC

		--To get used car listed by that customer along with respones on that car
		SELECT (CMK.Name + ' ' + CMO.Name + ' ' + CV.Name) AS CarName,
		CSI.ID AS SellInquiryId, CONVERT(CHAR(128), CSI.MakeYear, 107 ) AS MakeYear , CSI.Color, CT.Name AS RegCity, CSI.CarRegNo,CSI.ViewCount,
		CSI.IsPremium,COUNT(CR.Id) AS ResponseCount 
		FROM CRM_CrossSellInquiries CRSI WITH (NOLOCK)
		JOIN CustomerSellInquiries CSI WITH (NOLOCK) ON CRSI.SelInquiryId = CSI.ID
		JOIN CRM_Leads CL WITH (NOLOCK) ON CRSI.LeadId = CL.ID
		JOIN CRM_Customers CUS WITH (NOLOCK) ON CL.CNS_CustId = CUS.ID AND CUS.ID = @CRMCustomerId
		JOIN CarVersions CV WITH (NOLOCK) ON CSI.CarVersionId = CV.ID
		JOIN CarModels CMO WITH (NOLOCK) ON CV.CarModelId = CMO.ID
		JOIN CarMakes CMK WITH (NOLOCK) ON CMO.CarMakeId = CMK.ID
		JOIN Cities CT WITH (NOLOCK) ON CSI.CityId = CT.ID
		LEFT JOIN ClassifiedRequests CR ON CSI.ID = CR.SellInquiryId
		GROUP BY (CMK.Name + ' ' + CMO.Name + ' ' + CV.Name),
		CSI.ID, CSI.MakeYear, CSI.Color, CT.Name, CSI.CarRegNo,CSI.ViewCount, CSI.IsPremium
	END
END
