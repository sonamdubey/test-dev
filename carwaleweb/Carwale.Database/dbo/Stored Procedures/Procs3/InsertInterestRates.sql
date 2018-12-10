IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertInterestRates]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertInterestRates]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[InsertInterestRates]
	@Id		NUMERIC,
	@FinancerId	INT,	
	@ModelId	NUMERIC,
	@UserType	INT,	
	@InterestRates	FLOAT,	
	@Tenure	INT,
	@isUsed	BIT
	
 AS
	
BEGIN
	
	IF  @Id = -1
		BEGIN
			INSERT INTO FinanceInterestRates(CarModelId, FinancerId, UserType, CustomerPayout, InterestRate, Tenure, isActive, isUsed)
			VALUES(@ModelId, @FinancerId, @UserType, 0, @InterestRates, @Tenure, 1, @isUsed)
		END
	ELSE -- Updation
		BEGIN
			UPDATE FinanceInterestRates SET CarModelId=@ModelId, 
			FinancerId=@FinancerId, UserType=@UserType, InterestRate=@InterestRates, Tenure=@Tenure,
			isActive=1, isUsed=@isUsed
			WHERE ID = @Id
		END
END
