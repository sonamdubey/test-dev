IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateAllUserPasswordHash]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateAllUserPasswordHash]
GO

	-- ==========================================================================
-- Author		: Suresh Prajapati
-- Created on	: 25th Feb, 2015
-- Description	: To Update all TC_Users HashSalt and PasswordHash
-- ==========================================================================
CREATE PROCEDURE [dbo].[TC_UpdateAllUserPasswordHash] @Hashpassword TC_UserHashPassword READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE TC_Users
	SET HashSalt = HP.HashSalt
		,PasswordHash = HP.PasswordHash
	FROM TC_Users AS U WITH (NOLOCK)
	JOIN @Hashpassword AS HP ON HP.UserId = U.Id
	WHERE HP.UserId = U.Id

	-- cw executive
	UPDATE TC_Users
	SET HashSalt = '8g2GlY'
		,PasswordHash = '3517e98e1007e43e47ac78e3c2b0bdfbe347d15ec0e77b078f6dfb4087c5e484'
	WHERE Email LIKE '%CW@CarWale.com'
		AND IsCarwaleUser = 1
		AND Password = 'Default'

	-- Access Trading cars for deals dealer
	UPDATE TC_Users
	SET HashSalt = '8g2GlY'
		,PasswordHash = '3517e98e1007e43e47ac78e3c2b0bdfbe347d15ec0e77b078f6dfb4087c5e484'
	WHERE Email LIKE '%cw@carwale.com'
		AND Password = 'Default'
END


