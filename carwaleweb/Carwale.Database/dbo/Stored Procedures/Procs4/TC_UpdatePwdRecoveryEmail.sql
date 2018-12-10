IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdatePwdRecoveryEmail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdatePwdRecoveryEmail]
GO

	
-- Created by:	Umesh Ojha
-- Created Date: 18-7-2013
-- Description:	Update Password recovery email for users
-- Modified By : Umesh On 9-july-2013 for adding password recovery email
-- =============================================
create procedure [dbo].[TC_UpdatePwdRecoveryEmail]  
@UserID BIGINT,
@PwdRecoveryEmail VARCHAR(50),
@IsSpecial TINYINT,
@UpdateStatus TINYINT Output 
As  
Begin  
Declare @ID Numeric  
	IF(@IsSpecial = 0)
	BEGIN		
		Update TC_Users Set PwdRecoveryEmail = @PwdRecoveryEmail
		Where ID=@UserID	
		SET @UpdateStatus = 1		
	END 
	ELSE
	BEGIN		 
		Update TC_SpecialUsers Set PwdRecoveryEmail = @PwdRecoveryEmail
		Where TC_SpecialUsersId=@UserID 
		SET @UpdateStatus = 1		
	END
End
