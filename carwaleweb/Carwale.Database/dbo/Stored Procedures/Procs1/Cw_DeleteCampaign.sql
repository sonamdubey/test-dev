IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Cw_DeleteCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Cw_DeleteCampaign]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(12th April 2016)
-- Description	:	Delete sponsored campaigns
-- =============================================
CREATE PROCEDURE [dbo].[Cw_DeleteCampaign]
	@CampaignId	INT,
	@UserId	INT , 
	@IsDeleted SMALLINT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE SponsoredCampaigns SET IsDeleted=1 ,UpdatedOn = GETDATE() , UpdatedBy =  @UserId
	WHERE Id=@CampaignId
	IF @@ROWCOUNT > 0 
		SET @IsDeleted = 1
	ELSE 
		SET @IsDeleted = 0
END
