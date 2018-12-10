IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE NAME = 'InsertNewBlockedCookie'
			AND xtype = 'P'
		)
BEGIN
	DROP PROCEDURE InsertNewBlockedCookie
END
GO

-- =============================================
-- Author:		Vicky Lund
-- Create date: 28-10-2016
-- Description:	Insert new blocked Cookie
-- =============================================
CREATE PROCEDURE [dbo].[InsertNewBlockedCookie] @Cookie VARCHAR(50)
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