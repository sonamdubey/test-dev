IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetBlockedKeywords]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetBlockedKeywords]
GO

	
-- =============================================
-- Author:		<Anuj Dhar>
-- Create date: <13/09/2016>
-- Description:	<To fetch the list of blocked keywords>
-- =============================================
CREATE PROCEDURE [dbo].[GetBlockedKeywords]
AS
BEGIN
	SELECT Keyword FROM BlockedKeywords WITH (NOLOCK)
END

