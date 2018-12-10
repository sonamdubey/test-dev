IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCertificationOrganization]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCertificationOrganization]
GO

	-- =============================================
-- Author:		<Nilesh Utture>
-- Create date: <21/12/2012>
-- Description:	<Get CertificationOrganization for API>
-- Modified By: Nilesh Utture on 03rd April, 2013 Added IsCertified field in SELECT Clause and added right outer join
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetCertificationOrganization]
	@BranchId BIGINT
AS
BEGIN
	SELECT C.Id, C.CertifiedOrgName AS Name, 
	C.HostURL+C.DirectoryPath+C.LogoURL AS Url,
	CASE WHEN D.CertificationId = -1 THEN CONVERT(BIT,0) ELSE CONVERT(BIT,1) END AS IsCertified 
	FROM Classified_CertifiedOrg C 
	RIGHT OUTER JOIN Dealers D 
	                           ON C.Id=D.CertificationId 
	AND C.IsActive=1 
	AND D.IsDealerActive =1 
	WHERE D.ID=@BranchId
END
