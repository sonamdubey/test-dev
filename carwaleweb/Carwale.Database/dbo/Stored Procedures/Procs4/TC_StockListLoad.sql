IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockListLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockListLoad]
GO

	-- ===========================================================
-- Author:		<Author,VIVEK GUPTA>
-- Create date: <Create Date,27-02-2013>
-- Description:	<Description,This SP filters Stock List According to Buyer's Preferences And SHOWS STOCK LIST FOR LOOSE BUYER>
-- TC_StockListLoad 5,1,20,NULL,NULL,1,119
-- ===========================================================
CREATE PROCEDURE [dbo].[TC_StockListLoad] 
@BranchId BIGINT,
@FromIndex INT , 
@ToIndex INT,
@MakeId SMALLINT,
@ModelId SMALLINT,
@WithPref BIT,
@InqId BIGINT
AS
BEGIN
IF(@WithPref = 0)
BEGIN
WITH Cte1 
AS (
		 SELECT Id, 
		 V.Car, 
		 S.Price, 
		 S.makeyear,
		 S.kms,
		 S.Colour,
		 S.RegNo,
		 S.EntryDate
		 
		 FROM			TC_Stock AS S WITH(NOLOCK)
		 INNER JOIN     vwMMV AS V    WITH(NOLOCK) ON S.VersionId=V.VersionId		     
         WHERE			S.BranchId=@BranchId 
                    AND S.StatusId = 1 
                    AND S.IsActive = 1
                    AND S.IsBooked = 0
                    AND ((@MakeId IS NULL) OR (V.MakeId = @MakeId))
                    AND ((@ModelId IS NULL) OR (V.ModelId = @ModelId))
                   
                ),
     Cte2
     AS (SELECT *, 
                      ROW_NUMBER() 
                      OVER ( 
                          ORDER BY EntryDate DESC ) RowNumber 
               FROM   Cte1)
         
              SELECT * 
              INTO   #TblTemp 
              FROM   Cte2 

		      SELECT * 
		      FROM   #TblTemp 
		      WHERE  rownumber BETWEEN @FromIndex AND @ToIndex 
		      
		      SELECT COUNT(*) AS RecordCount 
		      FROM   #TblTemp 

		      DROP TABLE #TblTemp  
END
ELSE IF(@WithPref = 1)
BEGIN
WITH Cte1 
AS (
		 SELECT Id, 
		 V.Car, 
		 S.Price, 
		 S.makeyear,
		 S.kms,
		 S.Colour,
		 S.RegNo,
		 S.EntryDate
		 FROM			TC_Stock AS S WITH(NOLOCK)
		 INNER JOIN     vwMMV AS V    WITH(NOLOCK) ON S.VersionId=V.VersionId
		 INNER JOIN    (SELECT B.TC_BuyerInquiriesId, P.ModelId FROM TC_BuyerInquiries AS B WITH(NOLOCK) INNER JOIN TC_PrefModelMake AS P WITH(NOLOCK) ON B.TC_BuyerInquiriesId=P.TC_BuyerInquiriesId WHERE (P.TC_BuyerInquiriesId=@InqId)) AS T ON V.ModelId=T.ModelId		     
         WHERE			S.BranchId=@BranchId 
                    AND S.StatusId = 1 
                    AND S.IsActive = 1
                    AND S.IsBooked = 0
                    
                ),
     Cte2
     AS (SELECT *, 
                      ROW_NUMBER() 
                      OVER ( 
                          ORDER BY EntryDate DESC ) RowNumber 
               FROM   Cte1)
         
              SELECT * 
              INTO   #TblTemp1 
              FROM   Cte2 

		      SELECT * 
		      FROM   #TblTemp1 
		      WHERE  rownumber BETWEEN @FromIndex AND @ToIndex 
		      
		      SELECT COUNT(*) AS RecordCount 
		      FROM   #TblTemp1 

		      DROP TABLE #TblTemp1  
END
END





SET ANSI_NULLS ON
