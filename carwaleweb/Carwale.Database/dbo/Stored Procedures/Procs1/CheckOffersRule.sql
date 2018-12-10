IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckOffersRule]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckOffersRule]
GO

	
-- ==========================================================================================
-- Author: Chetan Thambad
-- Create date: 16/12/2015
-- Description:	Get Rules for Offers Used in OPR
-- exec [dbo].[CheckOffersRule] 
-- ==========================================================================================
CREATE PROCEDURE [dbo].[CheckOffersRule] @OfferId INT
AS
BEGIN
	SELECT CASE 
			WHEN DOD.ZoneId IS NULL
				THEN DOD.CityId
			ELSE NULL
			END AS CityId
		,DOD.ZoneId AS ZoneId
	FROM DealerOffersDealers DOD WITH (NOLOCK)
	WHERE DOD.OfferId = @OfferId
END

