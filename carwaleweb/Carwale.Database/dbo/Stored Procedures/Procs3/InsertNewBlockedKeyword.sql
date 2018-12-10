IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertNewBlockedKeyword]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertNewBlockedKeyword]
GO

	-- =============================================
-- Author:		<Anuj Dhar>
-- Create date: <22/09/2016>
-- Description:	<Insert new blocked keyword>
-- =============================================
CREATE PROCEDURE [dbo].[InsertNewBlockedKeyword]
	@Keyword VARCHAR(50)
AS
BEGIN
	IF NOT EXISTS (SELECT Keyword FROM BlockedKeywords WITH (NOLOCK) WHERE Keyword = @Keyword)
	BEGIN
		INSERT INTO BlockedKeywords (Keyword) VALUES (@Keyword)
	END
END

