IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FeedbackCalling_GetReplacedLeadsData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FeedbackCalling_GetReplacedLeadsData]
GO

	
--Author: Ruchira Patil On 28Th sEPT 2016
--Description: To Get replacement data
--Modified By : Nilima More on 6th oct 2016, added isnull check for int datatypes.
--==================================================
CREATE PROCEDURE [dbo].[TC_FeedbackCalling_GetReplacedLeadsData] --5,'2016/02/05','2016/10/06'
	@DealerId INT,
	@FromDate DATETIME,
	@ToDate DATETIME
AS
BEGIN
	SET @ToDate = CONVERT(DATETIME, CONVERT(VARCHAR(10), @ToDate, 120) + ' 23:59:59')
	
	--added By : Nilima More on 6th oct 2016, added isnull check for int datatypes.
	--Don't change the sequence of the columns that are being fetched,it will affect the code
	SELECT CustomerName,Email,Mobile,Address,CarDetails,ISNULL(VersionId,0) VersionId,ISNULL(ExistingNewCarInquiriesId,0) ExistingNewCarInquiriesId,ISNULL(OldInquirySourceId,0) OldInquirySourceId,
	ISNULL(OldDealerId,0) OldDealerId,ISNULL(DealerLeadDispositionId,0) DealerLeadDispositionId,ISNULL(CityId,0) CityId,
	ISNULL(DealerSubDispositionId,0)  DealerSubDispositionId,LeadDispositionReason,DealerDispositionDate,OriginalImgPath,DMSScreenShotHostUrl,
	ISNULL(DMSScreenShotStatusId,0) DMSScreenShotStatusId,DMSScreenShotUrl
	FROM
	(
		SELECT DISTINCT C.CustomerName,C.Email,C.Mobile,C.Address,
		TCIL.CarDetails,TCNI.VersionId,
		TCNI.TC_NewCarInquiriesId ExistingNewCarInquiriesId,TCNI.TC_InquirySourceId OldInquirySourceId,C.BranchId OldDealerId,
		TCNI.TC_LeadDispositionId DealerLeadDispositionId,TCNI.CityId CityId, TCNI.TC_SubDispositionId DealerSubDispositionId,TCNI.TC_LeadDispositionReason LeadDispositionReason,
		TCNI.DispositionDate DealerDispositionDate,
		TCNI.OriginalImgPath,TCNI.DMSScreenShotHostUrl,TCNI.DMSScreenShotStatusId,TCNI.DMSScreenShotUrl,
		ROW_NUMBER() OVER(PARTITION BY TCIL.TC_InquiriesLeadId ORDER BY TCNI.TC_NewCarInquiriesId DESC) AS RowNum

		FROM TC_CustomerDetails C WITH(NOLOCK)
		JOIN TC_Lead AS TCL WITH(NOLOCK) ON C.Id = TCL.TC_CustomerId
		JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TCL.TC_LeadId=TCIL.TC_LeadId
		JOIN TC_NewCarInquiries AS TCNI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId=TCNI.TC_InquiriesLeadId
		--JOIN TC_DispositionLog DL WITH (NOLOCK) ON DL.TC_LeadId = TCL.TC_LeadId
		LEFT JOIN TC_FeedbackCalling_Inquiries FCI WITH (NOLOCK) ON FCI.TC_NewCarInquiriesId = TCNI.TC_NewCarInquiriesId
		WHERE FCI.TC_NewCarInquiriesId IS NULL AND
		C.BranchId= @DealerId
		AND (TCIL.TC_LeadDispositionId IN(87,90,69,66))
		AND TCIL.TC_LeadInquiryTypeId = 3 
		AND TCNI.CreatedOn BETWEEN @FromDate AND @TODate	
	) AS T
	WHERE T.RowNum = 1;
END
