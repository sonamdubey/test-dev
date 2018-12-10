IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ProcedureMapDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ProcedureMapDealers]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR Class Packages TABLE Packages

CREATE PROCEDURE [dbo].[ProcedureMapDealers]
	@DealerId		INT,	-- Id. Will be -1 if Its Insertion
	@Name			VARCHAR(50),	
	@Email			VARCHAR(50),
	@RegistrationDate	DATETIME	-- DATE OF REGISTRATION
	
 AS
	DECLARE @CustomerId NUMERIC
	
BEGIN
	
	IF @DealerId <> -1 

		BEGIN
			SELECT Email FROM Customers WHERE Email = @Email

			IF @@ROWCOUNT =  0
				
				BEGIN
					INSERT INTO CUSTOMERS(Name, Email, RegistrationDateTime, IsVerified) Values(@NAME, @EMAIL, @RegistrationDate, 1)
					Set @CustomerId = Scope_Identity()
								
					--MAP DEALER TO CUSTOMER
					INSERT INTO MAPDEALERS(DealerId, CustomerId) VALUES(@DealerId, @CustomerId) 
				END
			
		END

		
	
END