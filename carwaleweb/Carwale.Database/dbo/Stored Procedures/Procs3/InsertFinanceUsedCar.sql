IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertFinanceUsedCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertFinanceUsedCar]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR FinanceUsedCar TABLE

CREATE PROCEDURE [dbo].[InsertFinanceUsedCar]
	@Id				NUMERIC,
	@FinancerId		 	INT,	
	@ModelId			INT,	
	@UserType			INT,
	@NotOlderThen		 	FLOAT,
	@MaxAmountRs	 	NUMERIC,	
	@MaxAmountPer		FLOAT,
	@ProcessingChargesPer  	FLOAT,  
	@ProcessingChargesMin  	INT,
	@ProcessingChargesMax 	INT
	
 AS
	
BEGIN
	
	IF  @Id = -1
		BEGIN
			INSERT INTO FinanceUsedCar(ModelId, FinancerId, UserType, NotOlderThen, MaxAmountRs, MaxAmountPer,
					ProcessingChargesPer, ProcessingChargesMin, ProcessingChargesMax, isActive)
			VALUES(@ModelId, @FinancerId, @UserType, @NotOlderThen, @MaxAmountRs, @MaxAmountPer,
				@ProcessingChargesPer, @ProcessingChargesMin, @ProcessingChargesMax,1)
		END
	ELSE -- Updation
		BEGIN
			UPDATE FinanceUsedCar SET ModelId=@ModelId, FinancerId=@FinancerId, UserType=@UserType,  
			NotOlderThen=@NotOlderThen, MaxAmountRs=@MaxAmountRs, MaxAmountPer=@MaxAmountPer, 
			ProcessingChargesPer=@ProcessingChargesPer, 
			ProcessingChargesMin=@ProcessingChargesMin, ProcessingChargesMax=@ProcessingChargesMax,  isActive=1
			WHERE ID = @Id
		END
END
