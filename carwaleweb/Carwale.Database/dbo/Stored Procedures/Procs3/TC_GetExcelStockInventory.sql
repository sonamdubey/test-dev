IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetExcelStockInventory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetExcelStockInventory]
GO
	
-- =============================================
-- Author:		Tejashree Patil
-- Create date: 19 Sept,2013
-- Description:	This Proc Returns all inventories depend on type imported from excel.
-- Modified By: Tejashree Patil on 30 Sept 2013, Changed WHERE clause in both query.
-- Modified By: Tejashree Patil on 6 Jan 2014, Commented Status 'Booked' and changed order by clause.
-- TC_GetExcelStockInventory 3,null,null,1,1,1,100
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetExcelStockInventory]
	@UserId    BIGINT,
	@BranchId  BIGINT,
	@ChassisNumber VARCHAR(17)=NULL,
	@IsSpecialUser BIT = 0,--From dealership and 1 for special user.
	@ReqFromInvntoryReportPage BIT=1,--0 for inventory invalid page where all valid records will be fetched.
	@FromIndex  INT, 
	@ToIndex  INT
AS
BEGIN
	DECLARE @SpecialUserId INT = @UserId
	IF(@IsSpecialUser=0)
	BEGIN
		SET @IsSpecialUser = NULL
		SET @SpecialUserId = NULL
	END

	IF(@ReqFromInvntoryReportPage=1)
	BEGIN		
		WITH cte1 AS (
			SELECT
				 I.TC_StockInventoryId AS Id,
				 I.TC_ExcelStockInventoryId, 
				 I.BranchId, 
				 I.ChassisNumber,
				 I.ColourCode,
				 I.DealerCompanyName,
				 I.ModelCode, 
				 I.SellingDealerCode,
				 I.ModelYear,
				 V.Model + ' ' + V.Version AS VersionName,
				 VCC.Color,
				 CASE 
					WHEN (N.ChassisNumber IS NULL OR N.BookingStatus=31) THEN '--' 
					WHEN N.BookingStatus=32 AND N.InvoiceDate IS NULL THEN 'Booked'
					WHEN (N.InvoiceDate IS NOT NULL AND N.DeliveryDate IS NULL) THEN 'Retailed' ELSE 'Delivered' 
				 END AS Status,
				 I.TC_ExcelSheetStockInventoryId AS ExcelSheetId,
				 ROW_NUMBER() OVER( ORDER BY N.BookingStatus, (N.DeliveryEntryDate-N.InvoiceDate) )AS RowNo -- Modified By: Tejashree Patil on 6 Jan 2014
					 FROM TC_StockInventory I WITH(NOLOCK)
					 INNER JOIN TC_vwVersionColorCode VCC WITH (NOLOCK)
											  ON VCC.CarVersionCode=I.ModelCode AND VCC.ColorCode=I.ColourCode
					 INNER JOIN vwMMV V WITH (NOLOCK)
											  ON V.VersionId=VCC.CarVersionId
					 INNER JOIN Dealers D WITH (NOLOCK)
											  ON I.SellingDealerCode = D.DealerCode
					 LEFT JOIN TC_NewCarBooking AS N  WITH (NOLOCK)
												ON I.ChassisNumber=N.ChassisNumber
					 WHERE  
						(((@SpecialUserId IS NOT NULL) AND(I.TC_UserId=@UserId AND I.IsSpecialUser=@IsSpecialUser)) OR ((@IsSpecialUser IS NULL) AND(I.BranchId=@BranchId)))
						AND (@ChassisNumber IS NULL OR I.ChassisNumber=@ChassisNumber)
						)
											
				SELECT * 
				INTO   #TblTempInventory 
				FROM   cte1 		

				SELECT * 
				FROM   #TblTempInventory
				WHERE  RowNo BETWEEN @FromIndex AND @ToIndex 
				ORDER BY RowNo 
	      

				SELECT COUNT(*) AS RecordCount 
				FROM   #TblTempInventory 
				
				DROP TABLE #TblTempInventory 					
	END
	ELSE
	BEGIN
		IF(@ReqFromInvntoryReportPage=0)--Invalid page
		BEGIN	
			WITH cte1 AS (			
				SELECT
				 I.TC_ExcelStockInventoryId AS Id, 
				 I.BranchId, 
				 I.ChassisNumber,
				 I.ColourCode,
				 I.DealerCompanyName,
				 I.ModelCode, 
				 I.SellingDealerCode,
				 I.IsValid ,
				 I.IsDeleted,
				 I.TC_ExcelSheetStockInventoryId AS ExcelSheetId,
				 ROW_NUMBER() OVER( ORDER BY I.TC_ExcelSheetStockInventoryId DESC, I.EntryDate  )AS RowNo
					 FROM TC_ExcelStockInventory I WITH(NOLOCK)
					 --INNER JOIN TC_Users U WITH (NOLOCK) 
						--								 ON I.CreatedBy = U.Id 
					--INNER JOIN Dealers D WITH (NOLOCK)
					--						ON I.SellingDealerCode = D.DealerCode
					 WHERE  I.IsValid=ISNULL(@ReqFromInvntoryReportPage,0)
						AND I.IsDeleted=0 
						--AND I.CreatedBy=@UserId 
						AND ((@BranchId IS NULL AND I.CreatedBy=@UserId) OR I.BranchId=@BranchId))
						--AND (@BranchId IS NULL OR I.BranchId=@BranchId))
						
				SELECT * 
				INTO   #TblTemp 
				FROM   cte1 		

				SELECT * 
				FROM   #TblTemp 
				WHERE  RowNo BETWEEN @FromIndex AND @ToIndex 
				ORDER BY RowNo 
	      

				SELECT COUNT(*) AS RecordCount 
				FROM   #TblTemp 
				
				DROP TABLE #TblTemp 		
		END	
	END
END
