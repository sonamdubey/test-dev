IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FeedbackCalling_UpdateInqFeedBack]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FeedbackCalling_UpdateInqFeedBack]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 26 Sep, 2016
-- Description : To update feedbackcalling inquiry
-- =============================================
CREATE PROCEDURE [dbo].[TC_FeedbackCalling_UpdateInqFeedBack]  
	@TC_FeedbackCalling_InquiriesId INT,
	@TC_LeadDispoition INT
AS
BEGIN
	BEGIN
		UPDATE TC_FeedbackCalling_Inquiries 
			SET TC_LeadDispositionId = @TC_LeadDispoition
			WHERE TC_FeedbackCalling_InquiriesId = @TC_FeedbackCalling_InquiriesId ;
	END
END




