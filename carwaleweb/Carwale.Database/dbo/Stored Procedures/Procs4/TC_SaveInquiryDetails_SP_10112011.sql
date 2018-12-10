IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveInquiryDetails_SP_10112011]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveInquiryDetails_SP_10112011]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

-- Modified By:		Surendra
-- Create date: 20th Oct, 2011
-- Description:	To addopt Securiry if user is trying to access other than his stock	   
-- =============================================      
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

create Procedure [dbo].[TC_SaveInquiryDetails_SP_10112011]          
@DealerId Numeric,  
@CustomerName VarChar(100),          
@CustomerMobile Numeric,          
@CustomerEmail VarChar(100),          
@Comments VarChar(500),      
@InterestedIn VarChar(500),          
@StatusId Int,         
@SourceId Int,      
@FollowUp DateTime,         
@StockId Int,          
@InquiryId Int,          
@RequestDateTime DateTime,          
@Id Numeric Output      
    
As           
Begin
	Declare @customerId Numeric            
	If @InquiryId = -1                    
	Begin          
		Set @customerId = IsNull( ( Select top 1 Id From TC_CustomerDetails Where (Mobile =  @CustomerMobile or Email= @CustomerEmail)), 0 )              

		If @customerId = 0         
			Begin      
				Insert Into TC_CustomerDetails( CustomerName, Mobile, Email )                     
				Values( @CustomerName, @CustomerMobile, @CustomerEmail )                    
				                 
				Set @customerId = SCOPE_IDENTITY()    
			End      
		         
		Insert Into TC_PurchaseInquiries( BranchId, CustomerId, StockId, Comments, InterestedIn, InquiryStatusId, SourceId, FollowUp, RequestDateTime )          
		Values (@DealerId,@customerId, @StockId, @Comments, @InterestedIn, @StatusId, @SourceId, @FollowUp, @RequestDateTime )            
		Set @Id = SCOPE_IDENTITY()    
	End                          
	Else                      
	Begin               
		Set @customerId = ( Select CustomerId From TC_PurchaseInquiries Where Id = @InquiryId )          

		Update TC_CustomerDetails Set CustomerName = @CustomerName, Mobile = @CustomerMobile, Email = @CustomerEmail          
		Where ID=@customerId
		        
		Update TC_PurchaseInquiries Set Comments = @Comments, InterestedIn = @InterestedIn, InquiryStatusId = @StatusId, SourceId = @SourceId, FollowUp = @FollowUp
		Where Id = @InquiryId AND BranchId=@DealerId   
		Set @Id = @InquiryId          
	End       
END 

