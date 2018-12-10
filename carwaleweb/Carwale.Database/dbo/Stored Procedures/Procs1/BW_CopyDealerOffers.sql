IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_CopyDealerOffers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_CopyDealerOffers]
GO

	-- =============================================
-- Author		:	Sumit Kate
-- Create date	:	15 Jun 2015
-- Description	:	Copies the Existing Offers to the different cities				
-- Parameters	:
--	@DealerId	:	Dealer Id
--	@CityIds	:	Comma Seperated City Ids (E.g. 1,2,3)
--	@OfferIds	:	Comma Seperated Offer Ids (E.g. 1,2,3)
-- =============================================
CREATE PROCEDURE [dbo].[BW_CopyDealerOffers] 
@DealerId INT,
@CityIds VARCHAR(250),
@OfferIds VARCHAR(250)
AS
BEGIN
DECLARE @tblCity AS TABLE(id VARCHAR(250));
DECLARE @tblOffer AS TABLE(id VARCHAR(250));

--Temp table to get the exising offers
DECLARE @tblExistingOffer AS
TABLE(
	Id INT
	,DealerId INT
	,CityId INT
	,ModelId INT
	,OfferCategoryId INT
	,OfferText VARCHAR(500)
	,OfferValue INT
	,EntryDate DATETIME
	,LastUpdated DATETIME
	,UpdatedBy BIGINT
	,OfferValidTill DATETIME
	,IsActive BIT
);

--get the cities into a table
INSERT INTO @tblCity
	SELECT * FROM SplitText(@CityIds,',')

--get the offers into a table
INSERT INTO @tblOffer
	SELECT * FROM SplitText(@OfferIds,',')
	
--Populate the temporary existing offers table
INSERT INTO @tblExistingOffer
	SELECT 
		offers.Id
		,DealerId
		,CityId
		,ModelId
		,OfferCategoryId
		,OfferText
		,OfferValue
		,EntryDate
		,LastUpdated
		,UpdatedBy
		,OfferValidTill		
		,IsActive 
	FROM BW_PQ_Offers offers WITH(NOLOCK)
	INNER JOIN @tblOffer tmpOffers
	ON offers.Id = tmpOffers.id

--Insert the temporary table offers to BikeWale Offers table
INSERT INTO BW_PQ_Offers (DealerId,CityId,ModelId,OfferCategoryId,OfferText,OfferValue,EntryDate,LastUpdated,UpdatedBy,OfferValidTill,IsActive)
SELECT 
	DealerId
	,city.id AS [CityId]
	,ModelId
	,OfferCategoryId
	,OfferText
	,OfferValue
	,GETDATE() AS [EntryDate]
	,GETDATE() AS [Last Updated]
	,UpdatedBy
	,OfferValidTill		
	,IsActive
FROM @tblExistingOffer offer
CROSS JOIN @tblCity city
END