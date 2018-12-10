IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SP_Classified_UpdateViewCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SP_Classified_UpdateViewCount]
GO

	
CREATE PROCEDURE [dbo].[SP_Classified_UpdateViewCount]  
@InquiryId numeric(18, 0),  
@IsDealer bit
AS  
BEGIN  
if @IsDealer = 0 
  update CustomerSellInquiries set ViewCount =ISNULL(viewcount,0) + 1 where ID = @InquiryId
else
  update SellInquiries set ViewCount =ISNULL(viewcount,0) + 1 where ID = @InquiryId

END
