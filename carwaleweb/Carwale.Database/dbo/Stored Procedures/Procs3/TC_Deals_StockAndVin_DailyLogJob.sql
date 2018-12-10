IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_StockAndVin_DailyLogJob]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_StockAndVin_DailyLogJob]
GO
	-- =============================================
-- Author:		Manish
-- Create date: 18-02-2016
-- Description: This SP will capture daily snapshot of the complete data for table TC_Deals_Stock and TC_Deals_StockVIN.
-- -- =============================================
  CREATE PROCEDURE [dbo].[TC_Deals_StockAndVin_DailyLogJob]
    AS 
	  BEGIN 
	         INSERT INTO TC_Deals_Stock_DailyLog (
			                                        AsOnDate
													,Id
													,BranchId
													,CarVersionId
													,MakeYear
													,VersionColorId
													,InteriorColor
													,EnteredOn
													,EnteredBy
													,LastUpdatedOn
													,LastUpdatedBy
													,Offers
													,IsApproved
												 )
								           SELECT   CONVERT(DATE,getdate())
													,Id
													,BranchId
													,CarVersionId
													,MakeYear
													,VersionColorId
													,InteriorColor
													,EnteredOn
													,EnteredBy
													,LastUpdatedOn
													,LastUpdatedBy
													,Offers
													,IsApproved
								          FROM TC_Deals_Stock WITH (NOLOCK)


				         INSERT INTO TC_Deals_StockVIN_DailyLog (
																    AsOnDate
																	,TC_DealsStockVINId
																	,TC_Deals_StockId
																	,VINNo
																	,Status
																	,LastRefreshedOn
																	,LastRefreshedBy
																	,EnteredOn
																	,EnteredBy
																  )
														SELECT     CONVERT(DATE,getdate())
														           ,TC_DealsStockVINId
																   ,TC_Deals_StockId
																   ,VINNo
																   ,Status
																   ,LastRefreshedOn
																   ,LastRefreshedBy
																   ,EnteredOn
																   ,EnteredBy
														FROM TC_Deals_StockVIN WITH (NOLOCK)
						   
	  END

