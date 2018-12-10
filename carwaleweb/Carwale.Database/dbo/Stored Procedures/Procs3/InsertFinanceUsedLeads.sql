IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertFinanceUsedLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertFinanceUsedLeads]
GO

	

--THIS PROCEDURE IS FOR INSERTING into UsedCarLoan

CREATE PROCEDURE [dbo].[InsertFinanceUsedLeads]
	@CarVersionId			NUMERIC,	-- Car Version Id
	@CustomerId				NUMERIC,	-- Car RegistrationNo 
	@CustomerName			VARCHAR(200),
	@Email					VARCHAR(100),
	@ContactNo				VARCHAR(100), 
	@ProfileId				VARCHAR(50), 
	@CityId					NUMERIC, 
	@InqDate				DATETIME,	-- Entry Date
	@InquiryId				NUMERIC OUTPUT

 AS
	BEGIN
	
		INSERT INTO UsedCarLoan
			(
				CarVersionId, CustomerId, CustomerName, Email, ContactNo, 
				ProfileId, CityId, InqDate
			)
		VALUES
			(
				@CarVersionId, @CustomerId, @CustomerName, @Email, @ContactNo, 
				@ProfileId, @CityId, @InqDate
			)
		
		SET @InquiryId = SCOPE_IDENTITY()
	END


