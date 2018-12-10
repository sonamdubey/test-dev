CREATE TABLE [dbo].[TC_MappingOfferWithStock] (
    [StockId]           INT      NULL,
    [TC_UsedCarOfferId] INT      NULL,
    [StartDate]         DATETIME NULL,
    [EndDate]           DATETIME NULL,
    [IsActive]          AS       ([dbo].[CheckIsOfferActive]([StartDate],[EndDate])),
    CONSTRAINT [UK_MappOffWhSt] UNIQUE NONCLUSTERED ([StockId] ASC, [TC_UsedCarOfferId] ASC)
);


GO

-- =============================================
-- Author:                Vivek Gupta
-- Create date: 13-02-2014 
-- Description:It updates livelisting table if offer is active for the stocks while updating or inserting tc_mappingofferswithstock table.
-- =============================================
CREATE TRIGGER [dbo].[TR_TC_MappingOfferWithStock]
  ON  [dbo].[TC_MappingOfferWithStock]
  FOR INSERT,UPDATE,DELETE
AS 
BEGIN
       SET NOCOUNT ON;

	  UPDATE LL SET LL.OfferStartDate=a.StartDate, LL.OfferEndDate=a.EndDate
	  FROM Livelistings  AS LL WITH (NOLOCK)
	  JOIN SellInquiries AS SI  WITH (NOLOCK) ON LL.Inquiryid=SI.ID AND LL.SellerType=1
	  JOIN  
			(
				SELECT TCS.StockId StockId, MIN(TCS.StartDate) StartDate,MAX(TCS.EndDate) EndDate
				FROM TC_MappingOfferWithStock TCS WITH (NOLOCK)
				JOIN INSERTED AS TCM WITH(NOLOCK) ON TCS.StockId=TCM.StockId 
				WHERE  (TCS.IsActive=1 OR TCS.EndDate>CONVERT(DATE,GETDATE()))
				GROUP BY TCS.StockId
			)
			   A ON SI.TC_StockId=A.StockId

	  UPDATE LL SET LL.OfferStartDate=a.StartDate, LL.OfferEndDate=a.EndDate
	  FROM Livelistings  AS LL WITH (NOLOCK)
	  JOIN SellInquiries AS SI  WITH (NOLOCK) ON LL.Inquiryid=SI.ID AND LL.SellerType=1
	  JOIN  
			(
				SELECT ISNULL(TCS.StockId,TCM.StockId) StockId, MIN(TCS.StartDate) StartDate,MAX(TCS.EndDate) EndDate
				FROM  DELETED AS TCM WITH(NOLOCK)
				LEFT JOIN TC_MappingOfferWithStock TCS WITH (NOLOCK)  ON TCS.StockId=TCM.StockId AND  (TCS.IsActive=1 OR TCS.EndDate>CONVERT(DATE,GETDATE()))
				GROUP BY TCS.StockId,TCM.StockId
			)
			   A ON SI.TC_StockId=A.StockId

      
      
END

/****** Object:  StoredProcedure [dbo].[LL_UpdateListing]    Script Date: 2/19/2014 11:07:05 AM ******/
SET ANSI_NULLS ON
