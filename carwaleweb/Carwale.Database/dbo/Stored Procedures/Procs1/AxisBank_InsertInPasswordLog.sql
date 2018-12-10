IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_InsertInPasswordLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_InsertInPasswordLog]
GO

	
-- =============================================
-- Author:		Akansha
-- Create date: 17.12.2013
-- Description:	Insert Password Log for all users
-- =============================================
CREATE PROCEDURE [dbo].[AxisBank_InsertInPasswordLog]
@UserId numeric(18,0),
@PasswordSalt varchar(10),
@PasswordHash varchar(64)
AS
BEGIN
	INSERT INTO AxisBank_UserPasswordLog(UserId, PasswordSalt, PasswordHash,ChangeDateTime)
	VALUES (@UserId, @PasswordSalt, @PasswordHash,GetDate())	
END

