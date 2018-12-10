IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateAllCarsResponsesOnLivelistings]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateAllCarsResponsesOnLivelistings]
GO

	-- ============================================= 
-- Author:    Manish 
-- Create date: 17-APR-2014 
-- Description: Update the Response of the all used cars in Livelistings table. 
-- ============================================= 
CREATE PROCEDURE [dbo].[UpdateAllCarsResponsesOnLivelistings]
AS
BEGIN

         --------------Update Response for Dealers Car
		UPDATE LL SET LL.Responses=[Count]
		  FROM   
		  LiveListings AS LL WITH (NOLOCK)
		  JOIN 
		  (
			SELECT LL.InquiryId,LL.SellerType,COUNT(UCPI.ID) AS [Count]
			FROM 	LiveListings AS LL WITH (NOLOCK)
			LEFT JOIN UsedCarPurchaseInquiries AS UCPI WITH (NOLOCK) 
			                     ON  UCPI.sellinquiryid = LL.Inquiryid
			                     AND  UCPI.IsApproved=1
			                     AND  UCPI.IsFake=0 
			WHERE  LL.SellerType=1
			GROUP BY LL.InquiryId,LL.SellerType
		  ) AS A
		   ON  A.inquiryid = LL.Inquiryid
		  AND  A.SellerType= LL.SellerType

          --------------Update Response for Individuals Car
		UPDATE LL SET LL.Responses=[Count]
		  FROM   
		  LiveListings AS LL WITH (NOLOCK)
		  JOIN 
		  (
			SELECT LL.InquiryId,LL.SellerType,COUNT(UCPI.ID) AS [Count]
			FROM 	LiveListings AS LL WITH (NOLOCK)
			LEFT JOIN    ClassifiedRequests AS UCPI WITH (NOLOCK) 
			           ON  UCPI.sellinquiryid = LL.Inquiryid
			           AND  UCPI.IsActive=1
			WHERE  LL.SellerType=2
			GROUP BY LL.InquiryId,LL.SellerType
		  ) AS A
		   ON  A.inquiryid = LL.Inquiryid
		  AND  A.SellerType= LL.SellerType
END

