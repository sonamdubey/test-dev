IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_BindAdUnit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_BindAdUnit]
GO

	
-- =============================================
-- Author	:	Ajay Singh(29th Oct 2015)
-- Description	:	Get count for inventory
-- =============================================
CREATE PROCEDURE [dbo].[ESM_BindAdUnit]

@PageId INT = NULL,
@AdUnitId INT = NULL

AS
BEGIN
	SELECT DISTINCT EPA.AdUnitId AS Id,EAU.AdUnitName AS AdUnitName
		FROM ESM_PageAndAdUnit  EPA WITH(NOLOCK)
		LEFT JOIN ESM_AdUnit EAU WITH(NOLOCK) ON EAU.Id =EPA.AdUnitId
		WHERE (@PageId IS NULL OR EPA.PageId=@PageId) AND
			  (@AdUnitId IS NULL OR EPA.AdUnitId=@AdUnitId)			
		ORDER BY AdUnitId
END
