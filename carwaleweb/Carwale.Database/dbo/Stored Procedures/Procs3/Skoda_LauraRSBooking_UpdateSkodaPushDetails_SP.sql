IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Skoda_LauraRSBooking_UpdateSkodaPushDetails_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Skoda_LauraRSBooking_UpdateSkodaPushDetails_SP]
GO

	
/*  
Procedure Name:Skoda_LauraRSBooking_UpdateSkodaPushDetails_SP  
Created By: Vikas  
Created On: 29/08/2011  
  
Procedure Desc:  
To update the next set of values to the Skoda_LauraRSBooking Table  
with the values of the OutletName, OrderNo, LeadTokenNo, TokenDateTime,
PushSuccess And PushErrorMsg  
  
*/  
Create Procedure [dbo].[Skoda_LauraRSBooking_UpdateSkodaPushDetails_SP]  
@BookingId Numeric,  
@OutletName VarChar(50),  
@OrderNo VarChar(50),
@LeadTokenNo VarChar(50),  
@TokenDateTime DateTime,  
@PushSuccess Bit,
@PushErrorMsg VarChar(2000)  
As  
Begin  
  
 Update Skoda_LauraRSBooking Set OutletName = @OutletName,
		 OrderNo = @OrderNo,   
         LeadTokenNo = @LeadTokenNo,   
         TokenDateTime = @TokenDateTime,   
         PushSuccess = @PushSuccess,
         PushErrorMsg = @PushErrorMsg
        Where ID = @BookingId  
  
End
