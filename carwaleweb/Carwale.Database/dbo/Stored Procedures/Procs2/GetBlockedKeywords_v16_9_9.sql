IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetBlockedKeywords_v16_9_9]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetBlockedKeywords_v16_9_9]
GO

	
-- =============================================
-- Author:		<Anuj Dhar>
-- Create date: <13/09/2016>
-- Description:	<To fetch the list of blocked keywords>
-- Modified : Vicky Lund, 27/09/2016, Fetched Id column and renamed column name
-- =============================================
CREATE PROCEDURE [dbo].[GetBlockedKeywords_v16_9_9]
AS
BEGIN
	SELECT BK.Id
		,BK.Keyword [Name]
	FROM BlockedKeywords BK WITH (NOLOCK)
END

