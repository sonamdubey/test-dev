IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetChatManagementPages_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetChatManagementPages_v16]
GO

	
-- =============================================
-- Author:		Vicky Lund
-- Create date: 01/06/2016
-- EXEC [GetChatManagementPages]
-- Modified By Sanjay On 24/06/2016 Added PageId in select query 
-- =============================================
CREATE PROCEDURE [dbo].[GetChatManagementPages_v16.6.1]
AS
BEGIN
--WARNING! ERRORS ENCOUNTERED DURING SQL PARSING!
SELECT Distinct CM.Id
		,CM.PageName AS NAME
		,CM.IsChatOn
	,STUFF((
			SELECT ',' + Convert(VARCHAR, CMM.PageId)
			FROM ChatManagementMapping CMM WITH (NOLOCK) WHERE CMM.ChatManagementId = CM.ID
			FOR XML PATH('')
			), 1, 1, '')  AS PageIds
FROM ChatManagement CM WITH (NOLOCK)
INNER JOIN ChatManagementMapping CMM WITH (NOLOCK) ON CMM.ChatManagementId = CM.ID

END

