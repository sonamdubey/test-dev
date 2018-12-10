IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MixMatchLeadsForMFC]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MixMatchLeadsForMFC]
GO

	-- =============================================
-- Author:		Vivek GUpta
-- Create date: 05-02-2015
-- Description:	Inserting Mix Match Leads for MFC dealers into TC_MixMatchLeadsMFC
-- =============================================
CREATE PROCEDURE [dbo].[TC_MixMatchLeadsForMFC]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	TRUNCATE TABLE TC_MixMatchLeadsMFC
	CREATE TABLE #MixMatchDealerAndStock
	( 
		DealerId INT,
		StockId INT,
		StockModelId INT ,
		CityId INT		
	)

	DECLARE @ALLMFCDealers VARCHAR(MAX) = NULL
	DECLARE @Separator VARCHAR(1) = ','
	DECLARE @Separator_position INT 
    DECLARE @array_value VARCHAR(1000)
	DECLARE @DealerStockId INT
	DECLARE @CityId INT
	DECLARE @StockModelId INT
	
	SELECT @ALLMFCDealers = COALESCE(@ALLMFCDealers + ', ', '') + CAST(DealerId AS VARCHAR(10))
	FROM TC_MFCDealers TF INNER JOIN ConsumerCreditPoints CP ON TF.DealerId = CP.ConsumerId 
		AND CP.ConsumerType = 1 AND CP.ExpiryDate >= CONVERT(DATE,GETDATE())
	WHERE TF.SendMixMatchLead = 1 AND ISNULL(LeadCntperDay,0) > 0
	
	INSERT INTO #MixMatchDealerAndStock (DealerId, StockId, StockModelId, CityId)
	SELECT MFCD.DealerId, TCS.Id, CV.CarModelId, D.CityId
	FROM TC_MFCDealers MFCD WITH(NOLOCK)
	JOIN TC_Stock TCS WITH(NOLOCK) ON MFCD.DealerId = TCS.BranchId 
	JOIN CarVersions CV WITH(NOLOCK) ON TCS.VersionId = CV.ID
	JOIN Dealers D WITH(NOLOCK) ON D.ID = MFCD.DealerId
	AND TCS.IsActive = 1 
	AND TCS.StatusId = 1
	AND MFCD.DealerId IN (SELECT * FROM fnSplitCSV(@ALLMFCDealers))
	ORDER BY DealerId
	
	DECLARE @MFCDealers VARCHAR(MAX) = NULL
	SET @MFCDealers = @ALLMFCDealers
	
	SELECT  @MFCDealers = STUFF(@MFCDealers, 1, 0, '')
	SET @MFCDealers = @MFCDealers + @Separator

	WHILE PATINDEX('%' + @Separator + '%', @MFCDealers) <> 0   
	BEGIN
	
		  SELECT  @Separator_position = PATINDEX('%' + @Separator + '%',@MFCDealers)  
		  SELECT  @array_value = LEFT(@MFCDealers, @Separator_position - 1)  

		  WHILE EXISTS (SELECT Top 1 StockId FROM #MixMatchDealerAndStock WHERE DealerId = @array_value)
		  BEGIN
			
				SET @DealerStockId = (SELECT Top 1 StockId FROM #MixMatchDealerAndStock WHERE DealerId = @array_value)				
				SET @CityId = (SELECT CityId FROM  #MixMatchDealerAndStock WHERE StockId = @DealerStockId )--(SELECT CityId FROM Dealers WITH(NOLOCK) WHERE Id = @array_value)
				SET @StockModelId = (SELECT StockModelId FROM  #MixMatchDealerAndStock WHERE StockId = @DealerStockId )--(SELECT CarModelId FROM CarVersions WITH(NOLOCK) WHERE ID = (SELECT VersionId FROM TC_Stock WITH(NOLOCK) WHERE Id = @DealerStockId))
				DELETE FROM #MixMatchDealerAndStock WHERE StockId = @DealerStockId
				
				
				--INSERT INTO TC_MMCustomerDetails   (DealerId,
				--										CWCustomersId,
				--										MatchedStockId,
				--										CWInquiryId,
				--										CWSellInquiryId,
				--										SellerType,
				--										CustomerResponseDate,
				--										IsPurchased,
				--										CreatedOn
				--										)

				--SELECT DISTINCT REPLACE(@array_value,' ','') AS DealerId,
				--U.CustomerId AS CWCustomersId,
				--@DealerStockId AS MatchedStockId,
				--U.InquiryId AS CWInquiryId,
				--U.SellInquiryId AS CWSellInquiryId,
				--U.SellerType,
				--U.CustomerResponseDate,
				--1 AS IsPurchased,
				--GETDATE()
				--FROM TC_MMvwUsedCarInquiries AS U WITH (NOLOCK)
				--WHERE U.CarModelId=@StockModelId
				----AND U.FuelTypeId=@StockFuelType
				----AND U.Price <= (@StockPrice*30/100 )+ @StockPrice
				----AND U.MakeYear>= (@StockYear-2) --YEAR(U.MakeYear) By Deepak on 25th Nov 2013
				----AND U.Kms<= (@StockKms*30/100)+ @StockKms
				--AND (U.CityId=@CityId OR U.CityId IN(SELECT CityId FROM CityGroups WHERE MainCityId =@CityId ))
				--AND (U.DealerId<>@array_value OR U.DealerId IS NULL)
				
				INSERT INTO TC_MixMatchLeadsMFC 
				(CWCustomersId ,
				 CustomerName,
				 CustomerMobile, 
				 CustomerEmail, 
				 CarYear, 
				 Kms,
				 Price,
				 InquiryId,
				 SellerType,
				 CarName,
				 SellInquiryId,
				 FuelType,
				 AreaNames,
				 ResponseDays,
				 ResDate,
				 MFCDealerId,
				 StockId)
	
	
				SELECT 
				DISTINCT TVW.CustomerId AS ID, 
				TVW.Name, 
				TVW.CustomerMobile,
				TVW.CustomerEmail,
				TVW.MakeYear, 
				TVW.Kms,
				TVW.Price,
				TVW.InquiryId , 
				TVW.SellerType , 
				(VM.Model + ' ' + VM.Version) as CarName, 
				TVW.SellInquiryId, 
				FT.FuelType, 
				ISNULL(TVW.AreaName,'') AS AreaNames,
				CONVERT(VARCHAR,(DATEDIFF(DAY, TVW.CustomerResponseDate, GETDATE()))) + ' days ago' AS ResponseDate, 
				TVW.CustomerResponseDate, 
				REPLACE(@array_value,' ','') AS DealerId,
				@DealerStockId AS StockId
				FROM TC_MMvwUsedCarInquiries TVW  WITH (NOLOCK)
					INNER JOIN vwMMV VM WITH (NOLOCK)  ON VM.VersionId = TVW.CarVersionId
					LEFT JOIN CarFuelType FT WITH (NOLOCK)  ON FT.FuelTypeId = TVW.FuelTypeId
				WHERE  TVW.CarModelId=@StockModelId
				AND (TVW.CityId=@CityId OR TVW.CityId IN(SELECT CityId FROM CityGroups WHERE MainCityId =@CityId ))
				AND ISNULL(TVW.DealerId, -1)<>@array_value
				AND DATEDIFF(DAY, TVW.CustomerResponseDate, GETDATE()) = 3
				AND TVW.CustomerId NOT IN(SELECT CWCustomersId FROM TC_MixMatchLeadsMFC WHERE MFCDealerId = REPLACE(@array_value,' ',''))
				GROUP BY TVW.CustomerId, TVW.Name,TVW.CustomerMobile, TVW.CustomerEmail, 
				TVW.MakeYear, TVW.Kms, TVW.Price,TVW.InquiryId, TVW.SellerType,VM.Model, 
				VM.Version, TVW.SellInquiryId, FT.FuelType, TVW.AreaName, TVW.CustomerResponseDate

		  END

		  SELECT  @MFCDealers = STUFF(@MFCDealers, 1, @Separator_position, '')  
	END

	DROP Table #MixMatchDealerAndStock;
	
	
	--Make Data Unique to dealer
	WITH Cte1
	AS
	(SELECT ID AS Id,
	  CwCustomersId,
	  SellInquiryId AS SellInquiryId,
	  StockId AS StockId,
	  MFCDealerId AS BranchId,
	  CustomerName,
	  CustomerMobile,
	  CustomerEmail,
	  AreaNames AS CustomerLocation,
	  InquiryId
	FROM TC_MixMatchLeadsMFC WITH(NOLOCK)
	),
	Cte2 
           AS (SELECT *, 
                      ROW_NUMBER() 
                        OVER (                                               
                               PARTITION BY CwCustomersId,BranchId,StockId ORDER BY CwCustomersId
                         ) RowNumber
               FROM   Cte1 
               )
               
	DELETE FROM TC_MixMatchLeadsMFC WHERE ID NOT IN(SELECT Id FROM Cte2	WHERE	RowNumber = 1)
	
	--Run loop again for all dealers and remove extra data from table
	SET @MFCDealers = @ALLMFCDealers
	SELECT  @MFCDealers = STUFF(@MFCDealers, 1, 0, '')
	SET @MFCDealers = @MFCDealers + @Separator

	WHILE PATINDEX('%' + @Separator + '%', @MFCDealers) <> 0   
		BEGIN
	
			SELECT  @Separator_position = PATINDEX('%' + @Separator + '%',@MFCDealers)  
			SELECT  @array_value = LEFT(@MFCDealers, @Separator_position - 1)  


			DECLARE  @TOPVAL INT
			SELECT @TOPVAL = ISNULL(LeadCntperDay,0) FROM TC_MFCDealers WHERE DealerId = @array_value
			IF @TOPVAL IS NOT NULL
				BEGIN
					DELETE FROM TC_MixMatchLeadsMFC WHERE MFCDealerId = @array_value 
						AND ID NOT IN(SELECT TOP (@TOPVAL) ID FROM TC_MixMatchLeadsMFC TM WHERE MFCDealerId = @array_value)
				END
				SELECT  @MFCDealers = STUFF(@MFCDealers, 1, @Separator_position, '')
		END
	
END
