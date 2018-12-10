IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_DealersOpenPitchingProduct]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_DealersOpenPitchingProduct]
GO

	-- =============================================
-- Author	:	Sachin Bharti
-- Create date	:	26th Jun 2014
-- Description	:	Get open pitching products of dealer
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_DealersOpenPitchingProduct]
	@DealerId	VARCHAR(10)
AS
BEGIN
	
	SET NOCOUNT ON;

    SELECT P.Name AS Package,P.ID as PackageID , DSD.ID,DSD.ClosingAmount,
	DSD.PitchDuration,DSD.ClosingProbability,OU.UserName AS InitiatedBy,
	CONVERT(VARCHAR,DSD.ClosingDate,106) AS ExpctdClsngDate , 
	CONVERT(VARCHAR,DSD.ClosingDate,101) AS ClosingDate,
	ISNULL(P.InqPtCategoryId ,0) AS InquiryPointId,
	ISNULL(DSD.Quantity,0) AS RSAPkgQuantity,
	ISNULL(DSD.DealerType,1)AS ProductType
	FROM DCRM_SalesDealer DSD WITH(NOLOCK) 
	LEFT JOIN Packages P WITH(NOLOCK) ON DSD.PitchingProduct = P.Id 
	LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = DSD.InitiatedBy 
	LEFT JOIN DCRM_ADM_DealerTypes DAD(NOLOCK) ON DAD.Id = DSD.DealerType
	WHERE DSD.DealerId = @DealerID AND DSD.ClosingProbability <> 100 AND DSD.LeadStatus IN(1,2)
END
