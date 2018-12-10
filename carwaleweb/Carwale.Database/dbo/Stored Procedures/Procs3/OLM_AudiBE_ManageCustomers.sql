IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_ManageCustomers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_ManageCustomers]
GO

	
-- =============================================
-- Created By : Ashish G. Kamble on 26 July 2013
-- Description : If customer is already registered, proc will update the data and return customer id.
--				 Else customer will be registered as new customer.
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AudiBE_ManageCustomers]
	@customerId Numeric OUT,
	@name VARCHAR(100),
	@mobile VARCHAR(50),
	@stateId Numeric,
	@cityId Numeric,
	@email VARCHAR(50),
	@sourceId INT,
	@createdDate DATETIME,
	@IsNewCustomerInserted BIT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SET @IsNewCustomerInserted = 0;
	
	DECLARE @customerIdTemp Numeric
	SELECT @customerIdTemp = C.Id FROM OLM_AudiBE_Customers C WHERE Email = @email 
	
	IF(@customerIdTemp IS NOT NULL)
	BEGIN
		SET @customerId = @customerIdTemp
		UPDATE OLM_AudiBE_Customers
			SET 
				Name = CASE WHEN @name IS NOT NULL THEN @name ELSE Name END,
				Mobile = CASE WHEN @mobile IS NOT NULL THEN @mobile ELSE Mobile END,
				StateId = CASE WHEN @stateId IS NOT NULL THEN @stateId ELSE StateId END,
				CityId = CASE WHEN @cityId IS NOT NULL THEN @cityId ELSE CityId END,
				SourceId = CASE WHEN @sourceId IS NOT NULL THEN @sourceId ELSE SourceId END
		WHERE Email = @email
	END
	ELSE
	BEGIN
		-- Insert statements for procedure here
		INSERT INTO OLM_AudiBE_Customers
		(
			Name,
			Mobile,
			StateId,
			CityId,
			Email,
			SourceId,
			CreatedDate
		)
		VALUES
		(
			@name,
			@mobile,
			@stateId,
			@cityId,
			@email,
			@sourceId,
			@createdDate
		)
		
		SET @IsNewCustomerInserted = 1;
		SET @customerId= SCOPE_IDENTITY();
	END
END
