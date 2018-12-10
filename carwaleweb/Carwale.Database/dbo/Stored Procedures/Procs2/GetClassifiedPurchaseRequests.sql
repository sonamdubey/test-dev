IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetClassifiedPurchaseRequests]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetClassifiedPurchaseRequests]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 18 Nov 2013
-- Description:	Proc to get customer details for a given inquiry id
-- =============================================
CREATE PROCEDURE GetClassifiedPurchaseRequests

	-- Add the parameters for the stored procedure here	
	@InquiryId BIGINT,
	@CustomerId BIGINT,
	@RequestDate SMALLINT = 7
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
 
	IF (@RequestDate = -1)
	BEGIN
		SELECT C.Name,C.Mobile, C.email, R.RequestDateTime FROM 
		Customers C
		INNER JOIN ClassifiedRequests R ON R.CustomerId = C.Id
		INNER JOIN CustomerSellInquiries CSI ON R.SellInquiryId = CSI.ID
		WHERE CSI.ID = @InquiryId AND CSI.CustomerId = @CustomerId

		SELECT COUNT(R.SellInquiryId) AS TotalPurchaseRequests FROM 
		Customers C
		INNER JOIN ClassifiedRequests R ON R.CustomerId = C.Id
		INNER JOIN CustomerSellInquiries CSI ON R.SellInquiryId = CSI.ID
		WHERE CSI.ID = @InquiryId AND CSI.CustomerId = @CustomerId
	END
	ELSE
	BEGIN
		DECLARE @StartDate DATETIME, @endDate DATETIME
		SET @StartDate = CONVERT(DATETIME, CONVERT(VARCHAR(10), GETDATE() - @RequestDate, 120)+ ' 00:00:00')

		IF (@RequestDate = 1)
			SET @endDate = CONVERT(DATETIME, CONVERT(VARCHAR(10), GETDATE()-1, 120)+ ' 23:59:59');
		ELSE
			SET @endDate = CONVERT(DATETIME, CONVERT(VARCHAR(10), GETDATE(), 120)+ ' 23:59:59');

		SELECT C.Name,C.Mobile, C.email, R.RequestDateTime 
		FROM Customers C
		INNER JOIN ClassifiedRequests R ON R.CustomerId = C.Id
		INNER JOIN CustomerSellInquiries CSI ON R.SellInquiryId = CSI.ID
		WHERE CSI.ID = @InquiryId AND CSI.CustomerId = @CustomerId AND R.RequestDateTime BETWEEN @StartDate AND @endDate
		GROUP BY R.SellInquiryId, C.Name,C.Mobile, C.email, R.RequestDateTime

		SELECT COUNT(R.SellInquiryId) AS TotalPurchaseRequests
		FROM Customers C
		INNER JOIN ClassifiedRequests R ON R.CustomerId = C.Id
		INNER JOIN CustomerSellInquiries CSI ON R.SellInquiryId = CSI.ID
		WHERE CSI.ID = @InquiryId AND CSI.CustomerId = @CustomerId AND R.RequestDateTime BETWEEN @StartDate AND @endDate
		GROUP BY R.SellInquiryId
	END
END
