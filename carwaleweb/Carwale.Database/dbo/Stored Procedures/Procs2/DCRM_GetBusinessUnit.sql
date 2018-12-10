IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetBusinessUnit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetBusinessUnit]
GO

	

-- =============================================
-- Author	:	Sachin Bharti(14th May 2015)
-- Description	:	Used to get page business units
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetBusinessUnit]

AS
	BEGIN
	
		--get business units
		SELECT 
			BU.Id, BU.Name 
		FROM 
			DCRM_BusinessUnit BU(NOLOCK) 
		WHERE 
			BU.IsActive = 1
			AND BU.Id <> 4
		ORDER BY Name     
	
	END

