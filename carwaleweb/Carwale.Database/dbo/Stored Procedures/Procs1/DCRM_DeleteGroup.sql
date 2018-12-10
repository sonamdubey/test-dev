IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_DeleteGroup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_DeleteGroup]
GO

	-- =============================================
-- Author:		Amit Yadav 
-- Create date: 12th Feb 2016
-- Description:	To delete the multioutlets/groups from 
-- EXEC [DCRM_DeleteGroup] 
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_DeleteGroup]
	
	@AdminId INT 
AS
BEGIN
	
	--SET NOCOUNT ON;

	UPDATE TC_DealerAdmin
	SET IsActive = 0
	WHERE Id = @AdminId

	UPDATE D
	SET D.IsGroup = 0,D.ISMultioutlet = 0
	FROM TC_DealerAdmin AS DA WITH(NOLOCK)
	INNER JOIN Dealers AS D WITH(NOLOCK) ON DA.DealerId=D.ID
	WHERE DA.Id = @AdminId

END
