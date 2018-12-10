IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerCertificationDetailsSelect]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerCertificationDetailsSelect]
GO

	
-- =============================================
-- Author:		Vivek Gupta
-- Create date: 22nd April,2013
-- Description:	Select Certification Details of Dealers
-- TC_DealerCertificationDetailsSelect 5
-- =============================================
CREATE PROCEDURE [dbo].[TC_DealerCertificationDetailsSelect]
	@DealersId INT
AS
BEGIN

  IF EXISTS (SELECT DealersId FROM TC_DealerCertificationDetail WHERE DealersId=@DealersId)
   BEGIN
	SELECT 
			CC.HostURL+CC.DirectoryPath+LogoURL AS LogoUrl,
			CC.CertifiedOrgName AS OrgName,
			DC.Description,
			DC.Advantages,
			DC.Criteria,
			DC.CoreBenefits,
			DC.CheckPoints,
			DC.WarrantyServices

	FROM                   TC_DealerCertificationDetail DC WITH(NOLOCK)

				INNER JOIN Classified_CertifiedOrg      CC WITH(NOLOCK)

											   ON DC.Classified_CertifiedOrgId = CC.Id

	WHERE DealersID=@DealersId
	
	END
	
	ELSE
	BEGIN
	 SELECT 
			CC.HostURL+CC.DirectoryPath+LogoURL AS LogoUrl,
			CC.CertifiedOrgName AS OrgName,
			CC.Description,
			CC.Advantages,
			CC.Criteria,
			CC.CoreBenefits,
			CC.CheckPoints,
			CC.WarrantyServices

	FROM    Classified_CertifiedOrg CC WITH(NOLOCK)
	
	WHERE   CC.Id=(SELECT D.CertificationId FROM Dealers D WITH(NOLOCK) WHERE D.ID=@DealersId) 
	
	END

END

