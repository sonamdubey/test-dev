IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_InquiryFollowup_Insert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_InquiryFollowup_Insert]
GO

	
CREATE Proc [dbo].[NCD_InquiryFollowup_Insert]
(
@InquiryId int,
@CustomerId int,
@NextCallTime datetime,
@LeadStatusId int,
@Comment varchar(500),
@IsAccepted bit,
@ExecId int,
@PrefTdDate datetime = null
)
as
Declare @Id int
Insert Into NCD_InquiryFollowup
(InquiryId,CustomerId,LastCallTime,NextCallTime,LeadStatusId,Comment,TestDriveDate,AssignedExecutive)
Values(@InquiryId,@CustomerId,GETDATE(),@NextCallTime,@LeadStatusId,@Comment,@PrefTdDate,@ExecId)
set @Id=@@IDENTITY
Update NCD_Inquiries set NextCallTime=@NextCallTime,
LeadStatusId=@LeadStatusId,LatestActionDesc=@Comment,
IsActionTaken=1,IsAccepted=@IsAccepted,AssignedExecutive=@ExecId where Id=@InquiryId
select @Id

