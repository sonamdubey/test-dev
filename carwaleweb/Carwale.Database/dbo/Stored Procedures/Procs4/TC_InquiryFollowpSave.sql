IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiryFollowpSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiryFollowpSave]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Modifiedby: Binu Date: 21 jun,2012 
--Desc:lastfollowupdate and comments updated in inqleqd table
-- Author:		Binumon George
-- Create date: 23 Jan 2012
-- Description:	This procedure is used to add Followupdetails like buyer and seller
-- =============================================
CREATE PROCEDURE [dbo].[TC_InquiryFollowpSave]
(
	@LeadId BIGINT,
	@TC_UserId INT,
	@NextFollowupDate DATETIME,
	@Comment VARCHAR(MAX),
	@TC_InquiryStatusId SMALLINT,
	@TC_InquiriesFollowupActionId SMALLINT
)
AS
BEGIN

	IF EXISTS (SELECT TOP 1 * FROM TC_InquiriesLead WHERE TC_InquiriesLeadId=@LeadId)
	BEGIN
		INSERT INTO TC_InquiriesFollowup(TC_InquiriesLeadId,TC_UserId ,FollowUpDate,NextFollowupDate , Comment , TC_InquiryStatusId,TC_InquiriesFollowupActionId)
		VALUES
		(@LeadId,@TC_UserId ,GETDATE(),@NextFollowupDate , @Comment , @TC_InquiryStatusId,@TC_InquiriesFollowupActionId)
		--Updating below table after follow up added. Means taken action aginist inquiry and follow up date
		UPDATE TC_InquiriesLead SET IsActionTaken=1, NextFollowUpDate= @NextFollowupDate,LastFollowUpDate=GETDATE(), 
		TC_InquiriesFollowupActionId= @TC_InquiriesFollowupActionId,TC_InquiryStatusId=@TC_InquiryStatusId,LastFollowUpComment=@Comment
		WHERE TC_InquiriesLeadId=@LeadId
	END
END

