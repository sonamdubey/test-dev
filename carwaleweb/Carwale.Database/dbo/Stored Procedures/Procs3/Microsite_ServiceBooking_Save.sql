IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_ServiceBooking_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_ServiceBooking_Save]
GO

	
-- =============================================      
-- Author:  Kritika Choudhary
-- Create date: 9th Oct 2015
-- Description: To Save Service Booking Details

-- =============================================      
CREATE PROCEDURE [dbo].[Microsite_ServiceBooking_Save]  
(      
 @DealerId INT=NULL, 
 @Id  INT=NULL OUTPUT,      
 @Name VARCHAR(50)=NULL,  
 @EmailId VARCHAR(50)=NULL,  
 @MobileNum VARCHAR(10)=NULL,  
 @ModelId int=NULL,
 @PreDatetime datetime=NULL, 
 @PickupAddr varchar(200)=NULL,
 @Comm varchar(200)=NULL,
 @AutobizInqId int=NULL,
 @ServiceCenterId int =NULL,
 @ServiceBooking_Id INT=NULL OUTPUT
)      
AS      
BEGIN 
  
   -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON; 
 IF(@Id IS NULL)
 BEGIN     
   INSERT INTO Microsite_Customers(Name,EmailId,MobileNum)     
   VALUES(@Name,@EmailId,@MobileNum) ;
   SELECT @Id=SCOPE_IDENTITY(); 

   INSERT INTO Microsite_ServiceBooking(Microsite_CustomerId,DealerId,ModelId,PreferredDateTime,PickupAddress,Comments,ServiceCenterId)     
   VALUES(@Id,@DealerId,@ModelId,@PreDatetime,@PickupAddr,@Comm,@ServiceCenterId) ;
   SELECT @ServiceBooking_Id=SCOPE_IDENTITY(); 
 END
 ELSE
 BEGIN
	UPDATE Microsite_ServiceBooking
	SET AutobizInqId  = ISNULL(@AutobizInqId, AutobizInqId), ModifiedDate= GETDATE()
	WHERE ID=@ID;
 END
END 

