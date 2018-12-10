IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetMappingDealersList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetMappingDealersList]
GO
	-- =============================================
-- Author:		Tejashree Patil.
-- Create date: 10 Dec 2013.
-- Description:	This sp is used to get list of mapped dealers.
-- EXEC [TC_GetMappingDealersList]
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetMappingDealersList]
AS
BEGIN
	SELECT	DISTINCT TCD.TC_DealerMappingId,
			D.Organization AS AdminOrganization,
			[dbo].[SubDealers](TCD.TC_DealerMappingId) AS SubDealerOrganization
	FROM	TC_DealerMapping AS TCD
			INNER JOIN	Dealers D WITH(NOLOCK) ON 
						D.ID=TCD.DealerAdminId
			INNER JOIN	TC_SubDealers AS TCS WITH(NOLOCK) ON 
						TCS.TC_DealerMappingId=TCD.TC_DealerMappingId
	WHERE	TCD.IsActive=1 
			AND TCS.IsActive=1

END

