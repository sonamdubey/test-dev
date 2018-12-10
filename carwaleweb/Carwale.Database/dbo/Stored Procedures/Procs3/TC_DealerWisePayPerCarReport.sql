IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerWisePayPerCarReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerWisePayPerCarReport]
GO

	
-- =============================================
-- Author:		Manish
-- Create date: 30-07-2013
-- Description: Month wise car upload report for Carnation Mumbai dealer
-- No. Of Cars	Cost per Day
--- 0-200      	5
--  201-500	    4
--  501-750	    3.5
--  751-1000    3
-- Modified by Manish on 27-08-2013 changing the whole logic of the report capturing unique cars only
-- Modified by Manish on 13-11-2014 commented condition  TotalLiveCars<=1000 
-- -- =============================================
CREATE PROCEDURE [dbo].[TC_DealerWisePayPerCarReport]
@Month TINYINT,
@YEAR  SMALLINT,
@Organization VARCHAR(150)
AS 
BEGIN 

DECLARE @FirstSlab INT =1000,
        @SecondSlab INT= 1200,
        @ThirdSlab INT=875,
        @FourthSlab INT=750;

WITH CTE AS (
				
				SELECT    ISNULL([A].[Day],[B].[Day] ) [Day], 
				isnull([No. of Car on Website at the start of the day (A)],0) [No. of Car on Website at the start of the day (A)],
				ABS(ISNULL ([No. of Car Uploaded during the day(B)],0)-isnull([No. of Duplicate Car Uploaded],0)) [No. of New Cars Uploaded during the day(C)],
				isnull( [No. of Car on Website at the start of the day (A)],0) +ABS(ISNULL ([No. of Car Uploaded during the day(B)],0)-isnull([No. of Duplicate Car Uploaded],0)) AS [TotalLiveCars], 
				ISNULL ([No. of Car Uploaded during the day(B)],0) AS [No. of Car Uploaded during the day(B)],                                 
				ISNULL ([No. of Cars Removed during the day(D)],0) AS [No. of Cars Removed during the day(D)]
						 FROM 
				(	
				
				 SELECT CONVERT(DATE,LL.AsOnDate) [Day],
			  			COUNT(DISTINCT LL.ProfileId)  [No. of Car on Website at the start of the day (A)]
				 FROM LiveListingsDailyLog AS LL WITH (NOLOCK)
				 JOIN SellInquiries AS SI WITH (NOLOCK) ON LL.Inquiryid=SI.ID 
				 JOIN TC_DealerMappingPayPerCarReport AS DM  WITH (NOLOCK) ON DM.DealerId=SI.DealerId
				 JOIN TC_OrganizationListPayPerCar AS OL WITH (NOLOCK) ON DM.TC_OrganizationListPayPerCarId=OL.TC_OrganizationListPayPerCarId
				 WHERE LL.SellerType=1 
				 AND   OL.Organization=@Organization 
				 AND OL.IsActive=1
				 AND DM.IsActive=1     
				 AND   MONTH(AsOnDate)=@Month
				 AND   YEAR(AsOnDate)=@YEAR
				 GROUP BY CONVERT(DATE,LL.AsOnDate)
				) A
				FULL OUTER JOIN  
				(   SELECT CONVERT(DATE,SI.CreatedOn) [Day],
							   ISNULL(COUNT(DISTINCT (CASE WHEN SI.IsCarUploaded=1 THEN SellInquiriesId END )),0) [No. of Car Uploaded during the day(B)],
							   ISNULL(COUNT(DISTINCT (CASE WHEN SI.IsCarUploaded=0 THEN SellInquiriesId END )),0) [No. of Cars Removed during the day(D)]
							FROM TC_StockUploadedLog AS SI	WITH (NOLOCK)
					   JOIN TC_DealerMappingPayPerCarReport AS DM  WITH (NOLOCK) ON DM.DealerId=SI.DealerId
				       JOIN TC_OrganizationListPayPerCar AS OL WITH (NOLOCK) ON DM.TC_OrganizationListPayPerCarId=OL.TC_OrganizationListPayPerCarId
				       WHERE  OL.Organization=@Organization      
						AND OL.IsActive=1
				        AND DM.IsActive=1
						AND MONTH(CreatedOn)=@Month
						AND YEAR(CreatedOn)=@YEAR
						GROUP BY CONVERT(DATE,SI.CreatedOn)
				)	B  ON [A].[Day]=[B].[DAY] 
				LEFT JOIN
				( SELECT CONVERT(DATE,SI.CreatedOn) [Day],
				ISNULL(COUNT(DISTINCT (LL.Inquiryid)),0) [No. of Duplicate Car Uploaded]
				   FROM  LiveListingsDailyLog AS LL WITH (NOLOCK)
				   JOIN TC_StockUploadedLog AS SI  WITH (NOLOCK) ON LL.Inquiryid=SI.SellInquiriesId AND LL.SellerType=1 
				   JOIN SellInquiries AS SEL WITH (NOLOCK) ON SEL.ID=LL.Inquiryid
				   JOIN TC_DealerMappingPayPerCarReport AS DM WITH (NOLOCK) ON DM.DealerId=SI.DealerId AND DM.IsActive=1
				    JOIN TC_OrganizationListPayPerCar AS OL WITH (NOLOCK) ON DM.TC_OrganizationListPayPerCarId=OL.TC_OrganizationListPayPerCarId
				     WHERE CONVERT(DATE,si.CreatedOn)=ll.AsOnDate  
				     AND MONTH(LL.AsOnDate)=@Month 
				     AND YEAR(LL.AsOnDate)=@YEAR
				     AND SI.IsCarUploaded=1
				     AND OL.Organization=@Organization    
				     	GROUP BY CONVERT(DATE,SI.CreatedOn)
				) c			    	
				 ON [A].[Day]=[C].[DAY]  
            )
	SELECT CONVERT(VARCHAR,[Day]) [Day],
		   [No. of Car on Website at the start of the day (A)],
		   [No. of Car Uploaded during the day(B)],
		   [No. of New Cars Uploaded during the day(C)],
		   [No. of Cars Removed during the day(D)],
		 TotalLiveCars AS   [Total No. of Unique Cars came on Website in Whole Day(A+C)],
		   CASE WHEN  TotalLiveCars<=200 THEN 1000
				WHEN  TotalLiveCars>200 AND TotalLiveCars<=500 THEN @FirstSlab+((TotalLiveCars-200)*4)
				WHEN  TotalLiveCars>500 AND TotalLiveCars<=750 THEN @FirstSlab+@SecondSlab +((TotalLiveCars-500)*3.5)
				WHEN  TotalLiveCars>750 /*AND TotalLiveCars<=1000*/ THEN @FirstSlab+@SecondSlab+@ThirdSlab+((TotalLiveCars-750)*3)
			END   AS [Cost per Day]    
	FROM CTE
	ORDER BY  [Day]
END