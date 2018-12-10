IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MMInsertMatchingInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MMInsertMatchingInquiry]
GO

	-- =============================================
-- Author:		Manish Chourasiya
-- Create date: 31/10/2013
-- Description: For inserting count and detail of inquiries matched to the stock.
-- Modified by Manish on 03-03-2014 since stated using IsDeleted bit in "TC_MMCustomerDetails" table.
-- Modified by Manish on 26-08-2015 changed for optimization. In place of  ISNULL(L.TC_LeadId, 0) = 0  used isnull.
-- Modified by Manish on 02-09-2015 reverted the change 
-- =============================================
CREATE PROCEDURE [dbo].[TC_MMInsertMatchingInquiry]
@TC_StockIdList VARCHAR(MAX),
@PageId INT,
@DealerId INT
 AS 
	BEGIN

	 SET NOCOUNT ON ;
	      
	     --Lock the running process
	     --Commented By Deepak, this will fail in case of Page Sorting
		--INSERT INTO TC_MMExecutionFlg (DealerId,PageId)
		--VALUES (@DealerId,@PageId)
			
		-- Variable Declaration
		DECLARE @StockModelId INT,
	          @StockFuelType TINYINT,
			  @StockPrice  FLOAT,
			  @StockYear  SMALLINT,
			  @StockKms  FLOAT,
			  @StockDealerId INT,
			  @TC_StockId INT,
			  @CityId INT,
			  @TotalWhileLoop INT,
			  @WhileLoopControl INT =1,
			  @NoOfDayOldCustomer SMALLINT =30
				 
          
        -- Table to take all stock Id's
		DECLARE  @TblStockList  TABLE (TblStockListId INT IDENTITY(1,1) ,TC_StockId INT)
		DECLARE @TempTblMnMInquiries TABLE(DealerId INT, CWCustomersId INT, MatchedStockId INT, CWInquiryId INT,
													CWSellInquiryId INT, SellerType SMALLINT, CustomerResponseDate DATETIME,
													IsPurchased BIT, CreatedOn DATETIME, InqCount SMALLINT)
		  
		INSERT INTO @TblStockList  (TC_StockId)
		SELECT ListMember FROM [dbo].[fnSplitCSV] (@TC_StockIdList)
		
		--Loop the process for all stockId's
		SELECT @TotalWhileLoop = count (*) FROM @TblStockList
		--PRINT @TotalWhileLoop

		WHILE @WhileLoopControl<=@TotalWhileLoop
			BEGIN 
			
				--PRINT @WhileLoopControl
				--Get the current stock details necessary for MIX & MATCH Inquiries
				SELECT @StockModelId=CV.CarModelId,
					 @StockFuelType=CV.CarFuelType,
					 @StockPrice=S.Price,
					 @StockYear=YEAR(S.MakeYear),
					 @StockKms=S.Kms,
					 @TC_StockId=S.ID,
					 @CityId=D.CityId
				FROM TC_Stock  AS S WITH (NOLOCK)
				  JOIN Dealers  AS D WITH (NOLOCK) ON D.ID=S.BranchId
				  JOIN @TblStockList AS TS ON TS.TC_StockId=S.Id
				  JOIN CarVersions AS CV WITH (NOLOCK) ON S.VersionId=CV.ID   
				WHERE TS.TblStockListId=@WhileLoopControl;
		
				-- Get MIX & MATCH Inquiries
				-- Put Row number so that one cutomer one matching inquiry and it should be latest one
				INSERT INTO @TempTblMnMInquiries
				SELECT DISTINCT @DealerId AS DealerId,
						 U.CustomerId AS CWCustomersId,
						 @TC_StockId AS MatchedStockId,
						 U.InquiryId AS CWInquiryId,
						 U.SellInquiryId AS CWSellInquiryId,
						 U.SellerType,
						 U.CustomerResponseDate,
						 0 AS IsPurchased,
						 GETDATE() AS CreatedOn,
						 ROW_NUMBER() OVER( PARTITION BY U.CustomerId ORDER BY U.CustomerResponseDate DESC)  AS InqCount
				FROM TC_MMvwUsedCarInquiries AS U WITH (NOLOCK) --TC_MMUsedCarInquiries By Deepak on 25th Nov 2013
					  LEFT JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON  U.CustomerMobile=C.Mobile AND C.BranchId=@DealerId
					  LEFT JOIN TC_InquiriesLead AS L WITH (NOLOCK)  ON L.TC_CustomerId=C.Id  AND L.LatestInquiryDate > (GETDATE()-@NoOfDayOldCustomer)
				WHERE U.CarModelId=@StockModelId 
				   --AND  U.FuelTypeId=@StockFuelType
				   --AND  U.Price <= (@StockPrice*30/100 )+ @StockPrice
				   --AND  U.MakeYear>= (@StockYear-2)  --YEAR(U.MakeYear) By Deepak on 25th Nov 2013
				   --AND  U.Kms<= (@StockKms*30/100)+ @StockKms 
				   AND  (U.CityId=@CityId OR U.CityId IN(SELECT CityId FROM CityGroups WITH (NOLOCK) WHERE MainCityId =@CityId ))
				   AND  (U.DealerId<>@DealerId OR U.DealerId IS NULL)
				    AND  ISNULL(L.TC_LeadId, 0) = 0  -- Commented by Manish on 26-08-2015 --Uncommented by manish on 02-09-2015
				--	AND L.TC_LeadId IS NULL  -- Added by Manish on 26-08-2015 for optimization - Commented by Manish on 02-09-2015
				  
				 -- Added By Deepak on 25th Nov 2013
				 --Delete exisitnng data for this stock Id
				--DELETE FROM TC_MMCustomerDetails WHERE MatchedStockId = @TC_StockId AND DealerId = @DealerId
				-- Delete commented by manish on 03-03-2014 using IsDeleted column in place of delete statement

				UPDATE TC_MMCustomerDetails  SET IsDeleted=1
			    WHERE MatchedStockId = @TC_StockId AND DealerId = @DealerId

				
				--Save the matched data in table
				INSERT INTO TC_MMCustomerDetails   (DealerId,
													CWCustomersId,
													MatchedStockId,
													CWInquiryId,
													CWSellInquiryId,
													SellerType,
													CustomerResponseDate,
													IsPurchased,
													CreatedOn
													)
				SELECT DealerId,
						CWCustomersId,
						MatchedStockId,
						CWInquiryId,
						CWSellInquiryId,
						SellerType,
						CustomerResponseDate,
						IsPurchased,
						CreatedOn FROM @TempTblMnMInquiries WHERE InqCount  = 1
						
				--Empty Tem Table
				DELETE FROM @TempTblMnMInquiries
				
			DELETE FROM TC_MMDealersMatchCount WHERE StockId = @TC_StockId AND DealerId = @DealerId
			
			INSERT INTO TC_MMDealersMatchCount (DealerId,
			     							StockId,
											MatchViewCount,
											CreatedOn,
											LastUpdatedOn)
			SELECT  S.BranchId,
				S.Id,
				COUNT(CD.CWCustomersId),
				GETDATE(),
				GETDATE()
			FROM TC_Stock AS S WITH (NOLOCK)
			LEFT JOIN TC_MMCustomerDetails AS CD WITH (NOLOCK)  ON S.Id=CD.MatchedStockId
   															AND CD.IsPurchased=0
															AND CD.MatchedStockId=@TC_StockId
															AND CD.DealerId=@DealerId
															AND CD.IsDeleted=0  --Condition added by Manish on 03-03-2014
			WHERE S.Id = @TC_StockId
			GROUP BY  S.BranchId,S.Id
					--END  
		   
			SET @WhileLoopControl= @WhileLoopControl+1
	 END 
	   
	--Release the lock
	DELETE FROM TC_MMExecutionFlg  
	WHERE DealerId=@DealerId 
	AND PageId=@PageId

	RETURN 1 

END
