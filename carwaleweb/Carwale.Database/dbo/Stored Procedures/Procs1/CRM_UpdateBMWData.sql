IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_UpdateBMWData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_UpdateBMWData]
GO

	-- =============================================
-- Author      : Chetan Navin		
-- Create date : 12 Sep 2013
-- Description : To Update BMW Data to push
-- =============================================
CREATE PROCEDURE [dbo].[CRM_UpdateBMWData]
	--Parameters
	@CBDId				BIGINT,
	@Salutation			VARCHAR(5),
	@SalutationValue	VARCHAR(100),
	@FirstName			VARCHAR(25),
	@LastName			VARCHAR(25),
	@Mobile				VARCHAR(15),
	@AlternateContact	VARCHAR(15),
	@Email				VARCHAR(25),
	@DealerNameValue	VARCHAR(100),
	@DealerId			NUMERIC,
	@City				VARCHAR(50),
	@MakeNameValue		VARCHAR(100),
	@ModelNameValue		VARCHAR(100),
	@VersionNameValue	VARCHAR(100),
	@MakeId				Numeric ,
	@ModelId			Numeric ,
	@VersionId			Numeric ,
	@PurchaseIntention	VARCHAR(100),
	@PurchaseIntentionValue VARCHAR(100),
	@OtherCars          VARCHAR(100),
	@OtherBMWCars		VARCHAR(100),
	@GeneralRemarks		VARCHAR(5000),
	@IsFinaceRequired	VARCHAR(5),
	@LookingFor			VARCHAR(20),
	@TradeCarMake		VARCHAR(50),
	@TradeCarModel		VARCHAR(50),
	@TradeCarVersion	VARCHAR(50),
	@TradeCarYear		VARCHAR(20),
	@TradeCarKms		VARCHAR(20),
	@CurrentCarOwned	VARCHAR(250),
	@BMWRefId           VARCHAR(250) 
AS

BEGIN
	 UPDATE CRM_BMW_APIData SET Salutation			= @Salutation,
								SalutationValue	= @SalutationValue,
								FirstName			= @FirstName,
								LastName			= @LastName,
								Mobile				= @Mobile,
								AlternateMobile		= @AlternateContact,
								Email				= @Email,
								DealerNameValue		= @DealerNameValue,
								DealerId			= @DealerId,
								City				= @City,
								MakeNameValue		= @MakeNameValue,
								ModelNameValue		= @ModelNameValue,
								VersionNameValue	= @VersionNameValue,
								MakeId				= @MakeId,
								ModelId			= @ModelId,
								VersionId			= @VersionId,
								PurchaseIntention	= @PurchaseIntention,
								PurchaseIntentionValue = @PurchaseIntentionValue,
								OtherCars          = @OtherCars,
								OtherBMWCars		= @OtherBMWCars,
								GeneralRemarks		= @GeneralRemarks,
								IsFinaceRequired	= @IsFinaceRequired,
								LookingFor			= @LookingFor,
								TradeCarMake		= @TradeCarMake,
								TradeCarModel		= @TradeCarModel,
								TradeCarVersion	= @TradeCarVersion,
								TradeCarYear		= @TradeCarYear,
								TradeCarKms		= @TradeCarKms,
								BMWRefId			= @BMWRefId,
								CurrentCarOwned		= @CurrentCarOwned,
								UpdatedOn			= GETDATE()
	WHERE CBDId = @CBDId								
END
