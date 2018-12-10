IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SubmitBuyerFeedbackNew]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SubmitBuyerFeedbackNew]
GO

	CREATE PROCEDURE [dbo].[DCRM_SubmitBuyerFeedbackNew]    

@ContactStatus SMALLINT ,
@ReceivedDealerCall BIT = NULL,
@ReceivedDealerCallOn SMALLINT = NULL,
@DealerResponseSatisfaction BIT = NULL,
@CommentsDealerSatisfaction VARCHAR(1000) = NULL,
@FoundCarInterestedIn BIT = NULL,
@NeedHelpFromCW BIT = NULL,
@CommentsHelp VARCHAR(1000) = NULL,
@CustomerId INT,
@InquiryDate VARCHAR(20)=NULL,
@FeedBackBy INT,
@UpdateId INT,
@AllDealers VARCHAR(1000) ,
@SelectDealers VARCHAR(1000) = NULL,
@CalledDealers VARCHAR(1000) = NULL,
@SatisfyDealers VARCHAR(1000) = NULL,
@StatusId INT = NULL OUTPUT 
AS    
    
BEGIN
     -- Condition true when Status is contacted
      DECLARE @Dealer VARCHAR(50) 
      DECLARE @DealerIndx VARCHAR(50)
      SET @StatusId = 1  
	  IF @ContactStatus = '1'
	  BEGIN		  	
         SET @AllDealers =  @AllDealers + ',' 	  
		 WHILE @AllDealers <> ''
		 BEGIN
		  
			SET @DealerIndx = CHARINDEX(',' , @AllDealers)
			IF  @DealerIndx > 0
			   BEGIN 
				  SET @Dealer = LEFT(@AllDealers, @DealerIndx-1)  
				  SET @AllDealers = RIGHT(@AllDealers, LEN(@AllDealers)- @DealerIndx)
				  
				  IF @Dealer IS NOT NULL
					 BEGIN
					 SET @SelectDealers = ',' + @SelectDealers + ','
					 SET @CalledDealers = ',' + @CalledDealers + ','
					 SET @SatisfyDealers = ',' + @SatisfyDealers + ','
					 DECLARE @TempDealers Table(SelectDealers VARCHAR(500), CalledDealers VARCHAR(500), SatisfyDealers VARCHAR(500))
					 INSERT INTO @TempDealers VALUES(@SelectDealers, @CalledDealers, @SatisfyDealers)
					
           -- If Buyer get call from Dealer					 
					 SELECT *FROM @TempDealers WHERE SelectDealers LIKE '%,' + @Dealer + ',%' 
					 IF @@ROWCOUNT > 0
						SET @ReceivedDealerCall = 1
					 ELSE
						SET @ReceivedDealerCall = 0
						
		   -- If Buyer get call on same Day
					 SELECT *FROM @TempDealers WHERE CalledDealers LIKE '%,' + @Dealer + ',%'			
				     IF @@ROWCOUNT > 0
						SET @ReceivedDealerCallOn = 1
					 ELSE 
						SET @ReceivedDealerCallOn = 4
						   
		   -- If Buyer get response from Dealer
					SELECT *FROM @TempDealers WHERE SatisfyDealers LIKE '%,' + @Dealer + ',%'
					IF @@ROWCOUNT > 0
						SET @DealerResponseSatisfaction = 1
					ELSE
						SET @DealerResponseSatisfaction	= 0
								  
		   -- Save feedback by buyer	
		   			  
						 INSERT INTO DCRM_BuyerFeedBackNew  
						 (ContactStatus, ReceivedDealerCall, ReceivedDealerCallOn, DealerResponseSatisfaction, CommentsDealerSatisfaction, FoundCarInterestedIn, NeedHelpFromCW, CommentsHelp, 
						 DealerId, CustomerId, FeedBackDate, FeedBackBy)
						 VALUES  
						 (@ContactStatus, @ReceivedDealerCall, @ReceivedDealerCallOn, @DealerResponseSatisfaction, @CommentsDealerSatisfaction, @FoundCarInterestedIn, @NeedHelpFromCW, @CommentsHelp, 
						 @Dealer, @CustomerId, GETDATE(), @FeedBackBy)
									
				     END
		     	 END
			END  
		END	 
			  
		ELSE
			BEGIN
			        SET @AllDealers =  @AllDealers + ',' 	  
				    WHILE @AllDealers <> ''
						 BEGIN
							SET @DealerIndx = CHARINDEX(',' , @AllDealers)
							IF  @DealerIndx > 0
							   BEGIN 
								  SET @Dealer = LEFT(@AllDealers, @DealerIndx-1)  
								  SET @AllDealers = RIGHT(@AllDealers, LEN(@AllDealers)- @DealerIndx)
								 INSERT INTO DCRM_BuyerFeedBackNew  
								 (ContactStatus,  ReceivedDealerCall,  ReceivedDealerCallOn, DealerResponseSatisfaction, CommentsDealerSatisfaction, FoundCarInterestedIn, NeedHelpFromCW, CommentsHelp, 
								 DealerId, CustomerId, FeedBackDate, FeedBackBy)
								 VALUES  
								 (@ContactStatus, @ReceivedDealerCall,  @ReceivedDealerCallOn, @DealerResponseSatisfaction, @CommentsDealerSatisfaction, @FoundCarInterestedIn, @NeedHelpFromCW, @CommentsHelp, 
								  @Dealer, @CustomerId, GETDATE(), @FeedBackBy)
								END
					    END
				 	   
					
			END
			IF @UpdateId = '1'
			BEGIN 
			  UPDATE DCRM_CustomerCalling  SET ActionID = @ContactStatus, ActionTakenBy = @FeedBackBy , ActionTakenOn = GETDATE() WHERE CustomerId = @CustomerId AND DAY(InquiryDate) = DAY(@InquiryDate) AND MONTH(InquiryDate) = MONTH(@InquiryDate) AND YEAR(InquiryDate) = YEAR(@InquiryDate) 
	        END   
			
END	