IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertDealersFinancePartner]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertDealersFinancePartner]
GO
	-- PROCEDURE TO INSERT AND UPDATE INTO DealerFinancerData TABLE
CREATE PROCEDURE [dbo].[InsertDealersFinancePartner] 

	@DealerId	INT,
	@FinancerId	INT,
	@Status	INTEGER OUTPUT
AS

	Declare @Id	INT

BEGIN
	SELECT @Id=Id FROM DealersFinancePartner WHERE DealerId = @DealerId AND FinancerId = @FinancerId

	IF @Id IS NULL
		
		BEGIN 
			INSERT INTO DealersFinancePartner(DealerId, FinancerId) VALUES(@DealerId, @FinancerId)
			SET @Status = 1
		END
	ELSE
		BEGIN
			UPDATE DealersFinancePartner SET DealerId = @DealerId, FinancerId = @FinancerId
					WHERE Id = @Id
			
			SET @Status = 2
		END
END