IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCustomerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCustomerDetails]
GO

	--THIS PROCEDURE WILL ADD THE CUSTOMER DETAILS INTO THE CUSTOMERS TABLE. IF THE CUSTOMER IS VERIFIED THEN THE 
--ENTRIES FROM THE TEMP CUSTOMERS TABLE WILL BE DELETED. ELSE IF IT IS NOT MARK AS VERIFIED THEN THE DETAILS
--ARE UPDATED IN THE TEMPCUSTOMERS TABLE ONLY.
--ALSO IF THE CUSTOMER IS MARK AS FAKE THEN ALL THE INQUIRIES ARE MARK AS FAKE AND ALSO AS VERIFIED
---Modified by Aditi Dhaybar on 23/09/2014 for updating customers details.

CREATE PROCEDURE [dbo].[UpdateCustomerDetails]
	@CustomerId		NUMERIC,
	@Name			VARCHAR(100), 
	@Email			VARCHAR(100),  
	@Address		VARCHAR(250), 
	@StateId		NUMERIC, 
	@CityId			NUMERIC, 
	@AreaId		NUMERIC, 
	@Phone1		VARCHAR(50), 
	@Phone2		VARCHAR(50), 
	@Mobile		VARCHAR(50), 
	@PrimaryPhone		TINYINT, 
	@Industry		VARCHAR(100), 
	@Designation		VARCHAR(100), 
	@Organization		VARCHAR(100), 
	@DOB			DATETIME , 
	@ContactHours		VARCHAR(50), 
	@ContactMode		VARCHAR(50), 
	@CurrentVehicle	VARCHAR(100), 
	@InternetUsePlace	VARCHAR(50), 
	@CarwaleContact	VARCHAR(50), 
	@InternetUseTime	VARCHAR(50), 
	@ReceiveNewsletters	BIT, 
	@IsVerified		BIT,
	@IsFake		BIT,
	@Comment		VARCHAR(1500)		
AS
	DECLARE 	@TempId 		AS NUMERIC, 
		    	@City			VARCHAR(50),
			@Area			VARCHAR(50),
			@PinCode		VARCHAR(10)
			
BEGIN
	
	--CHECK THE ISVERIFIED FIELD IF IT IS 1 THEN ADD THE DETAILS INTO THE CUSTOMERS TABLE
	-- AND DELETE THE RECORDS FROM THE TEMPCUSTOMERS TABLE, ALSO SET THE 
	--TEMPID OF THE CUSTOMERS TABLE TO -1.
	--if it is not equal to 1 then also update the entries in the tempcustomers table only
	--ALSO IF THE CUSTOMER IS MARKED AS FAKED THEN MAKE THE MARK THE FAKE FLAG OF THE 
	--CUSTOMERS TABLE TO 1 AND ALSO TO ALL THE INQUIRIES FOR THIS PARTICULAR CUSTOMER

	BEGIN TRANSACTION TRANSCUSTOMER

		UPDATE CUSTOMERS SET
			 Name 			= @Name,
			 Address 		= @Address, 
			 StateId  		= @StateId,
			 CityId  			= @CityId,
			 AreaId  		= @AreaId,
			 Phone1  		= @Phone1,
			 --Phone2  		= @Phone2,		
			 Mobile  		= @Mobile,
			 --PrimaryPhone  		= @PrimaryPhone,    Modified by Aditi Dhaybar on 23/09/2014 for updating customers details.
			 --Industry  		= @Industry,
			 --Designation  		= @Designation,
			 --Organization  		= @Organization,
			 --DOB  			= @DOB,
			 --ContactHours  		= @ContactHours,
			 --ContactMode  		= @ContactMode,
			 --CurrentVehicle  		= @CurrentVehicle,
			 --InternetUsePlace  	= @InternetUsePlace,
			 --CarwaleContact  	= @CarwaleContact,
			 --InternetUseTime  	= @InternetUseTime,
			 ReceiveNewsletters 	= @ReceiveNewsletters,
			 IsVerified 		= @IsVerified
			--Comment		= @Comment
		
		WHERE 
			ID = @CustomerId			
		

		--Commented by Aditi Dhaybar on 23/09/2014 as not required  for updating customer details.

		--fetch the TEMPID
		--SELECT @TempId = IsNull(TempId, -1)  FROM Customers WHERE ID = @CustomerId  
		
		--PRINT @TempId
	
		--IF @IsVerified = 0	--UPDATE THE TABLE
		--BEGIN
		--	--get the city name, state name and the pincode name
		--	SELECT @City = C.Name 
		--	FROM Cities AS C
		--	WHERE C.ID = @CityId

		--	UPDATE TEMPCUSTOMERS SET
		--		 Name 			= @Name,
		--		 Address 		= @Address, 
		--		 State	  		= @StateId,
		--		 City			= @City,
		--		 Area	  		= '',
		--		 PinCode  		= -1,
		--		 Phone1  		= @Phone1,
		--		 Phone2  		= @Phone2,
		--		 Mobile  		= @Mobile,
		--		 PrimaryPhone  		= @PrimaryPhone,
		--		 Industry  		= @Industry,
		--		 Designation  		= @Designation,
		--		 Organization  		= @Organization,
		--		 DOB  			= @DOB,
		--		 ContactHours  		= @ContactHours,
		--		 ContactMode  		= @ContactMode,
		--		 CurrentVehicle  		= @CurrentVehicle,
		--		 InternetUsePlace  	= @InternetUsePlace,
		--		 CarwaleContact  	= @CarwaleContact,
		--		 InternetUseTime  	= @InternetUseTime,
		--		 ReceiveNewsletters 	= @ReceiveNewsletters				 
			
		--	WHERE 
		--		ID = @TempId	
		
		--END
		--ELSE
		--BEGIN
		--	--DELETE THE ENTRY FROM THE TEMP TABLE
		--	DELETE FROM TempCustomers WHERE Id = @TempId
			
		--	UPDATE Customers SET TempId = -1 WHERE Id = @CustomerId 
		--END

		--NOW CHECK THE ISFAKE FLAG. IF THE CUSTOMER IS FAKE THEN MAKE THE ISVERIFIED FLAG ANAD THE 
		--FAKE FLAG TO TRUE AND DELETE THENENTRY FROM THE TEMPCUSTOMERS TABLE AND ALSO MAKE ALL 
		--THE INQUIRIES TO BE FAKE 
		IF @IsFake = 1 
		BEGIN

			--call the procedure MakeCustomerFake to make the customers fake, it takes as inout the customer id
			EXEC MakeCustomerFake  @CustomerId

		END
		

	COMMIT TRANSACTION TRANSCUSTOMER
END


