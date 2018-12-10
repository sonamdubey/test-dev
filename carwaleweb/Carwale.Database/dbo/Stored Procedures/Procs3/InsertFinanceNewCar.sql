IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertFinanceNewCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertFinanceNewCar]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR FinanceUsedCar TABLE

CREATE PROCEDURE [dbo].[InsertFinanceNewCar]
	@Id			 NUMERIC,
	@FinancerId		 INT,	
	@ModelId		 INT,	
	@UserType		 INT,
	@MaxAmountRs	 NUMERIC,	
	@MaxAmountPer	 FLOAT,
	@ProcessingChargesPer  FLOAT,
	@ProcessingChargesMin  INT,
	@ProcessingChargesMax INT	
 AS
	
BEGIN
	
	IF  @Id = -1
		BEGIN
			INSERT INTO FinanceNewCar(ModelId, FinancerId, UserType, MaxAmountRs, MaxAmountPer,ProcessingChargesPer,
			ProcessingChargesMin, ProcessingChargesMax , isActive)
			VALUES(@ModelId, @FinancerId, @UserType, @MaxAmountRs, @MaxAmountPer, @ProcessingChargesPer,
			 @ProcessingChargesMin, @ProcessingChargesMax, 1)
		END
	ELSE -- Updation
		BEGIN
			UPDATE FinanceNewCar SET ModelId=@ModelId, FinancerId=@FinancerId, UserType=@UserType,
			MaxAmountRs=@MaxAmountRs, MaxAmountPer=@MaxAmountPer,
			ProcessingChargesPer=@ProcessingChargesPer, ProcessingChargesMin=@ProcessingChargesMin,
			ProcessingChargesMax=@ProcessingChargesMax, isActive=1
			WHERE ID = @Id
		END
END
