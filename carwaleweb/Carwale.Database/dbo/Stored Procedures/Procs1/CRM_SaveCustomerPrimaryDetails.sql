IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveCustomerPrimaryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveCustomerPrimaryDetails]
GO

	CREATE PROCEDURE [dbo].[CRM_SaveCustomerPrimaryDetails]

	@CustomerId			Numeric,
	@FirstName			VarChar(200),
	@LastName			VarChar(100),
	@Email				VarChar(200),
	@Landline			VarChar(50),
	@Mobile				VarChar(15),
	@CityId				Numeric,
	@Source				VarChar(200),
	@CWCustId			Numeric,
	@Comments			VarChar(1500),
	@AltEmail			VarChar(200),
	@AltContactNo		VarChar(100),
	@CustomerCreatedOn	DateTime,
	@CustomerUpdatedOn	DateTime,
	@UpdatedBy			Numeric,
	@NewCustomerId		Numeric OutPut,
	@AreaId             INT = NULL,
	@Salutation			VARCHAR(10) = NULL
				
 AS
	
BEGIN
	SET @NewCustomerId = -1
	IF @CustomerId = -1
		BEGIN
			INSERT INTO CRM_Customers
				( 
				  FirstName, LastName, Email, Landline, Mobile, CityId,AreaId, Source, CWCustId,
				  Comments, AlternateEmail, AlternateContactNo, CreatedOn, 
				  UpdatedOn, UpdatedBy, Salutation
				)
			VALUES
				( 
				  @FirstName, @LastName, @Email, @Landline, @Mobile, @CityId,@AreaId, @Source, 
				  @CWCustId, @Comments, @AltEmail, @AltContactNo, @CustomerCreatedOn, 
				  @CustomerUpdatedOn, @UpdatedBy, @Salutation
				)
				
				SET @NewCustomerId = SCOPE_IDENTITY()
				
				--Score Lead for Customer City
				--EXEC CRM.LSUpdateLeadScore 7, -1, @NewCustomerId
		END
	ELSE
		BEGIN
			UPDATE CRM_Customers SET 
				FirstName			= @FirstName, 
				LastName			= @LastName, 
				Email				= @Email, 
				Landline			= @Landline, 
				Mobile				= @Mobile, 
				CityId				= @CityId, 
				AreaId              = @AreaId,
				Source				= @Source, 
				Comments			= @Comments, 
				AlternateEmail		= @AltEmail, 
				AlternateContactNo	= @AltContactNo, 
				UpdatedOn			= @CustomerUpdatedOn,
				UpdatedBy			= @UpdatedBy,
				Salutation			= @Salutation
			WHERE
				Id = @CustomerId
				
		END
END














