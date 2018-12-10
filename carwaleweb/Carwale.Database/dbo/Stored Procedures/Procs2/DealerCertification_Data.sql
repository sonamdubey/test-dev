IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DealerCertification_Data]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DealerCertification_Data]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 24 Apr 2013
-- Description:	This SP Provides fetch data for paritcular certification for updation
-- =============================================
CREATE PROCEDURE [dbo].[DealerCertification_Data] 
@CertificationId INT = NULL
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CertifiedOrgName AS Certification,
		Description , HostURL + ISNULL(DirectoryPath,'') + LogoURL AS ImagePath,
		Advantages,Criteria,CoreBenefits,CheckPoints,WarrantyServices
		FROM Classified_CertifiedOrg
		WHERE   Id = @CertificationId Order by CertifiedOrgName 
END
