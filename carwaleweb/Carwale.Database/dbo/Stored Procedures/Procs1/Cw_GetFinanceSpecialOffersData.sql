IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Cw_GetFinanceSpecialOffersData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Cw_GetFinanceSpecialOffersData]
GO

	-- =============================================
-- Author	:	Piyush sahu
-- Description	:	Get HDFC special offers data
-- Time  :  9/26/2016 4:13:33
-- =============================================

CREATE  PROCEDURE [dbo].[Cw_GetFinanceSpecialOffersData]
@CW_CompanyCategoryId SMALLINT = NULL,
@MinLoanAmount	INT = NULL,
@MaxLoanAmount INT = NULL
AS
BEGIN
	SELECT 
		CC.CategoryName,
		FSO.CW_CompanyCategoryId as CompanyId,
		FSO.MinLoanAmount,
		FSO.MaxLoanAmount,
		FSO.ROI,
		FSO.IsCampaignActive,
		FSO.StartDate,
		FSO.EndDate

	FROM  
		CW_FinanceSpecialOffers FSO WITH (NOLOCK)
		INNER JOIN CW_CompanyCategories CC WITH (NOLOCK) ON FSO.CW_CompanyCategoryId = CC.Id
 WHERE 
		(@CW_CompanyCategoryId IS NULL OR FSO.CW_CompanyCategoryId=@CW_CompanyCategoryId )
		AND (@MinLoanAmount IS NULL OR FSO.MinLoanAmount = @MinLoanAmount)
		AND (@MaxLoanAmount IS NULL OR FSO.MaxLoanAmount = @MaxLoanAmount)
		
	ORDER BY
		FSO.CW_CompanyCategoryId,
		FSO.MinLoanAmount,
		FSO.MaxLoanAmount


END
