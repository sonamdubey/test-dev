IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OAuthCheck]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OAuthCheck]
GO

	-- =============================================
-- Author:		Rohan Sapkal
-- Create date: 13-2-2015
-- Description:	Check OAuth key for specific user
-- =============================================
CREATE PROCEDURE [dbo].[OAuthCheck]
	-- Add the parameters for the stored procedure here
	@OAuth VarChar(40),
	@CustId Numeric output,
	@Email varchar(50) output,
	@Name varchar(100) output,
	@IsEmailVerified bit output,
	@Mobile varchar(15) output
AS
BEGIN
	SELECT TOP 1
	@CustId=Id
	,@Email=email
	,@Name=Name
	,@IsEmailVerified=IsEmailVerified
	,@Mobile=Mobile
	 from Customers WITH(NOLOCK)
	 WHERE
	 OAuth=@OAuth

	IF (@@ROWCOUNT = 0)
	BEGIN
		SET @CustId = -1
		SET @Email=''
		SET @Name=''
		SET @Mobile=''
		SET @IsEmailVerified=0
	END
END

