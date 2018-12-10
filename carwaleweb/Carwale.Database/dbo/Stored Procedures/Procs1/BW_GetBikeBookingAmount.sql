IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetBikeBookingAmount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetBikeBookingAmount]
GO

	
-- =============================================
-- Author:		Ashwini Todkar
-- Create date: 17th Dec 2014
-- Description:	To get bike booking amount of a dealer
-- EXEC BW_GetBikeBookingAmount 4
-- Modified By : Sushil kumar 11 Aug 2015
-- Summary : To get New bike Booking Amount List
-- Modified By : Sadhana Upadhyay on 9 Oct 2015
-- Summary : To get only new new models
--Modified by Sadhana on 13-10-2015 commented condition bmo.used
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetBikeBookingAmount] @DealerId INT
AS
BEGIN
	SELECT BA.ID AS Id
		,BM.NAME AS BikeMake
		,BMO.NAME AS BikeModel
		,BV.Name AS BikeVersion
		,BA.Amount AS Amount
	FROM BW_DealerBikeBookingAmounts AS BA WITH(NOLOCK)
	INNER JOIN BikeVersions AS BV WITH (NOLOCK)  ON BV.ID=BA.VersionId
	INNER JOIN BikeModels AS BMO WITH (NOLOCK) ON BMO.ID = BV.BikeModelId
	INNER JOIN BikeMakes AS BM WITH (NOLOCK) ON BM.ID = BMO.BikeMakeId
	WHERE BA.IsActive = 1
		AND BA.DealerId = @DealerId
		AND BA.Amount >= 0
		--AND BMO.Used = 0
		AND BMO.New = 1
		AND BV.New = 1 
	ORDER BY BA.Id DESC
END
