IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RemoveBlockedKeyword]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RemoveBlockedKeyword]
GO

	-- =============================================
-- Author:		<Anuj Dhar>
-- Create date: <22/09/2016>
-- Description:	<Remove existing blocked keyword>
-- =============================================
CREATE PROCEDURE [dbo].[RemoveBlockedKeyword] @Ids VARCHAR(100)
AS
BEGIN
	DELETE
	FROM BlockedKeywords
	WHERE Id IN (
			SELECT ListMember
			FROM dbo.fnSplitCSVMAx(@Ids)
			)
END

