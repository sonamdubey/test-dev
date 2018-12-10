IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[EndRememberMeSession]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[EndRememberMeSession]
GO

	
-- =============================================
-- Author:		amit vema
-- Create date: 12 june 2014
-- Description:	End remember be session for the input customerid
-- =============================================
CREATE PROCEDURE [dbo].[EndRememberMeSession] 
	-- Add the parameters for the stored procedure here
	@CustomerId [numeric](18, 0),
	@Identifier [varchar](30)
AS
BEGIN
	--SET NOCOUNT ON
	UPDATE RememberMeSessions SET IsActive = 0
	WHERE CustomerId = @CustomerId and Identifier = @Identifier
END

