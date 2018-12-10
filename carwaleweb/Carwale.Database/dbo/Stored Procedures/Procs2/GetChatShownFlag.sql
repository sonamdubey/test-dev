IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetChatShownFlag]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetChatShownFlag]
GO

	
-- =============================================
-- Author:		<Sanjay Soni>
-- Create date: <21/06/2016>
-- Description:	to fetch Chat Enable Flag
-- =============================================
CREATE PROCEDURE [dbo].[GetChatShownFlag] @pageId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT cm.IsChatOn
	FROM ChatManagement CM WITH(NOLOCK)
	INNER JOIN ChatManagementMapping CMM WITH(NOLOCK) ON CM.Id = CMM.ChatManagementId
		AND CM.IsActive = 1
	WHERE PageId = @pageId
END