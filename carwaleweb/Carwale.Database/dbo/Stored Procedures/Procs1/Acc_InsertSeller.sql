IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_InsertSeller]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_InsertSeller]
GO

	
-- PROCEDUTRE TO INSERT/UPDATE ACCESSORIES SELLER DETAILS

CREATE PROCEDURE [dbo].[Acc_InsertSeller]
	@Id				NUMERIC,
	@SellerName			VARCHAR(100),
	@Address		 	VARCHAR(200),
	@CityId				INT,
	@Email				VARCHAR(100),
	@AlternateEmail			VARCHAR(300),
	@Phone			VARCHAR(50),
	@AlternatePhone		VARCHAR(50),
	@Fax				VARCHAR(50),
	@Mobile			NUMERIC,
	@JoiningDate			DATETIME,
	@SellerId			NUMERIC OUTPUT
 AS
	
BEGIN
	
	IF @Id = -1
		BEGIN
			SELECT  @SellerId = Id  FROM Acc_Seller WHERE SellerName = @SellerName AND CityId = @CityId

			IF @@RowCount = 0
				BEGIN
					INSERT INTO Acc_Seller ( SellerName, Address , CityId, Email,  AlternateEmail, Phone, AlternatePhone, Fax, Mobile, JoiningDate)
					VALUES (@SellerName, @Address , @CityId, @Email,  @AlternateEmail, @Phone, @AlternatePhone, @Fax, @Mobile, @JoiningDate)
					
					SET @SellerId = SCOPE_IDENTITY()
				END
				
		END
	ELSE
		BEGIN
			UPDATE Acc_Seller 
			SET	SellerName = @SellerName, Address = @Address, CityId = @CityId,  Email = @Email,  
				AlternateEmail = @AlternateEmail, Phone =@Phone , AlternatePhone = @AlternatePhone, Fax = @Fax, 
				Mobile = @Mobile, JoiningDate = @JoiningDate
			WHERE Id = @Id
			
			SET @SellerId = -1
		END
END
