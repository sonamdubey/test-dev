IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DO_InsertCityforGenericOffer_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DO_InsertCityforGenericOffer_v15]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 10th Dec 2014
-- Description:	to insert multiple cities for generic offers
-- Modified BY: Shalini Nair on 09/11/2015
-- =============================================
CREATE PROCEDURE [dbo].[DO_InsertCityforGenericOffer_v15.11.1]
	@OfferId	INT,
	@CityId		INT,
	@ZoneId		INT,
	@EnteredBy	INT
AS
BEGIN
	SET NOCOUNT ON;

	--SELECT OfferId FROM DealerOffersDealers WITH(NOLOCK) WHERE OfferId = @OfferId
	
	--IF @@ROWCOUNT > 0
	--BEGIN
	--	DELETE FROM DealerOffersDealers  WHERE OfferId = @OfferId
	--END

	INSERT INTO DealerOffersDealers(OfferId,DealerId,CityId,ZoneId) VALUES(@OfferId,-1,@CityId,@ZoneId)
	--SELECT @OfferId,-1,ListMember,NULL FROM fnSplitCSV(@CityId)

	INSERT INTO DealerOffersDealersLog(OfferId,DealerId,CityId,ZoneId,EnteredBy,EntryDate)
	SELECT @OfferId,-1,ListMember,@ZoneId,@EnteredBy,GETDATE() FROM fnSplitCSV(@CityId)
END
