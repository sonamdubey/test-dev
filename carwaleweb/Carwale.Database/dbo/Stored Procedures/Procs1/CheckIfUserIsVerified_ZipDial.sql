IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckIfUserIsVerified_ZipDial]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckIfUserIsVerified_ZipDial]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 19.06.2013
-- Description:	Update TokenResponseTime For particular client id Exec CheckIfUserIsVerified_ZipDial '','9988774455'
-- =============================================
CREATE Procedure [dbo].[CheckIfUserIsVerified_ZipDial]
@TransactionToken varchar(50)=null,
@EmailId varchar(50),
@MobileNumber varchar(15),
@Status bit=0 output
As
IF EXISTS(SELECT MobileNo FROM CV_MobileEmailPair WHERE EmailId = @EmailId AND MobileNo = @MobileNumber)
		BEGIN
			SET @Status = 1
		END
	ELSE
		BEGIN
			SET @Status = 0
		END


