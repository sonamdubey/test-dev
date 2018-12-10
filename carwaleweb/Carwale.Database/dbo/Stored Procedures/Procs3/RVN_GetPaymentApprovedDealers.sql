IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_GetPaymentApprovedDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_GetPaymentApprovedDealers]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(8th Oct 2014)
-- Description	:	Get Dealers those payments are approved
--					for that city only
-- =============================================
CREATE PROCEDURE [dbo].[RVN_GetPaymentApprovedDealers]
	
	@CityId		INT,
	@PackageStatus	INT = NULL
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
			DISTINCT D.ID AS Value, 
			D.Organization AS Text 
	FROM 
			RVN_DealerPackageFeatures RVN(NOLOCK) 
			INNER JOIN Dealers D(NOLOCK) ON D.ID  = RVN.DealerId 
	WHERE 
			D.CityId = @CityId
			AND (@PackageStatus IS NULL OR RVN.PackageStatus = @PackageStatus)
	ORDER BY D.Organization
END

