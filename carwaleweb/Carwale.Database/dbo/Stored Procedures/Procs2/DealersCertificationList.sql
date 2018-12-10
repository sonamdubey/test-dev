IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DealersCertificationList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DealersCertificationList]
GO

	-- =============================================  
-- Author:  Umesh Ojha  
-- Create date: 9/4/2013  
-- Description: select certification list & certification of dealers
-- =============================================  
CREATE PROCEDURE [dbo].[DealersCertificationList]  
@DealerId INT
AS   
BEGIN  
	SET NOCOUNT ON;  
	SELECT Id,CertifiedOrgName  AS Name  
	FROM Classified_CertifiedOrg  
	WHERE IsActive=1 Order BY CertifiedOrgName   
	
	SELECT C.ID AS CertificationId,C.HostURL+ISNULL(C.DirectoryPath,'')+C.LogoURL AS ImagePath 
		FROM Classified_CertifiedOrg AS C INNER JOIN Dealers D
		ON C.Id = D.CertificationId
		WHERE D.ID = @DealerId
	
END 
