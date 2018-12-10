IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveCustomerAliases]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveCustomerAliases]
GO

	CREATE PROCEDURE [dbo].[CRM_SaveCustomerAliases]

	@CustomerId			Numeric,
	@FirstName			VarChar(200),
	@LastName			VarChar(100),
	@Email				VarChar(200),
	@Landline			VarChar(50),
	@Mobile				VarChar(15),
	@CityId				Numeric,
	@Source				VarChar(200),
	@CWCustId			Numeric,
	@CustomerCreatedOn	DateTime
				
 AS
	
BEGIN
	DECLARE @OldEMail AS VARCHAR(200)
	DECLARE @OldMobile AS VARCHAR(15)
	DECLARE @OldFirstName AS VARCHAR(200)
	DECLARE @OldLastName AS VARCHAR(200)
	DECLARE @DayLastUpdated AS NUMERIC
			
	IF @CustomerId <> -1
		BEGIN
			--Get Existing Details
			SELECT @OldEMail = Email, @OldMobile = Mobile, @OldFirstName = FirstName, @OldLastName = LastName, @DayLastUpdated = DATEDIFF(DD, ISNULL(UpdatedOn, GETDATE()), GETDATE())
			FROM CRM_Customers WITH(NOLOCK) WHERE ID = @CustomerId
	
			DECLARE @CustCount INT, @CustAliasesCount	INT
			
			SELECT Id FROM CRM_Customers WITH(NOLOCK) WHERE Email = @Email AND Mobile = @Mobile
			SET @CustCount = @@ROWCOUNT;
			
			-- Check if email, mobile pair already exist or not
			SELECT Id FROM CRM_CustomerAliases WITH(NOLOCK) WHERE Email = @Email AND MobileNo = @Mobile
			SET @CustAliasesCount = @@ROWCOUNT;
			
			-- Insert customer details to the 'CRM_CustomerAliases' if any one between
			-- email or mobile differs. 
			IF( @CustCount = 0 OR @CustAliasesCount = 0 )
				BEGIN
					--If customer is 90days old Put existing details in Alias and replace it with new details
					--Vaibhav K 29 Nov commented below condition of 90 days old customer
					--IF @DayLastUpdated > 90
					--BEGIN
						UPDATE CRM_Customers SET Email = @Email, Mobile = @Mobile, FirstName = @FirstName, LastName = @LastName
						WHERE ID = @CustomerId
						
						SET @Email = @OldEMail
						SET @Mobile = @OldMobile
						SET @FirstName = @OldFirstName
						SET @LastName = @OldLastName
					--END
					
					
					INSERT INTO CRM_CustomerAliases
						( 
						  CRM_CustomerId, FirstName, LastName, Email, LandlineNo, MobileNo, 
						  CityId, Source, CWCustomerId, CreatedOn
						)
					VALUES
						( 
						  @CustomerId, @FirstName, @LastName, @Email, @Landline, @Mobile, 
						  @CityId, @Source, @CWCustId, @CustomerCreatedOn
						)
				END
		END
END