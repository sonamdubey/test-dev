IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerStockResponses]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerStockResponses]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <24/04/2013>
-- Description:	<Returns modelwise no. of listings and responses on them>
--              EXEC GetDealerStockResponses '2013-03-01 00:13:32.010','2013-04-01 00:13:32.010'
-- =============================================
CREATE PROCEDURE [dbo].[GetDealerStockResponses] 
	-- Add the parameters for the stored procedure here
	@FromDate	datetime,
	@ToDate		datetime,
	@MakeId		INT = NULL,
	@ModelId	INT = NULL,
	@CityId		INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
		SELECT Make,
			Model,
			CI.Name AS City,
			COUNT(DISTINCT s.Id) AS StockCount,
			COUNT(DISTINCT u.Id) AS Response,
			COUNT(distinct u.CustomerID) as UniqueCustomers
		FROM SellInquiries AS s WITH(NOLOCK)
		INNER JOIN UsedCarPurchaseInquiries AS u WITH(NOLOCK) ON s.ID = u.SellInquiryId
		INNER JOIN vwMMV AS vw WITH(NOLOCK) ON vw.VersionId = s.CarVersionId
		INNER JOIN Dealers AS DS WITH(NOLOCK) ON DS.ID=S.DealerId
		INNER JOIN Cities AS CI WITH(NOLOCK) ON CI.ID=DS.CityId
		WHERE EntryDate BETWEEN @FromDate AND @ToDate AND ( vw.MakeId = @MakeId OR @MakeId IS   NULL ) 
		AND (vw.ModelId = @ModelId or @ModelId IS  NULL) AND ( CI.ID = @CityId OR @CityId IS   NULL)
		
		GROUP BY vw.Make , vw.model, CI.Name 
		ORDER BY vw.Make 
		
	

END
