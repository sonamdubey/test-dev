IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DealerCertificationList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DealerCertificationList]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 24 Apr 2013
-- Description:	This SP Provides all ceritfication list.
-- =============================================
CREATE PROCEDURE [dbo].[DealerCertificationList] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id AS CerificationId,CertifiedOrgName AS Certification,
		Description , HostURL + ISNULL(DirectoryPath,'') + LogoURL AS ImagePath
		FROM Classified_CertifiedOrg
		WHERE IsActive = 1 Order by CertifiedOrgName 
END

