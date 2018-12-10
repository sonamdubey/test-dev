IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertNewDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertNewDealers]
GO
	


--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR Customer in Billing  TABLE





CREATE PROCEDURE [dbo].[InsertNewDealers]
	@Id			NUMERIC,	-- Id. Will be -1 if Its Insertion
	@Name			VARCHAR(50),	
	@ContactPerson	VARCHAR(50),
	@Email			VARCHAR(50),
	@Address		VARCHAR(250),
	@PhoneNo		VARCHAR(25),
	@City			NUMERIC,
	@Area			VARCHAR(50),
	@Pincode		VARCHAR(6),
	@EntryDate 		DATETIME,
	@Organisation		VARCHAR(50),
	@STATUS		INTEGER OUTPUT	--return value, -1 for unsuccessfull attempt, and 0 for success
	
 AS
	DECLARE @TEMP  AS INTEGER
	
BEGIN
	SET @STATUS = -1
				
	IF @Id = -1 -- Insertion
		
		BEGIN
			
			 INSERT INTO NewDealers
			 ( Name ,ContactPerson,   Address , PhoneNo , EmailID , Area , City , Pincode , IsActive , EntryDate, Organisation )
			 VALUES
			 ( @Name , @ContactPerson, @Address , @PhoneNo , @Email , @Area , @City , @Pincode , 1, @EntryDate, @Organisation )
			
			
			SELECT @TEMP=MAX(ID) FROM NewDealers 
			
			SET @STATUS = @TEMP
				
		END
	ELSE
		BEGIN

			UPDATE NewDealers SET 
				Name 		= @Name  , 
				ContactPerson	= @ContactPerson,
				Address 	= @Address , 
				PhoneNo	 = @PhoneNo , 
				EmailID	 	= @Email , 
				Area	 	= @Area , 
				City	 	= @City , 
				Pincode 	= @Pincode,
				Organisation	= @Organisation
			WHERE 
				ID = @Id
			
			SET @STATUS = 0
				
			
		END
					
	
END
