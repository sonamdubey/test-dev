IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetStockList_12Apr]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetStockList_12Apr]
GO

	

-- =============================================
-- Author:		Binumon George
-- Create date: 30th Dec 2011
-- Description:	converted reg no and color in to capital letters
-- =============================================
-- Author:		Binumon George
-- Modified date: 14th Dec 2011
-- Description:	Added SynStatus parameter for checking stock uploaded cw or not
-- =============================================
-- =============================================
-- Author:		Binumon George
-- Create date: 16th November 2011
-- Description:	For Listing entire stock in Excel sheet
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetStockList_12Apr]
@BranchId INT,
@StatusuId INT,
@MakeId INT=NULL,
@ModelId INT=NULL,
@Price VARCHAR(50)=NULL,
@Kms VARCHAR(50)=NULL,
@SynStatus INT=NULL
AS
BEGIN
 DECLARE @Sql VARCHAR(MAX)
	SET @Sql='Select * From (Select  Row_Number() Over (Order By st.EntryDate DESC) AS RowN,  
		St.Id, St.MakeYear, St.Kms, dbo.TitleCase(St.Colour)Colour, 
		(ISNULL(Sa.CWResponseCount, '''') + ISNULL(Sa.TCResponseCount,'''')) as InquiryCount , 
		upper(St.RegNo)RegNo, St.Price, St.EntryDate, dbo.TC_GetOwnerValue(CC.Owners)Owners,
		( Ma.Name +'' '' + Mo.Name +'' ''+ Ve.Name ) As MakeModelVersion  
		From  TC_Stock St  INNER JOIN TC_StockStatus Tcs On Tcs.Id = St.StatusId 
		LEFT JOIN TC_CarCondition CC ON CC.StockId=St.Id 
		LEFT JOIN CarVersions Ve On Ve.Id=St.VersionId  LEFT JOIN CarModels Mo On Mo.Id=Ve.CarModelId  
		LEFT Join CarMakes Ma On Ma.Id=Mo.CarMakeId  LEFT JOIN SellInquiries Si On Si.TC_StockId=St.Id 
		LEFT OUTER JOIN TC_StockAnalysis Sa On St.Id=Sa.StockId  Where St.BranchId ='
		
		SET @Sql=@Sql+ CAST(@BranchId AS VARCHAR(5))
		SET @Sql=@Sql+' AND St.StatusId=' + CAST(@StatusuId AS VARCHAR(2))
		
		IF(@MakeId IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' And Ma.Id =' + CAST(@MakeId AS VARCHAR(5))
			END
		IF(@ModelId IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' And Mo.Id =' + CAST(@ModelId AS VARCHAR(5))
			END
		IF(@SynStatus IS NOT NULL)  -- Binumon George Added @SynStatus 20-12-2011
			BEGIN
				SET @Sql=@Sql+' And St.IsSychronizedCW =' + CAST(@SynStatus AS VARCHAR(2))
			END
		IF(@Price IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' '+@Price
			END
		IF(@Kms IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+' '+@Kms
			END
			
		SET @Sql=@Sql+' ) AS TopRecords'
		EXEC (@Sql)
END


