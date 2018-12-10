IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ProcessBuyerInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ProcessBuyerInquiries]
GO

	CREATE procedure [dbo].[ProcessBuyerInquiries] 
(
@CustId BIGINT,
@AssignedTo bigint
)
as
-- getting all inquiry for i/p customer and adding id into @TblPurInq
declare @TblPurInq table (id int identity(1,1) ,InqId BIGINT)
insert into @TblPurInq(InqId) 
select id from TC_PurchaseInquiries P WITH(NOLOCK)
where P.CustomerId=@CustId Order by P.RequestDateTime asc

declare @rowCount smallint,@loopCount smallint=1
select @rowCount=COUNT(*) from @TblPurInq

declare @LeadId BIGINT

while @rowCount>=@loopCount -- loop through all inquiries
	begin
		declare @InqId BIGINT--,@AssignedTo bigint
		select @InqId=InqId from @TblPurInq	WHERE ID=@loopCount
		
		declare @BranchId bigint,@StockId bigint,@Comments varchar(800) --InterestedIn
		declare @InquiryStatusId tinyint ,@SourceId tinyint,@NextFollowUpDate Datetime,@RequestDateTime datetime
		declare @IsActionTaken BIT,@FollowupComment VARCHAR(800)
		
		
		
		-- fetching all details for this inquiry
		SELECT @BranchId=BranchId,@StockId=StockId,@Comments=Comments,@InquiryStatusId=InquiryStatusId,
				@SourceId=SourceId,@NextFollowUpDate=FollowUp,@RequestDateTime=RequestDateTime,@IsActionTaken=IsActionTaken,
				@FollowupComment=FollowupComment
				from TC_PurchaseInquiries WITH(NOLOCK) where id=@InqId and StockId is not null
		
		-- checking to avoid duplication		
		IF NOT EXISTS(SELECT CustomerId FROM TC_Inquiries WITH(NOLOCK) WHERE CustomerId=@CustId AND CarId=@StockId AND IsActive=1 AND InquiryType=1 AND BranchId=@BranchId)
		BEGIN
			declare @VersionId INT
			SELECT @VersionId=VersionId from TC_Stock WITH(NOLOCK) where id=@StockId

			-- adding in tc_inquiries
			INSERT INTO TC_Inquiries(BranchId,CustomerId,CarId,VersionId,SourceId,InquiryType,CreatedBy)
			VALUES(@BranchId,@CustId,@StockId,@VersionId,@SourceId,1,-1)					
				

			declare @ActionId tinyint
			if(@InquiryStatusId>3) -- bcos id>3 is closed ,lost
			begin
			set @ActionId=@InquiryStatusId
			set @InquiryStatusId=null
			end
			-- latest inquiry will be fetch for lead

			if(@loopCount=1) -- bcoz lead will be same for all inquires for that customer
			begin
			INSERT INTO TC_InquiriesLead
						(BranchId,TC_CustomerId,TC_UserId,InquiryCount,NextFollowUpDate,LastFollowUpDate,
						LastFollowUpComment,TC_InquiryTypeId,TC_InquiryStatusId,TC_InquiriesFollowupActionId,CreatedBy)
			VALUES
						(@BranchId,@CustId,@AssignedTo,0,@NextFollowUpDate,@NextFollowUpDate,@FollowupComment,1,@InquiryStatusId,
						@ActionId,-1)
			set @LeadId=SCOPE_IDENTITY()
			end

			-- only need to update for other inquiries at the end latest inquiry status will override
			UPDATE TC_InquiriesLead SET InquiryCount=InquiryCount+1,TC_UserId=@AssignedTo,LastFollowUpComment=@FollowupComment,
			TC_InquiryStatusId=@InquiryStatusId,TC_InquiriesFollowupActionId=@ActionId,LastFollowUpDate=@NextFollowUpDate,NextFollowUpDate=@NextFollowUpDate
			WHERE TC_CustomerId=@CustId AND TC_InquiryTypeId=1 

			-- inert all followup for this inquiry
			INSERT INTO TC_InquiriesFollowup
			(TC_InquiriesLeadId,TC_UserId,FollowUpDate,Comment,TC_InquiryStatusId,TC_InquiriesFollowupActionId)
			select @LeadId,@AssignedTo,NextFollowUp,Comments,InquiryStatusId,@ActionId from TC_InquiryFollowUp
			WHERE InquiryId=@InqId  order by id asc
				
		END
		set @loopCount=@loopCount+1
	end -- end of the loop





