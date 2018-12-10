IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUsedCarBuyerCustomerCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUsedCarBuyerCustomerCount]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 25-Jan-2012
-- Description:	Get distinct  Customer Count shown interest in Used cars 
-- =============================================
CREATE PROCEDURE GetUsedCarBuyerCustomerCount
	-- Add the parameters for the stored procedure here
	@FromDate datetime,@ToDate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT COUNT(CustomerId)
	FROM (
	SELECT DISTINCT CustomerId FROM ClassifiedRequests AS CR
	WHERE CR.RequestDateTime BETWEEN @FromDate AND @ToDate
	UNION
	SELECT DISTINCT CustomerId FROM UsedCarPurchaseInquiries AS UPI
	WHERE UPI.RequestDateTime BETWEEN @FromDate AND @ToDate
	) a
END
