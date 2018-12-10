IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetTop5CarInUsedCarShowroom]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetTop5CarInUsedCarShowroom]
GO

	-- =============================================
-- Modified By: Binumon George & Avishkar
-- Create date: 24 Apr,2014
-- Description:	Used union for displaying featured and defaulf featured cars
-- =============================================
-- Author:		Surendra Chouksey
-- Create date: 14 Dec,2011
-- Description:	This procedure will return top 5 featured car
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetTop5CarInUsedCarShowroom]
(
@DealerId NUMERIC
)	
AS
BEGIN
WITH DealerCars AS
       (SELECT CP.Id,CP.ImageUrlThumbSmall,CP.HostURL,CP.DirectoryPath,FC.DealerId,
               ('D' + Convert(Varchar, LL.InquiryId)) ProfileId,
               LL.InquiryId,(LL.MakeName +' '+ LL.ModelName +' '+ LL.VersionName) AS Car,LL.Price, LL.MakeYear,
               LL.Kilometers, LL.Color, FC.UpdateOn as DealerFeatureUpdated,  
               ROW_NUMBER() OVER (PARTITION BY CP.InquiryId ORDER BY CP.Id DESC) AS RowNumber  FROM LiveListings LL  
               JOIN SellInquiries as S on LL.Inquiryid=S.ID and LL.SellerType=1  
               JOIN DealerFeaturedCars FC ON LL.Inquiryid=FC.CarId  
               --LEFT OUTER JOIN DealerFeaturedCars FC ON LL.Inquiryid=FC.CarId
               LEFT OUTER JOIN CarPhotos CP ON LL.InquiryId=CP.InquiryId AND CP.IsMain=1  AND CP.IsActive=1 AND CP.IsApproved=1  and CP.IsDealer=1
               where  S.DealerId=@DealerId
UNION
SELECT CP.Id,CP.ImageUrlThumbSmall,CP.HostURL,CP.DirectoryPath,S.DealerId,
               ('D' + Convert(Varchar, LL.InquiryId)) ProfileId,
               LL.InquiryId,(LL.MakeName +' '+ LL.ModelName +' '+ LL.VersionName) AS Car,LL.Price, LL.MakeYear,
               LL.Kilometers, LL.Color, NULL as DealerFeatureUpdated,  
               (ROW_NUMBER() OVER (PARTITION BY CP.InquiryId ORDER BY CP.Id DESC)+5) AS RowNumber  FROM LiveListings LL  
               JOIN SellInquiries as S on LL.Inquiryid=S.ID and LL.SellerType=1
               --LEFT OUTER JOIN DealerFeaturedCars FC ON LL.Inquiryid=FC.CarId         and FC.CarId is null
               LEFT OUTER JOIN CarPhotos CP ON LL.InquiryId=CP.InquiryId AND CP.IsMain=1  AND CP.IsActive=1 AND CP.IsApproved=1  and CP.IsDealer=1
               WHERE  S.DealerId=@DealerId
               and LL.Inquiryid NOT IN ( SELECT CarId FROM DealerFeaturedCars WHERE DealerId=@DealerId )                
               )
SELECT top 5 * FROM DealerCars
ORDER BY RowNumber asc,DealerFeatureUpdated desc

END