IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SubmitDealerFeedback]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SubmitDealerFeedback]
GO

	-- =============================================        
-- Author:  <Dipti Bhoir>        
-- Create date: <17-jan-2012>        
-- Description: <This sp used to insert feedback details submitted by dealer about carwale services.>        
-- =============================================        
CREATE PROCEDURE [dbo].[DCRM_SubmitDealerFeedback]        
     
 @IsReceivedWC BIT = NULL,      
 @IsCTAccessible BIT = NULL,      
 @IsStockUpdated BIT = NULL,      
 @ReaNoTime VARCHAR(50)= NULL,      
 @ReaNoInfo VARCHAR(50)= NULL,      
 @ReaTechProb VARCHAR(50)= NULL,      
 @ReaOther VARCHAR(1000)= NULL,      
 @IsCRVisit BIT = NULL,      
 @LeadReceived SMALLINT = NULL,      
 @LeadQuality SMALLINT = NULL,      
 @LeadConversion SMALLINT = NULL,      
 @CWExInteraction SMALLINT = NULL,      
 @ReminderService SMALLINT = NULL,      
 @FieldExInteraction SMALLINT = NULL,      
 @Feedback VARCHAR(1000) = NULL,     
 @FeedbackBy NUMERIC,     
 @Name VARCHAR(250) = NULL,    
 @Email VARCHAR(250) = NULL,    
 @Mobile VARCHAR(50) = NULL,    
 @StatusId   INT OUTPUT       
AS        
        
BEGIN 
       
   --IF EXISTS(SELECT ID from DCRM_DealerFeedback WHERE DealerId = @DealerId)    
   --BEGIN    
   --UPDATE DCRM_DealerFeedback    
   --SET IsReceivedWC = @IsReceivedWC, IsCTAccessible = @IsCTAccessible, IsStockUpdated = IsStockUpdated, ReaNoTime = ReaNoTime    
   --, ReaNoInfo = @ReaNoInfo, ReaTechProb=@ReaTechProb, ReaOther = @ReaOther, IsCRVisit = @IsCRVisit, LeadReceived = @LeadReceived     
   --, LeadQuality = @LeadQuality, LeadConversion = @LeadConversion, CWExInteraction = @CWExInteraction, ReminderService = ReminderService     
   --, FieldExInteraction = @FieldExInteraction, Feedback = @Feedback, FeedbackBy = @FeedbackBy, FeedbackOn = GETDATE()    
   --WHERE DealerId =@DealerId    
   --SET @StatusId = 2    
   --END    
   --ELSE    
   --BEGIN 
            
   SELECT FeedbackBy FROM DCRM_DealerFeedback WHERE FeedbackBy= @FeedbackBy AND MONTH(FeedbackOn)= MONTH(GETDATE())  
   
   IF @@ROWCOUNT = 0
	   BEGIN 
	       
		   INSERT INTO DCRM_DealerFeedback      
				(IsReceivedWC, IsCTAccessible, IsStockUpdated, ReaNoTime, ReaNoInfo, ReaTechProb, ReaOther      
				 , IsCRVisit, LeadReceived, LeadQuality, LeadConversion, CWExInteraction, ReminderService, FieldExInteraction      
				 , Feedback, FeedbackBy, FeedbackOn, Name, EmailId, MobileNo)      
		   VALUES      
				(@IsReceivedWC, @IsCTAccessible, @IsStockUpdated, @ReaNoTime, @ReaNoInfo, @ReaTechProb, @ReaOther      
				 , @IsCRVisit, @LeadReceived, @LeadQuality, @LeadConversion, @CWExInteraction, @ReminderService, @FieldExInteraction      
				 , @Feedback, @FeedbackBy, GETDATE(), @Name, @Email, @Mobile)      
		           
			SET @StatusId = 1       
	   END           
       
END 