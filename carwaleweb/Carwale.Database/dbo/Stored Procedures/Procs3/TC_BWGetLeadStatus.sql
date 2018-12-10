IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BWGetLeadStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BWGetLeadStatus]
GO

	--=========================================================
-- Author		: Suresh Prajapati
-- Created On	: 22nd Jan, 2016
-- Description	: To get List Active Bikewale Lead Status
--=========================================================
CREATE PROCEDURE [dbo].[TC_BWGetLeadStatus]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TC_BWLeadStatusId
		,[Description]
	FROM TC_BWLeadStatus WITH(NOLOCK)
	WHERE IsActive = 1
	ORDER BY [Description]
END

