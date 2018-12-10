IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetBrandZoneList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetBrandZoneList]
GO
	-- Author		:	Tejashree Patil.
-- Create date	:	10 Oct 2013.
-- Description	:	This SP used to get zone list.
-- =============================================    
CREATE PROCEDURE [dbo].[TC_GetBrandZoneList] 
 -- Add the parameters for the stored procedure here    
 @MakeId INT
AS    
BEGIN    
	
	SELECT	DISTINCT TC_BrandZoneId Value, ZoneName Text, MakeId
    FROM	TC_BrandZone BZ (NOLOCK)
    WHERE	BZ.MakeId=@MakeId
			AND IsActive=1
	ORDER BY Text
		
END
