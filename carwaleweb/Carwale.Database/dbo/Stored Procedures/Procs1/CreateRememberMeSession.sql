IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CreateRememberMeSession]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CreateRememberMeSession]
GO

	
-- =============================================
-- Author:		amit vema
-- Create date: 12 june 2014
-- Description:	insert remember be session details for the input customerid
-- =============================================
CREATE PROCEDURE [dbo].[CreateRememberMeSession] 
	-- Add the parameters for the stored procedure here
	@CustomerId [numeric](18, 0),
	@Identifier [varchar](30),
	@AccessToken [varchar](30),
	@IPAddress [varchar](40),
	@UserAgent [varchar](max)
AS
BEGIN
	--SET NOCOUNT ON
	INSERT INTO RememberMeSessions (CustomerId,Identifier,AccessToken,IPAddress,UserAgent)
	VALUES(@CustomerId,@Identifier,@AccessToken,@IPAddress,@UserAgent)
END

