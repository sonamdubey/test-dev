IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetAllConvertedProducts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetAllConvertedProducts]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(6th Dec 2014)	
-- Description	:	Get all converted products for the dealer
-- Modified by :Kartik Rathod,on 26 Nov,2015 fetched Comments
-- Modified By : Amit Yadav(14th march 2016)
-- Purpose : To get the groupType for products. 
-- modified By :Mihir Chheda [05-04-2016]
-- purpose : to get package id 
-- Modifier		: KARTIK RATHOD on 6 April 2016,fetched isActive column
-- [M_GetAllConvertedProducts] 3377
-- =============================================
CREATE PROCEDURE [dbo].[M_GetAllConvertedProducts]
	
	@DealerId	INT 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT 
		DSD.DealerId ,DSD.Id AS SalesId , DSD.ClosingAmount ,
		PK.Name AS Package ,DSD.UpdatedOn , ISNULL(DSD.Comments,'-') AS Comments,
		IPC.GroupType ,PK.ID as PackageID ,
		PK.IsActive -- Modifier		: KARTIK RATHOD on 6 April 2016,fetched isActive column
	FROM 
		DCRM_SalesDealer DSD(NOLOCK) 
		INNER JOIN Packages PK(NOLOCK) ON DSD.PitchingProduct = PK.Id 
		AND DSD.DealerId = @DealerId 
		AND DSD.LeadStatus = 2 
		INNER JOIN InquiryPointCategory IPC(NOLOCK) ON IPC.Id=PK.InqPtCategoryId --To get the GroupType
	WHERE 
		DSD.DealerId = @DealerId
		AND DSD.TransactionId IS NULL
	ORDER BY 
		DSD.UpdatedOn DESC
END


------------------------------------------------------------------------------------------------------------------------


