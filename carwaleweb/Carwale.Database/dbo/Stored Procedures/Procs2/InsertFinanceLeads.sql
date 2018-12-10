IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertFinanceLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertFinanceLeads]
GO
	--THIS PROCEDURE IS FOR INSERTING into financeleads
--Id, VersionId, CustomerId, UsedCarProfileId, isUsed, Tenure, FinanceAmountPercent, FirstName, LastName, Email, Phone1, Mobile, CityId, 
--Comments, EntryDate, IsApproved, IsFake, IsForwarded, IsRejected, IsMailSend, IsViewed

CREATE PROCEDURE [dbo].[InsertFinanceLeads]
	@VersionId			NUMERIC,	-- Car Version Id
	@CustomerId			NUMERIC,	-- Car RegistrationNo 
	@UsedCarProfileId		VARCHAR(50),	-- Car RegistrationNo 
	@IsUsed			BIT,
	@Tenure			INT,
	@FinanceAmountPercent	INT,
	@FirstName			VARCHAR(50),
	@LastName			VARCHAR(50),
	@Email				VARCHAR(100),
	@Phone1			VARCHAR(50), 
	@Mobile			VARCHAR(50), 
	@CityId				NUMERIC, 
	@Comments			VARCHAR(1000), 
	@EntryDate			DATETIME,	-- Entry Date
	@isApproved			BIT,
	@Finalized			VARCHAR(50),
	@Age				VARCHAR(50),
	@SourceId			SMALLINT,
	@MonthlyIncome			NUMERIC,
	@Dob				DATETIME,
	@EmpType			VARCHAR(50),
	@InquiryId			NUMERIC OUTPUT

 AS
	DECLARE	@ShowroomPrice 	NUMERIC,
		@LoanAmount	NUMERIC

	BEGIN
		SELECT @ShowroomPrice = Price FROM NewCarShowroomPrices WHERE CarVersionId = @VersionId AND CityId = @CityId AND IsActive = 1
		
		IF @ShowroomPrice > 0
			BEGIN
				SET @LoanAmount = ( @ShowroomPrice * @FinanceAmountPercent)/100
			END
		ELSE
			SET @LoanAmount = 0

		INSERT INTO FinanceLeads
			(
				VersionId, 		CustomerId, 		UsedCarProfileId, 	isUsed, 		Tenure, 		
				FinanceAmountPercent, 	FirstName, 		LastName, 		Email, 		Phone1, 
				Mobile, 			CityId, 			Comments, 		EntryDate, 	IsApproved,
				Finalized, 		Age,			SourceId,		LoanAmount,	MonthlyIncome,	Dob,
				EmpType
			)
		VALUES
			(
				@VersionId, 		@CustomerId, 		@UsedCarProfileId, 	@isUsed, 	@Tenure, 		
				@FinanceAmountPercent, @FirstName, 		@LastName, 		@Email, 	@Phone1, 
				@Mobile, 		@CityId, 		@Comments, 		@EntryDate, 	@IsApproved,
				@Finalized, 		@Age,			@SourceId,		@LoanAmount,	@MonthlyIncome,
				@Dob,			@EmpType
			)
		
		SET @InquiryId = SCOPE_IDENTITY()
	END