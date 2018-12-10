IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SkodaRapidBooking_Insert_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SkodaRapidBooking_Insert_SP]
GO

	
/*  
Procedure: SkodaRapidBooking_Insert_SP  
Created By: Vikas  
Created On: 27-Aug-2011  
  
Procedure Desc:  
To Insert the contact details of the customer and also the colour of Laura RS that the customer prefers.   
Other Details would be updated as the customer progresses in the form.  
*/  
  
CREATE Procedure [dbo].[SkodaRapidBooking_Insert_SP]  
@CustomerName VarChar(100),  
@Mobile VarChar(20),  
@CityID Int,  
@Email VarChar(250),  
@EntryDate DateTime,  
@ID Int Out  
As  
Begin  
  
Insert Into SkodaRapidBooking ( CustomerName, Mobile, CityID, Email, EntryDate) Values(@CustomerName, @Mobile, @CityID, @Email, @EntryDate)  
Set @ID = SCOPE_IDENTITY()  
  
End  
