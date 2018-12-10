IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetDealerBookingAmount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetDealerBookingAmount]
GO

	-------------------------------------------------------
-- Created By : Sadhana Upadhyay on 18 Dec 2014
-- Summary : To get dealer booking amount for bike
-- exec BW_GetDealerBookingAmount 164,4
-- Modified By : Ashwini Todkar on 23 Dec 2014
-- retrieved make and model details
-------------------------------------------------------
CREATE PROCEDURE [dbo].[BW_GetDealerBookingAmount] @VersionId INT
	,@DealerId INT
AS
BEGIN
	SELECT ISNULL(BA.Amount, 0) AS Amount
		,BA.IsActive
		,BA.Id
		,BM.Name MakeName
		,BMO.Name ModelName
	FROM BW_DealerBikeBookingAmounts BA WITH (NOLOCK)
	INNER JOIN BikeVersions BV WITH (NOLOCK) ON BV.ID = BA.VersionId
	INNER JOIN BikeModels BMO WITH(NOLOCK) ON BMO.ID = BV.BikeModelId
	INNER JOIN BikeMakes BM WITH (NOLOCK) ON BM.ID = BMO.BikeMakeId
	INNER JOIN Dealers D WITH (NOLOCK) ON BA.DealerId = D.ID
	WHERE BA.VersionId = @VersionId
		AND BA.DealerId = @DealerId
		AND BA.IsActive = 1
		AND D.IsDealerActive = 1
		AND D.IsDealerDeleted = 0
		AND ApplicationId = 2
END

