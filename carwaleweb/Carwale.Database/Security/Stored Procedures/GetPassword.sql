IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[Security].[GetPassword]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [Security].[GetPassword]
GO

	

-- ===================================================
-- Author:		chetan
-- Create date: 6/2/2014
-- Description:	SP for userinformation 
-- ===================================================

CREATE PROCEDURE [Security].[GetPassword] 
	 @UserName varchar(100),
	 @PassWord varchar(100) output
AS
BEGIN

	Select @PassWord =  ui.PassWord from Security.UserInfo  as ui WITH(NOLOCK) where UserName = @UserName

END

