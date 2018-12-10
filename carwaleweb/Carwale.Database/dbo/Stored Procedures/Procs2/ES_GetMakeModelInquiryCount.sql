IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ES_GetMakeModelInquiryCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ES_GetMakeModelInquiryCount]
GO

	
-- =============================================
-- Author:		Ashwini
-- Create date: 16-sept-2015
-- Description:	Make model inquiry count
-- =============================================
CREATE PROCEDURE [dbo].[ES_GetMakeModelInquiryCount] 
AS
BEGIN
	SELECT V.MakeId
		,COUNT(N.ID) InquiriesCount
	FROM VWMMV AS V WITH(NOLOCK)
	LEFT JOIN NewCarPurchaseInquiries AS N WITH(NOLOCK) ON N.CarVersionId = V.VersionId
	WHERE N.RequestDateTime <= GETDATE() - 7
		AND IsApproved = 1
		AND IsFake = 0
	GROUP BY V.MakeId

	SELECT V.ModelId
		,COUNT(N.ID) InquiriesCount
	FROM VWMMV AS V WITH(NOLOCK)
	LEFT JOIN NewCarPurchaseInquiries AS N WITH(NOLOCK) ON N.CarVersionId = V.VersionId
	WHERE N.RequestDateTime <= GETDATE() - 7
		AND IsApproved = 1
		AND IsFake = 0
	GROUP BY V.MOdelId
END