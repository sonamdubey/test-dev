IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetAllPendingApprovalProducts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetAllPendingApprovalProducts]
GO

	-- =============================================
-- Author:		Mihir A Chheda
-- Create date: 01-04-2016
-- Description:	Get all approval pending products for the dealer
-- Modifier		: KARTIK RATHOD on 6 April 2016,fetched isActive column
-- [M_GetAllPendingApprovalProducts] 3377
-- =============================================
CREATE PROCEDURE [dbo].[M_GetAllPendingApprovalProducts] 
	@DealerId	INT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT		DSD.DealerId ,DSD.Id AS SalesId , DSD.ClosingAmount ,
				PK.Name AS Package ,DSD.UpdatedOn , ISNULL(DSD.Comments,'-') AS Comments,
				IPC.GroupType ,PK.ID as PackageId,PK.IsActive,RVN.*
	FROM		DCRM_SalesDealer DSD(NOLOCK) 
	INNER JOIN	Packages PK(NOLOCK) ON DSD.PitchingProduct = PK.Id 
	INNER JOIN	InquiryPointCategory IPC(NOLOCK) ON IPC.Id=PK.InqPtCategoryId 
	INNER JOIN	DCRM_PaymentTransaction DPT(NOLOCK) ON DPT.TransactionId=DSD.TransactionId 
	LEFT  JOIN  RVN_DealerPackageFeatures RVN(NOLOCK) ON DPT.TransactionId=RVN.TransactionId
	WHERE		DSD.DealerId = @DealerId AND DSD.LeadStatus = 2 AND DPT.IsActive=1  AND RVN.DealerPackageFeatureID IS NULL
	ORDER BY	DSD.UpdatedOn DESC
END

---------------------------------------------------------------------------------------------------------------------------



