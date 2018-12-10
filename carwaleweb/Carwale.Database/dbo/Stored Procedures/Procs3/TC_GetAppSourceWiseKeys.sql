IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetAppSourceWiseKeys]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetAppSourceWiseKeys]
GO
	--==============================================
-- Author:		Tejashree Patil
-- Create date: 6 Oct 2014
-- Description:	To get souce and respective key for apps security.
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetAppSourceWiseKeys]
	@SourceId SMALLINT,
	@CWK VARCHAR(50)
AS
BEGIN
	
	SELECT	ID, SourceId, CWK
	FROM	WA_Keys 
	WHERE	SourceId=@SourceId 
			AND CWK=@CWK 
			AND IsActive=1
END


