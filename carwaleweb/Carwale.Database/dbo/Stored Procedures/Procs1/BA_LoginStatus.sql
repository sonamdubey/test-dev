IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_LoginStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_LoginStatus]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 26-May-14
-- Description:	Validate User and return the status 
-- =============================================
CREATE PROCEDURE [dbo].[BA_LoginStatus]
	@UserName varchar(50),
	@Password varchar(50),
	@Version varchar(20)
AS
BEGIN

	DECLARE @Status INT  =  -1  --Error
	DECLARE @IsActive BIT = 0 --not active
	DECLARE @IsCompatible BIT = 0 
	DECLARE @IsSupported BIT = 0
	DECLARE @BrokerId INT = 0
	DECLARE @ID BIGINT = NULL
	SET NOCOUNT ON;

	SELECT @ID = BL.ID ,@IsActive = BL.IsActive, @BrokerId = BL.BrokerID FROM BA_Login AS BL WITH(NOLOCK) WHERE BL.UserName = @UserName AND BL.Password = @Password 
	
	IF @ID > 0 --VALID USER
		BEGIN
		--check  version
			SELECT @IsSupported = AV.IsSupported,@IsCompatible= AV.IsCompatible FROM BA_AppVersion as AV   WITH(NOLOCK) WHERE AV.Version = @Version
				BEGIN
					IF @IsSupported =  0 ---not suported || Need to update
						SET @Status =  4
				ELSE
					BEGIN
						IF @IsCompatible = 0
							SET @Status = 0 --not compatible || partially supported
						ELSE
							SET @Status = 1 ---Valid User and Updated ||
					END
				END
		END
	ELSE --WRONG PASS. \ USER
		BEGIN
			SET @Status = 2 --Not valid 
			SET @BrokerId = 0 
		END

 SELECT @Status AS Status, @IsActive AS IsActive, @BrokerId AS BrokerId   --Return Status 
END

SET ANSI_NULLS ON
