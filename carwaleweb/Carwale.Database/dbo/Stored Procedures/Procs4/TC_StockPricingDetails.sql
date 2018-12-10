IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockPricingDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockPricingDetails]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 28 Aug 2012 at 5 pm
-- Description:	Get Details for Stock Pricing
 --DECLARE @CityName varchar(20),@AvgDealers BIGINT,@AvgIndivisuals BIGINT,@AvgAskPrice BIGINT,@DepricatedPrice BIGINT,@ExShowroomPrice BIGINT,@OnRoadPrice BIGINT
 --EXEC [TC_StockPricingDetails] 5,2012,1,@CityName OUTPUT,@AvgDealers OUTPUT,@AvgIndivisuals OUTPUT,@AvgAskPrice OUTPUT,@DepricatedPrice OUTPUT,@ExShowroomPrice OUTPUT,@OnRoadPrice OUTPUT
 --SELECT @CityName,@AvgDealers ,@AvgIndivisuals ,@AvgAskPrice ,@DepricatedPrice ,@ExShowroomPrice ,@OnRoadPrice 
 -- Modified By: Tejashree Patil on 22 Oct 2012 at 7 pm Added condition BranchId NOT IN (3838,4271) in query for Average Asking Price
 -- Modified By: Nilesh Utture on 04th December, 2012 at 12.30 pm 
-- =============================================
CREATE PROCEDURE [dbo].[TC_StockPricingDetails]
	-- Add the parameters for the stored procedure here
	@BranchId BIGINT, -- dealerId
	@MakeYear INT,    -- Car Make Year
	@VersionId SMALLINT	,-- Car Version
	@CityName VARCHAR(20) OUTPUT,-- Dealer's City
	@AvgDealers BIGINT OUTPUT,
	@AvgIndivisuals BIGINT OUTPUT,
	@AvgAskPrice BIGINT OUTPUT,
	@DepricatedPrice BIGINT OUTPUT,
	@ExShowroomPrice BIGINT OUTPUT,
	@OnRoadPrice BIGINT OUTPUT
	
AS
BEGIN
	SET NOCOUNT ON;
			
	DECLARE @CityId SMALLINT --Dealer's cityId
	
	SELECT @CityId=D.CityId, 
		   @CityName=C.Name 
	FROM   Dealers D WITH(NOLOCK) 
		   INNER JOIN Cities C WITH(NOLOCK)
				   ON C.ID=D.CityId 
	WHERE  D.ID=@BranchId
    
    --Average Asking Price by Dealers
	SELECT @AvgDealers = ROUND(AVG(DISTINCT Price),0)
	FROM   SellInquiries SI WITH(NOLOCK)
		   INNER JOIN Dealers D WITH(NOLOCK) 
				   ON D.ID= SI.DealerId
	WHERE  CarVersionId=@VersionId  
		   AND DATEPART(YEAR,MakeYear)=@MakeYear
		   AND (DATEDIFF(MONTH,EntryDate,GETDATE()) BETWEEN 0 AND 3)
		   AND D.CityId=@CityId
		   AND D.ID NOT IN (3838,4271) -- Modified By: Nilesh Utture on 04th December, 2012 at 12.30 pm Added condition BranchId NOT IN (3838,4271)in query for Average Asking Price should be accurate
       
	--Average Asking Price by Individuals
	SELECT	@AvgIndivisuals = ROUND(AVG(DISTINCT CSI.Price) ,0)
	FROM	CustomerSellInquiries CSI WITH(NOLOCK)
	WHERE	PackageType=2 
			AND CSI.CarVersionId=@VersionId  
			AND DATEPART(YEAR,CSI.MakeYear)=@MakeYear 
			AND CSI.IsFake=0 
			AND CSI.PackageExpiryDate IS NOT NULL
			AND (DATEDIFF(MONTH,EntryDate,GETDATE()) BETWEEN 0 AND 3)
			AND CSI.CityId=@CityId
			
	
	--Your(looged in dealer in Trading cars) Average Asking Price
	SELECT	@AvgAskPrice = ROUND(AVG(DISTINCT Price),0)
	FROM	TC_STOCK WITH(NOLOCK)
	WHERE	VersionId=@VersionId 
			AND (DATEDIFF(MONTH,EntryDate,GETDATE()) BETWEEN 0 AND 6)
			AND IsApproved=1 
			AND IsActive=1 
			AND BranchId=@BranchId
			AND DATEPART(YEAR,MakeYear)=@MakeYear -- Nilesh Utture on 04th December, 2012 at 12.30 pm Removed condition BranchId NOT IN (3838,4271) as is not required

	-- Ex-Showroom price and OnRoadPrice for given version
	SELECT	@ExShowroomPrice = ROUND(Price,0),  -- Modified By: Nilesh Utture on 04th December, 2012 at 12.30 pm Removed SUM as it not required
			@OnRoadPrice = ROUND(Price+ISNULL(RTO,0)+ISNULL(Insurance,0),0) -- Modified By: Nilesh Utture on 04th December, 2012 at 12.30 pm Removed SUM as it not required
	FROM	NewCarShowroomPrices WITH(NOLOCK)
	WHERE	IsActive=1
			AND CarVersionId=@VersionId
			AND CityId=@CityId

	-- Calculating depreciation on Exshowroom price
	DECLARE @Diff SMALLINT = DATEPART(YEAR,GETDATE())+1-@MakeYear	
	--Modified By: Tejashree Patil on 11 September 2012 10 am
	--Deprication on ExShowroom price calculated from current year
	
	SELECT @DepricatedPrice = ROUND(@ExShowroomPrice * POWER(CAST (0.9 AS FLOAT),@Diff),0)
	
END





