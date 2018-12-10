IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MMInsertMatchingInquiryForAllStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MMInsertMatchingInquiryForAllStock]
GO

	-- =============================================
-- Author:		Manish Chourasiya
-- Create date: 27/01/2014
-- Description: SP will insert all matching inquiry for the active stocks of the dealers.
-- Modified By Deepak for Paid Dealer Only on 8th Sept 2015
-- =============================================
CREATE PROCEDURE [dbo].[TC_MMInsertMatchingInquiryForAllStock]
 AS 
	BEGIN
	     
	     SET NOCOUNT ON ;
		  TRUNCATE TABLE TC_MMCustomerDetails;
		  TRUNCATE TABLE TC_MMDealersMatchCount;
		
		 DECLARE @AllDealerList TABLE  (Id INT IDENTITY(1,1), DealerId INT)
		 CREATE TABLE  #DealerWiseStockList (DealerWiseStockListId INT IDENTITY(1,1), StockId INT,BranchId INT)
		 
		 DECLARE @WhileLoopDealerControl INT=1,
				 @TotalWhileLoopDealerCount INT,
				 
				 @WhileLoopStockControl INT=0,
				 @TotalWhileLoopStockCount INT,
				 
				 @DealerId INT,
				 @TC_StockIdList VARCHAR(MAX) ='',
				 @NoOfStockCalculatedAtaTime SMALLINT =20


		 INSERT INTO @AllDealerList(DealerId) 
		 SELECT D.ID FROM Dealers D WITH (NOLOCK) 
			INNER JOIN ConsumerCreditPoints CP WITH (NOLOCK) ON D.ID = CP.ConsumerId 
							AND CP.ConsumerType = 1 AND CONVERT(DATE,CP.ExpiryDate) >= CONVERT(DATE,GETDATE())
							AND CP.PackageType IN(18,19,29) --Opt, Max, Premium
		 WHERE D.Status=0
		 AND D.IsTcDealer=1
		 AND D.TC_DealerTypeId IN (1,3)
		 

		  SELECT @TotalWhileLoopDealerCount=COUNT(ID)
		  FROM @AllDealerList


		  WHILE (@WhileLoopDealerControl<=@TotalWhileLoopDealerCount)
		    BEGIN

			   SELECT @DealerId=DealerId FROM @AllDealerList
			   WHERE ID=@WhileLoopDealerControl


			       
				    TRUNCATE TABLE #DealerWiseStockList;
					 				    
					   INSERT INTO #DealerWiseStockList (StockId, BranchId)
					   SELECT ID , BranchId
					   FROM TC_Stock WITH (NOLOCK)
					   WHERE BranchId=@DealerId
					   AND IsActive=1
					   AND StatusId=1

					    SELECT @TotalWhileLoopStockCount=COUNT(DealerWiseStockListId)
		                FROM #DealerWiseStockList
				       
					   SET @WhileLoopStockControl=0

			     
				 WHILE (@WhileLoopStockControl<=@TotalWhileLoopStockCount)
				  BEGIN 
			 
					


					  SELECT @TC_StockIdList =  coalesce(@TC_StockIdList+',','') + CAST(StockId AS VARCHAR(250))
					   FROM  #DealerWiseStockList 
					   WHERE DealerWiseStockListId  BETWEEN @WhileLoopStockControl+1 AND (@WhileLoopStockControl + @NoOfStockCalculatedAtaTime)
					   AND BranchId=@DealerId
					  

					   /*PRINT  CONVERT (VARCHAR, @WhileLoopStockControl+1)+'-'
					         +CONVERT (VARCHAR,(@WhileLoopStockControl + @NoOfStockCalculatedAtaTime)) 
							 +  '  DealerId '+CONVERT(varchar,@DealerId) +'  StockId  '+ @TC_StockIdList*/
				
						EXEC	 [dbo].[TC_MMInsertMatchingInquiry]    @TC_StockIdList=@TC_StockIdList,
																	   @PageId =NULL,
																	   @DealerId =@DealerId 
					  
					 -- PRINT  /*'DealerId:'  +convert(VARCHAR, @DealerId) + '~' + 'StockIdList:'  +*/ CAST(@TC_StockIdList AS VARCHAR(250))
 
            
							  SET @TC_StockIdList =''
							  SET @WhileLoopStockControl=@WhileLoopStockControl+@NoOfStockCalculatedAtaTime
                   
				   
				END

					  SET @WhileLoopDealerControl=@WhileLoopDealerControl+1
			END 
	 
	        DROP TABLE #DealerWiseStockList
    END

