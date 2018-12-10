IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SavePriceSource]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SavePriceSource]
GO

	
-- =============================================
-- Author:		Vicky Lund
-- Create date: 07/06/2016
-- EXEC [SavePriceSource]
-- =============================================
CREATE PROCEDURE [dbo].[SavePriceSource] @PriceSourceMappingId INT
	,@PriceSourceId INT
	,@MakeId INT
	,@CityId INT
	,@DealerId INT
	,@UpdatedBy INT
AS
BEGIN
	DECLARE @StateId INT

	SELECT @StateId = C.StateId
	FROM Cities C WITH (NOLOCK)
	WHERE C.ID = @CityId

	IF @PriceSourceMappingId IS NULL
	BEGIN
		INSERT INTO PriceSourceMapping (
			PriceSourceId
			,MakeId
			,StateId
			,CityId
			,DealerId
			,UpdatedBy
			,UpdatedOn
			)
		VALUES (
			@PriceSourceId
			,@MakeId
			,@StateId
			,@CityId
			,@DealerId
			,@UpdatedBy
			,GETDATE()
			)

		SET @PriceSourceMappingId = IDENT_CURRENT('PriceSourceMapping')

		INSERT INTO PriceSourceMappingLogs (
			PriceSourceMappingId
			,PriceSourceId
			,MakeId
			,StateId
			,CityId
			,DealerId
			,Remarks
			,UpdatedBy
			,UpdatedOn
			)
		VALUES (
			@PriceSourceMappingId
			,@PriceSourceId
			,@MakeId
			,@StateId
			,@CityId
			,@DealerId
			,'Record Inserted'
			,@UpdatedBy
			,GETDATE()
			)
	END
	ELSE
	BEGIN
		UPDATE PriceSourceMapping
		SET PriceSourceId = @PriceSourceId
			,DealerId = @DealerId
			,UpdatedBy = @UpdatedBy
			,UpdatedOn = GETDATE()
		WHERE Id = @PriceSourceMappingId

		INSERT INTO PriceSourceMappingLogs (
			PriceSourceMappingId
			,PriceSourceId
			,MakeId
			,StateId
			,CityId
			,DealerId
			,Remarks
			,UpdatedBy
			,UpdatedOn
			)
		VALUES (
			@PriceSourceMappingId
			,@PriceSourceId
			,@MakeId
			,@StateId
			,@CityId
			,@DealerId
			,'Record Updated'
			,@UpdatedBy
			,GETDATE()
			)
	END
END

