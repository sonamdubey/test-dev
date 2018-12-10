IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiryFollowpSave_12Apr]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiryFollowpSave_12Apr]
GO

	

-- Author:		Binumon George
-- Create date: 23 Jan 2012
-- Description:	This procedure is used to add Followupdetails like buyer and seller
-- =============================================
CREATE PROCEDURE [dbo].[TC_InquiryFollowpSave_12Apr]
(
	@CustId BIGINT,
	@TC_UserId INT,
	@NextFollowupDate DATETIME,
	@Comment VARCHAR(MAX),
	@TC_InquiryStatusId SMALLINT,
	@TC_InquiriesFollowupActionId SMALLINT,
	@InqType BIGINT
)
AS
BEGIN

	DECLARE @TC_InquiriesLeadId BIGINT
	SELECT @TC_InquiriesLeadId=TC_InquiriesLeadId  from TC_InquiriesLead
	WHERE TC_CustomerId=@CustId AND TC_InquiryTypeId=@InqType

	IF EXISTS (SELECT TOP 1 * FROM TC_InquiriesLead WHERE TC_InquiriesLeadId=@TC_InquiriesLeadId AND TC_InquiryTypeId=@InqType)
	BEGIN
		INSERT INTO TC_InquiriesFollowup(TC_InquiriesLeadId,TC_UserId ,FollowUpDate,NextFollowupDate , Comment , TC_InquiryStatusId,TC_InquiriesFollowupActionId)
		VALUES
		(@TC_InquiriesLeadId,@TC_UserId ,GETDATE(),@NextFollowupDate , @Comment , @TC_InquiryStatusId,@TC_InquiriesFollowupActionId)
		--Updating below table after follow up added. Means taken action aginist inquiry and follow up date
		UPDATE TC_InquiriesLead SET IsActionTaken=1, NextFollowUpDate= @NextFollowupDate, 
		TC_InquiriesFollowupActionId= @TC_InquiriesFollowupActionId,TC_InquiryStatusId=@TC_InquiryStatusId
		WHERE TC_InquiriesLeadId=@TC_InquiriesLeadId
	END
END



