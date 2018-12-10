IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCarAdditionalInformation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCarAdditionalInformation]
GO

	
CREATE    PROCEDURE [dbo].[GetCarAdditionalInformation]
	@InquiryId		NUMERIC(18,0),
	@IsCarInWarranty	BIT OUTPUT,	
	@WarrantyValidTill	DATETIME OUTPUT,
	@WarrantyProvidedBy	VARCHAR(20) OUTPUT,	
	@ThirdPartyWarrantyProviderName	VARCHAR(50) OUTPUT,	
	@StockId		INT OUTPUT,
	@WarrantyDetails  VARCHAR(200) OUTPUT,
	@HasExtendedWarranty BIT OUTPUT,
	@ExtendedWarrantyValidFor VARCHAR(20) OUTPUT,
	@ExtendedWarrantyProviderName VARCHAR(50) OUTPUT,
	@ExtendedWarrantyDetails VARCHAR(200) OUTPUT, 
	@HasAnyServiceRecords BIT OUTPUT,
	@ServiceRecordsAvailableFor VARCHAR(20) OUTPUT,
	@HasRSAAvailable BIT OUTPUT,
	@RSAValidTill DATETIME OUTPUT,
	@RSAProviderName VARCHAR(50) OUTPUT,
	@RSADetails VARCHAR(200) OUTPUT,
	@HasFreeRSA BIT OUTPUT,
	@FreeRSAValidFor VARCHAR(20) OUTPUT,
	@FreeRSAProvidedBy VARCHAR(20) OUTPUT,
	@FreeRSADetails VARCHAR(200) OUTPUT

 AS
	BEGIN

	    
		
		SET @StockId = (SELECT TOP 1 TC_StockId FROM SellInquiries WITH (NOLOCK) WHERE ID = @InquiryId)
		
		
		If @StockId IS NOT NULL
		BEGIN
			SELECT @IsCarInWarranty=IsCarInWarranty
				   ,@WarrantyValidTill=WarrantyValidTill
				   ,@WarrantyProvidedBy=WarrantyProvidedBy
				   ,@ThirdPartyWarrantyProviderName=ThirdPartyWarrantyProviderName
				   ,@WarrantyDetails=WarrantyDetails
				   ,@HasExtendedWarranty=HasExtendedWarranty
				   ,@ExtendedWarrantyValidFor=ExtendedWarrantyValidFor
				   ,@ExtendedWarrantyProviderName=ExtendedWarrantyProviderName
				   ,@ExtendedWarrantyDetails=ExtendedWarrantyDetails
				   ,@HasAnyServiceRecords=HasAnyServiceRecords
				   ,@ServiceRecordsAvailableFor=ServiceRecordsAvailableFor
				   ,@HasRSAAvailable=HasRSAAvailable
				   ,@RSAValidTill=RSAValidTill
				   ,@RSAProviderName=RSAProviderName
				   ,@RSADetails=RSADetails
				   ,@HasFreeRSA=HasFreeRSA
				   ,@FreeRSAValidFor=FreeRSAValidFor
				   ,@FreeRSAProvidedBy=FreeRSAProvidedBy
				   ,@FreeRSADetails = FreeRSADetails
			 FROM TC_CarAdditionalInformation WITH(NOLOCK)
			 WHERE StockId=@StockId
		END
	END

