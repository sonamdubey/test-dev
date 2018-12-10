IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCertifiedOrg]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCertifiedOrg]
GO

	-- =============================================
-- Author:		Binumon George
-- Create date: 29th Dec 2011
-- Description:	This procedure is used to get dealer certified organization
-- Modified By: Tejashree Patil on 21 Sept 2012 at 7 pm
-- Description: LogoURL column added in SELECT clause and WITH(NOLOCK)implemented
-- [TC_GetCertifiedOrg] 968
-- Modified By: Umesh on 15 Apr 2013 dynamic path for displaying certification in Add Stock Page
-- =============================================
CREATE PROCEDURE  [dbo].[TC_GetCertifiedOrg]
(
@DealerId NUMERIC
)
AS
BEGIN
	-- Modified By: Tejashre  Patil on 21 Sept 2012 at 7 pm
	SELECT	C.Id, C.CertifiedOrgName,
			C.HostURL + ISNULL(C.DirectoryPath,'') + C.LogoURL AS  LogoURL
	FROM	Classified_CertifiedOrg C WITH(NOLOCK)
			INNER JOIN Dealers D WITH(NOLOCK)ON C.Id=D.CertificationId 
			AND D.ID=@DealerId AND C.IsActive=1	
END

