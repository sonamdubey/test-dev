IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CWAwards_SaveSubscription]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CWAwards_SaveSubscription]
GO

	
-- =============================================
-- Author:		Vaibhav K
-- Create date: 15-Mar-2013
-- Description:	To insert email subscription for Carwale Awards 2013 results
-- =============================================
CREATE PROCEDURE [dbo].[CWAwards_SaveSubscription]
	-- Add the parameters for the stored procedure here
	@Email		VARCHAR(50),
	@Status		SMALLINT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status = 0
	
	INSERT INTO CWAwardsSubscriptions( Email )
	VALUES ( @Email )
	
	SET @Status = 1
END

