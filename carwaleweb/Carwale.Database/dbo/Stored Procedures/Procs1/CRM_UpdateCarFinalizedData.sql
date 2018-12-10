IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_UpdateCarFinalizedData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_UpdateCarFinalizedData]
GO

	CREATE PROCEDURE [dbo].[CRM_UpdateCarFinalizedData]

	@CarBasicDataId			Numeric,
	@UpdatedById			Numeric,

	@IsProductExplained		Bit,
	@IsPQMailed				Bit,
	@IsPQMailedExternal		Bit,
	@IsPQMailInternalReq	Bit,
	@IsPQMailExternalReq	Bit,
	@PriceQuoteNotRequired	Bit,
	@IsFinalized			Bit,
	@IsPQRearrange			Bit,
	@IsPENotRequired		Bit,
	
	@ExpectedBuyingDate		DateTime,	
	@UpdatedOn				DateTime,
	
	@Status		Bit			OutPut	
				
 AS
	
BEGIN
	
	IF @CarBasicDataId <> -1
	BEGIN 
		UPDATE CRM_CarBasicData
		SET UpdatedBy = @UpdatedById, IsProductExplained = @IsProductExplained, IsPENotRequired = @IsPENotRequired,
			IsFinalized = @IsFinalized, ExpectedBuyingDate = @ExpectedBuyingDate, UpdatedOn = @UpdatedOn
		WHERE Id = @CarBasicDataId

		SET @Status = 1
		
		IF @IsPQMailedExternal = 1
		BEGIN 
			UPDATE CRM_CarBasicData SET IsPQMailedExternal = 1, IsPQRearrange = 0 WHERE Id = @CarBasicDataId
		END
		
		IF @IsPQMailExternalReq = 1
		BEGIN 
			UPDATE CRM_CarBasicData SET IsPQMailExternalReq = 1 WHERE Id = @CarBasicDataId
		END
		
		IF @PriceQuoteNotRequired = 1
		BEGIN 
			UPDATE CRM_CarBasicData SET PriceQuoteNotRequired = 1, IsPQRearrange = 0 WHERE Id = @CarBasicDataId
		END
		
		IF @IsPQRearrange = 1
		BEGIN 
			UPDATE CRM_CarBasicData SET PriceQuoteNotRequired = 0, IsPQMailedExternal = 0,
				IsPQRearrange = 1 WHERE Id = @CarBasicDataId
		END
			
	END
	ELSE SET @Status = 0
	
END













