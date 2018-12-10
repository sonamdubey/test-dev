IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_GetAllConvertedDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_GetAllConvertedDealers]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(4th April 2014)
-- Description	:	Get Package details and dealer details of converted dealers
-- Modifier		:	Sachin Bharti(16th July 2014)
-- Purpose		:	Added Used Car Packages also
-- =============================================
CREATE PROCEDURE [dbo].[RVN_GetAllConvertedDealers] 
	
	@StateId	INT = NULL,
	@CityId		INT = NULL,
	@DealerId	NUMERIC(18,0) = NULL,
	@DealerName VARCHAR(200) = NULL

AS
BEGIN
	
	SET NOCOUNT ON;

    SELECT	DISTINCT D.ID AS DealerId,	
			D.Organization ,	
			D.StateId AS StateId,	
			D.TC_DealerTypeId ,
			TD.DealerType ,
			C.ID AS CityId,
			C.Name AS CityName,
			TD.DealerType,
			DSD.Id AS SalesDlrId,
			DSD.ClosingDate,	
			DSD.PitchingProduct,	
			DSD.PitchDuration,	
			DSD.EntryDate,		
			DSD.UpdatedOn,
			ISNULL(DSD.ClosingAmount,0) AS	ClosingAmount,	
			ISNULL(DSD.DiscountAmount,0)AS	DiscountAmount,
			ISNULL(DSD.ProductAmount,0) AS 	ProductAmount,
			ISNULL(DSD.ServiceTax,0)	AS 	ServiceTax,
			ISNULL(DSD.TotalAmount,0)	AS	TotalAmount,
			DSD.EntryDate,	
			DSD.ClosingProbability,		
			DSD.ClosingDate,	
			ISNULL(DSD.IsTDSGiven,0) AS IsTDS,
			OU.UserName AS ActionTakenBy,
			IPC.Id AS InqPntId,
			PK.Name AS PackageName,	PK.Id AS PackageId
			
	FROM	DCRM_SalesDealer DSD(NOLOCK)
			INNER	JOIN Dealers D(NOLOCK) ON D.ID = DSD.DealerId 
					AND DSD.ClosingProbability = 90	AND DSD.LeadStatus = 2 --AND D.Status = 0 -- Packages having closed status and dealer is active and get converted	
			INNER	JOIN Packages PK(NOLOCK) ON PK.Id = DSD.PitchingProduct
			INNER	JOIN InquiryPointCategory IPC(NOLOCK) ON IPC.Id = PK.InqPtCategoryId AND IPC.GroupType IN(1,2,3,4)
			INNER	JOIN Cities C (NOLOCK) ON C.ID = D.CityId 
			INNER	JOIN TC_DealerType TD(NOLOCK) ON TD.TC_DealerTypeId = D.TC_DealerTypeId 
			INNER	JOIN OprUsers OU(NOLOCK) ON OU.Id = DSD.UpdatedBy 
	WHERE 	(@CityId	IS NULL OR D.CityId = @CityId)	AND
			(@StateId	IS NULL OR D.StateId = @StateId)AND 
			(@DealerId	IS NULL OR D.ID = @DealerId)	AND
			(@DealerName IS NULL OR D.Organization LIKE '%'+@DealerName+'%')
	ORDER BY DSD.UpdatedOn DESC

END

