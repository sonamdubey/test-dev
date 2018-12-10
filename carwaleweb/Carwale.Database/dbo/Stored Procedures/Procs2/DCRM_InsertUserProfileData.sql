IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_InsertUserProfileData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_InsertUserProfileData]
GO

	CREATE PROCEDURE [dbo].[DCRM_InsertUserProfileData]
@MobileNO		VARCHAR(10),
@UserAddress	VARCHAR(250),
@HostURL		VARCHAR(250),
@UserID			INT,
@Result			INT OUTPUT
AS
BEGIN
	UPDATE OprUsers SET PhoneNo = @MobileNO ,Address = @UserAddress, --HostURL = @HostURL,--, IsReplicated = 0 --By default set IsReplicated value 0
	 IsApproved=0 --'0' means UnApproved Image
	WHERE Id = @UserID
	
	IF @@ROWCOUNT <> 0
		SET @Result = 1
END
