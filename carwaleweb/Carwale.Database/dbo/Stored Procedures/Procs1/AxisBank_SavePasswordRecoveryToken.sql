IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_SavePasswordRecoveryToken]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_SavePasswordRecoveryToken]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 5 Nov 2012
-- Description:	Proc to save OR update the password recovery tokens
-- =============================================
Create PROCEDURE AxisBank_SavePasswordRecoveryToken
	-- Add the parameters for the stored procedure here
	@UserId BIGINT,
	@Token VARCHAR(200)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @IsNew Bit = 1
	
	IF EXISTS (SELECT Id FROM AxisBank_PasswordRecoveryTokens WHERE UserId = @UserId)	
		SET @IsNew = 0	

	IF @IsNew = 1
	BEGIN
		INSERT INTO AxisBank_PasswordRecoveryTokens
		(UserId, Token, IsActiveToken, EntryDate, ExpiryDate)
		VALUES (@UserId, @Token, 1, GETDATE(), dateadd("hh",24,GETDATE()))
    END
    ELSE
    BEGIN
		UPDATE AxisBank_PasswordRecoveryTokens
		SET 
			Token = @Token,
			IsActiveToken = 1,
			EntryDate = GETDATE(),
			ExpiryDate = dateadd("hh",24,GETDATE())
		WHERE UserId = @UserId
    END
END
