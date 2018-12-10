IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_MOBDealerLogin]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_MOBDealerLogin]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 06-July-2012
-- Description:	Check if user is authenticated or not from table CRM_MobDealers
--				If authenticated return dealer id
-- =============================================
CREATE PROCEDURE [dbo].[CRM_MOBDealerLogin] 
	-- Add the parameters for the stored procedure here
	@LoginId		VARCHAR(50),
	@Password		VARCHAR(50),
	@DealerId		INT OUTPUT,
	@Id				INT OUTPUT,
	@IsSuccess		BIT OUTPUT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SET @IsSuccess = 1
	SET @DealerId = -1
   
	SELECT @DealerId = DealerId, @Id = Id 
	FROM CRM_MobDealers 
	WHERE LoginId = @LoginId AND Password = @Password
	
	IF @@ROWCOUNT = 0 
		SET @IsSuccess = 0
END
