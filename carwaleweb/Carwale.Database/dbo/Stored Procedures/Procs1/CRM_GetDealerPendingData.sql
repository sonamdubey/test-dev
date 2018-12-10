IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetDealerPendingData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetDealerPendingData]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 10th Aug 2014
-- Description : To fetch pending tasks,contact points and pending customers data of dealer
-- Module      : New CRM
-- =============================================
CREATE PROCEDURE [dbo].[CRM_GetDealerPendingData] 
	@DealerId BIGINT,
	@PageNo   SMALLINT = 1,
	@PageSize TINYINT = 10,
	@Type     TINYINT
AS
BEGIN
   IF (@Type = 1)
   BEGIN
	--Pending Customers
		WITH CTE AS
		(
			SELECT DISTINCT ND.ID AS DealerId, ND.Name AS DealerName,CBD.ID AS CBDId, CL.ID AS LeadId,CUS.ID AS CustomerId,
			CUS.FirstName + ' ' + CUS.LastName AS CustomerName, CUS.Mobile AS CustomerMobile, CUS.Email AS CustomerEmail,
			VW.Car AS CarName, VW.Make, VW.Model, VW.Version,VW.MakeId AS MakeId,ISNULL(PQ.Id,-1) AS PQId, ISNULL(TD.Id, -1) AS TDId, 
			ISNULL(BL.Id, -1) AS BLId,PQ.PQRequestDate, TD.TDRequestDate,BL.BookingRequestDate,ISNULL(TD.IsTDCompleted,0) AS IsTDCompleted,
			ISNULL(PQ.IsPQCompleted,0) AS IsPQCompleted,ISNULL(PQ.IsPQNotRequired,0) AS IsPQNotRequired,
			ISNULL(TD.IsTDDirect,0) AS IsTDDirect,ISNULL(TD.ISTDNotPossible,0) AS ISTDNotPossible,
			ISNULL(BL.IsBookingCompleted,0) AS IsBookingCompleted,ISNULL(BL.IsBookingNotPossible,0) AS IsBookingNotPossible,
			ISNULL(BL.IsPriorBooking,0) AS IsPriorBooking,ISNULL(BL.IsBookingRequested,0) AS IsBookingRequested,TD.TDCompleteDate AS TDCompleteDate,
			PQ.PQCompleteDate AS PQCompleteDate,BL.BookingCompleteDate AS BookingCompleteDate,CCI.InvoiceId AS InvoiceId,BL.RegisterPersonName AS RegisteredName,
			BL.Color AS CarColor,PQ.PQNRDispositionId AS PQNRDispositionId,TD.TDNPDispositionId AS TDNPDispositionId,BL.BookingNPDispositionId BookingNPDispositionId
			,ROW_NUMBER() OVER (ORDER BY CBD.ID DESC) AS [RowNumber]
			FROM NCS_Dealers ND WITH (NOLOCK)
			JOIN CRM_CarDealerAssignment CDA WITH (NOLOCK) ON CDA.DealerId = ND.ID
			JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CBD.ID  = CDA.CBDId
			JOIN CRM_Leads CL WITH (NOLOCK) ON CBD.LeadId = CL.ID AND CL.LeadStageId = 2
			JOIN CRM_Customers CUS WITH (NOLOCK) ON CL.CNS_CustId = CUS.ID
			JOIN CRM_InterestedIn CII WITH (NOLOCK) ON CII.LeadId = CL.ID AND CII.ProductTypeId = 1
			JOIN vwMMV VW ON CBD.VersionId = VW.VersionId
			--LEFT JOIN CRM_CientPendingApprovals CPA ON CBD.ID = CPA.CBDId AND CPA.IsApproved = 0
			LEFT JOIN CRM_CarPQLog PQ WITH (NOLOCK) ON CBD.ID = PQ.CBDId 
			LEFT JOIN CRM_CarTDLog TD WITH (NOLOCK) ON CBD.ID = TD.CBDId 
			LEFT JOIN CRM_CarBookingLog BL WITH (NOLOCK) ON CBD.ID = Bl.CBDId 
			LEFT JOIN CRM_CarInvoices CCI WITH (NOLOCK) ON CBD.ID = CCI.CBDId
			WHERE ND.DealerType = 0 AND ND.Id = @DealerId
		)
		SELECT *,(SELECT COUNT(*) FROM CTE) AS TotalRows FROM CTE 
		WHERE RowNumber >  (@PageNo - 1) * @PageSize AND RowNumber <= @PageNo * @PageSize
		GROUP BY DealerId,DealerName,CBDId, LeadId,CustomerId,CustomerName, CustomerMobile,CustomerEmail,CarName,Make,Model,Version,
		MakeId,PQId,TDId,BLId,PQRequestDate,TDRequestDate,BookingRequestDate,IsTDCompleted,IsPQCompleted,IsPQNotRequired,IsTDDirect,
		ISTDNotPossible,IsBookingCompleted,IsBookingNotPossible,IsPriorBooking,IsBookingRequested,TDCompleteDate,PQCompleteDate,
		BookingCompleteDate,InvoiceId,RegisteredName,CarColor,PQNRDispositionId,TDNPDispositionId,BookingNPDispositionId,RowNumber
		ORDER BY RowNumber

		--Gets Invoices
		SELECT ID AS Value,MakeId,InvoiceNo AS Text 
		FROM CRM_ADM_Invoices WITH(NOLOCK) WHERE Status = 1 
		ORDER BY InvoiceNo

		--Get Dispositions
		SELECT Id AS Value, Name AS Text,EventType 
		FROM CRM_ETDispositions WITH(NOLOCK)
		WHERE IsActive = 1 ORDER BY Text

	END

	IF(@Type = 2)
	BEGIN
		--Contact Points
		SELECT DCP.ContactName AS Name,DCP.Designation,DCP.Mobile AS Contact1,DCP.AlternateMobile AS Contact2,CPT.Name AS CallFor,ND.Name AS DealerName,C.Name AS DealerCity 
		FROM NCS_Dealers ND WITH(NOLOCK) 
		LEFT JOIN NCS_DealerContactPoint DCP WITH(NOLOCK) ON ND.Id = DCP.DealerId 
		LEFT JOIN Cities C WITH(NOLOCK) ON C.ID = ND.CityId 
		LEFT JOIN CRM_ContactPointType CPT ON DCP.ContactPointType = CPT.Id  
		WHERE ND.ID = @DealerId
	
		--Pending Tasks
		SELECT TD.TotalRequest, TD.PQPending AS PriceQuote, TD.TDPending AS TestDrive, TD.BookingPending AS BookingConfirmation, TD.ActiveFBPending AS ActiveLeadFeedback,TD.InvoicePending 
		FROM CRM_DCTaskListData TD WITH (NOLOCK)
		WHERE TD.DealerId = @DealerId	

	END
	--Get particular dispositions
	--SELECT ETD.Id AS Id,ET.ItemId AS ItemId,ET.Type AS ItemType,ETD.EventType AS EventType 
	--FROM CRM_ETDispositions ETD WITH (NOLOCK)
	--JOIN CRM_CarETDispositions ET WITH (NOLOCK) ON ETD.Id = ET.DispositonId

	--To get PQ Status
	--SELECT EventType AS EventType,ItemId AS ItemId 
	--FROM CRM_EventLogs WITH(NOLOCK)
 --   WHERE EventType IN(6,36,43,44,49,55) ORDER BY Id DESC
END

