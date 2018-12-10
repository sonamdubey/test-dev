IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertNewBlockedCookie]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertNewBlockedCookie]
GO

	
-- =============================================
-- Author:		Vicky Lund
-- Create date: 28-10-2016
-- Description:	Insert new blocked Cookie (done)
-- =============================================
CREATE PROCEDURE InsertNewBlockedCookie @Cookie VARCHAR(50)
AS
BEGIN
	IF NOT EXISTS (
			SELECT BCC.Cookie
			FROM BlockedCwCookies BCC WITH (NOLOCK)
			WHERE BCC.Cookie = @Cookie
			)
	BEGIN
		INSERT INTO BlockedCwCookies (Cookie)
		VALUES (@Cookie)
	END
END
