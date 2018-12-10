IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FeedbackCalling_SaveInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FeedbackCalling_SaveInquiries]
GO

	
-- =============================================
-- Author		:	Kritika Choudhary
-- Date			:	28th Sep 2016
-- Description  :   To insert data inside TC_FeedbackCalling_Inquiries
-- Modified By  :   Chetan Navin on 30 Sep, 2016 (Added parameters @TC_NewCarInquiriesId,@TC_InquirySourceId,@CreatedBy,@TC_LeadDispositionId) 
-- Modified By  :   Kritika Choudhary on 30th Sep 2016, removed TC_LeadDisposition from parameter and insert query
-- =============================================
CREATE PROCEDURE [dbo].[TC_FeedbackCalling_SaveInquiries]
	@InquiriesLeadId INT,
	@NewCarInquiriesId INT,
	@InquirySourceId INT,
	@CreatedBy INT,
	@OldInquirySourceId INT,
	@OldDealerId INT,
	@VersionId INT,
	@DealerLeadDispositionId INT,
	@CityId INT,
	@DealerSubDispositionId INT,
	@DealerDispositionDate DATETIME,
	@LeadDispositionReason VARCHAR(200),
	@OriginalImgPath VARCHAR(250),
	@DMSScreenShotHostUrl VARCHAR(100),
	@DMSScreenShotStatusId INT,
	@DMSScreenShotUrl VARCHAR(100),
	@FeedbackCalling_InquiriesId INT = NULL OUTPUT
AS
BEGIN
SET @FeedbackCalling_InquiriesId=0
	INSERT INTO TC_FeedbackCalling_Inquiries(TC_InquiriesLeadId,EntryDate,TC_NewCarInquiriesId,TC_InquirySourceId,
	CreatedBy,OldInquirySourceId,OldDealerId,VersionId,DealerLeadDispositionId,CityId,DealerSubDispositionId,
	DealerDispositionDate,TC_LeadDispositionReason,OriginalImgPath,DMSScreenShotHostUrl,DMSScreenShotStatusId,DMSScreenShotUrl)
	VALUES(	@InquiriesLeadId, GETDATE(),@NewCarInquiriesId,@InquirySourceId,@CreatedBy,
	@OldInquirySourceId,@OldDealerId,@VersionId,@DealerLeadDispositionId,@CityId,@DealerSubDispositionId,
	@DealerDispositionDate,@LeadDispositionReason,@OriginalImgPath,@DMSScreenShotHostUrl,@DMSScreenShotStatusId,@DMSScreenShotUrl)
	
	if @@ROWCOUNT>0
	BEGIN
		SET @FeedbackCalling_InquiriesId = SCOPE_IDENTITY()
	END
	
END

