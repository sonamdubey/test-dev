IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_InquiryFollowup_Rejection]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_InquiryFollowup_Rejection]
GO

	CREATE Procedure [dbo].[NCD_InquiryFollowup_Rejection]
(
@InquiryId int,
@IsAccepted bit,
@RejectionId int,
@RejectionComment varchar(500),
@ret_val Int = null OUTPUT
)
as
if(@RejectionId IN(7,12,14))--Fake, Researching, Existing User
begin
	Update NCD_Inquiries set 
	IsActionTaken=1,IsAccepted=@IsAccepted,
	LeadStatusId =@RejectionId,
	RejectionId=@RejectionId,
	RejectionComment=@RejectionComment,
	IsReclaimReplacement = 1 where Id=@InquiryId
end
else
begin
	Update NCD_Inquiries set 
	IsActionTaken=1,IsAccepted=@IsAccepted,
	LeadStatusId =@RejectionId,
	RejectionId=@RejectionId,
	RejectionComment=@RejectionComment where Id=@InquiryId
end
