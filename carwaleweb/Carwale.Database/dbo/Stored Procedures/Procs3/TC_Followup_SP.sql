IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Followup_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Followup_SP]
GO
	

-- =============================================
-- Author:		Binumon George
-- Create date: 29-07-2011
-- Description:	Inserting follow up information after varification of customer
-- =============================================
CREATE PROCEDURE [dbo].[TC_Followup_SP] 
	-- Add the parameters for the stored procedure here
	@InquiryId  INT ,
	@InquiryStatuId INT,
	@FollowupDate DATETIME,
	@FollowupComments VARCHAR(200),
	@Status INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status=0 -- Default staus will be 0.Insertion or updation not happened status will be 0
  
		UPDATE TC_PurchaseInquiries  SET InquiryStatusId=@InquiryStatuId, FollowupComment=@FollowupComments, FollowUp=@FollowupDate,IsActionTaken=1
		WHERE Id=@InquiryId
		  -- Inserting follow up commets regarding purchase inquiry
		INSERT INTO TC_InquiryFollowUp(InquiryId,Comments,EntryDate,NextFollowUp,InquiryStatusId)
		VALUES(@InquiryId,@FollowupComments,GETDATE(),@FollowupDate,@InquiryStatuId)
		set @Status=1
				
END


