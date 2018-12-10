IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RegisterEWalletAccount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RegisterEWalletAccount]
GO

	


-- =============================================
-- Author:		Vinay Kumar Prajapati
-- Create date:  6th jan 2016
-- Description:	  Register Related Users Ewallet Users(single Account Exist perUser per wallet )
-- =============================================
CREATE PROCEDURE  [dbo].[TC_RegisterEWalletAccount]
@UserId INT,
@EmailId VARCHAR(100),
@MobileNo VARCHAR(15),
@EWalletId Int ,
@DealerId  int ,
@IsSaved BIT OUTPUT
AS
BEGIN
      -- Avoid Extra message 
	   SET NOCOUNT ON
       
	    SELECT AD.Tc_UsersId
	    FROM TC_UsersEWalletAccountDetails AS AD WITH(NOLOCK)
	    WHERE  AD.TC_EWalletsId= @EWalletId AND  AD.Tc_UsersId =@UserId 
       
	    IF @@ROWCOUNT = 0
			BEGIN
				INSERT INTO TC_UsersEWalletAccountDetails(TC_EWalletsId,DealerId,Tc_UsersId ,EmailId,MobileNo,EntryDate) 
				VALUES(@EWalletId,@DealerId,@UserId,@EmailId,@MobileNo,GETDATE())

				IF SCOPE_IDENTITY() > 0 
					SET @IsSaved = 1
				ELSE
					SET @IsSaved = 0
			 END 
		ELSE
			SET @IsSaved = 0
	       
END
