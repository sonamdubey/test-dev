IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetAMListOnZone]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetAMListOnZone]
GO
	-- Author		:	Tejashree Patil.
-- Create date	:	8 Oct 2013.
-- Description	:	This SP used to get all area managers depend upon zoneId.
-- =============================================    
CREATE  PROCEDURE [dbo].[TC_GetAMListOnZone]
 -- Add the parameters for the stored procedure here   
 @ZoneId INT,
 @MakeId INT
AS    
BEGIN    
	
	SELECT	DISTINCT TSU.TC_SpecialUsersId AS Value,TSU.UserName Text
	FROM	DEALERS as D WITH (NOLOCK)
			INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = @MakeId AND  D.IsDealerActive= 1
			INNER JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_AMId=TSU.TC_SpecialUsersId 
	WHERE	TSU.IsActive=1
			AND (@ZoneId IS NULL OR D.TC_BrandZoneId=@ZoneId)
		
END
