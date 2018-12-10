IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdatePageChatStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdatePageChatStatus]
GO

	-- =============================================
-- Author:		Vicky Lund
-- Create date: 01/06/2016
-- EXEC [UpdatePageChatStatus] 1,1
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePageChatStatus] @PageId INT
	,@ChatStatus BIT
	,@UserId INT
AS
BEGIN
	UPDATE ChatManagement
	SET IsChatOn = @ChatStatus
		,UpdatedOn = getdate()
		,UpdatedBy = @UserId
	WHERE Id = @PageId

	INSERT INTO ChatManagementLogs (
		PageId
		,PageName
		,IsChatOn
		,IsActive
		,UpdatedOn
		,UpdatedBy
		,Remarks
		)
	SELECT CM.Id
		,CM.PageName
		,CM.IsChatOn
		,CM.IsActive
		,CM.UpdatedOn
		,CM.UpdatedBy
		,'Status changed'
	FROM ChatManagement CM WITH (NOLOCK)
	WHERE CM.Id = @PageId
END

