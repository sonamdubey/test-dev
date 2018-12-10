IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_FetchSellInqueriesForCommentRating]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_FetchSellInqueriesForCommentRating]
GO
	-- =============================================
-- Author:		Chetan Kane
-- Create date: 9th August 2012
-- Description:	to fetch sell inqueries of a perticular dear to rate the comments on the car 
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_FetchSellInqueriesForCommentRating]
	
	@DealerId NUMERIC
AS	
BEGIN
	SELECT  DISTINCT 
		SI.ID AS Id, LL.MakeName AS Make, LL.ModelName AS Model, LL.VersionName AS Version, 
		SI.Color AS Color, SI.CarRegNo AS RegNo, SI.Comments AS Comments , SI.DescRating AS Rating

	FROM
		SellInquiries SI WITH (NOLOCK) INNER JOIN  LiveListings LL ON LL.Inquiryid = SI.ID AND LL.SellerType = 1

	WHERE 
		SI.DealerId = @DealerId 
		ORDER BY LL.MakeName
END
