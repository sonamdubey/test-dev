IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OPR_GetDealerLeadDetail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OPR_GetDealerLeadDetail]
GO

	-- =============================================
-- Author:		Amit Yadav
-- Create date: 20th Nov,2015
-- Description:	To get Dealer Lead Detail.
-- =============================================
CREATE PROCEDURE [dbo].[OPR_GetDealerLeadDetail] 
		
	@DealerId AS INT = NULL,
	@Day AS VARCHAR(5) =NULL,
	@Month AS VARCHAR(20) =NULL,
	@Year AS VARCHAR(10) =NULL
AS
BEGIN

    -- Avoid extra message 
	 SET NOCOUNT ON

		SELECT 	
			DATENAME(MONTH,UPI.RequestDateTime) AS RDTMonth, C.Mobile AS Mobile, C.Name AS Customer, C.email AS Email, SI.ID AS ProfileId, VM.Car AS CarName

			FROM Dealers AS D WITH(NOLOCK)  
			 INNER JOIN SellInquiries AS SI WITH(NOLOCK) ON SI.DealerId =D.ID 
			 INNER JOIN vwMMV VM WITH(NOLOCK) ON vm.VersionId = SI.CarVersionId
			 LEFT JOIN UsedCarPurchaseInquiries AS UPI WITH(NOLOCK) ON  UPI.SellInquiryId = SI.ID
			 LEFT JOIN Customers AS C WITH(NOLOCK) ON C.Id = UPI.CustomerID
				
			WHERE  
				DAY(UPI.RequestDateTime)= @Day AND DATENAME(MONTH,UPI.RequestDateTime) =  @Month AND YEAR(UPI.RequestDateTime) = @Year AND D.ID = @DealerId 	
	UNION
		SELECT 
			DATENAME(MONTH,MMI.CreatedOn) AS RDTMonth, MMI.BuyerMobile AS Mobile, '' AS Customer, '' AS  Email, 0 AS ProfileId, '' AS CarName
		
		FROM MM_Inquiries AS MMI WITH(NOLOCK)  
		
		WHERE 
			DAY(MMI.CreatedOn)= @Day AND DATENAME(MONTH,MMI.CreatedOn) = @Month AND YEAR(MMI.CreatedOn) = @Year
			AND MMI.ConsumerType=1 AND MMI.ConsumerId =@DealerId


END

-----------------------------------------------------------------------------------------



