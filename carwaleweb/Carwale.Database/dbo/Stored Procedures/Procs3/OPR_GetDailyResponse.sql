IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OPR_GetDailyResponse]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OPR_GetDailyResponse]
GO

	-- =============================================
-- Author:		Amit Yadav
-- Create date: 19th Nov 2015
-- Description:	To get the daily response in DealerDailyResponse
-- =============================================
CREATE PROCEDURE [dbo].[OPR_GetDailyResponse]

	@DealerId AS INT = NULL,
	@StartDate AS DATETIME=NULL,  
	@EndDate AS DATETIME =NULL

AS
BEGIN
	SELECT RDTDay,RDTMonth,RDTYear, SUM(TResponse) AS TResponse FROM (
		SELECT Day(UPI.RequestDateTime) AS RDTDay, DateName(Month,UPI.RequestDateTime) AS RDTMonth, YEAR(UPI.RequestDateTime) AS RDTYear,
			   COUNT(DISTINCT UPI.CustomerID) AS TResponse 
		FROM Dealers AS D WITH(NOLOCK)  
			 INNER JOIN SellInquiries AS SI WITH(NOLOCK) ON SI.DealerId =D.ID AND D.ID = @DealerId
			 LEFT JOIN UsedCarPurchaseInquiries AS UPI WITH(NOLOCK) ON  UPI.SellInquiryId = SI.ID
			  
		WHERE UPI.RequestDateTime BETWEEN @StartDate  AND @EndDate+1
			 
		GROUP BY Day(UPI.RequestDateTime),DateName(Month,UPI.RequestDateTime),YEAR(UPI.RequestDateTime)
		
		UNION 
		
		SELECT Day(MMI.CreatedOn) AS RDTDay, DateName(Month,MMI.CreatedOn) AS RDTMonth, YEAR(MMI.CreatedOn) AS RDTYear,
			   COUNT(DISTINCT MMI.MM_InquiriesID) AS TResponse
		FROM Dealers AS D WITH(NOLOCK) 
			 LEFT JOIN MM_Inquiries AS MMI WITH(NOLOCK) ON MMI.ConsumerId = D.ID  
			  
		WHERE  
	      MMI.ConsumerType = 1 AND MMI.ProductTypeId = 1 
		  AND MMI.CreatedOn BETWEEN @StartDate AND @EndDate+1
		  AND D.ID = @DealerId

		GROUP BY  Day(MMI.CreatedOn),DateName(Month,MMI.CreatedOn),YEAR(MMI.CreatedOn)) AS T

	GROUP BY RDTDay,RDTMonth,RDTYear
	ORDER BY RDTYear, RDTMonth , RDTDay
 
END


-----------------------------------------------------------------------------------------


