IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UsedCarStocksOnPrice]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UsedCarStocksOnPrice]
GO

	
CREATE PROCEDURE [dbo].[UsedCarStocksOnPrice]
@MinPrice NUmeric(18,0)=0,
@MaxPrice NUMERIC(18,0)=NULL,
@DealerId NUMERIC(18,0)
AS 
--Author: Rakesh Yadav on 11 Jun 2015
--Desc: Fetch stocks on price ranges
--Modified By: rakesh yadav on 12 Aug 2015 to get OriginalImgPath
--Modified By: Vaibhav K(23-Dec-2015) to show unique stocks with main image if present or else any image
BEGIN
	/*
	SELECT top 10 CP.HostUrl,CP.DirectoryPath,CP.ImageUrlMedium,CP.OriginalImgPath,vm.Make,vm.Model,vm.Version,TS.Kms,TS.Price,TS.RegNo,TS.Id
	FROM 
	TC_Stock TS 
	JOIN TC_CarPhotos CP ON TS.Id=CP.StockId AND CP.IsActive=1
	JOIN vwMMV vm ON TS.VersionId=vm.VersionId
	WHERE TS.Price >= @MinPrice AND ( @MaxPrice IS NULL OR TS.Price<=@MaxPrice)
	AND TS.BranchId=@DealerId AND TS.IsActive=1 AND TS.IsApproved=1 AND TS.StatusId=1
	ORDER BY TS.IsFeatured
	*/
	SELECT TOP 10 T.*
	FROM
	(SELECT 
	CP.HostUrl,CP.DirectoryPath,CP.ImageUrlMedium,CP.OriginalImgPath,vm.Make,vm.Model,vm.Version,TS.Kms,TS.Price,TS.RegNo,TS.Id,TS.IsFeatured
	,cp.IsMain
	,ROW_NUMBER() OVER(PARTITION BY TS.Id ORDER BY CP.IsMain DESC ) AS RowNum
	FROM 
	TC_Stock TS WITH (NOLOCK)
	JOIN TC_CarPhotos CP WITH (NOLOCK) ON TS.Id=CP.StockId AND CP.IsActive=1
	JOIN vwMMV vm WITH (NOLOCK) ON TS.VersionId=vm.VersionId
	WHERE TS.BranchId=@DealerId AND TS.IsActive=1 AND TS.IsApproved=1 AND TS.StatusId=1) T
	WHERE T.RowNum = 1
	ORDER BY T.IsFeatured
END


