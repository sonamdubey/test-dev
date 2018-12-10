IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_RemoveDealerPrices]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_RemoveDealerPrices]
GO

	-- =============================================
-- Author:		Ashwini Todkar
-- Create date: 8 Nov 2014
-- Description:	Proc to remove(delete) prices of versions fro dealer   
-- =============================================
CREATE PROCEDURE [dbo].[BW_RemoveDealerPrices] @DealerId INT
	,@CityId INT
	,@BikeVersionId VARCHAR(200)
AS
BEGIN
	DELETE
	FROM BW_NewBikeDealerShowroomPrices  
	WHERE CityId = @CityId
		AND DealerId = @DealerId
		AND BikeVersionId IN (
			SELECT items
			FROM dbo.SplitText(@BikeVersionId, ',')
			)
END

