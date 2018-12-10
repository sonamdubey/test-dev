IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetDealersWithNoRcForSMS]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetDealersWithNoRcForSMS]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 30th july 2015
-- Description:	to fetch dealer mobile number for which rc image is pending to upload
-- Modified By : Ashwini Dhamankar on August 4,2015 , added constraints of RCImagePending
-- Modified By : Ashwini Dhamankar on Oct 9,2015 , Fetched RegNumber
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetDealersWithNoRcForSMS]
AS
BEGIN
	SELECT DISTINCT RC.AbSure_CarDetailsId,D.Id DealerId,D.MobileNo,ACD.StockId,ACD.RegNumber
	FROM AbSure_CarsWithoutRCImage RC WITH(NOLOCK)
	INNER JOIN AbSure_CarDetails ACD WITH(NOLOCK) ON ACD.Id = RC.AbSure_CarDetailsId
	INNER JOIN Dealers D WITH(NOLOCK) ON ACD.DealerId = D.ID
	WHERE ISNULL(ACD.RCImagePending,0) = 1
END

