IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Axisbank_UpdateUserDetails_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Axisbank_UpdateUserDetails_SP]
GO

	-- Written By : Ashish G. Kamble on 27 June 2013
-- Summary : Proc will update the customer details which are minimal required.
--			 If customer is fake it will update all the depending tables IsFake field to 1.
CREATE PROCEDURE [dbo].[Axisbank_UpdateUserDetails_SP]
	@UserId		NUMERIC,
	@FirstName			VARCHAR(50),
	@LastName			VARCHAR(50),
	@IsActive		BIT,
	@IsVerified		BIT
AS 
BEGIN
	BEGIN TRANSACTION TRANSUSER	
	
		UPDATE AxisBank_Users SET
			FirstName 			= @FirstName,
			LastName 			= @LastName,
			IsVerified 		= @IsVerified,
			IsActive		= @IsActive
		WHERE 
			UserId = @UserId
		--NOW CHECK THE ISFAKE FLAG. IF THE CUSTOMER IS FAKE THEN MAKE THE ISVERIFIED FLAG ANAD THE 
		--FAKE FLAG TO TRUE AND DELETE THENENTRY FROM THE TEMPCUSTOMERS TABLE AND ALSO MAKE ALL 
		--THE INQUIRIES TO BE FAKE 
		--IF @IsFake = 1 
		--	BEGIN
		--		--call the procedure MakeCustomerFake to make the customers fake, it takes as inout the customer id
		--		EXEC MakeCustomerFake  @CustomerId
		--	END		

	COMMIT TRANSACTION TRANSUSER
END
