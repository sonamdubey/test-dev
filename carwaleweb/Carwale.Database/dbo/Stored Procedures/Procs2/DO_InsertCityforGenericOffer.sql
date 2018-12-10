IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DO_InsertCityforGenericOffer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DO_InsertCityforGenericOffer]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 10th Dec 2014
-- Description:	to insert multiple cities for generic offers
-- =============================================
CREATE PROCEDURE [dbo].[DO_InsertCityforGenericOffer]
	@OfferId	INT,
	@CityId		VARCHAR(MAX),
	@EnteredBy	INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT OfferId FROM DealerOffersDealers WHERE OfferId = @OfferId
	
	IF @@ROWCOUNT > 0
	BEGIN
		DELETE FROM DealerOffersDealers WHERE OfferId = @OfferId
	END

	INSERT INTO DealerOffersDealers(OfferId,DealerId,CityId,ZoneId)
	SELECT @OfferId,-1,ListMember,NULL FROM fnSplitCSV(@CityId)

	INSERT INTO DealerOffersDealersLog(OfferId,DealerId,CityId,ZoneId,EnteredBy,EntryDate)
	SELECT @OfferId,-1,ListMember,NULL,@EnteredBy,GETDATE() FROM fnSplitCSV(@CityId)
END
