IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[InsertOrUpdateCoupons]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[InsertOrUpdateCoupons]
GO

	-- =============================================          
-- Author:  <Prashant Vishe>          
-- Create date: <02 july 2013>          
-- Description: <For Inserting and updating coupons related data> 
-- Modified by:Prashant Vishe On 12 Auguest 2013   for insertion/updation  of column OfferValidity         
-- =============================================          
CREATE PROCEDURE [cw].[InsertOrUpdateCoupons]          
 -- Add the parameters for the stored procedure here          
 @ConsumerType smallint,          
 @PackageId int,          
 @ConsumerId numeric,          
 @OfferCode varchar(50),          
 @OfferValidityFrom datetime,    
 @OfferValidityTo datetime,          
 @DiscountAmount numeric,          
 @Id int          
AS          
BEGIN          
 -- SET NOCOUNT ON added to prevent extra result sets from          
 -- interfering with SELECT statements.          
 SET NOCOUNT ON;          
          
    -- Insert statements for procedure here          
              
    IF @Id=-1          
  BEGIN          
   INSERT INTO PromotionalOffers(ConsumerType,PackageId,ConsumerId,OfferCode,OfferValidityFrom,OfferValidityTo,DiscountAmount,CreatedOn,OfferValidity)           
   VALUES(@ConsumerType,@PackageId,@ConsumerId,@OfferCode,@OfferValidityFrom,@OfferValidityTo,@DiscountAmount, GETDATE(),@OfferValidityTo)          
  END          
 ELSE          
  BEGIN          
   UPDATE  PromotionalOffers          
   SET           
   ConsumerType=@ConsumerType,          
   PackageId=@PackageId,          
   ConsumerId=@ConsumerId,          
   OfferCode=@OfferCode,          
   OfferValidityTo=@OfferValidityTo,  
   OfferValidity=@OfferValidityTo,     
   OfferValidityFrom=@OfferValidityFrom,         
   DiscountAmount=@DiscountAmount,      
   LastUpdatedOn = GETDATE()        
   WHERE  Id=@Id          
  END          
END 