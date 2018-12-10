IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetStockList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetStockList]
GO

	-- Author:		Binumon George
-- Create date: 30th Dec 2011
-- Description:	converted reg no and color in to capital letters
-- Modified date: 14th Dec 2011
-- Description:	Added SynStatus parameter for checking stock uploaded cw or not
-- Create date: 16th November 2011
-- Description:	For Listing entire stock in Excel sheet
-- Modified By: Tejashree Patil On 5 July 2012: Added IsParkNSale,Insurance and InsuranceExpiry in SELECT clause 
-- Modified By:	Tejashree Patil on 18 May 2012 ,For Listing Approved stocks in Excel sheet(Default IsApproved=1).
-- Modified By:	Tejashree Patil on 11 April 2012, For Listing Active stocks in Excel sheet.
-- Modified By:	Tejashree Patil on 10 July 2012, WITH(NOLOCK)implementation and added columns Insurance and InsuranceExp in select 
-- Modified By: Vivek Rajak on 06 Aug 2015 to achieve certified and non-certified data to be sent to excel
-- ===============================================================================================================================

CREATE PROCEDURE [dbo].[TC_GetStockList]
@BranchId INT,
@StatusId INT,
@MakeId INT=NULL,
@ModelId INT=NULL,
@Price VARCHAR(50)=NULL,
@Kms VARCHAR(50)=NULL,
@SynStatus INT=NULL,
@IsParkNSale BIT=NULL,
@chkCertifiedCarsStatus BIT=NULL
AS
BEGIN
 DECLARE @Sql VARCHAR(MAX)
 
    -- Modified By: Tejashree Patil On 5 July 2012: Added Insurance and InsuranceExpiry in SELECT clause
	SET @Sql='Select * From (Select  Row_Number() Over (Order By st.EntryDate DESC) AS RowN,  
		St.Id, St.MakeYear, St.Kms, dbo.TitleCase(St.Colour)Colour, 
		(ISNULL(Sa.CWResponseCount, '''') + ISNULL(Sa.TCResponseCount,'''')) as InquiryCount , 
		upper(St.RegNo)RegNo, St.Price, St.EntryDate, dbo.TC_GetOwnerValue(CC.Owners)Owners,
		( Ma.Name +'' '' + Mo.Name +'' ''+ Ve.Name ) As MakeModelVersion , ISNULL(CC.Insurance,'''') as Insurance,
		CC.InsuranceExpiry as InsuranceExpiry  
		From  TC_Stock St WITH(NOLOCK) 
		INNER JOIN TC_StockStatus Tcs WITH(NOLOCK) ON Tcs.Id = St.StatusId 
		LEFT JOIN TC_CarCondition CC WITH(NOLOCK) ON CC.StockId=St.Id 
		LEFT JOIN CarVersions Ve WITH(NOLOCK) ON Ve.Id=St.VersionId  
		LEFT JOIN CarModels Mo WITH(NOLOCK)ON Mo.Id=Ve.CarModelId  
		LEFT Join CarMakes Ma WITH(NOLOCK) ON Ma.Id=Mo.CarMakeId  
		LEFT JOIN SellInquiries Si WITH(NOLOCK) ON Si.TC_StockId=St.Id 
		LEFT OUTER JOIN TC_StockAnalysis Sa WITH(NOLOCK) ON St.Id=Sa.StockId  
		WHERE St.isActive=1 AND St.IsApproved=1 AND St.BranchId = '
		
		SET @Sql=@Sql+ CAST(@BranchId AS VARCHAR(5))
		SET @Sql=@Sql+' AND St.StatusId=' + CAST(@StatusId AS VARCHAR(2))
		
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
		IF(@IsParkNSale IS NOT NULL)
			BEGIN
				SET @Sql=@Sql+'AND St.IsParkNSale = '+CAST(@IsParkNSale AS VARCHAR(2))
			END

		IF(@chkCertifiedCarsStatus IS NOT NULL AND @chkCertifiedCarsStatus=1)     --vivek rajak Added chkCertifiedCarsStatus on 06-08-2015
			BEGIN
				SET @Sql=@Sql+'	AND ST.Id IN (SELECT StockId FROM AbSure_CarDetails ACD WITH(NOLOCK) WHERE DealerId = ' + CAST(@BranchId AS VARCHAR(5))+ ' 
								AND ACD.IsActive = 1 
								AND (ACD.Status = 4 AND DATEDIFF(DAY,ACD.SurveyDate,GETDATE())<=30)
								OR (ACD.Status = 8 AND DATEDIFF(DAY,ACD.SurveyDate,GETDATE())<=30))'
			END
		IF(@chkCertifiedCarsStatus IS NOT NULL AND @chkCertifiedCarsStatus=0)
			BEGIN
			    SET @Sql=@Sql+'AND ST.Id NOT IN(SELECT DISTINCT StockId FROM AbSure_CarDetails ACD WITH(NOLOCK) WHERE DealerId = ' + CAST(@BranchId AS VARCHAR(5)) + 'AND IsActive=1 AND StockId IS NOT NULL ) '
			END
			
		SET @Sql=@Sql+' ) AS TopRecords'
		--print @Sql
		EXEC (@Sql)
END



