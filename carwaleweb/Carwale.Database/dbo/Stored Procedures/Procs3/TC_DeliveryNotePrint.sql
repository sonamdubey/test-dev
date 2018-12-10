IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DeliveryNotePrint]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DeliveryNotePrint]
GO
	-- =============================================      
-- Author:  Surendra      
-- Create date: 18th Oct,2011      
-- Description: This procedure will return all the details to print receipt      
-- Modified By: Tejashree Patil on 22 Nov 2012 at 4 pm : Changed @DealerAddress length from 200 to 500      
-- Modified By: Nilesh Utture on 26 Nov 2012 at 3 pm : Added @Content, @City, @Customer, @CustomerMobile parameters    
-- =============================================      
CREATE PROCEDURE [dbo].[TC_DeliveryNotePrint]      
(      
@StockId BIGINT,      
@DealerId BIGINT,      
@RegNo VARCHAR(20) OUTPUT,      
@DealerAddress VARCHAR(500)OUTPUT,-- Modified By: Tejashree Patil on 22 Nov 2012 at 4 pm      
@DealerPhone VARCHAR(20)OUTPUT,      
@DealerPin VARCHAR(10)OUTPUT,      
@DealerFax VARCHAR(10)OUTPUT,      
@DealerWebsite VARCHAR(50)OUTPUT,      
@CarName VARCHAR(50)OUTPUT,      
@CarColor VARCHAR(50)OUTPUT,      
@CarYear  VARCHAR(100)OUTPUT,      
@DiliveryNotes VARCHAR(400) OUTPUT,      
@TC_BookingDelivery_Id INT  OUTPUT,      
@ChassisNo VARCHAR(30) OUTPUT,      
@LicenseNo VARCHAR(30) OUTPUT,      
@EngineNo VARCHAR(30) OUTPUT,      
@Content VARCHAR(700) OUTPUT,      
@City VARCHAR(100) OUTPUT,      
@Customer VARCHAR(100) OUTPUT,      
@CustomerMobile VARCHAR(10) OUTPUT      
      
)      
AS      
BEGIN      
	 -- SET NOCOUNT ON added to prevent extra result sets from      
	 -- interfering with SELECT statements.      
	 SET NOCOUNT ON;      
	 -- Modified By: Nilesh Utture on 26 Nov 2012, Added JOIN's with TC_CustomerDetails, TC_DeliveryNoteDetails  
	 SELECT @RegNo=ST.RegNo,@DealerAddress=DL.Address1,@DealerFax=DL.FaxNo,@DealerPhone=DL.PhoneNo,@DealerPin=DL.Pincode,      
	 @DealerWebsite=DL.WebsiteUrl,@CarName=( CM.Name + ' ' + MO.Name + ' ' + VR.Name ),@City = C.Name, @Customer = CD.CustomerName, @CustomerMobile = CD.Mobile,      
	 @CarColor=ST.Colour,@CarYear=SUBSTRING(CONVERT(VARCHAR(11), ST.MakeYear, 113), 4, 8),@DiliveryNotes=BD.DeliveryNotes,      
	 @TC_BookingDelivery_Id=BD.TC_BookingDelivery_Id,@ChassisNo=BD.ChassisNumber,@LicenseNo=BD.LincenseNumber, @EngineNo=BD.EngineNumber ,@Content = DN.Content       
	 FROM TC_CarBooking CB      
	 INNER JOIN TC_CustomerDetails CD WITH(NOLOCK)ON CD.Id = CB.CustomerId      
	 INNER JOIN TC_BookingDelivery BD WITH(NOLOCK)ON CB.TC_CarBookingId=BD.TC_CarBooking_Id      
	 INNER JOIN TC_Stock ST WITH(NOLOCK)ON CB.StockId=ST.Id      
	 INNER JOIN Dealers DL WITH(NOLOCK)ON ST.BranchId=DL.ID      
	 INNER JOIN Cities C WITH(NOLOCK)ON DL.CityId = C.ID      
	 LEFT JOIN CarVersions VR WITH(NOLOCK)On VR.Id=ST.VersionId        
	 LEFT JOIN CarModels MO WITH(NOLOCK)On Mo.Id=VR.CarModelId         
	 LEFT JOIN CarMakes CM WITH(NOLOCK)On CM.Id=Mo.CarMakeId       
	 LEFT JOIN TC_DeliveryNoteDetails DN WITH(NOLOCK)On DL.ID=DN.DealerId      
	 WHERE ST.Id=@StockId AND ST.BranchId=@DealerId      
END   
  
  
  
SET ANSI_NULLS ON
