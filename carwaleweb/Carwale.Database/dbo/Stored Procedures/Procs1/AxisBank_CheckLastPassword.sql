IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_CheckLastPassword]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_CheckLastPassword]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 17.12.2013
-- Description:	Gets all the password hash and salts for a particular user
-- exec AxisBank_CheckLastPassword 1212
-- =============================================
CREATE PROCEDURE AxisBank_CheckLastPassword
	@UserId Numeric(18,0)
AS
BEGIN
	SELECT passwordhash,passwordsalt
	FROM AxisBank_UserPasswordLog
	WHERE USERID=@UserId
END

