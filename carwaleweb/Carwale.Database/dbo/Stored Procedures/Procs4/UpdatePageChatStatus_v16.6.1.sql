IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdatePageChatStatus_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdatePageChatStatus_v16]
GO

	-- =============================================
-- Author:		Vicky Lund
-- Create date: 01/06/2016
-- EXEC [UpdatePageChatStatus] 1,1
-- Modified By Sanjay On 24/06/2016 Changes pageId into Id 
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePageChatStatus_v16.6.1] @Id INT
	,@ChatStatus BIT
	,@UserId INT
AS
BEGIN
	UPDATE ChatManagement
	SET IsChatOn = @ChatStatus
		,UpdatedOn = getdate()
		,UpdatedBy = @UserId
	WHERE Id = @Id

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
	WHERE CM.Id = @Id
END



/****** Object:  StoredProcedure [dbo].[GetChatShownFlag]    Script Date: 6/29/2016 5:03:47 PM ******/

SET QUOTED_IDENTIFIER ON
