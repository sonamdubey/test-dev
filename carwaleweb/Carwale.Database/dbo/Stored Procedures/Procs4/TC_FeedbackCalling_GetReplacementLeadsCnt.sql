IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FeedbackCalling_GetReplacementLeadsCnt]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FeedbackCalling_GetReplacementLeadsCnt]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 26 Sep, 2016
-- Description : To fetch replacement lead count on the basis of lead dispositions
-- EXEC TC_FeedbackCalling_GetReplacementLeadsCnt 3838,null,null
-- =============================================
CREATE PROCEDURE [dbo].[TC_FeedbackCalling_GetReplacementLeadsCnt] 
	-- Add the parameters for the stored procedure here
	@DealerId INT,
	@FromDate DATETIME,
	@ToDate DATETIME
AS
BEGIN

	SET NOCOUNT ON; 
	   
	SET @ToDate = CONVERT(DATETIME, CONVERT(VARCHAR(10), @ToDate, 120) + ' 23:59:59')
	--LeadDispositions :- 66 : Purchase postponed,69 : Existing inquiry , 87 : Out Of Territory , 90 : Wrong Number
	DECLARE @Total INT,@Pending INT , @Processed INT,@Qualified INT
	SELECT 	@Total = COUNT(DISTINCT CASE WHEN TCIL.TC_LeadDispositionId IN(87,90,69,66) THEN TCIL.TC_InquiriesLeadId END) 
	FROM TC_InquiriesLead AS TCIL WITH(NOLOCK)
	JOIN TC_NewCarInquiries AS TCNI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId=TCNI.TC_InquiriesLeadId
	WHERE TCIL.BranchId=@DealerId AND TCIL.TC_LeadInquiryTypeId = 3 AND TCNI.CreatedOn BETWEEN @FromDate AND @TODate

	SELECT @Processed = COUNT(DISTINCT TFC.TC_InquiriesLeadId) 
	FROM TC_FeedbackCalling_Inquiries TFC WITH(NOLOCK)
	INNER JOIN TC_NewCarInquiries AS TCNI WITH(NOLOCK) ON TFC.TC_NewCarInquiriesId=TCNI.TC_NewCarInquiriesId
	WHERE TFC.OldDealerId = @DealerId
	AND TCNI.CreatedOn BETWEEN @FromDate AND @TODate


	SELECT @Qualified = COUNT(DISTINCT TFCI.TC_InquiriesLeadId) 
	FROM TC_FeedbackCalling_InqFeedback TFCI WITH(NOLOCK)
	INNER JOIN TC_FeedbackCalling_Inquiries AS TFC WITH(NOLOCK)  ON TFCI.TC_InquiriesLeadId =  TFC.TC_InquiriesLeadId
	INNER JOIN TC_NewCarInquiries AS TCNI WITH(NOLOCK) ON TFC.TC_NewCarInquiriesId=TCNI.TC_NewCarInquiriesId
	INNER JOIN TC_InquiriesLead TIL WITH (NOLOCK) ON TIL.TC_InquiriesLeadId = TCNI.TC_InquiriesLeadId
	WHERE TIL.BranchId=@DealerId AND TFCI.TC_FeedbackCalling_QuestionsId = 12 AND TFCI.TC_FeedbackCalling_AnswerId = 1 and TCNI.CreatedOn BETWEEN @FromDate AND @TODate

	SET @Pending = @Total - @Processed

	SELECT @Total Total,
	CASE WHEN @Pending > 0 THEN @Pending ELSE 0 END AS Pending 
	,@Processed Processed,@Qualified Qualified

END
