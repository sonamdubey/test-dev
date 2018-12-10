IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_UpdateDCTaskListData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_UpdateDCTaskListData]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Created date: <18/12/2013>
-- Description:	<Update DC Task List Data>
-- =============================================
CREATE PROCEDURE [dbo].[CRM_UpdateDCTaskListData]
AS
BEGIN
	DECLARE @tblDCTaskListData table (DealerId INT, TotalRequest INT, PQPending INT, TDPending INT, BookingPending INT, ActiveFBPending INT, ApprovalPending INT, InvoicePending INT, DCId INT)
		
	INSERT INTO @tblDCTaskListData (DealerId, TotalRequest, PQPending, TDPending, BookingPending, ActiveFBPending, ApprovalPending, InvoicePending, DCId)
	SELECT ND.ID AS DealerId,
	COUNT(CDA.CBDId) AS TotalRequest
	,SUM(CASE WHEN PQ.IsPQRequested = 1 AND ISNULL(PQ.IsPQCompleted,0) = 0 AND ISNULL(PQ.IsPQNotRequired,0) = 0 THEN 1 ELSE 0 END) AS PQPending
	,SUM(CASE WHEN TD.IsTDRequested = 1 AND ISNULL(TD.IsTDCompleted,0) = 0 AND ISNULL(TD.ISTDNotPossible,0) = 0 THEN 1 ELSE 0 END) AS TDPending
	,SUM(CASE WHEN Bl.IsBookingRequested = 1 AND ISNULL(Bl.IsBookingCompleted,0) = 0 AND ISNULL(BL.IsBookingNotPossible,0) = 0 AND ISNULL(BL.IsPriorBooking,0) = 0 THEN 1 ELSE 0 END) AS BookingPending
	,SUM(CASE WHEN (DATEDIFF(dd, CDA.LastConnectedStatusDate, GETDATE()) <= (CASE CII.ClosingProbability WHEN 1 THEN 5 WHEN 2 THEN 5 WHEN 3 THEN 15 ELSE 20 END)) THEN 1 ELSE 0 END) AS ActiveFBPending
	,COUNT(DISTINCT CPA.Id) AS ApprovalPending,
	SUM(CASE WHEN ISNULL(Bl.IsBookingCompleted,0) = 1 AND ISNULL(CCI.InvoiceId,0) = 0 THEN 1 ELSE 0 END) AS InvoicePending, DCD.Id
	FROM NCS_Dealers ND WITH (NOLOCK)
	JOIN CRM_CarDealerAssignment CDA WITH (NOLOCK) ON CDA.DealerId = ND.ID
	JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CBD.ID  = CDA.CBDId
	JOIN CRM_Leads CL WITH (NOLOCK) ON CBD.LeadId = CL.ID AND CL.LeadStageId = 2
	JOIN Cities CT WITH (NOLOCK) ON ND.CityId = CT.ID
	JOIN CRM_InterestedIn CII WITH (NOLOCK) ON CII.LeadId = CL.ID AND CII.ProductTypeId = 1
	JOIN CRM_ADM_DCDealers DCD ON ND.ID = DCD.DealerId
	JOIN OprUsers OU WITH (NOLOCK) ON DCD.DCID = OU.Id
	LEFT JOIN CRM_CientPendingApprovals CPA ON CBD.ID = CPA.CBDId AND CPA.IsApproved = 0
	LEFT JOIN CRM_CarPQLog PQ WITH (NOLOCK) ON CBD.ID = PQ.CBDId
	LEFT JOIN CRM_CarTDLog TD WITH (NOLOCK) ON CBD.ID = TD.CBDId
	LEFT JOIN CRM_CarBookingLog BL WITH (NOLOCK) ON CBd.ID = Bl.CBDId
	LEFT JOIN CRM_CarInvoices CCI WITH (NOLOCK) ON CBd.ID = CCI.CBDId
	WHERE ND.DealerType = 0
	GROUP BY ND.ID, ND.Name, CT.Name, OU.Id, OU.UserName, DCD.Id;

	MERGE INTO CRM_DCTaskListData cdt USING @tblDCTaskListData dtld
    ON cdt.DealerId = dtld.DealerId
	
	WHEN NOT MATCHED THEN
		INSERT VALUES(dtld.DealerId, dtld.TotalRequest, dtld.PQPending, dtld.TDPending, dtld.BookingPending, dtld.ActiveFBPending, dtld.ApprovalPending, dtld.InvoicePending)
	WHEN MATCHED THEN
		UPDATE SET cdt.TotalRequest = dtld.TotalRequest, cdt.PQPending = dtld.PQPending,
		cdt.TDPending = dtld.TDPending, cdt.BookingPending = dtld.BookingPending, cdt.ActiveFBPending = dtld.ActiveFBPending,
		cdt.ApprovalPending = dtld.ApprovalPending, cdt.InvoicePending = dtld.InvoicePending;
	
	--Insert Calls
	DECLARE @NumberRecords AS INT
	DECLARE @RowCount AS INT
	DECLARE @DealerId AS INT
	DECLARE @ExecutiveId AS INT
	DECLARE @ScheduleDate AS DATETIME

	DECLARE @tblDCCallData table (RowID INT IDENTITY(1, 1), DealerId INT, ExecutiveId INT)	
	INSERT INTO @tblDCCallData
	SELECT DISTINCT DealerId, DCId FROM @tblDCTaskListData WHERE DealerId NOT IN(SELECT DealerId FROM CRM_DCActiveCalls)
	
	SET @NumberRecords = @@ROWCOUNT
	SET @RowCount = 1
	SET @ScheduleDate = GETDATE()

	WHILE @RowCount <= @NumberRecords
		BEGIN
			SELECT @DealerId = DealerId, @ExecutiveId = ExecutiveId FROM @tblDCCallData WHERE RowID = @RowCount
			EXEC CRM_DCScheduleNewCall @DealerId, 1, @ExecutiveId, @ScheduleDate, 'New Call', -1, 0
			SET @RowCount = @RowCount + 1
		END
END


