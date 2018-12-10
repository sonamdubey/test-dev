IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_IsLoginIdEmailIdUnique]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_IsLoginIdEmailIdUnique]
GO

	-- Author:		Akansha
-- Create date: 19.12.2013
-- Description:	Check if loginid and emailid exist
-- exec AxisBank_IsLoginIdEmailIdUnique 'test2','test@test.com',0
-- =============================================
CREATE PROCEDURE AxisBank_IsLoginIdEmailIdUnique 
	@LoginId VARCHAR(50),
	@EmailId VARCHAR(100),
	@IsExist BIT OUTPUT
AS
BEGIN
	SELECT UserId
	FROM AxisBank_Users
	WHERE loginid = @loginid
		OR email = @EmailId

	IF @@ROWCOUNT > 0
		SET @isExist = 1
	ELSE
		SET @IsExist = 0

	select @IsExist
END

