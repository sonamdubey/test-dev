IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SavePasswordChangeAT]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SavePasswordChangeAT]
GO

	
-- =============================================
-- Author:		amit vema
-- Create date: 12 june 2014
-- Description:	save password change access token specific to customerid
-- =============================================
CREATE PROCEDURE [dbo].[SavePasswordChangeAT] 
	-- Add the parameters for the stored procedure here
	@Email VARCHAR(MAX),
	@AccessToken VARCHAR(MAX),
	@CustomerId NUMERIC(18,0) OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	SET @CustomerId = -1
	SELECT @CustomerId = Id FROM Customers WITH(NOLOCK) WHERE email = @Email

	IF(@CustomerId != -1)
	BEGIN
		DELETE FROM PwdResetAT WHERE CustomerId = @CustomerId

		INSERT INTO PwdResetAT (CustomerId,AccessToken)
		VALUES (@CustomerId,@AccessToken)

		IF(@@ROWCOUNT = 0)
			SET @CustomerId = -1
	END

END

