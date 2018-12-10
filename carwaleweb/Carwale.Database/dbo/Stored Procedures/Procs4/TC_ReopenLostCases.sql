IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReopenLostCases]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReopenLostCases]
GO

	-- =============================================  
-- Author      : Nilima More
-- Create date : 10 December 2015
-- Details     : To Reopen Lost Cases.
-- exec TC_ReopenLostCases 16064,null,'13378,13379'
-- =============================================  
CREATE PROCEDURE [dbo].[TC_ReopenLostCases] 
	 @TC_LeadId BIGINT
	,@LeadOwnerId INT = NULL
	,@TC_NewCarInquiriesId VARCHAR(500) = NULL	
	,@LoggedUserId INT = NULL
AS
BEGIN
	DECLARE @TC_LeadStageId INT
		,@TC_LeadDispositionId INT
		,@TC_InquiryTypeId SMALLINT
		,@TC_InquiriesLeadId BIGINT
		,@BranchId NUMERIC
		,@TC_SubDispositionId SMALLINT
		,@TC_ActionApplicationId INT
		,@BookingStatus TINYINT
		,@TC_CustomerId INT
		,@TC_UsersId INT
		,@ScheduleDate DATETIME = GETDATE()
		--,@TC_CallsId INT

	SELECT @TC_LeadStageId = L.TC_LeadStageId
		,@TC_LeadDispositionId = NCI.TC_LeadDispositionId
		,@BranchId = L.BranchId
		,@TC_InquiriesLeadId = IL.TC_InquiriesLeadId
		,@TC_InquiryTypeId = IL.TC_LeadInquiryTypeId
		,@TC_SubDispositionId = NCI.TC_SubDispositionId
		,@TC_ActionApplicationId = NCI.TC_ActionApplicationId
		,@BookingStatus = NCI.BookingStatus
		,@TC_CustomerId = L.TC_CustomerId
		--,@TC_UsersId = c.TC_UsersId
		--,@TC_CallsId = C.TC_CallsId
	FROM TC_Lead L WITH (NOLOCK)
	INNER JOIN TC_InquiriesLead IL WITH (NOLOCK) ON IL.TC_LeadId = L.TC_LeadId
	INNER JOIN TC_NewCarInquiries NCI WITH (NOLOCK) ON NCI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
	INNER JOIN TC_CustomerDetails CD WITH (NOLOCK) ON CD.Id = L.TC_CustomerId
	-- INNER JOIN TC_Calls C WITH (NOLOCK) ON C.TC_LeadId = L.TC_LeadId
	WHERE L.TC_LeadStageId = 3 ---closed lead
		AND IL.TC_LeadInquiryTypeId = 3 ---New Car Inquiry
		AND NCI.TC_LeadDispositionId <> 4
		AND isnull(NCI.BookingStatus, 0) <> 32 ---Not Booked
		AND L.TC_LeadId = @TC_LeadId

	IF (
			@TC_LeadStageId = 3
			AND @TC_InquiryTypeId = 3
			AND @TC_LeadDispositionId <> 4
			AND isnull(@BookingStatus, 0) <> 32
			)
	BEGIN
		---To reopen lead with inquiry(TC_LeadStageId = 2 == Counsultation,TC_LeadDisposition == Reopen)
		UPDATE TC_Lead
		SET TC_LeadStageId = 2
			,TC_LeadDispositionId = 2
		WHERE TC_LeadId = @TC_LeadId

		UPDATE TC_InquiriesLead
		SET TC_LeadStageId = 2
			,TC_LeadDispositionID = NULL
		WHERE TC_InquiriesLeadId = @TC_InquiriesLeadId
			AND TC_LeadInquiryTypeId = 3

		UPDATE TC_NewCarInquiries
		SET TC_LeadDispositionReason = 'Reopen'
			,TC_LeadDispositionId = NULL
			,BookingStatus = NULL
			,BookingDate = NULL
		WHERE TC_NewCarInquiriesId IN (
				SELECT listmember
				FROM fnSplitCSV(@TC_NewCarInquiriesId)
				)

		UPDATE TC_CustomerDetails
		SET ActiveLeadId = @TC_LeadId
			,IsleadActive = 1
		WHERE id = @TC_CustomerId

		EXEC TC_ScheduleCall @LeadOwnerId,@TC_LeadId,3,@ScheduleDate,NULL,'Inquiry Added',NULL,NULL,NULL

		---Insert into Disposition Log
		INSERT INTO TC_DispositionLog (
			TC_LeadDispositionId
			,InqOrLeadId
			,TC_DispositionItemId
			,EventCreatedOn
			,EventOwnerId
			,TC_LeadId
			,LeadOwnerId
			,NewLeadOwnerId
			,TC_SubDispositionId
			,TC_ActionApplicationId
			,DispositionReason
			)
		VALUES (
			92
			,@TC_InquiriesLeadId
			,2
			,GETDATE()
			,@LoggedUserId
			,@TC_LeadId
			,@LeadOwnerId
			,NULL
			,@TC_SubDispositionId
			,@TC_ActionApplicationId
			,'Reopen'
			)
	END
END
