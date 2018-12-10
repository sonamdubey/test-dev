IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_NCDProcessLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_NCDProcessLead]
GO

	      
-- =============================================      
-- Author:  Deepak Tripathi      
-- Create date: 21-Aug-2012      
-- Description: Push NCD Lead into MSMQ      
-- Modifier: Vaibhav K (10-May-2013)  
--    Get all the details as parameters and directly insert NCD Lead  
-- =============================================      
CREATE PROCEDURE [dbo].[CRM_NCDProcessLead]  
   
 @CBDId   NUMERIC,      
 @DealerId  NUMERIC,       
 @Cityid   NUMERIC,          
 @VersionId  NUMERIC,         
 @BuyTime  VARCHAR(30),         
 @InquiryType TINYINT,        
 @InquirySource TINYINT,      
 @ForwardedBy INT,      
      
 @CustomerName VARCHAR(50),      
 @CustomerEmail VARCHAR(80),      
 @CustomerMobile VARCHAR(15),    
 @TCDealerId NUMERIC,      
 @TCQuoteId  NUMERIC,
 @Id NUMERIC OUTPUT
   
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;        
 SET @Id = -1
 --Save Data Before Push TO NCD  
 --Push Lead to CRM NCD Lead  
 INSERT INTO CRM_NCDLeads      
  (CBDId, DealerId, CityId, VersionId, InquiryType, InquirySource, BuyTime,       
   CustomerName, CustomerEmail, CustomerMobile, ForwardedBy, TCQuoteId)      
 VALUES      
  (@CBDId, @DealerId, @CityId, @VersionId, @InquiryType, @InquirySource, @BuyTime,       
   @CustomerName, @CustomerEmail, @CustomerMobile, @ForwardedBy, @TCQuoteId)      
        
 SET @Id=SCOPE_IDENTITY()
 
END 