IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerWiseDetailPayPerCarReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerWiseDetailPayPerCarReport]
GO

	
-- =============================================
-- Author:		Manish
-- Create date: 30-09-2013
-- Description: Month wise car upload report for Carnation Mumbai dealer contains all dealers mapped to carnation
-- Modified By: Manish on 11-11-2014  added one more field day wise no. of inquiries.
-- -- =============================================
CREATE PROCEDURE [dbo].[TC_DealerWiseDetailPayPerCarReport]
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
				ISNULL(A.DealerId,B.DEALERID) AS DealerId,
				ISNULL(A.ORGANISATION,B.ORGANISATION) AS Organisation,
				isnull([No. of Car on Website at the start of the day (A)],0) [No. of Car on Website at the start of the day (A)],
				ABS(ISNULL ([No. of Car Uploaded during the day(B)],0)-isnull([No. of Duplicate Car Uploaded],0)) [No. of New Cars Uploaded during the day(C)],
				isnull( [No. of Car on Website at the start of the day (A)],0) +ABS(ISNULL ([No. of Car Uploaded during the day(B)],0)-isnull([No. of Duplicate Car Uploaded],0)) AS [TotalLiveCars], 
				ISNULL ([No. of Car Uploaded during the day(B)],0) AS [No. of Car Uploaded during the day(B)],                                 
				ISNULL ([No. of Cars Removed during the day(D)],0) AS [No. of Cars Removed during the day(D)]
						 FROM 
				(	
				
				 SELECT CONVERT(DATE,LL.AsOnDate) [Day],
			  			COUNT(DISTINCT LL.ProfileId)  [No. of Car on Website at the start of the day (A)],
						DM.DealerId AS DealerId,
						D.Organization AS ORGANISATION
				 FROM LiveListingsDailyLog AS LL WITH (NOLOCK)
				 JOIN SellInquiries AS SI WITH (NOLOCK) ON LL.Inquiryid=SI.ID 
				 JOIN TC_DealerMappingPayPerCarReport AS DM  WITH (NOLOCK) ON DM.DealerId=SI.DealerId
				 JOIN TC_OrganizationListPayPerCar AS OL WITH (NOLOCK) ON DM.TC_OrganizationListPayPerCarId=OL.TC_OrganizationListPayPerCarId
				 JOIN Dealers  AS D  WITH (NOLOCK) ON D.ID=DM.DealerId
				 WHERE LL.SellerType=1 
				 AND   OL.Organization=@Organization 
				 AND OL.IsActive=1
				 AND DM.IsActive=1     
				 AND   MONTH(AsOnDate)=@Month
				 AND   YEAR(AsOnDate)=@YEAR
				 GROUP BY CONVERT(DATE,LL.AsOnDate),DM.DealerId,D.Organization
				) A
				FULL OUTER JOIN 
				(   SELECT CONVERT(DATE,SI.CreatedOn) [Day],
							   ISNULL(COUNT(DISTINCT (CASE WHEN SI.IsCarUploaded=1 THEN SellInquiriesId END )),0) [No. of Car Uploaded during the day(B)],
							   ISNULL(COUNT(DISTINCT (CASE WHEN SI.IsCarUploaded=0 THEN SellInquiriesId END )),0) [No. of Cars Removed during the day(D)],
							   SI.DealerId AS DealerId,
						        D.Organization AS ORGANISATION
							FROM TC_StockUploadedLog AS SI	WITH (NOLOCK)
					   JOIN TC_DealerMappingPayPerCarReport AS DM  WITH (NOLOCK) ON DM.DealerId=SI.DealerId
				       JOIN TC_OrganizationListPayPerCar AS OL WITH (NOLOCK) ON DM.TC_OrganizationListPayPerCarId=OL.TC_OrganizationListPayPerCarId
					   JOIN Dealers  AS D  WITH (NOLOCK) ON D.ID=DM.DealerId AND D.ID=SI.DealerId
				       WHERE  OL.Organization=@Organization      
						AND OL.IsActive=1
				        AND DM.IsActive=1
						AND MONTH(CreatedOn)=@Month
						AND YEAR(CreatedOn)=@YEAR
						GROUP BY CONVERT(DATE,SI.CreatedOn),SI.DealerId,D.Organization
				)	B  ON [A].[Day]=[B].[DAY]  AND  [A].[DealerID]=[B].[DealerID] AND [A].[Organisation]=[B].[Organisation]
				LEFT JOIN
				( SELECT CONVERT(DATE,SI.CreatedOn) [Day],
				ISNULL(COUNT(DISTINCT (LL.Inquiryid)),0) [No. of Duplicate Car Uploaded],
         				DM.DealerId AS DealerId,
						 D.Organization AS ORGANISATION
				   FROM  LiveListingsDailyLog AS LL WITH (NOLOCK)
				   JOIN TC_StockUploadedLog AS SI  ON LL.Inquiryid=SI.SellInquiriesId AND LL.SellerType=1 
				   JOIN SellInquiries AS SEL ON SEL.ID=LL.Inquiryid AND SI.DealerId=SEL.DealerId
				   JOIN TC_DealerMappingPayPerCarReport AS DM ON DM.DealerId=SI.DealerId AND DM.IsActive=1
				   JOIN TC_OrganizationListPayPerCar AS OL WITH (NOLOCK) ON DM.TC_OrganizationListPayPerCarId=OL.TC_OrganizationListPayPerCarId
				   JOIN Dealers  AS D  WITH (NOLOCK) ON D.ID=DM.DealerId
				     WHERE CONVERT(DATE,si.CreatedOn)=ll.AsOnDate  
				     AND MONTH(LL.AsOnDate)=@Month 
				     AND YEAR(LL.AsOnDate)=@YEAR
				     AND SI.IsCarUploaded=1
				     AND OL.Organization=@Organization    
				     	GROUP BY CONVERT(DATE,SI.CreatedOn),DM.DealerId,D.Organization
				) c			    	
				 ON [A].[Day]=[C].[DAY]  
				  AND  [A].[DealerID]=[C].[DealerID] AND [A].[Organisation]=[C].[Organisation]
            )
	SELECT CONVERT(VARCHAR,CTE.[Day]) [Day],
	        CTE.DealerId AS DealerId,
			Organisation,
		   [No. of Car on Website at the start of the day (A)],
		   [No. of Car Uploaded during the day(B)],
		   [No. of New Cars Uploaded during the day(C)],
		   [No. of Cars Removed during the day(D)],
		   TotalLiveCars AS   [Total No. of Unique Cars came on Website in Whole Day(A+C)],
		   ISNULL(A.TotalInquiryReceived,0) AS TotalInquiryReceived
	FROM CTE
  LEFT	JOIN 
	  (
		SELECT d.id AS DealerId, 
		d.organization AS DealerName,
		@YEAR AS [Year],
		@Month AS [Month],
		CONVERT(DATE,UCPI.RequestDateTime) [Day] ,
		COUNT(DISTINCT UCPI.ID) TotalInquiryReceived
		FROM  
		TC_DealerMappingPayPerCarReport AS DM WITH (NOLOCK)
		JOIN TC_OrganizationListPayPerCar AS OL WITH (NOLOCK) ON DM.TC_OrganizationListPayPerCarId=OL.TC_OrganizationListPayPerCarId
		JOIN Dealers  AS D  WITH (NOLOCK) ON D.ID=DM.DealerId
		JOIN SellInquiries AS SI WITH (NOLOCK) ON SI.DealerId=D.ID
		JOIN UsedCarPurchaseInquiries AS UCPI WITH(NOLOCK) ON UCPI.SellInquiryId=SI.ID
		AND  Year(UCPI.RequestDateTime)=@YEAR
		AND Month(UCPI.RequestDateTime)=@Month
		WHERE  OL.Organization=@Organization
		GROUP BY D.ID,D.organization,CONVERT(DATE,UCPI.RequestDateTime)
	   )  A  ON A.DealerId=CTE.DealerId AND CONVERT(DATE,CTE.[Day])=CONVERT(DATE,A.[Day])
	ORDER BY  CTE.[Day]
END