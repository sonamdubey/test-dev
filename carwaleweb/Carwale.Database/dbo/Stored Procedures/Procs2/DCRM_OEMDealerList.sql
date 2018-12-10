IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_OEMDealerList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_OEMDealerList]
GO

	-- =============================================
-- Author	:	Sachin Bharti
-- Create date	:	21st Nov 2013
-- Description	:	Get OEM Dealers
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_OEMDealerList]
	@StateId	INT		=	NULL,
	@CityId		INT		=	NULL,
	@AreaId		INT		=	NULL,
	@Organization	VARCHAR(100)	=	NULL,
	@EmailId		VARCHAR(100)	=	NULL
AS
BEGIN
	SELECT  ND.ID, ND.Name, C.Name AS City ,ND.EMail,ND.ContactPerson,ND.Mobile  
	FROM  NCS_Dealers AS ND WITH(NOLOCK) 
	INNER JOIN Cities C (NOLOCK) ON C.ID = ND.CityId	
	INNER JOIN States S (NOLOCK) ON S.ID = C.StateId	
	WHERE (@StateId IS NULL OR S.ID = @StateId) 
	AND (@CityId IS NULL OR C.ID = @CityId)
	AND (@AreaId IS NULL OR  ND.AreaId = @AreaId )
	AND (@Organization IS NULL OR ND.Name LIKE @Organization)
	AND (@EmailId IS NULL OR ND.EMail LIKE @EmailId)
	ORDER BY ND.Name	
END
