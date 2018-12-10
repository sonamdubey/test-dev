IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_Fun_GetOnRoadPrice]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[Microsite_Fun_GetOnRoadPrice]
GO

	
CREATE FUNCTION [dbo].[Microsite_Fun_GetOnRoadPrice]
(
@CarVersionId NUMERIC(18, 0)
,@CityId NUMERIC(18, 0)
)
RETURNS INT
AS
--Author:Rakesh Yadav,
--Date Created: 03-April-2015
--Descr: fetch on-roadPrice for version and city
BEGIN
DECLARE @OnRoadPrice INT

SELECT @OnRoadPrice = SUM(PQN.PQ_CategoryItemValue)
FROM CW_NewCarShowroomPrices PQN WITH (NOLOCK)
INNER JOIN PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = PQN.PQ_CategoryItem
INNER JOIN PQ_Category PQC WITH (NOLOCK) ON PQC.CategoryId = CI.CategoryId
LEFT JOIN PriceQuote_LocalTax PQLT WITH (NOLOCK) ON CI.Id = PQLT.CategoryItemid
AND PQLT.CityId = @CityId
WHERE CarVersionId = @CarVersionId
AND PQN.CityId = @CityId

RETURN @OnRoadPrice
END
