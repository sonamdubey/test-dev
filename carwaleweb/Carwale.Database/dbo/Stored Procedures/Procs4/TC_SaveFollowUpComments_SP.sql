IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveFollowUpComments_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveFollowUpComments_SP]
GO

	
CREATE Procedure [dbo].[TC_SaveFollowUpComments_SP]
@Comments VarChar(500),
@FollowUp DateTime,
@EntryDate DateTime,
@StatusId Int,
@InquiryId Numeric

As 
Begin
	
	Insert Into TC_InquiryFollowUp ( InquiryId, Comments, NextFollowUp, EntryDate ) Values( @InquiryId, @Comments, @FollowUp, @EntryDate )
	
	Update TC_PurchaseInquiries Set InquiryStatusId = @StatusId, FollowUp = @FollowUp Where Id = @InquiryId 

End
