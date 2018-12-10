IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_NewFetchDCProcessData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_NewFetchDCProcessData]
GO

	-- =============================================
-- Author:		<Deepak Tripathi>
-- Created date: <30/05/2014>
-- Description:	<Get DC queue Data to process>
-- =============================================
CREATE PROCEDURE [dbo].[CRM_NewFetchDCProcessData]

	@DCQueueDataId	NUMERIC
	
AS
BEGIN
	
	SELECT CDQ.CBDId, CDQ.PQStatus, CDQ.PQSubDisposition, ISNULL(CDQ.PQDate, GETDATE()) PQDate, CDQ.TDStatus, CDQ.TDSubDisposition, ISNULL(CDQ.TDDate, GETDATE())TDDate, 
		CDQ.BLStatus, CDQ.BLSubDisposition, ISNULL(CDQ.BLDate, GETDATE()) BLDate, CDQ.RegisterWith, CDQ.CarColor, CDQ.Invoice, CDQ.CreatedOn, CDQ.CreatedBy,
		
		ISNULL(PQ.IsPQRequested,0) IsPQRequested, ISNULL(PQ.IsPQCompleted,0) IsPQCompleted, ISNULL(PQ.IsPQNotRequired,0)IsPQNotRequired, PQ.PQNRDispositionId, PQ.PQRequestDate, PQ.PQCompleteDate,
		ISNULL(TD.IsTDRequested, 0) IsTDRequested, ISNULL(TD.IsTDCompleted,0) IsTDCompleted, ISNULL(TD.ISTDNotPossible, 0) ISTDNotPossible,
		ISNULL(TD.IsTDDirect,0)IsTDDirect, TD.TDNPDispositionId, TD.TDRequestDate, TD.TDCompleteDate, TD.TDComment,
		ISNULL(BL.IsBookingRequested,0) IsBookingRequested, ISNULL(BL.IsBookingCompleted,0) IsBookingCompleted, 
		ISNULL(BL.IsBookingNotPossible,0) IsBookingNotPossible, ISNULL(BL.IsPriorBooking,0) IsPriorBooking, BL.Comments, 
		ISNULL(BL.NIFeedback,0) NIFeedback, ISNULL(BL.NoFeedbackContact,0) NoFeedbackContact, 
		BL.BookingNPDispositionId, BL.BookingRequestDate, BL.BookingCompleteDate, BL.RegisterPersonName, BL.Color, CCI.InvoiceId AS PreInvoice,
		CL.ID AS LeadId, CL.Owner AS LeadOwner
	
	FROM CRM_DCQueueData CDQ WITH(NOLOCK)
		INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CBD.ID  = CDQ.CBDId
		INNER JOIN CRM_CarDealerAssignment CDA WITH (NOLOCK) ON CDA.CBDId = CBD.ID
		INNER JOIN CRM_Leads CL WITH (NOLOCK) ON CBD.LeadId = CL.ID 

		LEFT JOIN CRM_CarPQLog PQ WITH (NOLOCK) ON CBD.ID = PQ.CBDId
		LEFT JOIN CRM_CarTDLog TD WITH (NOLOCK) ON CBD.ID = TD.CBDId
		LEFT JOIN CRM_CarBookingLog BL WITH (NOLOCK) ON CBd.ID = Bl.CBDId
		LEFT JOIN CRM_CarInvoices CCI WITH (NOLOCK) ON CBd.ID = CCI.CBDId
	WHERE CDQ.Id = @DCQueueDataId
END

