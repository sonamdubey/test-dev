IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetChatManagementPages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetChatManagementPages]
GO

	-- =============================================
-- Author:		Vicky Lund
-- Create date: 01/06/2016
-- EXEC [GetChatManagementPages]
-- =============================================
CREATE PROCEDURE [dbo].[GetChatManagementPages]
AS
BEGIN
	SELECT CM.Id
		,CM.PageName AS NAME
		,CM.IsChatOn
	FROM ChatManagement CM WITH (NOLOCK)
END

