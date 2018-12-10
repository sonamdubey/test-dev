IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_UpdateTransaction]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_UpdateTransaction]
GO

	
CREATE PROCEDURE [dbo].[OLM_AudiBE_UpdateTransaction]
@TransactionId numeric(18, 0),
@CustomerId numeric(18, 0)=NULL,
@ModelId	numeric(18, 0)=NULL,
@VersionId	numeric(18, 0)=NULL,
@FuelTypeId	int=NULL,
@GradeId	numeric(18, 0)=NULL,
@ModelColorId	numeric(18, 0)=NULL,
@UpholestryColorId numeric(18, 0) = NULL,
@CityId	numeric(18, 0)=NULL,
@ExShowRoomPrice	varchar(50)=NULL,
@VersionPriceId	numeric(18, 0)=NULL,
@DealerId	numeric(18, 0)=NULL,
@PaymentMode	int=NULL,
@PaymentType	int=NULL,
@Amount	numeric(18, 0)=NULL,
@ChequeAddress	varchar(300)=NULL,
@ChequeStateId INT = NULL,
@ChequeCity	varchar(50)=NULL,
@ChequeDate	varchar(50)=NULL,
@ChequePinCode	varchar(10)=NULL,
@PGTransactionId	numeric(18, 0)=NULL,
@SourceId	int=NULL,
@TransactionDate	datetime=NULL

AS BEGIN
/*
Author: Rakesh Yadav
Date Created: 24 July 2013 
*/
UPDATE OLM_AudiBE_Transactions
SET 
	CustomerId= CASE WHEN @CustomerId IS NOT NULL THEN @CustomerId ELSE CustomerId END,

	ModelId= CASE WHEN @ModelId IS NOT NULL THEN @ModelId ELSE ModelId END,

	VersionId= CASE WHEN @VersionId IS NOT NULL THEN @VersionId ELSE VersionId END,

	FuelTypeId= CASE WHEN @FuelTypeId IS NOT NULL THEN @FuelTypeId ELSE FuelTypeId END,

	GradeId= CASE WHEN @GradeId IS NOT NULL THEN @GradeId ELSE GradeId END,

	ModelColorId= CASE WHEN @ModelColorId IS NOT NULL THEN @ModelColorId ELSE ModelColorId END,
	
	UpholestryColorId = CASE WHEN @UpholestryColorId IS NOT NULL THEN @UpholestryColorId ELSE UpholestryColorId END,

	CityId= CASE WHEN @CityId IS NOT NULL THEN @CityId ELSE CityId END,

	ExShowRoomPrice= CASE WHEN @ExShowRoomPrice IS NOT NULL THEN @ExShowRoomPrice ELSE ExShowRoomPrice END,

	VersionPriceId= CASE WHEN @VersionPriceId IS NOT NULL THEN @VersionPriceId ELSE VersionPriceId END,

	DealerId= CASE WHEN @DealerId IS NOT NULL THEN @DealerId ELSE DealerId END,

	PaymentMode= CASE WHEN @PaymentMode IS NOT NULL THEN @PaymentMode ELSE PaymentMode END,

	PaymentType= CASE WHEN @PaymentType IS NOT NULL THEN @PaymentType ELSE PaymentType END,

	Amount= CASE WHEN @Amount IS NOT NULL THEN @Amount ELSE Amount END,

	ChequeAddress= CASE WHEN @ChequeAddress IS NOT NULL THEN @ChequeAddress ELSE ChequeAddress END,
	
	ChequeStateId= CASE WHEN @ChequeStateId IS NOT NULL THEN @ChequeStateId ELSE ChequeStateId END,

	ChequeCity= CASE WHEN @ChequeCity IS NOT NULL THEN @ChequeCity ELSE ChequeCity END,

	ChequeDate= CASE WHEN @ChequeDate IS NOT NULL THEN @ChequeDate ELSE ChequeDate END,

	ChequePinCode= CASE WHEN @ChequePinCode IS NOT NULL THEN @ChequePinCode ELSE ChequePinCode END,

	PGTransactionId= CASE WHEN @PGTransactionId IS NOT NULL THEN @PGTransactionId ELSE PGTransactionId END,

	SourceId= CASE WHEN @SourceId IS NOT NULL THEN @SourceId ELSE SourceId END,

	TransactionDate= CASE WHEN @TransactionDate IS NOT NULL THEN @TransactionDate ELSE TransactionDate END

WHERE Id=@TransactionId

END
