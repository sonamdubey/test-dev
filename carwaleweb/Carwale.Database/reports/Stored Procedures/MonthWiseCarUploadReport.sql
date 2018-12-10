IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[MonthWiseCarUploadReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[MonthWiseCarUploadReport]
GO

	-- =============================================
-- Author:		Manish
-- Create date: 21-08-2013
-- Description: Month wise car upload report on Website
--Modified by Reshma Shetty 5/12/2013 To return a day's data if the day is specified and added a condition to consider the newly introduced individual free package
--Modified by Reshma Shetty 6/12/2013 Changed condition for Individual Paid listing since the earlier condition gave count overlapping with the Paid count
-- -- =============================================
CREATE PROCEDURE [reports].[MonthWiseCarUploadReport]
@Month TINYINT,
@Year  SMALLINT,
@Day TINYINT = NULL
AS
BEGIN

DECLARE  @TempTable TABLE (ItemName VARCHAR(150),TotalCount INT,OrderNo TINYINT )


 INSERT INTO @TempTable(ItemName,TotalCount,OrderNo)
 SELECT   'Individual''s Free Car',
          COUNT(*)/case when COUNT(DISTINCT AsOnDate)=0 then 1 else  COUNT(DISTINCT AsOnDate) END AS [No. of Individual Free Car Uploaded],
          1 
 FROM LiveListingsDailyLog AS LL WITH (NOLOCK)
 JOIN CustomerSellInquiries AS CSI WITH (NOLOCK) 
 ON LL.Inquiryid=CSI.ID AND LL.SellerType=2
 AND (CSI.SourceId=36 OR CSI.PackageType=30)--Modified by Reshma Shetty 5/12/2013 to consider the newly introduced individual free package
 AND MONTH(AsOnDate)=@Month
 AND YEAR(AsOnDate)=@Year
 AND (DAY(AsOnDate)=@Day OR @Day IS NULL)--Modified by Reshma Shetty 5/12/2013 To return a day's data if the day is specified
 
 INSERT INTO @TempTable(ItemName,TotalCount,OrderNo)
 SELECT 'Individual''s Paid Car',
         COUNT(*)/case when COUNT(DISTINCT AsOnDate)=0 then 1 else  COUNT(DISTINCT AsOnDate) END AS [No. of Individual Paid Car Uploaded],
         2 
 FROM LiveListingsDailyLog AS LL WITH (NOLOCK)
 JOIN CustomerSellInquiries AS CSI WITH (NOLOCK) 
 ON LL.Inquiryid=CSI.ID AND LL.SellerType=2  
 --AND (CSI.SourceId<>36 OR CSI.SourceId is null OR CSI.PackageType<>30)--Modified by Reshma Shetty 5/12/2013 to consider the newly introduced individual free package
 AND (CSI.SourceId <> 36  and CSI.PackageType IN (2,31))--Modified by Reshma Shetty 6/12/2013 Since the previous condition gave overlapping data
 AND MONTH(AsOnDate)=@Month
 AND YEAR(AsOnDate)=@Year
 AND (DAY(AsOnDate)=@Day OR @Day IS NULL)--Modified by Reshma Shetty 5/12/2013 To return a day's data if the day is specified
 
 INSERT INTO @TempTable(ItemName,TotalCount,OrderNo)
 SELECT 'Total No. of Individual''s Car',
 COUNT(*)/case when COUNT(DISTINCT AsOnDate)=0 then 1 else  COUNT(DISTINCT AsOnDate) END AS [Total No. of Individual Car Uploaded],
 3 
 FROM  LiveListingsDailyLog AS LL WITH (NOLOCK)
 WHERE LL.SellerType=2
 AND MONTH(AsOnDate)=@Month
 AND YEAR(AsOnDate)=@Year
 AND (DAY(AsOnDate)=@Day OR @Day IS NULL)--Modified by Reshma Shetty 5/12/2013 To return a day's data if the day is specified
 
 INSERT INTO @TempTable(ItemName,TotalCount,OrderNo)
 SELECT 'Dealers''s Free Car',
         COUNT(*)/case when COUNT(DISTINCT AsOnDate)=0 then 1 else  COUNT(DISTINCT AsOnDate) end AS [No. of Dealers's Free Car Uploaded],
         4 
 FROM LiveListingsDailyLog AS LL WITH (NOLOCK)
  WHERE  MONTH(AsOnDate)=@Month
  AND YEAR(AsOnDate)=@Year
  AND PackageType=28
  AND SellerType=1
  AND (DAY(AsOnDate)=@Day OR @Day IS NULL)--Modified by Reshma Shetty 5/12/2013 To return a day's data if the day is specified
  
 INSERT INTO @TempTable(ItemName,TotalCount,OrderNo) 
 SELECT 'Dealers''s Paid Car',
         COUNT(*)/case when COUNT(DISTINCT AsOnDate)=0 then 1 else  COUNT(DISTINCT AsOnDate) END AS [No. of Dealers's Paid Car Uploaded],
         5 
  FROM LiveListingsDailyLog AS LL WITH (NOLOCK)
  WHERE  MONTH(AsOnDate)=@Month
     AND YEAR(AsOnDate)=@Year
     AND PackageType<>28
     AND SellerType=1
	 AND (DAY(AsOnDate)=@Day OR @Day IS NULL)--Modified by Reshma Shetty 5/12/2013 To return a day's data if the day is specified
  
 INSERT INTO @TempTable(ItemName,TotalCount,OrderNo) 
 SELECT 'Total Dealer''s Car',
        COUNT(*)/case when COUNT(DISTINCT AsOnDate)=0 then 1 else  COUNT(DISTINCT AsOnDate) END AS [Total No. of Dealer's Car Uploaded],
        6 
 FROM LiveListingsDailyLog AS LL WITH (NOLOCK)
  WHERE  MONTH(AsOnDate)=@Month
     AND YEAR(AsOnDate)=@Year
     AND SellerType=1
	 AND (DAY(AsOnDate)=@Day OR @Day IS NULL)--Modified by Reshma Shetty 5/12/2013 To return a day's data if the day is specified
 
  INSERT INTO @TempTable(ItemName,TotalCount,OrderNo) 
   SELECT 'Total No. of Dealers',
          COUNT(distinct LL.DealerId) AS [Total No. of Dealers's ],
          7 
 FROM LiveListingsDailyLog AS LL WITH (NOLOCK)
 JOIN SellInquiries AS SI WITH (NOLOCK) 
 ON   LL.Inquiryid=SI.ID AND LL.SellerType=1
 AND MONTH(AsOnDate)=@Month
 AND YEAR(AsOnDate)=@Year
 AND (DAY(AsOnDate)=@Day OR @Day IS NULL)--Modified by Reshma Shetty 5/12/2013 To return a day's data if the day is specified
 
 INSERT INTO @TempTable(ItemName,TotalCount,OrderNo) 
 SELECT 'Total No. of Free Dealers',
        COUNT(distinct LL.DealerId) AS [Total No. of Free Dealers's ],
        8 
 FROM LiveListingsDailyLog AS LL WITH (NOLOCK)
 JOIN SellInquiries AS SI WITH (NOLOCK) 
 ON   LL.Inquiryid=SI.ID AND LL.SellerType=1
 AND MONTH(AsOnDate)=@Month
 AND YEAR(AsOnDate)=@Year
 AND LL.PackageType=28
 AND (DAY(AsOnDate)=@Day OR @Day IS NULL)--Modified by Reshma Shetty 5/12/2013 To return a day's data if the day is specified
 
 INSERT INTO @TempTable(ItemName,TotalCount,OrderNo)   
 SELECT 'Total No. of Paid Dealers',
         COUNT(distinct LL.DealerId) AS [Total No. of Paid Dealers's ],
         9 
 FROM LiveListingsDailyLog AS LL WITH (NOLOCK)
 JOIN SellInquiries AS SI WITH (NOLOCK) 
 ON   LL.Inquiryid=SI.ID AND LL.SellerType=1
 AND MONTH(AsOnDate)=@Month
 AND YEAR(AsOnDate)=@Year
 AND LL.PackageType<>28
 AND (DAY(AsOnDate)=@Day OR @Day IS NULL)--Modified by Reshma Shetty 5/12/2013 To return a day's data if the day is specified
 
 SELECT ItemName,TotalCount,@Day AS [Day], @Month As [Month] , @Year as [Year] FROM @TempTable ORDER BY OrderNo 
 
END