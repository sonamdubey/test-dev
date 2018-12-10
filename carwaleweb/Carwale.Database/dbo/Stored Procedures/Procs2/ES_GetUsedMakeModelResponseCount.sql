IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ES_GetUsedMakeModelResponseCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ES_GetUsedMakeModelResponseCount]
GO

	
-- =============================================
-- Author:		Ashwini
-- Create date: 16-Sept-2015
-- Description:	Get user response count for a make and model stock of  used cars
-- =============================================
CREATE PROCEDURE [dbo].[ES_GetUsedMakeModelResponseCount]
AS
BEGIN
	SELECT CM.ID MakeId
		,Count(A.ResponseId) ResponseCount
	FROM CarMakes CM WITH(NOLOCK)
	INNER JOIN Carmodels CMO WITH(NOLOCK) ON CMO.CarMakeId = CM.ID
	INNER JOIN CarVersions CV WITH(NOLOCK) ON CV.CarModelId = CMO.ID
	LEFT JOIN (
		SELECT u.id ResponseId
			,s.CarVersionId
			,U.RequestDateTime
		FROM UsedCarPurchaseInquiries AS u WITH(NOLOCK)
		JOIN SellInquiries AS s WITH(NOLOCK) ON u.SellInquiryId = s.ID
		WHERE U.RequestDateTime >= GETDATE() - 30
		
		UNION ALL
		
		SELECT u.id ResponseId
			,s.CarVersionId
			,U.RequestDateTime
		FROM ClassifiedRequests AS u WITH(NOLOCK)
		JOIN CustomerSellInquiries AS s WITH(NOLOCK) ON u.SellInquiryId = s.ID
		WHERE U.RequestDateTime >= GETDATE() - 30
		) AS A ON A.CarVersionId = CV.ID
	GROUP BY CM.ID

	SELECT cr.RootId RootId
		,Count(A.ResponseId) ResponseCount
	FROM CarModels cm WITH(NOLOCK)
	INNER JOIN CarModelRoots cr WITH(NOLOCK) ON cr.RootId = cm.RootId
	INNER JOIN CarVersions cv WITH(NOLOCK) ON cv.CarModelId = cm.ID
	LEFT JOIN (
		SELECT u.id ResponseId
			,s.CarVersionId
			,U.RequestDateTime
		FROM UsedCarPurchaseInquiries AS u WITH (NOLOCK)
		JOIN SellInquiries AS s WITH (NOLOCK) ON u.SellInquiryId = s.ID
		WHERE U.RequestDateTime >= GETDATE() - 30
	
		UNION ALL
	
		SELECT u.id ResponseId
			,s.CarVersionId
			,U.RequestDateTime
		FROM ClassifiedRequests AS u WITH (NOLOCK)
		JOIN CustomerSellInquiries AS s WITH (NOLOCK) ON u.SellInquiryId = s.ID
		WHERE U.RequestDateTime >= GETDATE() - 30
		) AS A ON A.CarVersionId = CV.ID
	GROUP BY cr.RootId

END