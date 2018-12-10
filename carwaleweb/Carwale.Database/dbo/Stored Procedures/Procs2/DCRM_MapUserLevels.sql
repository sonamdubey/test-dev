IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_MapUserLevels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_MapUserLevels]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <12/2/2015>
-- Description:	<MapUserLevels>
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_MapUserLevels]
	@UserId		VARCHAR(MAX),
	@LevelId	SMALLINT
AS
BEGIN
	UPDATE DCRM_ADM_Users SET MappedLevel = @LevelId WHERE UserId
	IN(SELECT LISTMEMBER FROM fnSplitCSV(@UserId))
END
