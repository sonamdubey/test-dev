IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertCustomerAccount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertCustomerAccount]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[InsertCustomerAccount]
	@CustId		NUMERIC,	-- CustomerId
	@CustTypeId		NUMERIC,	--Type Of Customer
	@AccountId		INTEGER OUTPUT

 AS
	DECLARE @CustomerName AS VARCHAR(50) ,
		     @Address	      AS VARCHAR(255) ,
		     @City	     AS  VARCHAR(50) ,
		     @Pincode 	     AS VARCHAR(6) 
			
	BEGIN

		SET @AccountId = 0

		IF ( @CustTypeId = 1 OR  @CustTypeId = 2 OR  @CustTypeId = 3 )
		BEGIN 
			SELECT @CustomerName = ( D.FirstName + ' ' + D.LastName )  , @Address = ( D.Address1 + ' ' + D.Address2), @City =  A.Name + ', ' + C.Name, @Pincode =  D.PinCode 
			FROM Dealers AS D, Areas AS A, Cities AS C WHERE A.ID = D.AreaId AND C.ID = D.CityId  AND D.ID = @CustId
		END 
		
		IF ( @CustTypeId = 4 )
		BEGIN 
			SELECT @CustomerName = C.Name , @Address = C.Address , @City =  A.Name + ', ' + CI.Name, @Pincode =  ' ' 
			FROM Customers AS C, Areas AS A, Cities AS CI  
			WHERE A.ID = C.AreaId AND CI.ID = C.CityId AND C.ID = @CustId
		END 

		INSERT INTO AccountCustomers 
			(
				CustId  , CustTypeId , CustomerName , Address , City , PinCode ,IsActive 
			 )
		 	VALUES 
			(
				@CustId, @CustTypeId , @CustomerName , @Address , @City , @Pincode , 1
			)	
		SET @AccountId = SCOPE_IDENTITY()


	END
