IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SubmitBuyerFeedback]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SubmitBuyerFeedback]
GO

	
-- =============================================  
-- Author:  <Dipti Bhoir>  
-- Create date: <05-jan-2012>  
-- Description: <This sp used to insert feedback details submitted by buyer for specific inquiry.>  
-- =============================================  
CREATE PROCEDURE [dbo].[DCRM_SubmitBuyerFeedback]  
 
 @ContactStatus  SMALLINT,  
 @ReceivedDealerCall  BIT = NULL,  
 @DealStatus   SMALLINT = NULL,  
 @BuyerStatus  SMALLINT = NULL,  
 @BuyerCWServiceComment  NVARCHAR(1000) = NULL,  
 @CalledDealer  BIT = NULL,  
 @VisitedDealer   BIT = NULL,  
 @VisitDealStatus   SMALLINT = NULL,  
 @VisitBuyerStatus SMALLINT = NULL,  
 @VisitComment NVARCHAR(1000) = NULL,  
 @NoVisitComment  NVARCHAR(1000) = NULL,  
 @BuyerCWComment NVARCHAR(1000) = NULL,  
 @FeedbackBy  NUMERIC,  -- BuyerId
 @InquiryId NUMERIC,
 @StatusId   INT OUTPUT  
   
AS  
  
BEGIN  
   IF EXISTS(SELECT ID from DCRM_BuyerFeedback      
	WHERE InquiryId = @InquiryId)
		BEGIN
			UPDATE DCRM_BuyerFeedback 
			SET ContactStatus = @ContactStatus, ReceivedDealerCall=@ReceivedDealerCall, DealStatus=@DealStatus, BuyerStatus=@BuyerStatus,  
			  BuyerCWServiceComment = @BuyerCWServiceComment, CalledDealer = @CalledDealer, VisitedDealer=@VisitedDealer, VisitDealStatus=@VisitDealStatus,  
			  VisitBuyerStatus = @VisitBuyerStatus, VisitComment=@VisitComment, NoVisitComment=@NoVisitComment, BuyerCWComment=@BuyerCWComment,  
			  FeedbackBy = @FeedbackBy, FeedbackDate=GETDATE()
			WHERE InquiryId =@InquiryId  
			SET @StatusId = 2
		END
   ELSE
		BEGIN    
 
			 INSERT INTO DCRM_BuyerFeedback  
			 (ContactStatus, ReceivedDealerCall, DealStatus, BuyerStatus,  
			  BuyerCWServiceComment, CalledDealer, VisitedDealer, VisitDealStatus,  
			  VisitBuyerStatus, VisitComment, NoVisitComment, BuyerCWComment,  
			  FeedbackBy, FeedbackDate,InquiryId)  
			 VALUES  
			 (@ContactStatus, @ReceivedDealerCall, @DealStatus, @BuyerStatus,  
			  @BuyerCWServiceComment, @CalledDealer, @VisitedDealer, @VisitDealStatus,  
			  @VisitBuyerStatus, @VisitComment, @NoVisitComment, @BuyerCWComment,  
			  @FeedbackBy, GETDATE(),@InquiryId) 
			  
			 SET @StatusId = 1 
	   END		   
    
 
    
END  
