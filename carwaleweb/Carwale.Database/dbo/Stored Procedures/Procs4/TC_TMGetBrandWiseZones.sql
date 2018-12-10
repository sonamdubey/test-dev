IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMGetBrandWiseZones]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMGetBrandWiseZones]
GO

	-- =============================================
-- Author	    :	Nilesh Utture
-- Create date	:	8th Nov, 2013
-- Description	:	To get Zones List
-- =============================================
CREATE  PROCEDURE [dbo].[TC_TMGetBrandWiseZones]
	@MakeId SMALLINT
 AS
BEGIN     
	SELECT	TC_BrandZoneId AS Value,
			ZoneName AS Text
	FROM TC_BrandZone WITH (NOLOCK) 
	WHERE IsActive=1
	AND	  MakeId = @MakeId		 
END
