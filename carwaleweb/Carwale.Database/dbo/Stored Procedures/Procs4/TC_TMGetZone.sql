IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMGetZone]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMGetZone]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 28/11/2013
-- Description:	Return zone
-- =============================================
CREATE PROCEDURE [dbo].[TC_TMGetZone]
	-- Add the parameters for the stored procedure here
	@RMid INT
AS
BEGIN
	SELECT DISTINCT Z.ZoneName,Z.TC_BrandZoneId FROM TC_BrandZone Z WITH(NOLOCK)
	INNER JOIN Dealers D WITH(NOLOCK) ON D.TC_BrandZoneId = Z.TC_BrandZoneId
	INNER JOIN TC_SpecialUsers S WITH(NOLOCK) ON S.TC_SpecialUsersId = D.TC_RMId
	WHERE S.TC_SpecialUsersId = @RMid  	
END
