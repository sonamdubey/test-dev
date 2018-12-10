IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SkodaRapidCancellation_Insert_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SkodaRapidCancellation_Insert_SP]
GO

	

/*  
Procedure: SkodaRapidCancellation_Insert_SP  
Created By: Dilip  
Created On: 15-Nov-2011  
  
Procedure Desc:  
To Insert the contact details of the customer and CarWale and Skoda Booking ID.
*/  
  
CREATE PROCEDURE [dbo].[SkodaRapidCancellation_Insert_SP]  
@Name VARCHAR(100),  
@Mobile VARCHAR(20),  
@CityID INT,  
@Email VARCHAR(250),
@CarWaleBookingId VARCHAR(50),
@ID INT OUT  
AS  
BEGIN    
	INSERT INTO SkodaRapidCancellation ( Name, Mobile, CityID, Email,CarWaleBookingId) 
	VALUES(@Name, @Mobile, @CityID, @Email,@CarWaleBookingId)  
	SET @ID = SCOPE_IDENTITY()    
END  

