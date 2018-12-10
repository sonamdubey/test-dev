IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetUserWalletAccount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetUserWalletAccount]
GO

	

-- =============================================
-- Author:		Vinay Kumar Prajapati
-- Create date:  6th jan 2016
-- Description:	  Get Related Users  Ewallet Account Details  
-- =============================================
CREATE PROCEDURE  [dbo].[TC_GetUserWalletAccount]
@UserId INT
AS
BEGIN
      -- Avoid Extra message 
	   SET NOCOUNT ON

	   SELECT AD.EmailId,Ad.MobileNo FROM TC_UsersEWalletAccountDetails AS AD WITH(NOLOCK) WHERE AD.Tc_UsersId= @UserId

END

