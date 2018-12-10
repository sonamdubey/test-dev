IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetBlockedCwCookies_v16_11_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetBlockedCwCookies_v16_11_1]
GO

	-- =============================================
-- Author:		Vicky Lund					
-- Create date: 28/10/2016
-- Description:	Get blocked CW Cookies (done)
-- =============================================
CREATE PROCEDURE [dbo].[GetBlockedCwCookies_v16_11_1]
AS
BEGIN
	SELECT BCW.Id
		,BCW.Cookie [Name]
	FROM BlockedCwCookies BCW WITH (NOLOCK)
END
