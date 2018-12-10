IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Skoda_LauraRSBooking_UpdateDealershipDetails_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Skoda_LauraRSBooking_UpdateDealershipDetails_SP]
GO

	
/*  
Procedure Name:Skoda_LauraRSBooking_UpdateDealershipDetails_SP  
Created By: Vikas  
Created On: 29/08/2011  
  
Procedure Desc:  
To update the next set of values to the Skoda_LauraRSBooking Table  
with the values of the DealerId, EstimatedDelivery, BookingAmount And ExShowroomPrice  
  
*/  
CREATE Procedure [dbo].[Skoda_LauraRSBooking_UpdateDealershipDetails_SP]  
@BookingId Numeric,  
@DealerId Int,  
@OutletCode VarChar(50),
@EstimatedDeliveryDate Date,  
@BookingAmount VarChar(10),  
@ExShowroomPrice VarChar(10)  
As  
Begin  
  
 Update Skoda_LauraRSBooking Set DealerId = @DealerId,
		 OutletCode = @OutletCode,   
         EstimatedDelivery = @EstimatedDeliveryDate,   
         BookingAmount = @BookingAmount,   
         ExShowroomPrice = @ExShowroomPrice   
        Where ID = @BookingId  
  
End
