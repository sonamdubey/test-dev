IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Skoda_LauraRSBooking_Insert_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Skoda_LauraRSBooking_Insert_SP]
GO

	
/*
Procedure: Skoda_LauraRSBooking_Insert_SP
Created By: Vikas
Created On: 27-Aug-2011

Procedure Desc:
To Insert the contact details of the customer and also the colour of Laura RS that the customer prefers. 
Other Details would be updated as the customer progresses in the form.
*/

CREATE Procedure [dbo].[Skoda_LauraRSBooking_Insert_SP]
@CustomerName VarChar(50),
@Mobile VarChar(10),
@CityID Int,
@Email VarChar(100),
@Colour VarChar(50),
@EntryDate DateTime,
@ID Numeric(18,0) Out
As
Begin

Insert Into Skoda_LauraRSBooking ( CustomerName, Mobile, CityID, Email, Colour, EntryDate) Values(@CustomerName, @Mobile, @CityID, @Email, @Colour, @EntryDate)
Set @ID = SCOPE_IDENTITY()

End

